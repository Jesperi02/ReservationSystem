using ReservationSystem22.Models;

namespace ReservationSysten22.Repositories
{
    public interface IReservationRepository
    {
        public Task<Reservation> GetReservationAsync(long id);
        public Task<IEnumerable<Reservation>> GetReservationsAsync();
        public Task<Reservation> AddReservationAsync(Reservation reservation);
        public Task<Reservation> UpdateReservationAsync(Reservation reservation);
        public Task<Boolean> DeleteReservationAsync(Reservation reservation);
    }
}
