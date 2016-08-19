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
    [Activity(Label = "BonusActivity")]
    public class BonusActivity : Activity
    {
        bool IgnoreBackPress;
        string BonusId;
        BonusOffer Bonus;
        Cafe Cafe;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.Bonus);

            BonusId = Intent.GetStringExtra(MainActivity.C_BONUS_ID);
            Bonus = Data.GetBonusOffer(new Guid(BonusId));
            Cafe = Data.GetCafe(Bonus.CafeId);

            FindViewById<TextView>(Resource.Id.baBonusDescrTV).Text = Bonus.Description;
            FindViewById<TextView>(Resource.Id.baAddressTV).Text = Cafe.FullAddress;
            FindViewById<TextView>(Resource.Id.baCafeNameTV).Text = Cafe.Name;
            FindViewById<TextView>(Resource.Id.baPriceTV).Text = Bonus.Price.ToString();

            // TODO: Load image
            ImageLoader imageLoader = ImageLoader.Instance;
            //imageLoader.DisplayImage(Cafe.Logo, FindViewById<ImageView>(Resource.Id.baCafeNameIV));
            //imageLoader.DisplayImage(Cafe.Image, FindViewById<ImageView>(Resource.Id.baCafeImageIV));

            var want = FindViewById<Button>(Resource.Id.baWantB);
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

            if (Rest.RequestOffer(accessToken, Bonus.Id)) {
                Data.UserInfo.Points -= (int)Bonus.Price;
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

            var want = FindViewById<Button>(Resource.Id.baWantB);
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