using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.ResrvationDTOs
{
    public class AddRoomToReservationDTO
    {
        public Guid RoomId { get; set; }
        public Guid OfferId { get; set; }
    }
}