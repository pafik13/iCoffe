namespace iCoffe.Shared
{
    public static class Settings
    {
        public static string HostUrl { get { return @"https://geolocwebapi.azurewebsites.net/"; } }
        public static string TokenPath { get { return @"Token"; } }
        public static string UserInfoPath { get { return @"Account/MobileApplicationUserInfo"; } }
        public static string UserBonusOffersPath { get { return @"Account/UserBonus"; } }

        public static string ApiUrl { get { return HostUrl + @"api"; } }
        public static string RegisterPath { get { return @"Account/Register"; } }

        public static string OffersPath { get { return @"Bonus/offersSel"; } }
        public static string BonusOffersPath { get { return @"BonusOffer/{latitude}/{longitude}/{radius}"; } }
        //public static string BonusOffersPath { get { return @"BonusOffer/{longitude}/{latitude}/{radius}"; } }

        public static string BonusOfferRequestPath { get { return @"BonusOffer/RequestOffer/{id}"; } }

        public static string CafesPath { get { return @"Cafe/{latitude}/{longitude}/{radius}"; } }
        //public static string CafesPath { get { return @"Cafe/{longitude}/{latitude}/{radius}"; } }
        public static string CafePath { get { return @"Cafe/{id}"; } }

        public static string GeoLocationPath { get { return @"GeoLocation/{id}"; } }

    }
}
