using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using SDiag = System.Diagnostics;

using Android.App;
using Android.Content;
//using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Net;
using Android.Locations;

using UniversalImageLoader.Core;

using iCoffe.Shared;
using System.Threading.Tasks;

namespace iCoffe.Droid
{
    [Activity(Label = "MainActivity")]
    public class MainActivity : Activity, ILocationListener
	{
        // Consts
        public const string C_WAS_STARTED_NEW_ACTIVITY = @"C_WAS_STARTED_NEW_ACTIVITY";
        public const string C_IS_USER_SIGN_IN = @"C_IS_USER_SIGN_IN";
        public const string C_DEFAULT_PREFS = @"I_COFFEE";
        public const string C_ACCESS_TOKEN = @"ACCESS_TOKEN";
        public const string C_IS_NEED_TUTORIAL = @"C_IS_NEED_TUTORIAL";
        public const string C_IS_BACK_PRESSED_IN_SIGN_IN = @"C_IS_BACK_PRESSED_IN_SIGN_IN";

        public const string C_OFFER_ID = @"C_OFFER_ID";


        // Layouts
        LinearLayout OffersTab;
        LinearLayout MapTab;
        LinearLayout AccountTab;

        // Views
        Fragment offers = null;
        Fragment map = null;
        Fragment account = null;

        // Location
        LocationManager LocMgr;
        bool IsLocationFound = false;
        string defaultPlace = string.Empty;
        double latitude;
        double longitude;
        int radius = 100;

        // Intermedia
        ProgressDialog ProgressDialog;

        // Data Service
        private CancellationTokenSource CTSData;
        private CancellationToken CTData;

        // User Desc
        private CancellationTokenSource CTSUserInfo;
        private CancellationToken CTUserInfo;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            var config = ImageLoaderConfiguration.CreateDefault(ApplicationContext);
            // Initialize ImageLoader with configuration.
            ImageLoader.Instance.Init(config);

            RequestWindowFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            OffersTab = FindViewById<LinearLayout>(Resource.Id.maBonusTabLL);
            OffersTab.Click += BonusTab_Click;

            MapTab = FindViewById<LinearLayout>(Resource.Id.maMapTabLL);
            MapTab.Click += MapTab_Click; ;

            AccountTab = FindViewById<LinearLayout>(Resource.Id.maUserTabLL);
            AccountTab.Click += UserTab_Click; ;
        }

        void OnTick()
        {
            // TODO: Your code here
            SDiag.Debug.Print("OnTick. Date: {0}, Thread: {1}", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
            if (account != null)
            {
                string accessToken = GetSharedPreferences(C_DEFAULT_PREFS, FileCreationMode.Private).GetString(C_ACCESS_TOKEN, string.Empty);
                SDiag.Debug.Print("accessToken " + accessToken);
                //Rest.GetUserInfo(accessToken);
                RunOnUiThread(() =>
                {
                    (account as Fragments.AccountFragment).RefreshUserInfo();
                });
            }

        }

        protected override void OnResume()
        {
            base.OnResume();
            SDiag.Debug.Print("OnResume called");

			if (ProgressDialog != null)
			{
				ProgressDialog.Dismiss();
			}
			
            if (CTUserInfo != null && CTUserInfo.CanBeCanceled && CTSUserInfo != null)
            {
                CTSUserInfo.Cancel();
            }

            CTSUserInfo = new CancellationTokenSource();
            CTUserInfo = CTSUserInfo.Token;

            var dueTime = TimeSpan.FromSeconds(5);
            var interval = TimeSpan.FromSeconds(5);

            // TODO: Add a CancellationTokenSource and supply the token here instead of None.
            var startPooling = Rest.RunPeriodicAsync(OnTick, dueTime, interval, CTUserInfo);

            ///
            /// Main code
            ///
            if (CTData != null && CTData.CanBeCanceled && CTSData != null)
            {
                CTSData.Cancel();
            }
            var sharedPreferences = GetSharedPreferences(C_DEFAULT_PREFS, FileCreationMode.Private);
            bool isBackPressedInSignIn = sharedPreferences.GetBoolean(C_IS_BACK_PRESSED_IN_SIGN_IN, false);

            if (isBackPressedInSignIn)
            {
                sharedPreferences.Edit().PutBoolean(C_IS_BACK_PRESSED_IN_SIGN_IN, false).Apply();

                Finish();
                return;
            }

            string accessToken = sharedPreferences.GetString(C_ACCESS_TOKEN, string.Empty);
            bool isNeedTutorial = sharedPreferences.GetBoolean(C_IS_NEED_TUTORIAL, true);

            if (string.IsNullOrEmpty(accessToken))
            {
                StartActivity(new Intent(this, typeof(SignInActivity)));
                return;
            }

            if (!Intent.GetBooleanExtra(C_WAS_STARTED_NEW_ACTIVITY, false))
            {
                if (account != null)
                {
                    RunOnUiThread(() =>
                    {
                        (account as Fragments.AccountFragment).RefreshUserInfo();
                    });
                }

                if (isNeedTutorial)
                {
                    StartActivity(new Intent(this, typeof(TutorialActivity)));
                    return;
                }

                if (IsInternetActive() && IsLocationActive())
                {

                    var locationCriteria = new Criteria();
                    locationCriteria.Accuracy = Accuracy.Coarse;
                    locationCriteria.PowerRequirement = Power.Medium;
                    string locationProvider = LocMgr.GetBestProvider(locationCriteria, true);
                    SDiag.Debug.Print("Starting location updates with " + locationProvider.ToString());
                    LocMgr.RequestLocationUpdates(locationProvider, 2000, 1, this);

                    // Progress
                    string message = @"Определение местоположения...";
                    ProgressDialog = ProgressDialog.Show(this, @"", message, true);

                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        Thread.Sleep(30000);

                        RunOnUiThread(() =>
                        {
                            if (!IsLocationFound)
                            {
                                if (ProgressDialog != null)
                                {
                                    ProgressDialog.Dismiss();
                                }
                            }
                        });
                    });
                }
            }
        }

        private async Task GetPlacesAndOffersAsync(CancellationToken cancellationToken, double lat, double lon, int rad)
        {
            SDiag.Debug.Print("GetPlacesAndOffersAsync started. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

            string message = string.IsNullOrEmpty(defaultPlace) ? @"Получение данных..." : "Получение данных. В качестве основной точки используется: " + defaultPlace;
            ProgressDialog = ProgressDialog.Show(this, @"", message, true);

            SDiag.Debug.Print("Radius " + radius.ToString());
            string accessToken = GetSharedPreferences(C_DEFAULT_PREFS, FileCreationMode.Private).GetString(C_ACCESS_TOKEN, string.Empty);
            SDiag.Debug.Print("accessToken " + accessToken);
            Data.Offers = new List<Offer>();
            Data.Places = new List<Place>();
            Data.PlaceInfos = new List<PlaceInfo>();
            Data.UserInfo = new UserInfo() { Id = @"<нет данных>", Login = @"<нет данных>", PointsAmount = -1 };
            Data.UserPurchasedOffers = new List<Offer>();

            LoadFragments(lat, lon);

			
			if (cancellationToken.IsCancellationRequested)
            {
                // do something here as task was cancelled mid flight maybe just
                return;
            }
			
            var offers = await Rest.GetOffersAsync(accessToken, lat, lon);
            SDiag.Debug.Print("GetOffersAsync running. Offers. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

			if (cancellationToken.IsCancellationRequested)
            {
                // do something here as task was cancelled mid flight maybe just
                return;
            }
			
            var placeInfos = await Rest.GetPlaceInfosAsync(accessToken, lat, lon, rad);
            SDiag.Debug.Print("GetPlaceInfosAsync running. Cafes Thread: {0}", Thread.CurrentThread.ManagedThreadId);

            var offrersPlacesIds = offers.Select(o => o.PlaceId).Distinct().ToArray();
            var places = await Rest.GetPlacesAsync(accessToken, offrersPlacesIds);

            if (cancellationToken.IsCancellationRequested)
            {
                // do something here as task was cancelled mid flight maybe just
                return;
            }
			
            var accountInfo = await Rest.GetAccountInfoAsync(accessToken);
            SDiag.Debug.Print("GetAccountInfoAsync running. UserInfo. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

            //var userBonuses = await Rest.GetUserBonusOffersAsync(accessToken);
            //SDiag.Debug.Print("GetCafesAndBonusOffers running. UserBonusOffer. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

            //var userBonusesCafeIds = userBonuses.Select(i => i.CafeId).Distinct().ToArray();
            //var cafesIds = places.Select(i => i.Id).Distinct().ToArray();
            //var userCafes = await Rest.GetCafesAsync(accessToken, userBonusesCafeIds.Except(cafesIds).ToArray());
            //places.AddRange(userCafes);
            //SDiag.Debug.Print("GetCafesAndBonusOffers running. Cafes for UserBonusOffer. Thread: {0}", Thread.CurrentThread.ManagedThreadId);


            if (cancellationToken.IsCancellationRequested)
            {
                // do something here as task was cancelled mid flight maybe just
                return;
            }

            Data.Offers = offers;
            Data.PlaceInfos = placeInfos;
            Data.Places = places;
            Data.UserInfo = accountInfo.User;
            Data.UserPurchasedOffers = accountInfo.Purchases;
            
            LoadFragments(lat, lon);
            MapTab_Click(MapTab, EventArgs.Empty);

            RunOnUiThread(() => {
                if (ProgressDialog != null)
                {
                    ProgressDialog.Dismiss();
                }
            });

            SDiag.Debug.Print("GetPlacesAndOffersAsync stopped. Thread: {0}", Thread.CurrentThread.ManagedThreadId);
        }

        private bool IsLocationActive()
        {
            LocMgr = GetSystemService(LocationService) as LocationManager;

            if ( LocMgr.IsProviderEnabled(LocationManager.NetworkProvider)
              || LocMgr.IsProviderEnabled(LocationManager.GpsProvider)
               )
            {
                return true;
            }
            else
            {
                new AlertDialog.Builder(this)
                            .SetTitle(Resource.String.warning_caption)
                            .SetMessage(Resource.String.no_location_provider)
                            .SetCancelable(false)
                            .SetPositiveButton(Resource.String.on_button, (sender, args) => {
                                var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                                StartActivity(intent);
                            }).SetNegativeButton(Resource.String.cancel_button, (sender, args) => {
                                (sender as Dialog).Dismiss();
                                defaultPlace = @"центр Омска";
                                latitude = 54.974362;
                                longitude = 73.418061;
                                radius = 4;
                                //GetCafesAndBonusOffers();
                            }).Show();

                return false;
            }
        }

        private bool IsInternetActive()
        {
            ConnectivityManager cm = GetSystemService(Context.ConnectivityService) as ConnectivityManager;
            if (cm.ActiveNetworkInfo != null)
            {
                if (cm.ActiveNetworkInfo.IsConnectedOrConnecting)
                {
                    return true;
                }
                else
                {
                    new AlertDialog.Builder(this)
                                .SetTitle(Resource.String.error_caption)
                                .SetMessage(Resource.String.no_internet_connection)
                                .SetCancelable(false)
                                .SetNegativeButton(Resource.String.cancel_button, (sender, args) => {
                                    (sender as Dialog).Dismiss();
                                }).Show();

                    return false;
                }
            }
            else
            {
                new AlertDialog.Builder(this)
                            .SetTitle(Resource.String.warning_caption)
                            .SetMessage(Resource.String.no_internet_provider)
                            .SetCancelable(false)
                            .SetPositiveButton(Resource.String.on_button, (sender, args) => {
                                var intent = new Intent(Android.Provider.Settings.ActionWirelessSettings);
                                StartActivity(intent);
                            })
                            .SetNegativeButton(Resource.String.cancel_button, (sender, args) => {
					            (sender as Dialog).Dismiss();
                            }).Show();
				
                return false;
            }
        }
        
        private void LoadFragments(double lat, double lon)
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            if (account == null)
            {
                account = new Fragments.AccountFragment();
                trans.Add(Resource.Id.mContentFL, account);
            }
            else
            {
                RunOnUiThread(() =>
                {
                    (account as Fragments.AccountFragment).RefreshUserInfo();
                    (account as Fragments.AccountFragment).RecreateAdapter();
                    (account as Fragments.AccountFragment).MoveCamera(new Android.Gms.Maps.Model.LatLng(lat, lon));
                });
            }

            if (map == null)
            {
                map = new Fragments.MapFragment();
                trans.Add(Resource.Id.mContentFL, map);
            }
            else
            {
                RunOnUiThread(() => {
                    (map as Fragments.MapFragment).RecreateMarkers();
                    //(map as Fragments.MapFragment).MoveCamera(new Android.Gms.Maps.Model.LatLng(lat, lon));
                });
            }

            if (offers == null)
            {
                offers = new Fragments.OffersFragment();
                trans.Add(Resource.Id.mContentFL, offers);
            }
            else
            {
                RunOnUiThread(() => (offers as Fragments.OffersFragment).RecreateAdapter());
            }
            trans.Commit();
            if (!MapTab.Selected && !OffersTab.Selected && !AccountTab.Selected)
            {
                MapTab_Click(MapTab, null);
            }
        }

        private void UserTab_Click(object sender, EventArgs e)
        {
            FragmentManager.BeginTransaction().Hide(offers).Hide(map).Show(account).Commit();
            RunOnUiThread(() => {
                MapTab.Selected = false;
                OffersTab.Selected = false;
                AccountTab.Selected = true;
                (account as Fragments.AccountFragment).RefreshUserInfo();
            });
        }

        private void MapTab_Click(object sender, EventArgs e)
        {
            FragmentManager.BeginTransaction().Hide(offers).Hide(account).Show(map).Commit();
            RunOnUiThread(() => { 
                MapTab.Selected = true;
                OffersTab.Selected = false;
                AccountTab.Selected = false;
            });
        }

        private void BonusTab_Click(object sender, EventArgs e)
        {
            FragmentManager.BeginTransaction().Hide(map).Hide(account).Show(offers).Commit();
            RunOnUiThread(() => {
                MapTab.Selected = false;
                OffersTab.Selected = true;
                AccountTab.Selected = false;
            });
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (CTUserInfo.CanBeCanceled && CTSUserInfo != null)
            {
                CTSUserInfo.Cancel();
            }

            if (CTData != null && CTData.CanBeCanceled && CTSData != null)
            {
                CTSData.Cancel();
            }

            if (ProgressDialog != null)
            {
                ProgressDialog.Dismiss();
            }

            // stop sending location updates when the application goes into the background
            // to learn about updating location in the background, refer to the Backgrounding guide
            // http://docs.xamarin.com/guides/cross-platform/application_fundamentals/backgrounding/


            // RemoveUpdates takes a pending intent - here, we pass the current Activity
            if (LocMgr != null)
            {
                LocMgr.RemoveUpdates(this);
            }

            SDiag.Debug.Print("Location updates paused because application is entering the background");
        }

        protected override void OnStop()
        {
            base.OnStop();
            SDiag.Debug.Print("OnStop called");
        }

        public override void OnBackPressed()
        {
            if (Intent.GetBooleanExtra(C_IS_USER_SIGN_IN, false))
            {
                MoveTaskToBack(true);
            }
            base.OnBackPressed();
        }

        #region LOCATION
        public void OnLocationChanged(Location location)
        {
            SDiag.Debug.Print("Location changed");
            SDiag.Debug.Print("Latitude: " + location.Latitude.ToString());
            SDiag.Debug.Print("Longitude: " + location.Longitude.ToString());
            SDiag.Debug.Print("Provider: " + location.Provider.ToString());

            IsLocationFound = true;
            latitude = location.Latitude;
            longitude = location.Longitude;
            LocMgr.RemoveUpdates(this);
            ProgressDialog.Hide();
            radius = 5;
            CTSData = new CancellationTokenSource();
            CTData = CTSData.Token;
            var task = GetPlacesAndOffersAsync(CTData, location.Latitude, location.Longitude, 40);
        }
        public void OnProviderDisabled(string provider)
        {
            SDiag.Debug.Print(provider + " disabled by account");
        }
        public void OnProviderEnabled(string provider)
        {
            SDiag.Debug.Print(provider + " enabled by account");
        }
        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            SDiag.Debug.Print(provider + " availability has changed to " + status.ToString());
        }
		#endregion
    }
}


