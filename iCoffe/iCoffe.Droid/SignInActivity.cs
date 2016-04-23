using System;
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
    [Activity(Label = "iCoffe.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class SignInActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SignIn);

            FindViewById<Button>(Resource.Id.siSkipBtn).Click += SkipBtn_Click;
        }

        private void SkipBtn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            StartActivity(typeof(MainActivity));
        }
    }
}