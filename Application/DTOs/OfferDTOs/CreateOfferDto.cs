using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.OfferDTOs
{
    public class CreateOfferDto
    { 
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DiscountValue { get; set; }

        public List<Guid> RoomIds { get; set; }
    }
}
