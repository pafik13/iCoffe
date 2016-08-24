using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;

using iCoffe.Shared;

using UniversalImageLoader.Core;

namespace iCoffe.Droid.Adapters
{
    public class UserBonusAdapter : BaseAdapter<BonusOffer>
    {
        Activity context = null;
        IList<BonusOffer> offers = new List<BonusOffer>();

        public UserBonusAdapter(Activity context, IList<BonusOffer> offers) : base()
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
            var cafe = Data.GetCafe(item.CafeId);

            var view = (convertView ?? context.LayoutInflater.Inflate(Resource.Layout.UserBonusItem, parent,false)) as LinearLayout;

            view.FindViewById<TextView>(Resource.Id.ubiText).Text = string.IsNullOrEmpty(item.Title) ? "<unknow offer>" : item.Title;

            // TODO: Load image
            ImageLoader imageLoader = ImageLoader.Instance;
            if ((cafe != null) && !string.IsNullOrEmpty(cafe.LogoUrl))
                imageLoader.DisplayImage(cafe.LogoUrl, view.FindViewById<ImageView>(Resource.Id.ubiLogoIV));

            //Finally return the view
            return view;
        }
    }
}