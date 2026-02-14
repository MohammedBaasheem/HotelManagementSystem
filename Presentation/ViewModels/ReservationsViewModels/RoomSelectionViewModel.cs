using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels.ReservationsViewModels
{
    public class RoomSelectionViewModel
    {
        public Guid RoomId { get; set; }
        public Guid OfferId { get; set; } = Guid.Empty;
    }
}