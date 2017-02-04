using System.Collections.Generic;

namespace tutCoffee.Shared
{
    public class OfferResponse
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float Radius { get; set; }
        public List<Offer> Items { get; set; }
    }
}
