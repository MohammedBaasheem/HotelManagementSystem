using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Models
{
    public class Reservation : BaseModel
    {
        public string ReservationNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public short NumberOfAdults { get; set; }
        public short NumberOfChildren { get; set; }
        public ReservationStatus Status { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid FeedbackId { get; set; }
        public Feedback Feedback { get; set; }

        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public HashSet<RoomReservation> RoomReservations { get; set; }
    }
}