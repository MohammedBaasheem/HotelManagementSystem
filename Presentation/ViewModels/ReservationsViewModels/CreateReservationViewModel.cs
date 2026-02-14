using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels.ReservationsViewModels
{
    public class CreateReservationViewModel
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public short NumberOfAdults { get; set; }
        public short NumberOfChildren { get; set; }
        public List<RoomSelectionViewModel> SelectedRooms { get; set; }
    }
}