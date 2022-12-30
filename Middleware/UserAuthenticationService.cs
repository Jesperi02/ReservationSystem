using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using ReservationSystem22.Models;

namespace ReservationSysten22.Middleware
{
    public interface IUserAuthenticationService
    {
        Task<User> Authenticate(string username, string password);
        Task<bool> isAllowed(string? username, ItemDTO itemDTO);
        Task<bool> isAllowed(string? username, User user);
        Task<bool> isAllowed(string? username, UserDTO user);
        Task<bool> isAllowed(string? username, ReservationDTO reservationDTO);
    }

    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly ReservationContext _context;

        public UserAuthenticationService(ReservationContext context)
        {
            _context = context;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            User? user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            byte[] salt = user.Salt;

            // Käyttäjällä ei suolaa. päästä läpi
            if (salt.Length == 0)
            {
                return user;
            }

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            if (hashedPassword != user.Password)
            {
                return null;
            }

            return user;
        }

        public async Task<bool> isAllowed(string? username, ItemDTO itemDTO)
        {
            User? user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            Item? dbItem = await _context.Items.Include(i => i.Owner).FirstOrDefaultAsync(i => i.Id == itemDTO.Id);
            
            if (dbItem == null && itemDTO.Owner == user.UserName) // uusi itemi
            {
                return true;
            }

            return user.Id == dbItem.Owner.Id;
        }

        public async Task<bool> isAllowed(string? username, User user)
        {
            User? dbUser = await _context.Users.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync();

            return dbUser.UserName == username;
        }

        public async Task<bool> isAllowed(string? username, UserDTO userDTo)
        {
            User? dbUser = await _context.Users.Where(x => x.UserName == userDTo.UserName).FirstOrDefaultAsync();

            return dbUser.UserName == username;
        }

        public async Task<bool> isAllowed(string? username, ReservationDTO reservationDTO)
        {
            User? user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            Reservation? dbReservation = await _context.Reservations.Include(i => i.Owner).FirstOrDefaultAsync(i => i.Id == reservationDTO.Id);

            return user.Id == dbReservation.Owner.Id;
        }
    }
}
