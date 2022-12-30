using Microsoft.EntityFrameworkCore;
using ReservationSystem22.Models;

namespace ReservationSysten22.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ReservationContext _context;

        public ItemRepository(ReservationContext context)
        {
            _context = context;
        }

        public async Task<Item> AddItemAsync(Item item)
        {
            _context.Items.Add(item);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return null;
            }

            return item;
        }

        public async Task<bool> ClearImagesAsync(Item item)
        {
            if (item.Images == null)
            {
                return false;
            }

            foreach (Image image in item.Images)
            {
                _context.Images.Remove(image);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteItemAsync(Item item)
        {
            _context.Items.Remove(item);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<Item> GetItemAsync(long id)
        {
            return await _context.Items.Include(i => i.Images).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await _context.Items.Include(i => i.Images).ToListAsync();

        }

        public async Task<IEnumerable<Item>> GetItemsAsync(User user)
        {
            return await _context.Items.Include(i => i.Owner).Where(x => x.Owner == user).ToListAsync();
        }

        public async Task<IEnumerable<Item>> QueryItemsAsync(string query)
        {
            return await _context.Items.Include(i => i.Owner).Where(x => x.Name.Contains(query)).ToListAsync();
        }

        public async Task<Item> UpdateItemAsync(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }

            return item;
        }
    }
}
