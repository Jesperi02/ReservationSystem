using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using ReservationSystem22.Models;
using ReservationSysten22.Repositories;
using System.Security.Cryptography;

namespace ReservationSysten22.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<UserDTO> CreateUserAsync(User user)
        {
            byte[] salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            User newUser = new User
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Salt = salt,
                Password = hashedPassword,
                JoinDate = DateTime.Now
            };

            newUser = await _repository.AddUserAsync(newUser);

            if (newUser == null)
            {
                return null;
            }

            return UserToDTO(newUser);
        }

        public async Task<bool> DeletUserAsync(long id)
        {
            User user = await _repository.GetUserAsync(id);
            return await _repository.DeleteUserAsync(user);
        }

        public async Task<UserDTO> GetUserAsync(long id)
        {
            User user = await _repository.GetUserAsync(id);
            return UserToDTO(user);
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            IEnumerable<User> userList = await _repository.GetUsersAsync();
            List<UserDTO> userDTOList = new List<UserDTO>();

            foreach (User user in userList)
            {
                userDTOList.Add(UserToDTO(user));
            }

            return userDTOList;
        }

        public async Task<UserDTO> UpdateUserAsync(User user)
        {
            User returnedUser = await _repository.UpdateUserAsync(user);
            return UserToDTO(returnedUser);
        }

        private UserDTO UserToDTO(User user)
        {
            UserDTO userDTO = new UserDTO();
            userDTO.UserName = user.UserName;
            userDTO.FirstName = user.FirstName;
            userDTO.LastName = user.LastName;
            userDTO.JoinDate = user.JoinDate;
            userDTO.LastLoginDate = user.LastLoginDate;
            return userDTO;
        }

        private async Task<User> DTOToUser(UserDTO userDTO)
        {
            User user = new User();
            user.UserName = userDTO.UserName;
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.JoinDate = userDTO.JoinDate;
            user.LastLoginDate = userDTO.LastLoginDate;
            return user;
        }
    }
}
