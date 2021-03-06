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

namespace iCoffe.Droid.Fragments
{
    public class MapFragment : Fragment, IOnMapReadyCallback, GoogleMap.IOnMarkerClickListener
    {
        MapView mapView;
        GoogleMap map;

        Dictionary<string, int> markers = new Dictionary<string, int>();


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);


            //return base.OnCreateView(inflater, container, savedInstanceState);
            //View view = inflater.Inflate(Resource.Layout.fragment, container, false);
            //TextView tv = view.FindViewById<TextView>(Resource.Id.frTextView);
            //string text = string.Empty;
            //foreach (var cafe in Data.Objs)
            //{
            //    text += string.Format(@"Id:{0}, X:{1}, Y:{2}", cafe.Id, cafe.geoloc.x, cafe.geoloc.y) + System.Environment.NewLine;
            //}
            //tv.Text = text;
            ////tv.Text = @"Map";

            View view = inflater.Inflate(Resource.Layout.MapFragment, container, false);
            mapView = view.FindViewById<MapView>(Resource.Id.mvMap);
            mapView.OnCreate(savedInstanceState);
            mapView.GetMapAsync(this); //this is important

            return view;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            map.UiSettings.ZoomControlsEnabled = true;  // GetUiSettings().setZoomControlsEnabled(true);
            map.MyLocationEnabled = true;

            MoveCamera(new LatLng(54.974362, 73.418061));

            RecreateMarkers();

            map.SetOnMarkerClickListener(this);
        }

        public void RecreateMarkers()
        {
            if (map != null)
            {
                map.Clear();

                foreach (var cafe in Data.Cafes)
                {
                    //text += string.Format(@"Id:{0}, X:{1}, Y:{2}", cafe.Id, cafe.geoloc.x, cafe.geoloc.y) + System.Environment.NewLine;
                    var position = new LatLng(cafe.GeoLocation.GeoPoint.Latitude, cafe.GeoLocation.GeoPoint.Longitude);
                    Marker m = map.AddMarker(new MarkerOptions().SetPosition(position).SetTitle(cafe.Name).SetSnippet(@"snippet"));
                    markers.Add(m.Id, cafe.Id);
                }
            }
        }

        public void MoveCamera(LatLng position)
        {
            if (map != null)
            {
                map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(position, 10)); // moveCamera(CameraUpdateFactory.newLatLngZoom(/*some location*/, 10));
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            mapView.OnResume();
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

        public bool OnMarkerClick(Marker marker)
        {
            //throw new NotImplementedException();
            SDiag.Debug.Print(string.Format(@"markerTitle : {0}", marker.Title));
            if (markers.ContainsKey(marker.Id))
            {
                SDiag.Debug.Print(string.Format(@"cafeId : {0}", markers[marker.Id]));
                BonusOffer offer = Data.GetBonusOffer(markers[marker.Id]);

                if (offer != null)
                {
                    Intent intent = new Intent(Activity, typeof(BonusActivity));
                    intent.PutExtra(MainActivity.C_BONUS_ID, offer.Id.ToString());
                    StartActivityForResult(intent, 1);
                }
            }
            else
            {
                marker.ShowInfoWindow();
            }

            return true;
        }

        public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if ((requestCode == 1) && (resultCode == Result.Ok))
            {
                Activity.Intent.PutExtra(MainActivity.C_WAS_STARTED_NEW_ACTIVITY, true);
            }
        }
    }
}