using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using iCoffe.Shared;
using iCoffe.Droid.Adapters;

namespace iCoffe.Droid.Fragments
{
    public class BonusFragment : Fragment
    {
        ListView bonusListView;

        BonusOffersAdapter offersAdapter;
        IList<BonusOffer> offers;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View view = inflater.Inflate(Resource.Layout.fragment, container, false);

            view.FindViewById<TextView>(Resource.Id.frTextView).Visibility = ViewStates.Gone;
            bonusListView = view.FindViewById<ListView>(Resource.Id.frListView);
            bonusListView.ItemClick += NetsListView_ItemClick;
            return view;
        }

        private void NetsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //throw new NotImplementedException();
            Toast.MakeText(Activity, string.Format(@"id : {0}; Descr: {1}", offersAdapter[e.Position].Id, offersAdapter[e.Position].Description), ToastLength.Short).Show();
            Intent intent = new Intent(Activity, typeof(BonusActivity));
            intent.PutExtra(MainActivity.C_BONUS_ID, offersAdapter[e.Position].Id.ToString());
            StartActivityForResult(intent, 1);
        }

        public override void OnResume()
        {
            base.OnResume();

            RecreateAdapter();
        }

        public void RecreateAdapter()
        {
            // get data
            // offers = Data.Offers;
            offers = Data.BonusOffers;

            // create our adapter
            offersAdapter = new BonusOffersAdapter(Activity, offers);

            //Hook up our adapter to our ListView
            Activity.RunOnUiThread(() => bonusListView.Adapter = offersAdapter);
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