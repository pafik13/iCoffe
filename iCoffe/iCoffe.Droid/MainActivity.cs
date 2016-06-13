using System;
using System.Drawing;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using SDiag = System.Diagnostics;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Net;
using Android.Locations;

using RestSharp;

using UniversalImageLoader.Core;

using iCoffe.Shared;

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

        // Layouts
        RelativeLayout rlNets;
        RelativeLayout rlMap;
        RelativeLayout rlUser;

        // Views
        Fragment nets = null;
        Fragment map = null;
        Fragment user = null;

        // Location
        LocationManager locMgr;
        bool isLocationFound = false;
        string defaultPlace = String.Empty;
        double latitude;
        double longitude;
        int radius = 4;

        // Intermedia
        AlertDialog.Builder builder;
        Dialog dialog;
        ProgressDialog progressDialog;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            var config = ImageLoaderConfiguration.CreateDefault(ApplicationContext);
            // Initialize ImageLoader with configuration.
            ImageLoader.Instance.Init(config);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            rlNets = FindViewById<RelativeLayout>(Resource.Id.rlNets);
            rlNets.Click += Nets_Click;

            rlMap = FindViewById<RelativeLayout>(Resource.Id.rlMap);
            rlMap.Click += Map_Click; ;

            rlUser = FindViewById<RelativeLayout>(Resource.Id.rlUser);
            rlUser.Click += User_Click; ;
        }

        protected override void OnResume()
        {
            base.OnResume();
            SDiag.Debug.Print("OnResume called");

            ISharedPreferences prefs = GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
            string accessToken = prefs.GetString(C_ACCESS_TOKEN, String.Empty);
            if (String.IsNullOrEmpty(accessToken))
            {
                StartActivity(new Intent(this, typeof(SignInActivity)));
                return;
            }

            if (!Intent.GetBooleanExtra(C_WAS_STARTED_NEW_ACTIVITY, false))
            {
                if (IsInternetActive() && IsLocationActive())
                {

                    if (defaultPlace == String.Empty)
                    {
                        // pass in the provider (GPS), 
                        // the minimum time between updates (in seconds), 
                        // the minimum distance the user needs to move to generate an update (in meters),
                        // and an ILocationListener (recall that this class impletents the ILocationListener interface)
                        ////if (locMgr.AllProviders.Contains(LocationManager.NetworkProvider)
                        ////    && locMgr.IsProviderEnabled(LocationManager.NetworkProvider))
                        ////{
                        ////    locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, 2000, 1, this);
                        ////}
                        ////else
                        ////{
                        ////    Toast.MakeText(this, "The Network Provider does not exist or is not enabled!", ToastLength.Long).Show();
                        ////}

                        // pass in the provider (GPS), 
                        // the minimum time between updates (in seconds), 
                        // the minimum distance the user needs to move to generate an update (in meters),
                        // and an ILocationListener (recall that this class impletents the ILocationListener interface)
                        ////if (locMgr.AllProviders.Contains(LocationManager.GpsProvider)
                        ////    && locMgr.IsProviderEnabled(LocationManager.GpsProvider))
                        ////{
                        ////    locMgr.RequestLocationUpdates(LocationManager.GpsProvider, 2000, 1, this);
                        ////}
                        ////else
                        ////{
                        ////    Toast.MakeText(this, "The GPS Provider does not exist or is not enabled!", ToastLength.Long).Show();
                        ////}
                        //Location loc = locMgr.GetLastKnownLocation();
                        //loc.
                        var locationCriteria = new Criteria();
                        locationCriteria.Accuracy = Accuracy.Coarse;
                        locationCriteria.PowerRequirement = Power.Medium;
                        string locationProvider = locMgr.GetBestProvider(locationCriteria, true);
                        SDiag.Debug.Print("Starting location updates with " + locationProvider.ToString());
                        locMgr.RequestLocationUpdates(locationProvider, 2000, 1, this);

                        // Progress
                        string message = @"Получение данных о местоположении...";
                        progressDialog = ProgressDialog.Show(this, @"", message, true);

                        //progressDialog.Show();
                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            Thread.Sleep(30000);

                            RunOnUiThread(() =>
                            {
                                if (!isLocationFound)
                                {
                                    progressDialog.Dismiss();
                                }
                            });

                            SDiag.Debug.Print("Location find stopped.");
                        });
                    }
                    else
                    {
                        GetNets();
                    }
                }
            }
        }

        private void GetNets()
        {
            /* throw new NotImplementedException();*/
            string message = defaultPlace == string.Empty ? @"Получение данных..." : "Получение данных. В качестве основной точки используется: " + defaultPlace;
            progressDialog = ProgressDialog.Show(this, @"", message, true);

            //progressDialog.Show();
            ThreadPool.QueueUserWorkItem(state =>
            {
                Data.BonusOffers = GetBonuses();

                LoadFragments();
                RunOnUiThread(() => progressDialog.Dismiss());
                //progressDialog.Dismiss();

                SDiag.Debug.Print("GetNets stopped.");
            });
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
                    GetNets();
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
        
        private void LoadFragments()
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            if (user == null)
            {
                user = new Fragments.UserFragment();
                trans.Add(Resource.Id.mContentFL, user);
            }
            if (map == null)
            {
                map = new Fragments.MapFragment();
                trans.Add(Resource.Id.mContentFL, map);
            }
            if (nets == null)
            {
                nets = new Fragments.NetsFragment();
                trans.Add(Resource.Id.mContentFL, nets);
            }
            trans.Commit();
            if (!rlMap.Selected && !rlNets.Selected && !rlUser.Selected)
            {
                Map_Click(rlMap, null);
            }
        }

        private void User_Click(object sender, EventArgs e)
        {
            FragmentManager.BeginTransaction().Hide(nets).Hide(map).Show(user).Commit();
            RunOnUiThread(() => {
                rlMap.Selected = false;
                rlNets.Selected = false;
                rlUser.Selected = true;
            });
        }

        private void Map_Click(object sender, EventArgs e)
        {
            FragmentManager.BeginTransaction().Hide(nets).Hide(user).Show(map).Commit();
            RunOnUiThread(() => { 
                rlMap.Selected = true;
                rlNets.Selected = false;
                rlUser.Selected = false;
            });
        }

        private void Nets_Click(object sender, EventArgs e)
        {
            FragmentManager.BeginTransaction().Hide(map).Hide(user).Show(nets).Commit();
            RunOnUiThread(() => {
                rlMap.Selected = false;
                rlNets.Selected = true;
                rlUser.Selected = false;
            });
        }

        protected override void OnPause()
        {
            base.OnPause();

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
            GetNets();
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


