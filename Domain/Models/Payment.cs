using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Payment : BaseModel
    {
        public int RoomReservationId { get; set; }   // FK
                                                     // public RoomReservation RoomReservation { get; set; }

        public enum PaymentMethod { get; set; }     // FK


        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime PaymentDate { get; set; }

        public bool IsCompleted { get; set; }

        public Invoice Invoice { get; set; }


    }
}