using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Facility : BaseModel
    {
        public string FacilityName { get; set; } = null!;
        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public HashSet<RoomFacility> RoomFacilities { get; set; } = new HashSet<RoomFacility>();


    }
}