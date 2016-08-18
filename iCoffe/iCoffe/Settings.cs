using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace iCoffe.Shared
{
    public static class Settings
    {
        public static string HostUrl { get { return @"http://geolocwebapi.azurewebsites.net/"; } }
        public static string TokenPath { get { return @"Token"; } }
        public static string UserInfoPath { get { return @"Account/MobileApplicationUserInfo"; } }

        public static string ApiUrl { get { return HostUrl + @"api"; } }
        public static string RegisterPath { get { return @"Account/Register"; } }

        public static string OffersPath { get { return @"Bonus/offersSel"; } }
        public static string BonusOffersPath { get { return @"BonusOffer/{latitude}/{longitude}/{radius}"; } }

    }
}
