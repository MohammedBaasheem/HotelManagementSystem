namespace Presentation.ViewModels.OfferVMs
{
    public class GetOfferVM
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DiscountValue { get; set; }

        public bool IsActive { get; set; }
        public List<Guid> RoomIds { get; set; } = new List<Guid>();
    }
}
