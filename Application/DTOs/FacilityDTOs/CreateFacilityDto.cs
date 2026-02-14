using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.FacilityDTOs
{
    public class CreateFacilityDto
    {
        public string FacilityName { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Price { get; set; }
    }
}
