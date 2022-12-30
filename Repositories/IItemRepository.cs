using ReservationSystem22.Models;

namespace ReservationSysten22.Repositories
{
    public interface IItemRepository
    {
        public Task<Item> GetItemAsync(long id);
        public Task<IEnumerable<Item>> GetItemsAsync();
        public Task<IEnumerable<Item>> GetItemsAsync(User user);
        public Task<IEnumerable<Item>> QueryItemsAsync(string query);
        public Task<Item> AddItemAsync(Item item);
        public Task<Item> UpdateItemAsync(Item item);
        public Task<Boolean> DeleteItemAsync(Item item);
        public Task<Boolean> ClearImagesAsync(Item item);
    } 
}
