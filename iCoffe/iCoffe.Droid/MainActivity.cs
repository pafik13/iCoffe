﻿using System;
using System.Drawing;
using System.Collections.Generic;
using SDiag = System.Diagnostics;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using System.Threading;
using Android.Widget;
using Android.OS;
using Android.Net;
using Android.Locations;

using RestSharp;

using iCoffe.Shared;

namespace iCoffe.Droid
{
    [Activity(Label = "MainActivity")]
    public class MainActivity : Activity, ILocationListener
	{
        // Layouts
        RelativeLayout rlNets;
        RelativeLayout rlMap;
        RelativeLayout rlGifts;

        // Views
        Fragment nets = null;
        Fragment map = null;
        Fragment gifts = null;

        // Location
        LocationManager locMgr;
        bool isLocationFound = false;
        string defaultPlace = String.Empty;
        double latitude;
        double longitude;
        int radius;

        // Intermedia
        AlertDialog.Builder builder;
        Dialog dialog;
        ProgressDialog progressDialog;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            rlNets = FindViewById<RelativeLayout>(Resource.Id.rlNets);
            rlNets.Click += Nets_Click;

            rlMap = FindViewById<RelativeLayout>(Resource.Id.rlMap);
            rlMap.Click += Map_Click; ;

            rlGifts = FindViewById<RelativeLayout>(Resource.Id.rlGifts);
            rlGifts.Click += Gifts_Click; ;


            //AlertDialog.Builder builder;
            //builder = new AlertDialog.Builder(this);
            //builder.SetTitle("Начало");
            //builder.SetMessage("Необходимо получить данные о Вашем местоположении.");
            //builder.SetCancelable(false);
            //builder.SetNegativeButton("Отмена", delegate {
            //    latitude = 54.974362;
            //    longitude = 73.418061;
            //    radius = 2;
            //    dialog.Dismiss();
            //    LoadViews();
            //});
            //builder.SetPositiveButton("Получить", delegate {
            //    dialog.Dismiss();

            //    progressDialog = ProgressDialog.Show(this, @"", @"Please wait...", true);

            //    ////progressDialog.Show();
            //    //ThreadPool.QueueUserWorkItem(state =>
            //    //{
            //    //    Thread.Sleep(5000);

            //    //    RunOnUiThread(() => progressDialog.Dismiss());
            //    //    //progressDialog.Dismiss();
            //    //});
            //    //mDialog.Show();

            //});
            //dialog = builder.Show();

            //FragmentTransaction trans = FragmentManager.BeginTransaction();
            //gifts = new Fragments.GiftsFragment();
            //trans.Add(Resource.Id.mContentFL, gifts);
            //map = new Fragments.MapFragment();
            //trans.Add(Resource.Id.mContentFL, map);
            //nets = new Fragments.NetsFragment();
            //trans.Add(Resource.Id.mContentFL, nets);
            //trans.Commit();
            //Map_Click(null, null);

            //Location
            //locMgr = GetSystemService(Context.LocationService) as LocationManager;
        }

        private void LoadViews()
        {
            //throw new NotImplementedException();
            // GetData();
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            if (gifts == null)
            {
                gifts = new Fragments.GiftsFragment();
                trans.Add(Resource.Id.mContentFL, gifts);
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
            if (!rlMap.Selected && !rlNets.Selected && !rlGifts.Selected)
            {
                Map_Click(null, null);
            }
        }

        private void Gifts_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //if (gifts == null)
            //{
            //gifts = new Fragments.GiftsFragment();
            //}
            //FragmentManager.BeginTransaction().Replace(Resource.Id.mContentFL, gifts).Commit();
            FragmentManager.BeginTransaction().Hide(nets).Hide(map).Show(gifts).Commit();
            RunOnUiThread(() => {
                rlMap.Selected = false;
                rlNets.Selected = false;
                rlGifts.Selected = true;
            });
        }

        private void Map_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //if (map == null)
            //{
            //map = new Fragments.MapFragment();
            //}
            //FragmentManager.BeginTransaction().Replace(Resource.Id.mContentFL, map).Commit(); 
            FragmentManager.BeginTransaction().Hide(nets).Hide(gifts).Show(map).Commit();
            RunOnUiThread(() => { 
                rlMap.Selected = true;
                rlNets.Selected = false;
                rlGifts.Selected = false;
            });
        }

        private void Nets_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //if (list == null)
            //{
            //list = new Fragments.NetsFragment();
            //}
            //FragmentManager.BeginTransaction().Replace(Resource.Id.mContentFL, list).Commit();
            //FragmentManager.BeginTransaction().
            FragmentManager.BeginTransaction().Hide(map).Hide(gifts).Show(nets).Commit();
            RunOnUiThread(() => {
                rlMap.Selected = false;
                rlNets.Selected = true;
                rlGifts.Selected = false;
            });
        }

        protected override void OnResume()
        {
            base.OnResume();
            SDiag.Debug.Print("OnResume called");

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

                        RunOnUiThread(() => {
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

        private void GetNets()
        {
            /* throw new NotImplementedException();*/
            string message = defaultPlace == string.Empty ? @"Получение данных..." : "Получение данных. В качестве основной точки используется: " + defaultPlace;
            progressDialog = ProgressDialog.Show(this, @"", message, true);

            //progressDialog.Show();
            ThreadPool.QueueUserWorkItem(state =>
            {
                List<CoffeeHouseNet> nets = Data.Nets;
                //Thread.Sleep(5000);
                var client = new RestClient(@"http://geolocwebapi.azurewebsites.net/");
                var request = new RestRequest(@"api/Objs/Sel", Method.GET);
                request.AddQueryParameter(@"plat", 54.974362.ToString());
                request.AddQueryParameter(@"plong", 73.418061.ToString());
                request.AddQueryParameter(@"dmax", 5.ToString());
                request.AddQueryParameter(@"grId", 1.ToString());
                request.AddQueryParameter(@"pgn", null);
                request.AddQueryParameter(@"pgs", null);
                var response1 = client.Execute<List<GeolocObj>>(request);
                Data.Objs = response1.Data;
                //for(int i = 0; i < Data.Objs.Count; i++)
                foreach (var obj in Data.Objs)
                {
                    request = new RestRequest(@"api/Objs/SelComplex", Method.GET);
                    request.AddQueryParameter(@"objId", obj.Id.ToString());
                    var response2 = client.Execute<GeolocComplexObj>(request);
                    Data.ComplexObjs.Add(response2.Data);
                }
                LoadViews();
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


