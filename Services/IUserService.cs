using ReservationSystem22.Models;

namespace ReservationSysten22.Services
{
    public interface IUserService
    {
        public Task<UserDTO> CreateUserAsync(User user);
        public Task<UserDTO> GetUserAsync(long id);
        public Task<IEnumerable<UserDTO>> GetUsersAsync();
        public Task<UserDTO> UpdateUserAsync(User user);
        public Task<Boolean> DeletUserAsync(long id);
    }
}
