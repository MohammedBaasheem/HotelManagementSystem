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

        public PaymentMethod PaymentMethod { get; set; }    


        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime PaymentDate { get; set; }

        public bool IsCompleted { get; set; }

        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }


    }
}