using NuGet.Protocol.Core.Types;
using ReservationSystem22.Models;
using ReservationSysten22.Repositories;

namespace ReservationSysten22.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;

        public ReservationService(IReservationRepository reservationRepository, IItemRepository itemRepository, IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }

        public async Task<ReservationDTO> CreateReservationAsync(ReservationDTO dto)
        {
            if (dto.StartTime >= dto.EndTime || dto.EndTime <= dto.StartTime)
            {
                return null;
            }

            Item target = await _itemRepository.GetItemAsync(dto.Target);

            if (target == null)
            {
                return null;
            }

            IEnumerable<Reservation> reservations = await _reservationRepository.GetReservationsAsync(target, dto.StartTime, dto.EndTime);

            if (reservations.Count() > 0)
            {
                return null;
            }

            Reservation reservation = await DTOToReservation(dto);
            Reservation created = await _reservationRepository.AddReservationAsync(reservation);
            return ReservationToDTO(created);
        }

        public async Task<bool> DeleteReservationAsync(ReservationDTO dto)
        {
            Reservation reservation = await _reservationRepository.GetReservationAsync(dto.Id);
            return await _reservationRepository.DeleteReservationAsync(reservation);
        }

        public async Task<ReservationDTO> GetReservationAsync(long id)
        {
            Reservation reservation = await _reservationRepository.GetReservationAsync(id);
            return ReservationToDTO(reservation);
        }

        public async Task<IEnumerable<ReservationDTO>> GetReservationsAsync()
        {
            IEnumerable<Reservation> reservations = await _reservationRepository.GetReservationsAsync();
            List<ReservationDTO> reservationDTOs = new List<ReservationDTO>();

            foreach (Reservation reservation in reservations)
            {
                reservationDTOs.Add(ReservationToDTO(reservation));
            }

            return reservationDTOs;
        }

        public async Task<ReservationDTO> UpdateReservationAsync(ReservationDTO reservationDTO)
        {
            Reservation reservation = await DTOToReservation(reservationDTO);
            Reservation updatedReservation = await _reservationRepository.UpdateReservationAsync(reservation);
            return ReservationToDTO(updatedReservation);
        }

        private ReservationDTO ReservationToDTO(Reservation reservation)
        {
            ReservationDTO dto = new ReservationDTO();
            dto.Id = reservation.Id;

            if (reservation.Owner != null)
            {
                dto.Owner = reservation.Owner.UserName;
            }

            if (reservation.Target != null)
            {
                dto.Target = reservation.Target.Id;
            }

            dto.StartTime = reservation.StartTime;
            dto.EndTime = reservation.EndTime;

            return dto;
        }

        private async Task<Reservation> DTOToReservation(ReservationDTO dto)
        {
            Reservation reservation = new Reservation();

            // Hae kannasta
            User owner = await _userRepository.GetUserAsync(dto.Owner);
            Item target = await _itemRepository.GetItemAsync(dto.Target);

            if (owner == null)
            {
                return null;
            }

            if (target == null)
            {
                return null;
            }

            reservation.Id = dto.Id;
            reservation.Owner = owner;
            reservation.Target = target;
            reservation.StartTime = dto.StartTime;
            reservation.EndTime = dto.EndTime;

            return reservation;
        }
    }
}
