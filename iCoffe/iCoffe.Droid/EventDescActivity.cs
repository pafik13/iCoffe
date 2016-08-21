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

using UniversalImageLoader.Core;

using iCoffe.Shared;

namespace iCoffe.Droid
{
    [Activity(Label = "EventDescActivity")]
    public class EventDescActivity : Activity
    {
        int objId;
        string BonusId;

        GeolocComplexObj complexObj;

        // Event info
        ImageView eventLogo;
        TextView eventName;
        ImageView eventImage;
        TextView eventAddresses;
        TextView eventOperTimes;
        TextView eventSite;
        TextView eventPhone;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EventDesc);

            eventLogo = FindViewById<ImageView>(Resource.Id.edEventNameIV);
            eventName = FindViewById<TextView>(Resource.Id.edEventNameTV);
            eventImage = FindViewById<ImageView>(Resource.Id.edEventImageIV);
            eventAddresses = FindViewById<TextView>(Resource.Id.edAddressesTV);
            eventOperTimes = FindViewById<TextView>(Resource.Id.edOperTimesTV);
            eventSite = FindViewById<TextView>(Resource.Id.edSiteTV);
            eventPhone = FindViewById<TextView>(Resource.Id.edPhoneTV);

            objId = Intent.GetIntExtra(@"ObjId", -1);
            if (objId != -1)
            {
                complexObj = Data.Get(objId);

                // Get singleton instance
                ImageLoader imageLoader = ImageLoader.Instance;

                // Load image
                imageLoader.DisplayImage(@"http://geolocwebapi.azurewebsites.net/images/Traveler.jpg", eventLogo);

                imageLoader.DisplayImage(@"http://cdn01.travelerscoffee.ru/files/joints/photos/41cfd6cd60ac93a0ff4748694c1f01b68794a33c%D0%A4%D0%BB%D0%B0%D0%B3%D0%BC%D0%B0%D0%BD%20(1).jpg", eventImage);

                eventName.Text = complexObj.Obj.Label != string.Empty ? @"L: " + complexObj.Obj.Label :
                                 complexObj.Obj.Title != string.Empty ? @"T: " + complexObj.Obj.Title : @"<No Label, No Title>";

                eventAddresses.Text = complexObj.Obj.Adress != string.Empty ? complexObj.Obj.Adress : @"<No address>";

                eventOperTimes.Text = @"<No OperTimes>";

                eventSite.Text = @"сайт: <No Site>";

                eventPhone.Text = @"тел.: <No Phone>";
            }
            SetResult(Result.Ok);
        }
    }
}