using ReservationSystem22.Models;

namespace ReservationSysten22.Services
{
    public interface IItemService
    {
        public Task<ItemDTO> CreateItemAsync(ItemDTO dto);
        public Task<ItemDTO> GetItemAsync(long id);
        public Task<IEnumerable<ItemDTO>> GetItemsAsync();
        public Task<ItemDTO> UpdateItemAsync(ItemDTO item);
        public Task<Boolean> DeleteItemAsync(long id);
    }
}
