using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.RoomFacilityDTOs
{
    public class RoomFacilityDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid FacilityId { get; set; }
        public bool IsActive { get; set; }
    }
}
