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

        public override void OnResume()
        {
            base.OnResume();
            //
            nets = new Data().Nets;

            // create our adapter
            netsAdapter = new NetsAdapter(Activity, nets);

            //Hook up our adapter to our ListView
            netsListView.Adapter = netsAdapter;
        }
    }
}