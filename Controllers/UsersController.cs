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
    public class UsersController : ControllerBase
    {
        private readonly IUserAuthenticationService _authenticationService;
        private readonly IUserService _service;


        public UsersController(IUserAuthenticationService authenticationService, IUserService service)
        {
            _authenticationService = authenticationService;
            _service = service;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _service.GetUsersAsync());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            UserDTO user = await _service.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            // Tarkista oikeudet
            string username = this.User.FindFirst(ClaimTypes.Name).Value;
            bool isAllowed = await _authenticationService.isAllowed(username, user);

            if (!isAllowed)
            {
                return Unauthorized();
            }

            UserDTO returnedUser = await _service.UpdateUserAsync(user);

            if (returnedUser == null)
            {
                return NotFound();
            }

            return Ok(returnedUser);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            UserDTO userDTO = await _service.CreateUserAsync(user);

            if (userDTO == null)
            {
                return Problem();
            }

            return CreatedAtAction(nameof(PostUser), new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(long id)
        {
            // Tarkista oikeudet
            string username = this.User.FindFirst(ClaimTypes.Name).Value;
            UserDTO userDTO = await _service.GetUserAsync(id);
            bool isAllowed = await _authenticationService.isAllowed(username, userDTO);

            if (!isAllowed)
            {
                return Unauthorized();
            }

            bool deleted = await _service.DeletUserAsync(id);
            if (deleted)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
