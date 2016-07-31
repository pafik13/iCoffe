using System;

using UIKit;

using Foundation;

using iCoffe.Shared;

namespace iCoffe.iOS
{
	public enum AvailbleTabs
	{
		atGifts,
		atMap,
		atUser
	}

	public partial class ViewController : UIViewController
	{
		int count = 1;

		UIColor Selected = UIColor.White.ColorWithAlpha ((nfloat)0.3f);
		UIColor NonSelected = UIColor.White.ColorWithAlpha ((nfloat)0.6f);

		AvailbleTabs currentTab;

		UserViewController UserVC;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
//			NSUserDefaults.StandardUserDefaults.SetBool(false, "isSigned");
//			NSUserDefaults.StandardUserDefaults.Synchronize();
			NavigationController.SetNavigationBarHidden (true, false);
			bool isSigned = NSUserDefaults.StandardUserDefaults.BoolForKey ("isSigned");
			bool isSkiped = NSUserDefaults.StandardUserDefaults.BoolForKey ("isSkiped");

			if (!isSigned && !isSkiped) {
				PerformSegue ("SignInSegue", this);
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			NSUserDefaults.StandardUserDefaults.SetBool(false, "isSkiped");
			NSUserDefaults.StandardUserDefaults.Synchronize();
			//Title.BackBarButtonItem.BackGr
			//vMap.Touche
			//myImage.Image = UIImage.FromBundle ("ic_place_white_48pt");
			// Perform any additional setup after loading the view, typically from a nib.
//			Button.AccessibilityIdentifier = "myButton";
//			Button.TouchUpInside += delegate {
//				var title = string.Format ("{0} clicks!", count++);
//				Button.SetTitle (title, UIControlState.Normal);
//			};
//			View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("fon"));

			UIGraphics.BeginImageContext(vMain.Frame.Size);
			//TODO: if this is an iphone5, then use an iphone5 pic (the default 2x picture would not have the same aspect ratio)
			UIImage i = UIImage.FromFile("fon");
			i = i.Scale(vMain.Frame.Size);

			vMain.BackgroundColor = UIColor.FromPatternImage(i);

//			nfloat red   = (nfloat)158.0f;
//			nfloat green = (nfloat)158.0f;
//			nfloat blue  = (nfloat)158.0f;
//			nfloat alpha = (nfloat)0.6f;
//			UIColor.FromRGBA (red, green, blue, alpha);
			vGifts.BackgroundColor = NonSelected;
			vMap.BackgroundColor = NonSelected;
			vUser.BackgroundColor = NonSelected;
			Map_Click ();
			ContainerGifts.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.2f);
			//ContainerGifts.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.2f);


			foreach (var item in ChildViewControllers) {
				if (item is UserViewController) {
					UserVC = (item as UserViewController);
				}
			}
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			UITouch touch = touches.AnyObject as UITouch;

			if (touch != null) {
				if (vMap.Frame.Contains (touch.LocationInView (vMain))) {
					//bgColor = vMap.BackgroundColor;
					//vMap.BackgroundColor = UIColor.Green; 
					//ShowMessage ();
				}
			}
		
		}

		public override void TouchesEnded (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			UITouch touch = touches.AnyObject as UITouch;

			if (touch != null) {
				if (vGifts.Frame.Contains (touch.LocationInView (vMain))) {
					if (currentTab != AvailbleTabs.atGifts) {
						Gifts_Click();
					}
				}
				if (vMap.Frame.Contains (touch.LocationInView (vMain))) {
					if (currentTab != AvailbleTabs.atMap) {
						Map_Click();
					}
				}
				if (vUser.Frame.Contains (touch.LocationInView (vMain))) {
					if (currentTab != AvailbleTabs.atUser) {
						User_Click();
					}
				}
//				UIView.Animate (1.0, ()=>{
//					vMap.BackgroundColor = UIColor.Red;
//				});
			}
		}

		void Gifts_Click()
		{
			UIView.Animate (
				0.5,
				() => {
					ContainerGifts.Alpha = 1;
					ivGifts.Image = UIImage.FromBundle("ic_bonus_red_48pt");

					ContainerMap.Alpha = 0;
					ivMap.Image = UIImage.FromBundle("ic_map_white_48pt");

					ContainerUser.Alpha = 0;
					ivUser.Image = UIImage.FromBundle("ic_user_white_48pt");
				},
				() => {
					Console.WriteLine("Gifts_Click ended");
				}
			);
			currentTab = AvailbleTabs.atGifts;

		}

		public void Map_Click()
		{
			UIView.Animate (
				0.5,
				() => {
					ContainerGifts.Alpha = 0;
					ivGifts.Image = UIImage.FromBundle("ic_bonus_white_48pt");

					ContainerMap.Alpha = 1;
					ivMap.Image = UIImage.FromBundle("ic_map_red_48pt");

					ContainerUser.Alpha = 0;
					ivUser.Image = UIImage.FromBundle("ic_user_white_48pt");
				},
				() => {
					Console.WriteLine("Map_Click ended");
				}
			);

			currentTab = AvailbleTabs.atMap;
		}

		void User_Click()
		{
			if (UserVC != null) {
				UserVC.UpdateUserInfo ();
			}

			UIView.Animate (
				0.5,
				() => {
					ContainerGifts.Alpha = 0;
					ivGifts.Image = UIImage.FromBundle("ic_bonus_white_48pt");

					ContainerMap.Alpha = 0;
					ivMap.Image = UIImage.FromBundle("ic_map_white_48pt");

					ContainerUser.Alpha = 1;
					ivUser.Image = UIImage.FromBundle("ic_user_red_48pt");
				},
				() => {
					Console.WriteLine("User_Click ended");
				}
			);
			currentTab = AvailbleTabs.atUser;
		}

		void ShowMessage()
		{
			UIAlertView alert = new UIAlertView () { 
				Title = "alert title", Message = string.Format ("{0} clicks!", count++)
			};
			alert.AddButton("OK");
			alert.Show ();
		}
	}
}

