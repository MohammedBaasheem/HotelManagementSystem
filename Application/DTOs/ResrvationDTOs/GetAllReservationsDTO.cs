using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.DTOs.ResrvationDTOs
{
    public class GetAllReservationsDTO
    {
        public string ReservationNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public short NumberOfAdults { get; set; }
        public short NumberOfChildren { get; set; }
        public ReservationStatus Status { get; set; }
    }
}