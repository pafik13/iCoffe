using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using RestSharp;
using RestSharp.Deserializers;
using System.IO;
using System.Xml.Serialization;

namespace iCoffe.Shared
{
    public static class Rest
    {
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

        public static List<BonusOffer> GetBonuses(int latitude, int longitude, int radius)
        {
            List<BonusOffer> results = new List<BonusOffer>();
            var client = new RestClient(Settings.HostUrl);
            var request = new RestRequest(Settings.OffersPath, Method.Get); // api/Bonus/offersSel
            request.AddQueryParameter(@"plat", latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddQueryParameter(@"plong", longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddQueryParameter(@"dmax", radius.ToString());
            var response = client.Execute<List<BonusOffer>>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (response.Data != null) 
                {
                    results = response.Data;
                }
            }
            return results;
        }
    }
}
