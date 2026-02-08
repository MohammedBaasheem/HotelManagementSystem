using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RoomReservation : BaseModel
    {
        public short numberOfNights { get; set; }
        public decimal pricePerNight { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DepositeAmount { get; set; }
        public decimal totalPrice { get; set; }

        public Guid RoomId { get; set; }
        public Room Room { get; set; }

        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public Guid OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}