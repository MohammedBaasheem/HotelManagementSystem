using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.ResrvationDTOs
{
    public class UpdateReservationDTO
    {
    public Guid Id { get; set; }               // ← required

    public DateTime? CheckInDate     { get; set; }
    public DateTime? CheckOutDate    { get; set; }
    public short?    NumberOfAdults  { get; set; }
    public short?    NumberOfChildren{ get; set; }
    }
}