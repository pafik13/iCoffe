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
    public class OffersFragment : Fragment
    {
        ListView OffersTable;

        OffersAdapter OffersAdapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View view = inflater.Inflate(Resource.Layout.OffersFragment, container, false);

            OffersTable = view.FindViewById<ListView>(Resource.Id.ofListView);
            OffersTable.ItemClick += OffersTable_ItemClick;
            return view;
        }

        private void OffersTable_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //throw new NotImplementedException();
            //Toast.MakeText(Activity, string.Format(@"id : {0}; Descr: {1}", OffersAdapter[e.Position].Id, OffersAdapter[e.Position].Description), ToastLength.Short).Show();
            Intent intent = new Intent(Activity, typeof(OfferActivity));
            intent.PutExtra(MainActivity.C_OFFER_ID, OffersAdapter[e.Position].Id);
            StartActivityForResult(intent, 1);
        }

        public override void OnResume()
        {
            base.OnResume();

            RecreateAdapter();
        }

        public void RecreateAdapter()
        {
            // create our adapter
            OffersAdapter = new OffersAdapter(Activity, Data.Offers);

            //Hook up our adapter to our ListView
            Activity.RunOnUiThread(() => OffersTable.Adapter = OffersAdapter);
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