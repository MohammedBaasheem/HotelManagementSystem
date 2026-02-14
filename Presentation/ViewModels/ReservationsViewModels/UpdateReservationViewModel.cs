using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels.ReservationsViewModels
{
    public class UpdateReservationViewModel
    {
        // We don't need the ID here if we get it from the URL route
        
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        // [Range(1, 10, ErrorMessage = "Adults must be between 1 and 10")]
        public short? NumberOfAdults { get; set; }

        // [Range(0, 10, ErrorMessage = "Children cannot exceed 10")]
        public short? NumberOfChildren { get; set; }
    }
}