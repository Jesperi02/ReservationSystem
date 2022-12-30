using NuGet.Protocol.Core.Types;
using ReservationSystem22.Models;
using ReservationSysten22.Repositories;
using System.Collections.Generic;

namespace ReservationSysten22.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;

        public ItemService(IItemRepository repository, IUserRepository userRepository)
        {
            _itemRepository = repository;
            _userRepository = userRepository;
        }

        public async Task<ItemDTO> CreateItemAsync(ItemDTO itemDTO)
        {
            Item item = await DTOToItem(itemDTO);
            Item updatedItem = await _itemRepository.AddItemAsync(item);

            if (updatedItem == null)
            {
                return null;
            }

            return ItemToDTO(updatedItem);
        }

        public async Task<bool> DeleteItemAsync(long id)
        {
            Item item = await _itemRepository.GetItemAsync(id);

            if (item == null)
            {
                return false;
            }

            await _itemRepository.ClearImagesAsync(item);
            return await _itemRepository.DeleteItemAsync(item);
        }

        public async Task<IEnumerable<ItemDTO>> GetItemsAsync()
        {
            IEnumerable<Item> itemList = await _itemRepository.GetItemsAsync();
            List<ItemDTO> itemDTOList = new List<ItemDTO>();

            foreach(Item item in itemList)
            {
                itemDTOList.Add(ItemToDTO(item));
            }
 
            return itemDTOList;
        }

        public async Task<IEnumerable<ItemDTO>> QueryItemsAsync(string query)
        {
            IEnumerable<Item> itemList = await _itemRepository.QueryItemsAsync(query);
            List<ItemDTO> itemDTOList = new List<ItemDTO>();

            foreach(Item item in itemList)
            {
                itemDTOList.Add(ItemToDTO(item));
            }

            return itemDTOList;
        }

        public async Task<ItemDTO> GetItemAsync(long id)
        {
            Item item = await _itemRepository.GetItemAsync(id);

            if (item == null)
            {
                return null;
            }

            item.accessCount++;
            await _itemRepository.UpdateItemAsync(item);

            return ItemToDTO(item);
        }

        public async Task<IEnumerable<ItemDTO>> GetItemsAsync(string username)
        {
            User owner = await _userRepository.GetUserAsync(username);

            if (owner == null)
            {
                return null;
            }

            IEnumerable<Item> itemList = await _itemRepository.GetItemsAsync(owner);
            List<ItemDTO> itemDTOList = new List<ItemDTO>();

            foreach (Item item in itemList)
            {
                itemDTOList.Add(ItemToDTO(item));
            }

            return itemDTOList;
        }

        public async Task<ItemDTO> UpdateItemAsync(ItemDTO itemDTO)
        {
            Item item = await _itemRepository.GetItemAsync(itemDTO.Id);
            
            if (item == null)
            {
                return null;
            }

            if (itemDTO.Name.Length > 0)
            {
                item.Name = itemDTO.Name;
            }

            if (itemDTO.Description.Length > 0)
            {
                item.Description = itemDTO.Description;
            }

            item.accessCount++;

            if (itemDTO.Images != null)
            {
                if (item.Images != null)
                {
                    await _itemRepository.ClearImagesAsync(item);
                }

                item.Images = new List<Image>();
                foreach (ImageDTO imageDTO in itemDTO.Images)
                {
                    Image image = DTOToImage(imageDTO);
                    image.Target = item;
                    item.Images.Add(image);
                }
            }

            Item updatedItem = await _itemRepository.UpdateItemAsync(item);

            return ItemToDTO(updatedItem);
        }

        private ItemDTO ItemToDTO(Item item)
        {
            ItemDTO dto = new ItemDTO();
            dto.Id = item.Id;
            dto.Name = item.Name;
            dto.Description = item.Description;

            if (item.Owner != null)
            {
                dto.Owner = item.Owner.UserName;
            }

            if (item.Images != null)
            {
                dto.Images = new List<ImageDTO>();
                foreach (Image image in item.Images)
                {
                    ImageDTO imageDTO = ImageToDTO(image);
                    dto.Images.Add(imageDTO);
                }
            }

            return dto;
        }

        private async Task<Item> DTOToItem(ItemDTO dto)
        {
            Item item = new Item();
            item.Id = dto.Id;
            item.Name = dto.Name;
            item.Description = dto.Description;

            // Hae kannasta
            User owner = await _userRepository.GetUserAsync(dto.Owner);

            if (owner != null)
            {
                item.Owner = owner;
            }

            if (dto.Images != null)
            {
                item.Images = new List<Image>();
                foreach (ImageDTO imageDTO in dto.Images)
                {
                    Image image = DTOToImage(imageDTO);
                    item.Images.Add(image);
                }
            }

            item.accessCount = 0;
            return item;
        }

        private Image DTOToImage(ImageDTO imageDTO)
        {
            Image image = new Image();
            image.Url = imageDTO.Url;
            image.Description = imageDTO.Description;
            return image;
        }

        private ImageDTO ImageToDTO(Image image)
        {
            ImageDTO imageDTO = new ImageDTO();
            imageDTO.Url = image.Url;
            imageDTO.Description = image.Description;
            return imageDTO;
        }
    }
}
