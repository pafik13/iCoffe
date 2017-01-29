namespace iCoffe.Shared
{
    public class Geolocation
    {

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude { get; set; }

        public Geolocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Geolocation()
        {

        }
    }
}
