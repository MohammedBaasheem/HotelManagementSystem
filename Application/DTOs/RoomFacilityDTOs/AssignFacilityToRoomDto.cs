using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.RoomFacilityDTOs
{
    public class AssignFacilityToRoomDto
    {
        public Guid RoomId { get; set; }
        public Guid FacilityId { get; set; }
    }
}
