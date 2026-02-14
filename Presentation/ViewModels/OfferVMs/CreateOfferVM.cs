namespace Presentation.ViewModels.OfferVMs
{
    public class CreateOfferVM
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DiscountValue { get; set; }

        public List<Guid> RoomIds { get; set; }
    }
}
