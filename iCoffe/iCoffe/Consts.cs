namespace tutCoffee.Shared
{
    public static class Consts
    {
        public const string HostUrl = "http://stockserveradmin.azurewebsites.net/";
        public const string TokenPath = "token";

        public readonly static string ApiUrl = string.Concat(HostUrl, "api/v1/");

        public const string AccountInfoPath = "Account/Info";
        public const string RegisterPath = "Account/Register";

        public const string OffersPath = "Offer/GetByPoint?lat={latitude}&lon={longitude}&radius={radius}&limit={limit}";
        public const string OfferBuyPath = "Offer/Buy";

        public const string PlacesPath = "Place/GetByPoint?lat={latitude}&lon={longitude}&radius={radius}&limit={limit}";
        public const string PlaceInfoPath = "Place/Info/{id}";
    }
}
