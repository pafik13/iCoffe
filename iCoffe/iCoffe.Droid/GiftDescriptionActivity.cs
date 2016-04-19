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
    [Activity(Label = "GiftDescriptionActivity")]
    public class GiftDescriptionActivity : Activity
    {
        LinearLayout mainLL;
        int  objId;
        GeolocComplexObj complexObj;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.GiftDescription);

            mainLL = FindViewById<LinearLayout>(Resource.Id.gdMainLL);

            objId = Intent.GetIntExtra(@"ObjId", -1);
            if (objId != -1)
            {
                complexObj = Data.Get(objId);

                var lbl = new TextView(this);
                lbl.Text = complexObj.Obj.Label;
                mainLL.AddView(lbl);

                var tl = new TextView(this);
                tl.Text = complexObj.Obj.Title;
                mainLL.AddView(tl);

                var desc = new TextView(this);
                desc.Text = complexObj.Obj.Descr;
                mainLL.AddView(desc);

                var adr = new TextView(this);
                adr.Text = complexObj.Obj.Adress;
                mainLL.AddView(adr);
            }
        }
    }
}