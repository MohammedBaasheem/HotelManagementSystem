using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.ResrvationDTOs
{
    public class AddResrvationDTO
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public short NumberOfAdults { get; set; }
        public short NumberOfChildren { get; set; }
        public Guid UserId { get; set; }
        
        // The collection of rooms to be booked
        public List<AddRoomToReservationDTO> SelectedRooms { get; set; }
    }
}