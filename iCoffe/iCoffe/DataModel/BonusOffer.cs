namespace iCoffe.Shared
{
	public class BonusOffer
	{
		public GUID Id { get; set; }
		public int Scheme_Id { get; set; }
		public int Obj_Id { get; set; }

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

