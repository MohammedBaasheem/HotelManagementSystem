using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.RoomDTOs
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public int FloorNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public string? RoomDescription { get; set; }
        public RoomTypes RoomType { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
