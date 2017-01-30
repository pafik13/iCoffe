using System;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using iCoffe.Shared;
using UniversalImageLoader.Core;

namespace iCoffe.Droid
{
    [Activity(Label = "OfferActivity")]
    public class OfferActivity : Activity
    {
        bool IgnoreBackPress;
        Offer Offer;
        Place Place;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.Offer);

            var offerId = Intent.GetIntExtra(MainActivity.C_OFFER_ID, -1);
            Offer = Data.GetOffer(offerId);
            Place = Data.GetPlace(Offer.PlaceId);

            FindViewById<TextView>(Resource.Id.oaBonusDescrTV).Text = Offer.Description;
            FindViewById<TextView>(Resource.Id.oaAddressTV).Text = "<нет адреса>";
            FindViewById<TextView>(Resource.Id.oaCafeNameTV).Text = Place.Name;
            FindViewById<TextView>(Resource.Id.oaPriceTV).Text = Offer.Price.ToString();

            // TODO: Load image
            ImageLoader imageLoader = ImageLoader.Instance;
            if (!string.IsNullOrEmpty(Place.LogoUrl))
                imageLoader.DisplayImage(Place.LogoUrl, FindViewById<ImageView>(Resource.Id.oaCafeNameIV));

            if (!string.IsNullOrEmpty(Offer.LogoUrl))
                imageLoader.DisplayImage(Offer.LogoUrl, FindViewById<ImageView>(Resource.Id.oaCafeImageIV));

            var want = FindViewById<Button>(Resource.Id.oaWantB);
            want.Click += Want_Click;

            SetResult(Result.Ok);
        }

        public override void OnBackPressed()
        {
            if (IgnoreBackPress) return;
            base.OnBackPressed();
        }

        private void Want_Click(object sender, EventArgs e)
        {
            IgnoreBackPress = true;

            var sharedPreferences = GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
            string accessToken = sharedPreferences.GetString(MainActivity.C_ACCESS_TOKEN, string.Empty);

            if (Rest.BuyOffer(accessToken, Offer.Id)) {
                Data.UserInfo.PointsAmount -= (double)Offer.Price;
                Data.UserPurchasedOffers.Add(Offer);
                ShowMessage(@"Приобретено"); 
            } else {
                ShowMessage(@"Неудача!");
            }
        }

        void ShowMessage(string message)
        {
            var locker = FindViewById<RelativeLayout>(Resource.Id.locker);
            locker.Visibility = ViewStates.Visible;

            var lock_message = FindViewById<TextView>(Resource.Id.lock_message);
            lock_message.Text = message;

            var want = FindViewById<Button>(Resource.Id.oaWantB);
            want.Enabled = false;

            ThreadPool.QueueUserWorkItem(state =>
            {
                Thread.Sleep(2000);

                RunOnUiThread(() =>
                {
                    locker.Visibility = ViewStates.Gone;
                    want.Enabled = true;
                    IgnoreBackPress = false;
                });

                //SDiag.Debug.Print("Location find stopped.");
            });
        }
    }
}