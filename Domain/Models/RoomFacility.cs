using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RoomFacility : BaseModel
    {
        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public Guid FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;
    }
}