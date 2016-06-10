using System;
using System.Collections.Generic;
using System.Text;

namespace iCoffe.Shared
{
    public static class Settings
    {
        public static string HostUrl { get { return @"http://geolocwebapi.azurewebsites.net/"; } }
        public static string TokenPath { get { return @"Token"; } }
        public static string ApiUrl { get { return HostUrl + @"api"; } }
        public static string RegisterPath { get { return @"Account/Register"; } }
    }
}
