using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem22.Models;
using ReservationSysten22.Middleware;
using ReservationSysten22.Services;

namespace ReservationSysten22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _service;
        private readonly IUserAuthenticationService _authenticationService;

        public ItemsController(IItemService service, IUserAuthenticationService authenticationService)
        {
            _service = service;
            _authenticationService = authenticationService;
        }

        // GET: api/Items
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return Ok(await _service.GetItemsAsync());
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Item>> GetItem(long id)
        {
            ItemDTO item = await _service.GetItemAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutItem(long id, ItemDTO itemDTO)
        {
            if (id != itemDTO.Id)
            {
                return BadRequest();
            }

            // Tarkista oikeudet
            bool isAllowed = await _authenticationService.isAllowed(this.User.FindFirst(ClaimTypes.Name).Value, itemDTO);

            if (!isAllowed)
            {
                return Unauthorized();
            }

            ItemDTO updatedItem = await _service.UpdateItemAsync(itemDTO);

            if (updatedItem == null)
            {
                return Problem();
            }

            return Ok(updatedItem);
        }

        // POST: api/Items
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ItemDTO>> PostItem(ItemDTO itemDTO)
        {
            // Tarkista oikeudet
            bool isAllowed = await _authenticationService.isAllowed(this.User.FindFirst(ClaimTypes.Name).Value, itemDTO);   

            if (!isAllowed)
            {
                return Unauthorized();
            }

            ItemDTO newDTO = await _service.CreateItemAsync(itemDTO);

            if (newDTO == null)
            {
                return Problem();
            }

            return CreatedAtAction("GetItem", new { id = newDTO.Id }, newDTO);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteItem(long id)
        {
            // Tarkista oikeudet
            ItemDTO itemDTO = new ItemDTO();
            itemDTO.Id = id;

            bool isAllowed = await _authenticationService.isAllowed(this.User.FindFirst(ClaimTypes.Name).Value, itemDTO);

            if (!isAllowed)
            {
                return Unauthorized();
            }

            bool deleted = await _service.DeleteItemAsync(id);

            if (deleted)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
