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
    //https://forums.xamarin.com/discussion/19362/xamarin-forms-splashscreen-in-android
    [Activity(Label = "iCoffee", Theme = "@style/MyTheme.Splash", Icon = "@drawable/Icon", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            StartActivity(new Intent(this, typeof(MainActivity)));
        }
    }
}