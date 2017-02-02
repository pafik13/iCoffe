using System.Threading;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

using UniversalImageLoader.Core;

using iCoffe.Shared;
using iCoffe.Droid.Adapters;

namespace iCoffe.Droid
{
    [Activity(Label = "PlaceActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class PlaceActivity : Activity
    {
        bool IgnoreBackPress;
        Place Place;
        List<OfferInfo> Offers;
        private string AccessToken;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.Place);

            var placeId = Intent.GetIntExtra(MainActivity.C_PLACE_ID, -1);
            Place = Data.GetPlace(placeId);
            Offers = Data.GetOffers(placeId);

            FindViewById<TextView>(Resource.Id.paDescrTV).Text = "<нет описания>";
            FindViewById<TextView>(Resource.Id.paAddressTV).Text = string.IsNullOrEmpty(Place.Address) ? "<нет адреса>" : Place.Address;
            FindViewById<TextView>(Resource.Id.paNameTV).Text = Place.Name;

            // TODO: Load image
            ImageLoader imageLoader = ImageLoader.Instance;
            if (!string.IsNullOrEmpty(Place.LogoUrl))
                imageLoader.DisplayImage(Place.LogoUrl, FindViewById<ImageView>(Resource.Id.paLogoIV));

            if (!string.IsNullOrEmpty(Place.ViewUrl))
                imageLoader.DisplayImage(Place.ViewUrl, FindViewById<ImageView>(Resource.Id.paViewIV));

            var offersTable = FindViewById<ListView>(Resource.Id.paOffersLV);
            offersTable.Adapter = new PurchasedOffersAdapter(this, Offers);
            offersTable.ItemClick += OffersTable_ItemClick;

            var sharedPreferences = GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
            AccessToken = sharedPreferences.GetString(MainActivity.C_ACCESS_TOKEN, string.Empty);

            SetResult(Result.Ok);
        }

        private void OffersTable_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var table = sender as ListView;
            var adapter = table.Adapter as PurchasedOffersAdapter;
            var offer = Data.GetOffer(adapter[e.Position].Id);

            new AlertDialog.Builder(this)
                        .SetTitle(esource.String.offer_caption)
                        .SetMessage(string.Concat(offer.Description, System.Environment.NewLine, "Цена: ", offer.Price))
                        .SetCancelable(false)
                        .SetPositiveButton(Resource.String.want_button, (caller, args) => {
                            IgnoreBackPress = true;

                            if (Rest.BuyOffer(AccessToken, offer.Id))
                            {
                                Data.UserInfo.PointsAmount -= (double)offer.Price;
                                Data.UserPurchasedOffers.Add(offer);
                                ShowMessage(@"Приобретено");
                            }
                            else
                            {
                                ShowMessage(@"Неудача!");
                            }
                        })
                        .SetNegativeButton(Resource.String.cancel_button, (caller, args) => {
                            (caller as Dialog).Dismiss();
                        }).Show();
        }

        public override void OnBackPressed()
        {
            if (IgnoreBackPress) return;
            base.OnBackPressed();
        }

        void ShowMessage(string message)
        {
            var fade = FindViewById<RelativeLayout>(Resource.Id.fade);
            fade.Visibility = ViewStates.Visible;
            var message = FindViewById<TextView>(Resource.Id.message);
			message.Text = message;
            message.Visibility = ViewStates.Visible;

            ThreadPool.QueueUserWorkItem(state =>
            {
                Thread.Sleep(2000);

                RunOnUiThread(() =>
                {
                    fade.Visibility = ViewStates.Gone;
					message.Visibility = ViewStates.Gone;
                    IgnoreBackPress = false;
                });

                //SDiag.Debug.Print("Location find stopped.");
            });
        }
    }
}