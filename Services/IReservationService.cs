using ReservationSystem22.Models;

namespace ReservationSysten22.Services
{
    public interface IReservationService
    {
        public Task<ReservationDTO> CreateReservationAsync(ReservationDTO dto);
        public Task<ReservationDTO> GetReservationAsync(long id);
        public Task<IEnumerable<ReservationDTO>> GetReservationsAsync();
        public Task<ReservationDTO> UpdateReservationAsync(ReservationDTO dto);
        public Task<Boolean> DeleteReservationAsync(ReservationDTO dto);
    }
}
