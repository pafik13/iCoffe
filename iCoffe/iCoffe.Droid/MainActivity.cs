using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using iCoffe.Shared;

namespace iCoffe.Droid
{
	[Activity (Label = "iCoffe.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

        List<CoffeeHouseNet> netsData = null;

        Fragment nets = null;
        Fragment map = null;
        Fragment gifts = null;

        // Location

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            RelativeLayout rlList = FindViewById<RelativeLayout>(Resource.Id.rlList);
            rlList.Click += List_Click;

            RelativeLayout rlMap = FindViewById<RelativeLayout>(Resource.Id.rlMap);
            rlMap.Click += Map_Click; ;

            RelativeLayout rlGifts = FindViewById<RelativeLayout>(Resource.Id.rlGifts);
            rlGifts.Click += Gifts_Click; ;

            netsData = new Data().Nets;
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            gifts = new Fragments.GiftsFragment();
            trans.Add(Resource.Id.mContentFL, gifts);
            //trans.Hide(gifts);
            map = new Fragments.MapFragment();
            trans.Add(Resource.Id.mContentFL, map);
            //trans.Hide(map);
            nets = new Fragments.NetsFragment();
            trans.Add(Resource.Id.mContentFL, nets);
            trans.Commit();
            List_Click(null, null);
            //LinearLayout ll = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            //foreach (var item in nets)
            //{
            //    ll.AddView (
            //        new TextView(this) {
            //            Text = item.Name,
            //            Gravity = GravityFlags.Center,
            //            LayoutParameters = new LinearLayout.LayoutParams (
            //                ViewGroup.LayoutParams.MatchParent, 
            //                ViewGroup.LayoutParams.WrapContent
            //            )
            //        }
            //    );
            //}
        }

        private void Gifts_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //if (gifts == null)
            //{
            //gifts = new Fragments.GiftsFragment();
            //}
            //FragmentManager.BeginTransaction().Replace(Resource.Id.mContentFL, gifts).Commit();
            FragmentManager.BeginTransaction().Hide(nets).Hide(map).Show(gifts).Commit();
        }

        private void Map_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //if (map == null)
            //{
            //map = new Fragments.MapFragment();
            //}
            //FragmentManager.BeginTransaction().Replace(Resource.Id.mContentFL, map).Commit();
            FragmentManager.BeginTransaction().Hide(nets).Hide(gifts).Show(map).Commit();
        }

        private void List_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //if (list == null)
            //{
            //list = new Fragments.NetsFragment();
            //}
            //FragmentManager.BeginTransaction().Replace(Resource.Id.mContentFL, list).Commit();
            //FragmentManager.BeginTransaction().
            FragmentManager.BeginTransaction().Hide(map).Hide(gifts).Show(nets).Commit();
        }
    }
}


