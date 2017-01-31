using System.Linq;
using System.Collections.Generic;
using SDiag = System.Diagnostics;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

using iCoffe.Shared;
using iCoffe.Droid.Adapters;

namespace iCoffe.Droid.Fragments
{
    public class MapFragment : Fragment, IOnMapReadyCallback, GoogleMap.IOnMarkerClickListener
    {
		View Fade;
		ListView OffersLV;
		
        MapView MapView;
        GoogleMap GoogleMap;

        Dictionary<string, int> markers = new Dictionary<string, int>();


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.MapFragment, container, false);
            MapView = view.FindViewById<MapView>(Resource.Id.mfMap);
            MapView.OnCreate(savedInstanceState);
            MapView.GetMapAsync(this); //this is important
			
			Fade = view.FindViewById<View>(Resource.Id.mfFadeV);
			Fade.Click += (sender, args) => {
                HideOffersList();
            };
			
			OffersLV = view.FindViewById<ListView>(Resource.Id.mfOffersLV);
			
			OffersLV.ItemClick += OffersTable_ItemClick;
            //OffersLV.Clickable = true;
            //OffersLV.Click += (s, e) => { HideOffersList(); };
            return view;
        }

        private void OffersTable_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Toast.MakeText(Activity, string.Format(@"id : {0}; Descr: {1}", OffersAdapter[e.Position].Id, OffersAdapter[e.Position].Description), ToastLength.Short).Show();
			var lv = (sender as ListView);
			var adapter = (lv.Adapter as PurchasedOffersAdapter); 
            Intent intent = new Intent(Activity, typeof(OfferActivity));
            intent.PutExtra(MainActivity.C_OFFER_ID, adapter[e.Position].Id);
            StartActivityForResult(intent, 1);
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            GoogleMap = googleMap;
            GoogleMap.UiSettings.ZoomControlsEnabled = true;  // GetUiSettings().setZoomControlsEnabled(true);
            GoogleMap.MyLocationEnabled = true;

            MoveCamera(new LatLng(54.974362, 73.418061));

            RecreateMarkers();

            GoogleMap.SetOnMarkerClickListener(this);
            GoogleMap.InfoWindowClick += MapOnInfoWindowClick;
        }

        public void RecreateMarkers()
        {
            if (GoogleMap != null)
            {
                GoogleMap.Clear();

                foreach (var place in Data.PlaceInfos)
                {
                    //text += string.Format(@"Id:{0}, X:{1}, Y:{2}", cafe.Id, cafe.geoloc.x, cafe.geoloc.y) + System.Environment.NewLine;
                    var position = new LatLng(place.GeoLocation.Latitude, place.GeoLocation.Longitude);
                    Marker m = GoogleMap.AddMarker(new MarkerOptions().SetPosition(position).SetTitle(place.Name).SetSnippet("snippet"));
                    markers.Add(m.Id, place.Id);
                }
            }
        }

        public void MoveCamera(LatLng position)
        {
            if (GoogleMap != null)
            {
                GoogleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(position, 10)); // moveCamera(CameraUpdateFactory.newLatLngZoom(/*some location*/, 10));
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            MapView.OnResume();
            if (GoogleMap != null)
            {
                GoogleMap.SetOnMarkerClickListener(this);
                GoogleMap.InfoWindowClick += MapOnInfoWindowClick;
            }
        }

        void HideOffersList()
        {
            OffersLV.Visibility = ViewStates.Gone;
            Fade.Visibility = ViewStates.Gone;
            OffersLV.Adapter = null;
        }

        public override void OnPause()
        {
            base.OnPause();
            MapView.OnPause();
            if (GoogleMap != null)
            {
                GoogleMap.InfoWindowClick -= MapOnInfoWindowClick;
                GoogleMap.SetOnMarkerClickListener(null);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            MapView.OnDestroy();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            MapView.OnSaveInstanceState(outState);
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            MapView.OnLowMemory();
        }

        public bool OnMarkerClick(Marker marker)
        {
            //TODO: fix errors
            SDiag.Debug.Print(string.Format(@"markerTitle : {0}", marker.Title));
            marker.ShowInfoWindow();
            
            return true;
        }
		
		private void MapOnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs infoWindowClickEventArgs)
		{
			var marker = infoWindowClickEventArgs.Marker;

            if (markers.ContainsKey(marker.Id))
            {
                SDiag.Debug.Print(string.Format(@"cafeId : {0}", markers[marker.Id]));
                var offers = Data.Offers.Where(o => o.PlaceId == (markers[marker.Id])).ToList<OfferInfo>();

                if (offers.Count == 0)
                {
                    Toast.MakeText(Activity, "¬ данном месте нет доступных предложений", ToastLength.Short).Show();
                    return;
                }

                if (offers.Count == 1)
                {
                    Intent intent = new Intent(Activity, typeof(OfferActivity));
                    intent.PutExtra(MainActivity.C_OFFER_ID, offers[0].Id);
                    StartActivityForResult(intent, 1);
                }
                else
                {
                    OffersLV.Adapter = new PurchasedOffersAdapter(Activity, offers);
					Fade.Visibility = ViewStates.Visible;
					OffersLV.Visibility = ViewStates.Visible;
                }
            }
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