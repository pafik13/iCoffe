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

namespace iCoffe.Droid.Adapters
{
    public class ComplexObjsAdapter : BaseAdapter<GeolocComplexObj>
    {
        Activity context = null;
        IList<GeolocComplexObj> objs = new List<GeolocComplexObj>();

        public ComplexObjsAdapter(Activity context, IList<GeolocComplexObj> objs) : base ()
		{
            this.context = context;
            this.objs = objs;
        }

        public override GeolocComplexObj this[int position]
        {
            get { return objs[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return objs.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for position
            var item = objs[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // gives us some performance gains by not always inflating a new view
            // will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()

            //			var view = (convertView ?? 
            //					context.LayoutInflater.Inflate(
            //					Resource.Layout.TaskListItem, 
            //					parent, 
            //					false)) as LinearLayout;
            //			// Find references to each subview in the list item's view
            //			var txtName = view.FindViewById<TextView>(Resource.Id.NameText);
            //			var txtDescription = view.FindViewById<TextView>(Resource.Id.NotesText);
            //			//Assign item's values to the various subviews
            //			txtName.SetText (item.Name, TextView.BufferType.Normal);
            //			txtDescription.SetText (item.Notes, TextView.BufferType.Normal);

            // TODO: use this code to populate the row, and remove the above view
            //var view = (convertView ??
            //    context.LayoutInflater.Inflate(
            //        Android.Resource.Layout.SimpleListItemChecked,
            //        parent,
            //        false)) as CheckedTextView;
            //view.SetText(item.Name == "" ? "<new net>" : item.Name, TextView.BufferType.Normal);
            //view.Checked = true;

            var view = (convertView ??
                                context.LayoutInflater.Inflate(
                                Resource.Layout.NetItem,
                                parent,
                                false)) as LinearLayout;
            view.FindViewById<TextView>(Resource.Id.niText).Text
                = item.Obj.Label != string.Empty ? @"L: " + item.Obj.Label :
                  item.Obj.Title != string.Empty ? @"T: " + item.Obj.Title : @"<No Label, No Title>";

            //Finally return the view
            return view;
        }
    }
}