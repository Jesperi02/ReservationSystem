using ReservationSystem22.Models;

namespace ReservationSysten22.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUserAsync(String userName);
        public Task<User> GetUserAsync(long id);
        public Task<User> AddUserAsync(User user);
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<User> UpdateUserAsync(User user);
        public Task<Boolean> DeleteUserAsync(User user);
    }
}
