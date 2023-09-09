using HotelReservation.Exceptions;
using HotelReservation.Services.ReservationConflictValidators;
using HotelReservation.Services.ReservationCreators;
using HotelReservation.Services.ReservationProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.Models
{
    public class ReservationBook
    {
        private readonly IReservationProvider _reservationProvider;
        private readonly IReservationCreator _reservationCreator;
        private readonly IReservationConflictValidator _reservationConflictValidator;
        private IEnumerable<Reservation> _reservations;

        public ReservationBook(IReservationProvider reservationProvider, IReservationCreator reservationCreator, IReservationConflictValidator reservationConflictValidator)
        {
            _reservationProvider=reservationProvider;
            _reservationCreator=reservationCreator;
            _reservationConflictValidator=reservationConflictValidator;
        }

        /// <summary>
        /// Get all reservations.
        /// </summary>
        /// <returns>All reservations in the reservation book</returns>
        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            return await _reservationProvider.GetAllReservations();
        }

        /// <summary>
        /// Add a reservation to the reservation book
        /// </summary>
        /// <param name="reservation">The incoming reservation.</param>
        /// <exception cref="ReservationConflictException"></exception>
        public async Task AddReservation(Reservation reservation) 
        {
            Reservation conflictingReservation = await _reservationConflictValidator.GetConflictingReservation(reservation);

            if (conflictingReservation != null) 
            {
                throw new ReservationConflictException(conflictingReservation, reservation);

            }
            
            await _reservationCreator.CreateReservation(reservation);
        }
    }
}
