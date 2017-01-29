namespace iCoffe.Shared
{
    public class Offer: OfferInfo
    {
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int AvailableAmount { get; set; }
    }
}
