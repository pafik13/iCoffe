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

namespace iCoffe.Droid.Fragments
{
    public class GiftsFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.fragment, container, false);
            TextView tv = view.FindViewById<TextView>(Resource.Id.frTextView);
            // DisplayInfo
            DisplayMetrics dm = new DisplayMetrics();
            Activity.WindowManager.DefaultDisplay.GetMetrics(dm);
            string text = String.Empty;
            text += "Density : " + dm.Density.ToString() + System.Environment.NewLine;
            text += "DensityDpi : " + dm.DensityDpi.ToString() + System.Environment.NewLine;
            text += "ScaledDensity : " + dm.ScaledDensity.ToString() + System.Environment.NewLine;
            text += "WidthPixels : " + dm.WidthPixels.ToString() + System.Environment.NewLine;
            text += "HeightPixels : " + dm.HeightPixels.ToString() + System.Environment.NewLine;
            text += "Xdpi : " + dm.Xdpi.ToString() + System.Environment.NewLine;
            text += "Ydpi : " + dm.Ydpi.ToString() + System.Environment.NewLine;
            tv.Text = text;
            //tv.Text = @"Gifts";
            return view;
        }
    }
}