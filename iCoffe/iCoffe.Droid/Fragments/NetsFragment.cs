using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using iCoffe.Shared;
using iCoffe.Droid.Adapters;

namespace iCoffe.Droid.Fragments
{
    public class NetsFragment : Fragment
    {
        NetsAdapter netsAdapter;
        IList<CoffeeHouseNet> nets;
        ListView netsListView;

        ComplexObjsAdapter objsAdapter;
        IList<GeolocComplexObj> objs;

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
            View view = inflater.Inflate(Resource.Layout.fragment, container, false);
            view.FindViewById<TextView>(Resource.Id.frTextView).Visibility = ViewStates.Gone;
            netsListView = view.FindViewById<ListView>(Resource.Id.frListView);
            netsListView.ItemClick += NetsListView_ItemClick;
            //LinearLayout llMain = view.FindViewById<LinearLayout>(Resource.Id.frMainLL);
            //llMain.RemoveAllViews();
            //netsListView = new ListView(Activity) {
            //    LayoutParameters = new LinearLayout.LayoutParams(
            //        LinearLayout.LayoutParams.MatchParent
            //      , LinearLayout.LayoutParams.MatchParent
            //    ),
            //    Divider = 
            //};
            //llMain.AddView(netsListView);
            return view;
        }

        private void NetsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //throw new NotImplementedException();
            //Toast.MakeText(Activity, string.Format(@"id : {0}", objsAdapter[e.Position].Obj.Id), ToastLength.Short).Show();
            Intent intent = new Intent(Activity, typeof(GiftDescriptionActivity));
            intent.PutExtra(@"ObjId", objsAdapter[e.Position].Obj.Id);
            StartActivity(intent);
        }

        public override void OnResume()
        {
            base.OnResume();
            // get data
            nets = Data.Nets;
            objs = Data.ComplexObjs;

            // create our adapter
            netsAdapter = new NetsAdapter(Activity, nets);
            objsAdapter = new ComplexObjsAdapter(Activity, objs);

            //Hook up our adapter to our ListView
            Activity.RunOnUiThread(() => netsListView.Adapter = objsAdapter);
        }
    }
}