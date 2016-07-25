using System;
using System.Collections.Generic;

using Android.Views;
using Android.Support.V4.View;

namespace iCoffe.Droid.Adapters
{
    public class TutorialAdapter : PagerAdapter
    {
        List<View> pages;

        public TutorialAdapter(List<View> pages)
        {
            if (pages == null)
            {
                throw new ArgumentNullException(nameof(pages));
            }

            this.pages = pages;
        }

        public override Java.Lang.Object InstantiateItem(View container, int position)
        {
            View v = pages[position];
            ((ViewPager)container).AddView(v, 0);

            return v;
        }

        public override void DestroyItem(View container, int position, Java.Lang.Object objectValue)
        {
            ((ViewPager)container).RemoveView((View)objectValue);
        }

        public override int Count
        {
            get
            {
                return pages.Count;
            }
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
        {
            return view.Equals(objectValue);
        }
    }
}