using Foundation;
using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using UIKit;

using iCoffe.Shared;

namespace iCoffe.iOS
{
	partial class GiftsViewController : UIViewController
	{
		BonusesTableSource source;

		public string AccessToken;
		public bool IsDataUpdating;

		public GiftsViewController (IntPtr handle) : base (handle)
		{
		}

		#region View Lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.0f);
			Table.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.0f);

			UpdateBonuses();
		}

		#endregion

		public void UpdateBonuses()
		{
			//if (Data.BonusOffers != null)
			//{
				source = new BonusesTableSource(this, Data.BonusOffers ?? new List<BonusOffer>());

				Table.Source = source;
			//}
		}
	}
}
