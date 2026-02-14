namespace Presentation.ViewModels.OfferVMs
{
    public class OffersFilterVM
    {
        public string? SearchText { get; set; }
        public DateTime? SearchDate { get; set; }
        public bool? IsActiveNow { get; set; }
        public decimal? MinDiscount { get; set; }
        public decimal? MaxDiscount { get; set; }
    }
}
