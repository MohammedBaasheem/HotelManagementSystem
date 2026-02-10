using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RoomOffer : BaseModel
    {
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public Guid OfferId { get; set; }
        public Offer Offer { get; set; }
    }
}