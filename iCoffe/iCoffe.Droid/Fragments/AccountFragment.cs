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
    public class AccountFragment : Fragment, IOnMapReadyCallback
    {
        View MainLayout;

        MapView mapView;
        GoogleMap map;

        ListView PurchasedOffersTable;
        PurchasedOffersAdapter PurchasedOffersAdapter;
		
		Animation ScaleUp;
		Animation ScaleDown;
		Animation RotateFromUpToDown;
		Animation RotateFromDownToUp;
		
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            MainLayout = inflater.Inflate(Resource.Layout.UserFragment, container, false);
            PurchasedOffersTable = MainLayout.FindViewById<ListView>(Resource.Id.afListView);

            RefreshUserInfo();

            MainLayout.FindViewById<Button>(Resource.Id.afExitB).Click += ExitButton_Click;
            MainLayout.FindViewById<ImageButton>(Resource.Id.afShowAndHideMapB).Click += ShowAndHideMapButton_Click;

            mapView = MainLayout.FindViewById<MapView>(Resource.Id.afUserMap);
            mapView.OnCreate(savedInstanceState);
            mapView.GetMapAsync(this); //this is important
			
			ScaleUp = AnimationUtils.LoadAnimation(this, Resource.Animation.scale_up);
			ScaleDown = AnimationUtils.LoadAnimation(this, Resource.Animation.scale_down);
			
			RotateFromUpToDown = AnimationUtils.LoadAnimation(this, Resource.Animation.rotate_from_up_to_down);
			RotateFromDownToUp = AnimationUtils.LoadAnimation(this, Resource.Animation.rotate_from_down_to_up);
			
            return MainLayout;
        }

        public void RefreshUserInfo()
        {
            MainLayout.FindViewById<TextView>(Resource.Id.afUserLoginTV).Text = Data.UserInfo.Login;
            MainLayout.FindViewById<TextView>(Resource.Id.afUserNameTV).Text = Data.UserInfo.Login;
            MainLayout.FindViewById<TextView>(Resource.Id.afUserPointsAmountTV).Text = Data.UserInfo.PointsAmount.ToString();
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

            Data.UserInfo = new UserInfo() { Id = "<нет данных>", Login = "<нет данных>", PointsAmount = -1 };
            RefreshUserInfo();
        }

        private void ShowAndHideMapButton_Click(object sender, EventArgs e)
        {
            SDiag.Debug.Print("ShowAndHideMapButton_Click called");
			var mapFrame = View.FindViewById<FrameLaout>(Resource.Id.afUserMapFL);
			if (mapFrame.Height == 0){
				mapFrame.StartAnimation(ScaleUp);
				(sender as Button).StartAnimation(RotateFromDownToUp);
			} else {
				mapFrame.StartAnimation(ScaleDown);
				(sender as Button).StartAnimation(RotateFromUpToDown);				
			}
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
            // create our adapter
            PurchasedOffersAdapter = new PurchasedOffersAdapter(Activity, Data.UserPurchasedOffers);

            //Hook up our adapter to our ListView
            Activity.RunOnUiThread(() => PurchasedOffersTable.Adapter = PurchasedOffersAdapter);
        }

    }
}