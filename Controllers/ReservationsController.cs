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
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;
        private readonly IUserAuthenticationService _authenticationService;

        public ReservationsController(IReservationService service, IUserAuthenticationService authenticationService)
        {
            _service = service;
            _authenticationService = authenticationService;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return Ok(await _service.GetReservationsAsync());
        }

        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(long id)
        {
            ReservationDTO reservation = await _service.GetReservationAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(long id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest();
            }

            // Tarkista oikeudet
            ReservationDTO reservationDTO = await _service.GetReservationAsync(reservation.Id);
            bool isAllowed = await _authenticationService.isAllowed(this.User.FindFirst(ClaimTypes.Name).Value, reservationDTO);

            if (!isAllowed)
            {
                return Unauthorized();
            }

            ReservationDTO updatedReservation = await _service.UpdateReservationAsync(reservationDTO);

            if (updatedReservation == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReservationDTO>> PostReservation(ReservationDTO reservationDTO)
        {
            // Tarkista oikeudet
            bool isAllowed = await _authenticationService.isAllowed(this.User.FindFirst(ClaimTypes.Name).Value, reservationDTO);

            if (!isAllowed)
            {
                return Unauthorized();
            }

            ReservationDTO createdDTO = await _service.CreateReservationAsync(reservationDTO);

            if (createdDTO == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetItem", new { id = createdDTO.Id }, createdDTO);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(long id)
        {
            // Tarkista oikeudet
            ReservationDTO reservationDTO = new ReservationDTO();
            reservationDTO.Id = id;

            bool isAllowed = await _authenticationService.isAllowed(this.User.FindFirst(ClaimTypes.Name).Value, reservationDTO);

            if (!isAllowed)
            {
                return Unauthorized();
            }

            if (await _service.DeleteReservationAsync(reservationDTO))
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
