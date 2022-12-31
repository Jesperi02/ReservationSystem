using Microsoft.EntityFrameworkCore;
using ReservationSystem22.Models;

namespace ReservationSysten22.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ReservationContext _context;

        public ReservationRepository(ReservationContext context)
        {
            _context = context;
        }

        public async Task<Reservation> AddReservationAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return null;
            }

            return reservation;
        }

        public async Task<bool> DeleteReservationAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);

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

        public async Task<Reservation> GetReservationAsync(long id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsAsync()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsAsync(Item target, DateTime start, DateTime end)
        {
            return await _context.Reservations.Include(i => i.Owner).Include(x => x.Target).Where(x =>
                x.Target == target && 
                x.StartTime <= end && 
                x.EndTime >= start
            ).ToListAsync();
        }

        public async Task<Reservation> UpdateReservationAsync(Reservation reservation)
        {
            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }

            return reservation;
        }
    }
}
