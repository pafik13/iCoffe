namespace iCoffe.Shared
{
	public class GeolocObj
	{
		public int Id { get; set; }
		public GeolocPoint geoloc { get; set; }
		public int group_Id { get; set; }

        // Info
        public string Adress { get; set; }
        public string Label { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
		public string ImageURL { get; set; }

        public GeolocObj ()
		{
		}
	}
}

