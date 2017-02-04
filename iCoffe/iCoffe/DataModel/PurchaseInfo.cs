namespace tutCoffee.Shared
{
    public class PurchaseInfo
    {
        public int OfferTransactionId { get; set; }

        public int Amount { get; set; }

        public OfferInfo Offer { get; set; }
    }
}
