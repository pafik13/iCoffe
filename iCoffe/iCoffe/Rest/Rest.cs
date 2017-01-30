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
        }

        public static HttpStatusCode SignUp(string email, string password, string confirm)
        {
            var client = new RestClient(Consts.ApiUrl);
            var request = new RestRequest(Consts.RegisterPath, Method.POST);
            request.AddParameter(@"Email", email);
            request.AddParameter(@"Password", password);
            request.AddParameter(@"ConfirmPassword", confirm);
            var response = client.Execute(request);
            return response.StatusCode;
        }

        public static string GetAccessToken(string username, string password)
        {
            var client = new RestClient(Consts.HostUrl);
            var request = new RestRequest(Consts.TokenPath, Method.POST);
            request.AddParameter(@"username", username);
            request.AddParameter(@"password", password);
            request.AddParameter(@"grant_type", @"password");
            var response = client.Execute<AccessRecord>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data.access_token;
            }

            return string.Empty;
        }

        public static async Task<AccountInfoResponse> GetAccountInfoAsync(string bearer)
        {
            var result = new AccountInfoResponse
            {
                User = new UserInfo() { Id = @"<нет данных>", Login = @"<нет данных>", PointsAmount = -1 },
                Purchases = new List<Offer>()
            };
            var client = new RestClient(Consts.ApiUrl);
            var request = new RestRequest(Consts.AccountInfoPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Bearer {0}", bearer));
            var response = await client.ExecuteTaskAsync<AccountInfoResponse>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = response.Data;
            }
            return result;
        }

        public static async Task<List<Offer>> GetOffersAsync(string bearer, double latitude, double longitude, int radius = 100, int limit = 100)
        {
            List<Offer> results = new List<Offer>();
            var client = new RestClient(Consts.ApiUrl);
            var request = new RestRequest(Consts.OffersPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Bearer {0}", bearer));
            request.AddUrlSegment(@"latitude", latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"longitude", longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"radius", radius.ToString());
            request.AddUrlSegment(@"limit", limit.ToString());
            var response = await client.ExecuteGetTaskAsync<OfferResponse>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                results = response.Data.Items;
            }
            return results;
        }

        public static async Task<List<PlaceInfo>> GetPlaceInfosAsync(string bearer, double latitude, double longitude, int radius = 100, int limit = 100)
        {
            List<PlaceInfo> results = new List<PlaceInfo>();
            var client = new RestClient(Consts.ApiUrl);
            var request = new RestRequest(Consts.PlacesPath, Method.GET);
            request.AddHeader(@"Authorization", string.Format(@"Bearer {0}", bearer));
            request.AddUrlSegment(@"latitude", latitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"longitude", longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")));
            request.AddUrlSegment(@"radius", radius.ToString());
            request.AddUrlSegment(@"limit", limit.ToString());
            var response = await client.ExecuteGetTaskAsync<PlaceInfoResponse>(request);
            if (response.StatusCode == HttpStatusCode.OK && response.Data.Items != null)
            {
                results = response.Data.Items;
            }
            return results;
        }

        public static bool BuyOffer(string bearer, int offerId, int amount = 1)
        {
            var client = new RestClient(Consts.ApiUrl);
            var request = new RestRequest(Consts.OfferBuyPath, Method.POST);
            request.AddHeader(@"Authorization", string.Format(@"Bearer {0}", bearer));
            request.AddParameter(@"OfferId", offerId);
            request.AddParameter(@"Amount", amount);
            var response = client.Execute(request);
            return (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent);
        }


		// http://stackoverflow.com/questions/14296644/how-to-execute-a-method-periodically-from-wpf-client-application-using-threading/14297203#14297203
		// The `onTick` method will be called periodically unless cancelled.
		public static async Task RunPeriodicAsync(Action onTick, TimeSpan dueTime, TimeSpan interval,
		                                   System.Threading.CancellationToken token)
		{
			// Initial wait time before we begin the periodic loop.
			if (dueTime > TimeSpan.Zero)
				await Task.Delay(dueTime, token);

			// Repeat this loop until cancelled.
			while (!token.IsCancellationRequested)
			{
				// Call our onTick function.
				onTick?.Invoke();

				// Wait to repeat again.
				if (interval > TimeSpan.Zero)
					await Task.Delay(interval, token);
			}
		}

        public static async Task<List<Place>> GetPlacesAsync(string bearer, int[] ids)
        {
            List<Place> results = new List<Place>();
            var client = new RestClient(Consts.ApiUrl);
            foreach (var id in ids)
            {
                var request = new RestRequest(Consts.PlaceInfoPath, Method.GET);
                request.AddHeader(@"Authorization", string.Format(@"Bearer {0}", bearer));
                request.AddUrlSegment(@"id", id.ToString(CultureInfo.CreateSpecificCulture("en-GB")));

                var response = await client.ExecuteTaskAsync<PlaceResponse>(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    results.Add(response.Data.Place);
                }
            }
            return results;
        }
    }
}
