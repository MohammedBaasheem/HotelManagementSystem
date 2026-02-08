using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Room : BaseModel
    {
        public string Code { get; set; } = null!;
        public int FloorNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public string? RoomDescription { get; set; }

        public Guid RoomTypeId { get; set; }

        public HashSet<RoomFacility> RoomFacilities { get; set; } = new HashSet<RoomFacility>();


    }
}