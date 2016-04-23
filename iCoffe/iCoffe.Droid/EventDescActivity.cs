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

using iCoffe.Shared;

namespace iCoffe.Droid
{
    [Activity(Label = "EventDescActivity")]
    public class EventDescActivity : Activity
    {
        int  objId;
        GeolocComplexObj complexObj;

        // Event info
        TextView eventName;
        TextView eventAddresses;
        TextView eventOperTimes;
        TextView eventSite;
        TextView eventPhone;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.EventDesc);

            eventName = FindViewById<TextView>(Resource.Id.edEventNameTV);
            eventAddresses = FindViewById<TextView>(Resource.Id.edAddressesTV);
            eventOperTimes = FindViewById<TextView>(Resource.Id.edOperTimesTV);
            eventSite = FindViewById<TextView>(Resource.Id.edSiteTV);
            eventPhone = FindViewById<TextView>(Resource.Id.edPhoneTV);

            objId = Intent.GetIntExtra(@"ObjId", -1);
            if (objId != -1)
            {
                complexObj = Data.Get(objId);

                eventName.Text = complexObj.Obj.Label != string.Empty ? @"L: " + complexObj.Obj.Label :
                                 complexObj.Obj.Title != string.Empty ? @"T: " + complexObj.Obj.Title : @"<No Label, No Title>";

                eventAddresses.Text = complexObj.Obj.Adress != string.Empty ? complexObj.Obj.Adress : @"<No address>";

                eventOperTimes.Text = @"<No OperTimes>";

                eventSite.Text = @"<No Site>";

                eventPhone.Text = @"<No Phone>";
            }
            SetResult(Result.Ok);
        }
    }
}