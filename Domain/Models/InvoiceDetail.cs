using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class InvoiceDetail : BaseModel
    {
        public Guid InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public int Nights { get; set; }

        public decimal PricePerNight { get; set; }

        public decimal TotalPrice { get; set; }

    }
}