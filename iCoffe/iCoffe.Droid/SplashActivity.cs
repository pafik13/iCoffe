using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace iCoffe.Droid
{
    //https://forums.xamarin.com/discussion/19362/xamarin-forms-splashscreen-in-android
    [Activity(Label = "tutCoffee", Theme = "@style/MyTheme.Splash", Icon = "@drawable/NewIcon", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
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