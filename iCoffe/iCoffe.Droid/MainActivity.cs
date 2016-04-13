using System;
using SDiag = System.Diagnostics;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;

namespace iCoffe.Droid
{
	[Activity (Label = "iCoffe.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, ILocationListener
	{
        // Views
        Fragment nets = null;
        Fragment map = null;
        Fragment gifts = null;

        // Location
        LocationManager locMgr;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            RelativeLayout rlList = FindViewById<RelativeLayout>(Resource.Id.rlList);
            rlList.Click += List_Click;

            RelativeLayout rlMap = FindViewById<RelativeLayout>(Resource.Id.rlMap);
            rlMap.Click += Map_Click; ;

            RelativeLayout rlGifts = FindViewById<RelativeLayout>(Resource.Id.rlGifts);
            rlGifts.Click += Gifts_Click; ;

            FragmentTransaction trans = FragmentManager.BeginTransaction();
            gifts = new Fragments.GiftsFragment();
            trans.Add(Resource.Id.mContentFL, gifts);
            map = new Fragments.MapFragment();
            trans.Add(Resource.Id.mContentFL, map);
            nets = new Fragments.NetsFragment();
            trans.Add(Resource.Id.mContentFL, nets);
            trans.Commit();
            List_Click(null, null);

            //Location
            //locMgr = GetSystemService(Context.LocationService) as LocationManager;

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
        }

        private void List_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //if (list == null)
            //{
            //list = new Fragments.NetsFragment();
            //}
            //FragmentManager.BeginTransaction().Replace(Resource.Id.mContentFL, list).Commit();
            //FragmentManager.BeginTransaction().
            FragmentManager.BeginTransaction().Hide(map).Hide(gifts).Show(nets).Commit();
        }

        protected override void OnResume()
        {
            base.OnResume();
            SDiag.Debug.Print("OnResume called");

            // initialize location manager
            locMgr = GetSystemService(Context.LocationService) as LocationManager;

            // pass in the provider (GPS), 
            // the minimum time between updates (in seconds), 
            // the minimum distance the user needs to move to generate an update (in meters),
            // and an ILocationListener (recall that this class impletents the ILocationListener interface)
            if (locMgr.AllProviders.Contains(LocationManager.NetworkProvider)
                && locMgr.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, 2000, 1, this);
            }
            else
            {
                Toast.MakeText(this, "The Network Provider does not exist or is not enabled!", ToastLength.Long).Show();
            }

            // pass in the provider (GPS), 
            // the minimum time between updates (in seconds), 
            // the minimum distance the user needs to move to generate an update (in meters),
            // and an ILocationListener (recall that this class impletents the ILocationListener interface)
            if (locMgr.AllProviders.Contains(LocationManager.GpsProvider)
                && locMgr.IsProviderEnabled(LocationManager.GpsProvider))
            {
                locMgr.RequestLocationUpdates(LocationManager.GpsProvider, 2000, 1, this);
            }
            else
            {
                Toast.MakeText(this, "The GPS Provider does not exist or is not enabled!", ToastLength.Long).Show();
            }

            //var locationCriteria = new Criteria();
            //locationCriteria.Accuracy = Accuracy.Coarse;
            //locationCriteria.PowerRequirement = Power.Medium;
            //string locationProvider = locMgr.GetBestProvider(locationCriteria, true);
            //SDiag.Debug.Print("Starting location updates with " + locationProvider.ToString());
            //locMgr.RequestLocationUpdates (locationProvider, 2000, 1, this);
        }

        protected override void OnPause()
        {
            base.OnPause();

            // stop sending location updates when the application goes into the background
            // to learn about updating location in the background, refer to the Backgrounding guide
            // http://docs.xamarin.com/guides/cross-platform/application_fundamentals/backgrounding/


            // RemoveUpdates takes a pending intent - here, we pass the current Activity
            locMgr.RemoveUpdates(this);
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


