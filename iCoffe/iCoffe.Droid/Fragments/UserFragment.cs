using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using iCoffe.Shared;
using iCoffe.Droid.Adapters;

namespace iCoffe.Droid.Fragments
{
    public class UserFragment : Fragment
    {
        User user;
        GiftsAdapter giftsAdapter;
        IList<Gift> gifts;
        ListView giftsListView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            user = new User() { City = @"<No City>", FirstName = @"<No First>", LastName = @"<No Last>" };

            ISharedPreferences prefs = Activity.GetSharedPreferences(@"icoffe", FileCreationMode.Private);
            string userSer = prefs.GetString(SignInActivity.C_USER, string.Empty);
            if (!string.IsNullOrEmpty(userSer))
            {
                user = Data.DeserializeUser(userSer);
            }

            //return base.OnCreateView(inflater, container, savedInstanceState);

            View view = inflater.Inflate(Resource.Layout.UserFragment, container, false);
            giftsListView = view.FindViewById<ListView>(Resource.Id.ufListView);

            view.FindViewById<TextView>(Resource.Id.ufUserNameTV).Text = user.LastName + @" " + user.FirstName;
            view.FindViewById<TextView>(Resource.Id.ufCityTV).Text = user.City;

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            // get data
            gifts = new List<Gift>();
            gifts.Add(new Gift() { Name = @"Gift1" });
            gifts.Add(new Gift() { Name = @"Gift2" });
            gifts.Add(new Gift() { Name = @"Gift3" });
            gifts.Add(new Gift() { Name = @"Gift4" });
            gifts.Add(new Gift() { Name = @"Gift5" });

            // create our adapter
            giftsAdapter = new GiftsAdapter(Activity, gifts);

            //Hook up our adapter to our ListView
            Activity.RunOnUiThread(() => giftsListView.Adapter = giftsAdapter);
        }
    }
}