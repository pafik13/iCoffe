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

        public const string C_BONUS_ID = @"C_BONUS_ID";


        // Layouts
        LinearLayout BonusTab;
        LinearLayout MapTab;
        LinearLayout UserTab;

        // Views
        Fragment bonus = null;
        Fragment map = null;
        Fragment user = null;

        // Location
        LocationManager locMgr;
        bool isLocationFound = false;
        string defaultPlace = string.Empty;
        double latitude;
        double longitude;
        int radius = 2;

        // Intermedia
        AlertDialog.Builder builder;
        Dialog dialog;
        ProgressDialog progressDialog;

        // Data Service
        private CancellationTokenSource cancelSource;
        private CancellationToken cancelToken;

        // User Desc
        CancellationTokenSource CSUserDesc;
        CancellationToken CTUserDesc;

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
            BonusTab = FindViewById<LinearLayout>(Resource.Id.maBonusTabLL);
            BonusTab.Click += BonusTab_Click;

            MapTab = FindViewById<LinearLayout>(Resource.Id.maMapTabLL);
            MapTab.Click += MapTab_Click; ;

            UserTab = FindViewById<LinearLayout>(Resource.Id.maUserTabLL);
            UserTab.Click += UserTab_Click; ;
        }

        protected override void OnResume()
        {
            base.OnResume();
            SDiag.Debug.Print("OnResume called");


            if (CTUserDesc.CanBeCanceled && CSUserDesc != null)
            {
                CSUserDesc.Cancel();
            }

            CSUserDesc = new CancellationTokenSource();
            CTUserDesc = CSUserDesc.Token;

            var dueTime = TimeSpan.FromSeconds(5);
            var interval = TimeSpan.FromSeconds(5);

            // TODO: Add a CancellationTokenSource and supply the token here instead of None.
            Rest.RunPeriodicAsync(OnTick, dueTime, interval, CTUserDesc);

            ///
            /// Main code
            ///
            if (cancelToken != null && cancelToken.CanBeCanceled && cancelSource != null)
            {
                cancelSource.Cancel();
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
                if (user != null)
                {
                    RunOnUiThread(() =>
                    {
                        (user as Fragments.UserFragment).RefreshUserInfo();
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
                    string locationProvider = locMgr.GetBestProvider(locationCriteria, true);
                    SDiag.Debug.Print("Starting location updates with " + locationProvider.ToString());
                    locMgr.RequestLocationUpdates(locationProvider, 2000, 1, this);

                    // Progress
                    string message = @"Определение местоположения...";
                    progressDialog = ProgressDialog.Show(this, @"", message, true);

                    //progressDialog.Show();
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        Thread.Sleep(30000);

                        RunOnUiThread(() =>
                        {
                            if (!isLocationFound)
                            {
                                if (progressDialog != null)
                                {
                                    progressDialog.Dismiss();
                                }
                            }
                        });
                    });
                }
            }
        }

        void OnTick()
        {
            // TODO: Your code here
            SDiag.Debug.Print("OnTick. Date: {0}, Thread: {1}", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
            if (user != null)
            {
                string accessToken = GetSharedPreferences(C_DEFAULT_PREFS, FileCreationMode.Private).GetString(C_ACCESS_TOKEN, string.Empty);
                SDiag.Debug.Print("accessToken " + accessToken);
                Rest.GetUserInfo(accessToken);
                RunOnUiThread(() =>
                {
                    (user as Fragments.UserFragment).RefreshUserInfo();
                });
            }

        }

        private void GetCafesAndBonusOffers()
        {
            /* throw new NotImplementedException();*/
            string message = string.IsNullOrEmpty(defaultPlace) ? @"Получение данных..." : "Получение данных. В качестве основной точки используется: " + defaultPlace;
            progressDialog = ProgressDialog.Show(this, @"", message, true);

            ThreadPool.QueueUserWorkItem(state =>
            {
                SDiag.Debug.Print("Radius " + radius.ToString());
                string accessToken = GetSharedPreferences(C_DEFAULT_PREFS, FileCreationMode.Private).GetString(C_ACCESS_TOKEN, string.Empty);
                SDiag.Debug.Print("accessToken " + accessToken);
                Data.BonusOffers = Rest.GetBonusOffers(accessToken, 54.974362, 73.418061, 10);
                Data.Cafes = Rest.GetCafes(accessToken, 54.974362, 73.418061, 10);
                Data.UserBonusOffers = Rest.GetUserBonusOffers(accessToken);

                //LoadFragments();
                MapTab_Click(MapTab, EventArgs.Empty);

                RunOnUiThread(() => {
                    if (progressDialog != null) {
                        progressDialog.Dismiss();
                    }
                });

                SDiag.Debug.Print("GetCafesAndBonusOffers stopped.");
            });
        }

        private async Task GetCafesAndBonusOffersAsync(CancellationToken cancellationToken, double lat, double lon, int rad)
        {
            SDiag.Debug.Print("GetCafesAndBonusOffers started. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

            string message = string.IsNullOrEmpty(defaultPlace) ? @"Получение данных..." : "Получение данных. В качестве основной точки используется: " + defaultPlace;
            progressDialog = ProgressDialog.Show(this, @"", message, true);

            SDiag.Debug.Print("Radius " + radius.ToString());
            string accessToken = GetSharedPreferences(C_DEFAULT_PREFS, FileCreationMode.Private).GetString(C_ACCESS_TOKEN, string.Empty);
            SDiag.Debug.Print("accessToken " + accessToken);
            Data.BonusOffers = new List<BonusOffer>();
            Data.Cafes = new List<Cafe>();
            Data.UserInfo = new UserInfo() { FullUserName = @"<нет данных>", Login = @"<нет данных>", Points = -1 };
            Data.UserBonusOffers = new List<BonusOffer>();

            LoadFragments(lat, lon);

            var offers = await Rest.GetBonusOffersAsync(accessToken, lat, lon, rad);
            SDiag.Debug.Print("GetCafesAndBonusOffers running. Offers. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

            var cafes = await Rest.GetCafesAsync(accessToken, lat, lon, rad);
            SDiag.Debug.Print("GetCafesAndBonusOffers running. Cafes Thread: {0}", Thread.CurrentThread.ManagedThreadId);

            var offrersCafeIds = offers.Select(i => i.CafeId).Distinct().ToArray();
            var offrersCafes = await Rest.GetCafesAsync(accessToken, offrersCafeIds);
            cafes.AddRange(offrersCafes);

            var userInfo = await Rest.GetUserInfoAsync(accessToken);
            SDiag.Debug.Print("GetCafesAndBonusOffers running. UserInfo. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

            var userBonuses = await Rest.GetUserBonusOffersAsync(accessToken);
            SDiag.Debug.Print("GetCafesAndBonusOffers running. UserBonusOffer. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

            var userBonusesCafeIds = userBonuses.Select(i => i.CafeId).Distinct().ToArray();
            var cafesIds = cafes.Select(i => i.Id).Distinct().ToArray();
            var userCafes = await Rest.GetCafesAsync(accessToken, userBonusesCafeIds.Except(cafesIds).ToArray());
            cafes.AddRange(userCafes);
            SDiag.Debug.Print("GetCafesAndBonusOffers running. Cafes for UserBonusOffer. Thread: {0}", Thread.CurrentThread.ManagedThreadId);


            if (cancellationToken.IsCancellationRequested)
            {
                // do something here as task was cancelled mid flight maybe just
                return;
            }

            Data.BonusOffers = offers;
            Data.Cafes = cafes;
            Data.UserInfo = userInfo;
            Data.UserBonusOffers = userBonuses;
            
            LoadFragments(lat, lon);
            MapTab_Click(MapTab, EventArgs.Empty);

            RunOnUiThread(() => {
                if (progressDialog != null)
                {
                    progressDialog.Dismiss();
                }
            });

            SDiag.Debug.Print("GetCafesAndBonusOffers stopped. Thread: {0}", Thread.CurrentThread.ManagedThreadId);
        }

        private bool IsLocationActive()
        {
            /*throw new NotImplementedException();*/
            locMgr = GetSystemService(LocationService) as LocationManager;

            if ( locMgr.IsProviderEnabled(LocationManager.NetworkProvider)
              || locMgr.IsProviderEnabled(LocationManager.GpsProvider)
               )
            {
                return true;
            }
            else
            {
                builder = new AlertDialog.Builder(this);
                builder.SetTitle(Resource.String.warning_caption);
                builder.SetMessage(Resource.String.no_location_provider);
                builder.SetCancelable(false);
                builder.SetPositiveButton(Resource.String.on_button, delegate {
                    var intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                    StartActivity(intent);
                });

                builder.SetNegativeButton(Resource.String.cancel_button, delegate {
                    dialog.Dismiss();
                    defaultPlace = @"центр Омска";
                    latitude = 54.974362;
                    longitude = 73.418061;
                    radius = 4;
                    GetCafesAndBonusOffers();
                });

                dialog = builder.Show();

                return false;
            }
        }

        private bool IsInternetActive()
        {
            /*throw new NotImplementedException();*/
            ConnectivityManager cm = GetSystemService(Context.ConnectivityService) as ConnectivityManager;
            if (cm.ActiveNetworkInfo != null)
            {
                if (cm.ActiveNetworkInfo.IsConnectedOrConnecting)
                {
                    return true;
                }
                else
                {
                    builder = new AlertDialog.Builder(this);
                    builder.SetTitle(Resource.String.error_caption);
                    builder.SetMessage(Resource.String.no_internet_connection);
                    builder.SetCancelable(false);
                    builder.SetNegativeButton(Resource.String.cancel_button, delegate {
                        dialog.Dismiss();
                    });

                    dialog = builder.Show();
                    return false;
                }
            }
            else
            {
                builder = new AlertDialog.Builder(this);
                builder.SetTitle(Resource.String.warning_caption);
                builder.SetMessage(Resource.String.no_internet_provider);
                builder.SetCancelable(false);
                builder.SetPositiveButton(Resource.String.on_button, delegate {
                    var intent = new Intent(Android.Provider.Settings.ActionWirelessSettings);
                    StartActivity(intent);
                });

                builder.SetNegativeButton(Resource.String.cancel_button, delegate {
                    dialog.Dismiss();
                });

                dialog = builder.Show();
                return false;
            }
        }
        
        private void LoadFragments(double lat, double lon)
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            if (user == null)
            {
                user = new Fragments.UserFragment();
                trans.Add(Resource.Id.mContentFL, user);
            }
            else
            {
                RunOnUiThread(() =>
                {
                    (user as Fragments.UserFragment).RefreshUserInfo();
                    (user as Fragments.UserFragment).RecreateAdapter();
                    (user as Fragments.UserFragment).MoveCamera(new Android.Gms.Maps.Model.LatLng(lat, lon));
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

            if (bonus == null)
            {
                bonus = new Fragments.BonusFragment();
                trans.Add(Resource.Id.mContentFL, bonus);
            }
            else
            {
                RunOnUiThread(() => (bonus as Fragments.BonusFragment).RecreateAdapter());
            }
            trans.Commit();
            if (!MapTab.Selected && !BonusTab.Selected && !UserTab.Selected)
            {
                MapTab_Click(MapTab, null);
            }
        }

        private void UserTab_Click(object sender, EventArgs e)
        {
            FragmentManager.BeginTransaction().Hide(bonus).Hide(map).Show(user).Commit();
            RunOnUiThread(() => {
                MapTab.Selected = false;
                BonusTab.Selected = false;
                UserTab.Selected = true;
                (user as Fragments.UserFragment).RefreshUserInfo();
            });
        }

        private void MapTab_Click(object sender, EventArgs e)
        {
            FragmentManager.BeginTransaction().Hide(bonus).Hide(user).Show(map).Commit();
            RunOnUiThread(() => { 
                MapTab.Selected = true;
                BonusTab.Selected = false;
                UserTab.Selected = false;
            });
        }

        private void BonusTab_Click(object sender, EventArgs e)
        {
            FragmentManager.BeginTransaction().Hide(map).Hide(user).Show(bonus).Commit();
            RunOnUiThread(() => {
                MapTab.Selected = false;
                BonusTab.Selected = true;
                UserTab.Selected = false;
            });
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (CTUserDesc.CanBeCanceled && CSUserDesc != null)
            {
                CSUserDesc.Cancel();
            }

            if (cancelToken != null && cancelToken.CanBeCanceled && cancelSource != null)
            {
                cancelSource.Cancel();
            }

            if (progressDialog != null)
            {
                progressDialog.Dismiss();
            }

            // stop sending location updates when the application goes into the background
            // to learn about updating location in the background, refer to the Backgrounding guide
            // http://docs.xamarin.com/guides/cross-platform/application_fundamentals/backgrounding/


            // RemoveUpdates takes a pending intent - here, we pass the current Activity
            if (locMgr != null)
            {
                locMgr.RemoveUpdates(this);
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

        //### LOCATION
        public void OnLocationChanged(Location location)
        {
            SDiag.Debug.Print("Location changed");
            SDiag.Debug.Print("Latitude: " + location.Latitude.ToString());
            SDiag.Debug.Print("Longitude: " + location.Longitude.ToString());
            SDiag.Debug.Print("Provider: " + location.Provider.ToString());

            isLocationFound = true;
            latitude = location.Latitude;
            longitude = location.Longitude;
            locMgr.RemoveUpdates(this);
            progressDialog.Hide();
            radius = 5;
            cancelSource = new CancellationTokenSource();
            cancelToken = cancelSource.Token;
            var task = GetCafesAndBonusOffersAsync(cancelToken, location.Latitude, location.Longitude, 40);
        }
        public void OnProviderDisabled(string provider)
        {
            SDiag.Debug.Print(provider + " disabled by user");
        }
        public void OnProviderEnabled(string provider)
        {
            SDiag.Debug.Print(provider + " enabled by user");
        }
        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            SDiag.Debug.Print(provider + " availability has changed to " + status.ToString());
        }
    }
}


