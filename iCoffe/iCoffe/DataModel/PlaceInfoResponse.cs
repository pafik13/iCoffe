using System.Collections.Generic;

namespace tutCoffee.Shared
{
    public class PlaceInfoResponse
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float Radius { get; set; }
        public List<PlaceInfo> Items { get; set; }
    }
}
