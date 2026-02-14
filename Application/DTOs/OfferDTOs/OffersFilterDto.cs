using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.OfferDTOs
{
    public class OffersFilterDto
    {
        
        public string? SearchText { get; set; } 
        public DateTime? SearchDate { get; set; } 
        public bool? IsActiveNow { get; set; }
        public decimal? MinDiscount { get; set; }
        public decimal? MaxDiscount { get; set; } 
    }
}
