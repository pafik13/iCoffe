using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDiag = System.Diagnostics;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Actions;

using iCoffe.Shared;
using iCoffe.Droid.Adapters;

namespace iCoffe.Droid.Fragments
{
    public class UserFragment : Fragment, IOnMapReadyCallback
    {
        View MainLayout;

        MapView mapView;
        GoogleMap map;

        ListView bonusListView;
        UserBonusAdapter bonusAdapter;
        IList<BonusOffer> bonuses;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            MainLayout = inflater.Inflate(Resource.Layout.UserFragment, container, false);
            bonusListView = MainLayout.FindViewById<ListView>(Resource.Id.ufListView);

            RefreshUserInfo();

            MainLayout.FindViewById<Button>(Resource.Id.ufExitB).Click += ExitButton_Click;

            mapView = MainLayout.FindViewById<MapView>(Resource.Id.ufUserMap);
            mapView.OnCreate(savedInstanceState);
            mapView.GetMapAsync(this); //this is important

            return MainLayout;
        }

        public void RefreshUserInfo()
        {
            MainLayout.FindViewById<TextView>(Resource.Id.ufUserIDTV).Text = Data.UserInfo.Login;
            MainLayout.FindViewById<TextView>(Resource.Id.ufUserNameTV).Text = Data.UserInfo.FullUserName;
            MainLayout.FindViewById<TextView>(Resource.Id.ufUserPointsTV).Text = Data.UserInfo.Points.ToString();
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            map.UiSettings.ZoomControlsEnabled = false;
            map.MyLocationEnabled = true;
        }

        public void MoveCamera(LatLng position)
        {
            if (map != null)
            {
                map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(position, 12)); // moveCamera(CameraUpdateFactory.newLatLngZoom(/*some location*/, 10));
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            SDiag.Debug.Print("ExitButton_Click called");

            Activity.GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private)
                .Edit()
                .PutString(MainActivity.C_ACCESS_TOKEN, string.Empty)
                .Apply();

            StartActivity(new Intent(Activity, typeof(SignInActivity)));

            Data.UserInfo = new UserInfo() { FullUserName = @"<нет данных>", Login = @"<нет данных>", Points = -1 };
            RefreshUserInfo();
        }

        public override void OnPause()
        {
            base.OnPause();
            mapView.OnPause();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            mapView.OnDestroy();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            mapView.OnSaveInstanceState(outState);
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            mapView.OnLowMemory();
        }

        public override void OnResume()
        {
            base.OnResume();
            mapView.OnResume();

            RecreateAdapter();
        }

        public void RecreateAdapter()
        {
            // get data
            // bonuses = Data.Offers.Take(5).ToList();
            bonuses = Data.UserBonusOffers;

            // create our adapter
            bonusAdapter = new UserBonusAdapter(Activity, bonuses);

            //Hook up our adapter to our ListView
            Activity.RunOnUiThread(() => bonusListView.Adapter = bonusAdapter);
        }

    }
}