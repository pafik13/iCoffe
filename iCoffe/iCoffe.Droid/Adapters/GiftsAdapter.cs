using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;

using iCoffe.Shared;

namespace iCoffe.Droid.Adapters
{
    public class GiftsAdapter : BaseAdapter<Gift>
    {
        Activity context = null;
        IList<Gift> gifts = new List<Gift>();

        public GiftsAdapter(Activity context, IList<Gift> gifts) : base()
        {
            this.context = context;
            this.gifts = gifts;
        }

        public override Gift this[int position]
        {
            get { return gifts[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return gifts.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for position
            var item = gifts[position];

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
                                Resource.Layout.GiftItem,
                                parent,
                                false)) as LinearLayout;
            view.FindViewById<TextView>(Resource.Id.niText).Text = item.Name == "" ? "<new net>" : item.Name;

            //Finally return the view
            return view;
        }
    }
}