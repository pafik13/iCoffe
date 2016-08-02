using Foundation;
using System;
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

			View.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.0f);
			GiftsTable.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.0f); 

			UserInfo.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.4f);
			//CityView.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.0f);

			UserMap.Layer.CornerRadius = 8.0f;
			UserMap.Layer.MasksToBounds = true;

			ExitButton.Layer.CornerRadius = 8.0f;
			ExitButton.Layer.MasksToBounds = true;

			// Gifts
			gifts = new List<Gift>();
			gifts.Add(new Gift() { Name = @"Gift1" });
			gifts.Add(new Gift() { Name = @"Gift2" });
			gifts.Add(new Gift() { Name = @"Gift3" });
			gifts.Add(new Gift() { Name = @"Gift4" });
			gifts.Add(new Gift() { Name = @"Gift5" });

			source = new UserBonusTableSource (this, gifts);

			GiftsTable.Source = source;
			// Gifts end


		}

		public void UpdateUserInfo()
		{
			bool isSigned = NSUserDefaults.StandardUserDefaults.BoolForKey("isSigned");
			string storedUserInfo = NSUserDefaults.StandardUserDefaults.StringForKey (SignInViewController.C_USER);

			if (isSigned && !string.IsNullOrEmpty(storedUserInfo)) {
				User user = Data.DeserializeUser (storedUserInfo);
				UserName.Text = user.LastName + @" " + user.FirstName;
				//CityLabel.Text = user.City;;
			} else {
				UserName.Text = @"<Unknown User>";
				//CityLabel.Text = @"<Unknown City>";
			}
		}

		partial void ExitButtonTouchDown (UIButton sender)
		{
			//throw new NotImplementedException ();
			NSUserDefaults.StandardUserDefaults.SetString(string.Empty, SignInViewController.C_USER);
			NSUserDefaults.StandardUserDefaults.SetBool(false, "isSigned");
			NSUserDefaults.StandardUserDefaults.Synchronize();
			ParentViewController.PerformSegue ("SignInSegue", ParentViewController);
		}
	}
}
