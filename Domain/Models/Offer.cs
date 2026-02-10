using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Offer : BaseModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndtDate { get; set; }
        public decimal DiscountValue { get; set; }

        public HashSet<RoomOffer> RoomOffer { get; set; } = new HashSet<RoomOffer>();
    }
}