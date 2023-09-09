using HotelReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.Exceptions
{
    public class ReservationConflictException : Exception
    {
        public Reservation ExistingReservation { get; }
        public Reservation IncomingReservation { get; }
        public ReservationConflictException(Reservation existingReservation, Reservation incomingReservation)
        {
            ExistingReservation = existingReservation;
            IncomingReservation = incomingReservation;
        }
        public ReservationConflictException(string message, Reservation existingReservation, Reservation incomingReservation) : base(message)
        {
            ExistingReservation = existingReservation;
            IncomingReservation = incomingReservation;
        }
        public ReservationConflictException(string message, Exception innerExcaption, Reservation existingReservation, Reservation incomingReservation) : base(message, innerExcaption)
        {
            ExistingReservation = existingReservation;
            IncomingReservation = incomingReservation;
        }

       /* public ReservationConflictException(string message) : base(message)
        {
        }
        public ReservationConflictException(string message, Exception innerExcaption) : base(message, innerExcaption)
        {
        }
        public ReservationConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
       */

    }
}

