using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;

using iCoffe.Shared;

namespace iCoffe.Droid.Adapters
{
    public class BonusOffersAdapter : BaseAdapter<BonusOffer>
    {
        Activity context = null;
        IList<BonusOffer> offers = new List<BonusOffer>();

        public BonusOffersAdapter(Activity context, IList<BonusOffer> offers) : base()
        {
            this.context = context;
            this.offers = offers;
        }

        public override BonusOffer this[int position]
        {
            get { return offers[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return offers.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for position
            var item = offers[position];

            var view = (convertView ??
                                context.LayoutInflater.Inflate(
                                Resource.Layout.NetItem,
                                parent,
                                false)) as LinearLayout;
            view.FindViewById<TextView>(Resource.Id.niText).Text = string.IsNullOrEmpty(item.Title) ? "<unknow offer>" : item.Title;

            //Finally return the view
            return view;
        }
    }
}