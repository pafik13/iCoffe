using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace iCoffe.Droid
{
    [Activity(Label = "BonusActivity")]
    public class BonusActivity : Activity
    {
        bool IgnoreBackPress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            SetContentView(Resource.Layout.Bonus);

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

            var locker = FindViewById<RelativeLayout>(Resource.Id.locker);
            locker.Visibility = ViewStates.Visible;

            var lock_message = FindViewById<TextView>(Resource.Id.lock_message);
            lock_message.Text = @"Приобретено";

            var want = FindViewById<Button>(Resource.Id.baWantB);
            want.Enabled = false;

            ThreadPool.QueueUserWorkItem(state =>
            {
                Thread.Sleep(5000);

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