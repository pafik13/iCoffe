using System.Collections.Generic;

using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Support.V4.View;

using Android.Content.PM;

using tutCoffee.Droid.Adapters;

namespace tutCoffee.Droid
{
    [Activity(Label = "TutorialActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TutorialActivity : Activity
    {
        Button allclear;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);

            // Create your application here
            LayoutInflater inflater = LayoutInflater.From(this);
            List<View> pages = new List<View>();

            //View page = inflater.Inflate(Resource.Layout.page, null);
            //TextView textView = page.FindViewById<TextView>(Resource.Id.text_view);
            //textView.Text = "Страница 1";
            //pages.Add(page);

            // 1st
            View main  = inflater.Inflate(Resource.Layout.Main, null);
            View frameLayout = main.FindViewById(Resource.Id.mContentFL);
            ViewGroup parent = (ViewGroup)frameLayout.Parent;
            int index = parent.IndexOfChild(frameLayout);
            parent.RemoveView(frameLayout);

            View tutor = inflater.Inflate(Resource.Layout.Tutorial_1_Fragment, null);
            
            parent.AddView(tutor, index);
            //textView = page.FindViewById<TextView>(Resource.Id.text_view);
            //textView.Text = "Страница 2";
            pages.Add(main);

            // 2nd
            main = inflater.Inflate(Resource.Layout.Main, null);
            frameLayout = main.FindViewById(Resource.Id.mContentFL);
            parent = (ViewGroup)frameLayout.Parent;
            index = parent.IndexOfChild(frameLayout);
            parent.RemoveView(frameLayout);

            tutor = inflater.Inflate(Resource.Layout.Tutorial_2_Fragment, null);
            allclear = tutor.FindViewById<Button>(Resource.Id.t2fAllclearB);
            allclear.Click += delegate
            {
                var sharedPreferences = GetSharedPreferences(MainActivity.C_DEFAULT_PREFS, FileCreationMode.Private);
                var sharedPreferencesEditor = sharedPreferences.Edit();
                sharedPreferencesEditor.PutBoolean(MainActivity.C_IS_NEED_TUTORIAL, false);
                sharedPreferencesEditor.Commit();
                Finish();
            };


            parent.AddView(tutor, index);
            //textView = page.FindViewById<TextView>(Resource.Id.text_view);
            //textView.Text = "Страница 3";
            pages.Add(main);

            TutorialAdapter pagerAdapter = new TutorialAdapter(pages);
            ViewPager viewPager = new ViewPager(this);
            viewPager.Adapter = pagerAdapter;
            //viewPager.CurrentItem = 1;

            SetContentView(viewPager);
        }
    }
}