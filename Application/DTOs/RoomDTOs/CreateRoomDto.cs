using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.RoomDTOs
{
    public class CreateRoomDto
    {
        public string Code { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public int FloorNumber { get; set; }
        public string? RoomDescription { get; set; }
        public RoomTypes RoomType { get; set; }
        public Guid UserId { get; set; }
    }
}
