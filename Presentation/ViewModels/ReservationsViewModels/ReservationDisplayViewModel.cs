using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels.ReservationsViewModels
{
    public class ReservationDisplayViewModel
    {
        public string ReservationNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public string Status { get; set; }
    }
}