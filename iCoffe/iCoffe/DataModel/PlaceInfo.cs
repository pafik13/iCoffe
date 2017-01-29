namespace iCoffe.Shared
{
    public class PlaceInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Geolocation GeoLocation { get; set; }

        public PlaceInfo()
        {

        }

        public PlaceInfo(int id, string name, Geolocation geolocation)
        {
            Id = id;
            Name = name;
            GeoLocation = geolocation;
        }
    }
}
