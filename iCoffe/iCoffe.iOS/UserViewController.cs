using Foundation;
using System;
using System.Linq;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using iCoffe.Shared;

namespace iCoffe.iOS
{
	partial class UserViewController : UIViewController
	{
		IList<Gift> gifts;
		UserBonusTableSource source;

		public UserViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewWillAppear (bool animated)
		{
//			this.InvokeOnMainThread (delegate {
//				this.View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("fon"));
//			});;

			base.ViewWillAppear (animated);
			//UserInfo
			UpdateUserInfo();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.White.ColorWithAlpha (0.0f);
			GiftsTable.BackgroundColor = UIColor.White.ColorWithAlpha (0.0f); 

			UserInfo.BackgroundColor = UIColor.White.ColorWithAlpha (0.4f);
			//CityView.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.0f);

			UserMap.Layer.CornerRadius = 8.0f;
			UserMap.Layer.MasksToBounds = true;

			ExitButton.Layer.CornerRadius = 8.0f;
			ExitButton.Layer.MasksToBounds = true;

			source = new UserBonusTableSource (this, Data.BonusOffers.Take(5).ToList());

			GiftsTable.Source = source;
			// Gifts end


		}

		public void UpdateUserInfo()
		{
			bool isSigned = NSUserDefaults.StandardUserDefaults.BoolForKey("isSigned");

            UserId.Text = string.IsNullOrEmpty(Data.UserInfo.Login) ? @"<No Login>" : Data.UserInfo.Login;
            UserName.Text = string.IsNullOrEmpty(Data.UserInfo.FullUserName) ? @"<No FullUserName>" : Data.UserInfo.Login;
            Count.Text = Data.UserInfo == null ? @"<No Points>" : Data.UserInfo.Points.ToString();
		}

		partial void ExitButtonTouchDown (UIButton sender)
		{
			NSUserDefaults.StandardUserDefaults.SetBool(false, "isSigned");
			NSUserDefaults.StandardUserDefaults.Synchronize();
			ParentViewController.PerformSegue ("SignInSegue", ParentViewController);
		}
	}
}
