using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

using RestSharp;
using System.Threading.Tasks;

namespace iCoffe.Shared
{
    public static class Rest
    {
        public const int C_TIME_OUT = 180000;
        public static DateTime LastRecivingData { set; get; }

        public class AccessRecord
        {
            public string access_token { set; get; }
            public string token_type { set; get; }
            public int expires_in { set; get; }

            public static string Serialize(AccessRecord accessRecord)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AccessRecord));

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, accessRecord);
                    return textWriter.ToString();
                }
            }

            public static AccessRecord Deserialize(string accessRecord)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(AccessRecord));

                using (TextReader reader = new StringReader(accessRecord))
                {
                    return (AccessRecord)xmlSerializer.Deserialize(reader);
                }
            }
        }

        public static HttpStatusCode SignUp(string email, string password, string confirm)
        {
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.RegisterPath, Method.POST);
            request.AddParameter(@"Email", email);
            request.AddParameter(@"Password", password);
            request.AddParameter(@"ConfirmPassword", confirm);
            var response = client.Execute(request);
            return response.StatusCode;
        }


        public static string GetAccessToken(string username, string password)
        {
            var client = new RestClient(Settings.HostUrl);
            var request = new RestRequest(Settings.TokenPath, Method.POST);
            request.AddParameter(@"username", username);
            request.AddParameter(@"password", password);
            request.AddParameter(@"grant_type", @"password");
            var response = client.Execute<AccessRecord>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                AccessRecord ar = response.Data;
                //return AccessRecord.Serialize(ar);
                return ar.access_token;
            }

            return string.Empty;
        }
        

        public static List<BonusOffer> GetBonuses(string bearer, double latitude, double longitude, int radius)
        {
            List<BonusOffer> results = new List<BonusOffer>();
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.OffersPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Bearer {0}", bearer));
            request.AddQueryParameter(@"plat", latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddQueryParameter(@"plong", longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddQueryParameter(@"dmax", radius.ToString());
            var response = client.Execute<List<BonusOffer>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Data != null)
                {
                    results = response.Data;
                }
            }
            return results;
        }
        
        
        public static string GetBasicToken(string username, string password)
        {
            var authData = string.Format("{0}:{1}", username, password);
            var authHeaderValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(authData));

            if (GetUserInfo(authHeaderValue)) {
                return authHeaderValue;
            }
            return string.Empty;
        }

        public static bool GetUserInfo(string basic)
        {
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.UserInfoPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
            var response = client.Execute<UserInfo>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Data.UserInfo = response.Data;
                return true;
            }
            return false;
        }

        public static async Task<UserInfo> GetUserInfoAsync(string basic)
        {
            UserInfo result = new UserInfo() {FullUserName = @"<нет данных>", Login = @"<нет данных>", Points = -1 };
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.UserInfoPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
            var response = await client.ExecuteTaskAsync<UserInfo>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = response.Data;
            }
            return result;
        }

        public static List<BonusOffer> GetBonusOffers(string basic, double latitude, double longitude, int radius)
        {
            List<BonusOffer> results = new List<BonusOffer>();
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.BonusOffersPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
            request.AddUrlSegment(@"latitude", latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"longitude", longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"radius", radius.ToString());
            var response = client.Execute<List<BonusOffer>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Data;
            }
            return results;
        }

        public static async Task<List<BonusOffer>> GetBonusOffersAsync(string basic, double latitude, double longitude, int radius)
        {
            List<BonusOffer> results = new List<BonusOffer>();
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.BonusOffersPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
            request.AddUrlSegment(@"latitude", latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"longitude", longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"radius", radius.ToString());
            var response = await client.ExecuteGetTaskAsync<List<BonusOffer>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Data;
                    
            }
            return results;
        }

        public static List<BonusOffer> GetUserBonusOffers(string basic)
        {
            List<BonusOffer> results = new List<BonusOffer>();
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.UserBonusOffersPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
            var response = client.Execute<List<BonusOffer>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Data;
            }
            return results;
        }

        public static async Task<List<BonusOffer>> GetUserBonusOffersAsync(string basic)
        {
            List<BonusOffer> results = new List<BonusOffer>();
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.UserBonusOffersPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
            var response = await client.ExecuteTaskAsync<List<BonusOffer>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Data;
            }
            return results;
        }

        public static List<Cafe> GetCafes(string basic, double latitude, double longitude, int radius)
        {
            List<Cafe> results = new List<Cafe>();
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.CafesPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
            request.AddUrlSegment(@"latitude", latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"longitude", longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"radius", radius.ToString());
            var response = client.Execute<List<Cafe>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Data;
                foreach (var cafe in results)
                {
                    request = new RestRequest(Settings.GeoLocationPath, Method.GET);
                    request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
                    request.AddUrlSegment(@"id", cafe.GeoLocationId.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
                    var responseGeoLocation = client.Execute<GeoLocation>(request);
                    if (responseGeoLocation.StatusCode == HttpStatusCode.OK) {
                        cafe.GeoLocation = responseGeoLocation.Data;
                    }
                };
            }
            return results;
        }

        public static async Task<List<Cafe>> GetCafesAsync(string basic, double latitude, double longitude, int radius)
        {
            List<Cafe> results = new List<Cafe>();
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.CafesPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
            request.AddUrlSegment(@"latitude", latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"longitude", longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"radius", radius.ToString());
            var response = await client.ExecuteTaskAsync<List<Cafe>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Data;
                foreach (var cafe in results)
                {
                    request = new RestRequest(Settings.GeoLocationPath, Method.GET);
                    request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
                    request.AddUrlSegment(@"id", cafe.GeoLocationId.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
                    var responseGeoLocation = await client.ExecuteTaskAsync<GeoLocation>(request);
                    if (responseGeoLocation.StatusCode == HttpStatusCode.OK)
                    {
                        cafe.GeoLocation = responseGeoLocation.Data;
                    }
                };
            }
            return results;
        }

        public static async Task<List<Cafe>> GetCafesAsync(string basic, int [] ids)
        {
            List<Cafe> results = new List<Cafe>();
            var client = new RestClient(Settings.ApiUrl);
            foreach (var id in ids)
            {
                var request = new RestRequest(Settings.CafePath, Method.GET);
                request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
                request.AddUrlSegment(@"id", id.ToString(CultureInfo.CreateSpecificCulture("en-GB")));

                var response = await client.ExecuteTaskAsync<Cafe>(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var cafe = response.Data;

                    request = new RestRequest(Settings.GeoLocationPath, Method.GET);
                    request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
                    request.AddUrlSegment(@"id", cafe.GeoLocationId.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
                    var responseGeoLocation = await client.ExecuteTaskAsync<GeoLocation>(request);
                    if (responseGeoLocation.StatusCode == HttpStatusCode.OK)
                    {
                        cafe.GeoLocation = responseGeoLocation.Data;
                    }
                    results.Add(cafe);
                }
            }
            return results;
        }

        public static bool RequestOffer(string basic, Guid id)
        {
            var client = new RestClient(Settings.ApiUrl);
            var request = new RestRequest(Settings.BonusOfferRequestPath, Method.POST);
            request.AddHeader(@"Authorization", string.Format(@"Basic {0}", basic));
            request.AddUrlSegment(@"id", id.ToString());
            var response = client.Execute(request);
            return (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent);
        }

        public static bool GetObjects(int latitude, int longitude, int radius)
        {
            if ( (DateTime.Now - LastRecivingData).TotalMilliseconds > C_TIME_OUT)
            {
                LastRecivingData = DateTime.Now;
                var client = new RestClient(@"http://geolocwebapi.azurewebsites.net/");
                //client.Default
                var request = new RestRequest(@"api/Objs/Sel", Method.GET);
                //request.AddQueryParameter(@"plat", latitude.ToString());
                //request.AddQueryParameter(@"plong", longitude.ToString());
                //request.AddQueryParameter(@"dmax", radius.ToString());
                request.AddQueryParameter(@"plat", "54.974362");
                request.AddQueryParameter(@"plong", "73.418061");
                request.AddQueryParameter(@"dmax", radius.ToString());
                request.AddQueryParameter(@"grId", 1.ToString());
                request.AddQueryParameter(@"pgn", null);
                request.AddQueryParameter(@"pgs", null);
                var response = client.Execute<List<GeolocObj>>(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Data.Objs = response.Data;
                    //for(int i = 0; i < Data.Objs.Count; i++)
                    foreach (var obj in Data.Objs)
                    {
                        request = new RestRequest(@"api/Objs/SelComplex", Method.GET);
                        request.AddQueryParameter(@"objId", obj.Id.ToString());
                        var response2 = client.Execute<GeolocComplexObj>(request);
                        Data.ComplexObjs.Add(response2.Data);
                    }
                }
            }

            return true;
        }
    }
}
