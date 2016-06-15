using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

using RestSharp;

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

        //public static List<BonusOffer> GetBonuses(int latitude, int longitude, int radius)
        //{
        //    List<BonusOffer> results = new List<BonusOffer>();
        //    var client = new RestClient(Settings.HostUrl);
        //    var request = new RestRequest(Settings.OffersPath, Method.Get); // api/Bonus/offersSel
        //    request.AddQueryParameter(@"plat", latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
        //    request.AddQueryParameter(@"plong", longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
        //    request.AddQueryParameter(@"dmax", radius.ToString());
        //    var response = client.Execute<List<BonusOffer>>(request);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        if (response.Data != null) 
        //        {
        //            results = response.Data;
        //        }
        //    }
        //    return results;
        //}

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
