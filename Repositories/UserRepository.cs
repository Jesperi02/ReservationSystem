using Microsoft.EntityFrameworkCore;
using ReservationSystem22.Models;

namespace ReservationSysten22.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ReservationContext _context;

        public UserRepository(ReservationContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return null;
            }

            return user;
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
;           
            return true;
        }

        public async Task<User> GetUserAsync(string userName)
        {
            User user = await _context.Users.Where(x =>
                x.UserName == userName
            ).FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetUserAsync(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return user;
        }

        private bool UserExists(User user)
        {
            return _context.Users.Any(e => e.Id == user.Id);
        }
    }
}
