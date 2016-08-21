using System;
using SDiag = System.Diagnostics;

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
		// Consts
		public const string C_WAS_STARTED_NEW_ACTIVITY = @"C_WAS_STARTED_NEW_ACTIVITY";
		public const string C_IS_USER_SIGN_IN = @"C_IS_USER_SIGN_IN";
		public const string C_DEFAULT_PREFS = @"I_COFFEE";
		public const string C_ACCESS_TOKEN = @"ACCESS_TOKEN";
		public const string C_IS_NEED_TUTORIAL = @"C_IS_NEED_TUTORIAL";

		bool CanClick;

		//int count = 1;

		//UIColor Selected = UIColor.White.ColorWithAlpha ((nfloat)0.3f);
		UIColor NonSelected = UIColor.White.ColorWithAlpha ((nfloat)0.4f);

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
			bool isNeedTutorial = NSUserDefaults.StandardUserDefaults.BoolForKey(C_IS_NEED_TUTORIAL)|| true;
			if (isNeedTutorial) {
				CanClick = false;
				ContainerTutor1.Alpha = 1;
				ContainerTutor2.Alpha = 0;
				ContainerGifts.Alpha = 0;
				ContainerMap.Alpha = 0;
				ContainerUser.Alpha = 0;
			} else {
				CanClick = true;
				Map_Click();
			}
			ContainerGifts.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.2f);
			//ContainerGifts.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.2f);


			foreach (var item in ChildViewControllers) {
				if (item is UserViewController) {
					UserVC = (item as UserViewController);
				}
			}

            System.Threading.ThreadPool.QueueUserWorkItem(state =>
            {
                int radius = 5; // in km
                SDiag.Debug.Print("Radius " + radius.ToString());
                string accessToken = NSUserDefaults.StandardUserDefaults.StringForKey(C_ACCESS_TOKEN);
                SDiag.Debug.Print("accessToken " + accessToken);
                Data.BonusOffers = Rest.GetBonusOffers(accessToken, 54.974362, 73.418061, radius);
                Data.Cafes = Rest.GetCafes(accessToken, 54.974362, 73.418061, radius);
                SDiag.Debug.Print("GetCafesAndBonusOffers stopped.");


            });
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
			if (!CanClick) return;

			ContainerTutor1.Alpha = 0;
			ContainerTutor2.Alpha = 0;

			UIView.Animate (
				0.5,
				() => {
					ContainerGifts.Alpha = 1;
					ivGifts.Image = UIImage.FromBundle("ic_bonus_red_48pt");
					BonusLabel.TextColor = UIColor.Red;

					ContainerMap.Alpha = 0;
					ivMap.Image = UIImage.FromBundle("ic_map_white_48pt");
					MapLabel.TextColor = UIColor.White;
					
					ContainerUser.Alpha = 0;
					ivUser.Image = UIImage.FromBundle("ic_user_white_48pt");
					UserLabel.TextColor = UIColor.White;
				},
				() => {
					Console.WriteLine("Gifts_Click ended");
				}
			);
			currentTab = AvailbleTabs.atGifts;

		}

		public void Map_Click()
		{
			if (!CanClick) return;

			ContainerTutor1.Alpha = 0;
			ContainerTutor2.Alpha = 0;

			UIView.Animate (
				0.5,
				() => {
					ContainerGifts.Alpha = 0;
					ivGifts.Image = UIImage.FromBundle("ic_bonus_white_48pt");
					BonusLabel.TextColor = UIColor.White;

					ContainerMap.Alpha = 1;
					ivMap.Image = UIImage.FromBundle("ic_map_red_48pt");
					MapLabel.TextColor = UIColor.Red;

					ContainerUser.Alpha = 0;
					ivUser.Image = UIImage.FromBundle("ic_user_white_48pt");
					UserLabel.TextColor = UIColor.White;
				},
				() => {
					Console.WriteLine("Map_Click ended");
				}
			);

			currentTab = AvailbleTabs.atMap;
		}

		void User_Click()
		{
			if (!CanClick) return;

			ContainerTutor1.Alpha = 0;
			ContainerTutor2.Alpha = 0;

			if (UserVC != null) {
				UserVC.UpdateUserInfo ();
			}

			UIView.Animate (
				0.5,
				() => {
					ContainerGifts.Alpha = 0;
					ivGifts.Image = UIImage.FromBundle("ic_bonus_white_48pt");
					BonusLabel.TextColor = UIColor.White;

					ContainerMap.Alpha = 0;
					ivMap.Image = UIImage.FromBundle("ic_map_white_48pt");
					MapLabel.TextColor = UIColor.White;

					ContainerUser.Alpha = 1;
					ivUser.Image = UIImage.FromBundle("ic_user_red_48pt");
					UserLabel.TextColor = UIColor.Red;
				},
				() => {
					Console.WriteLine("User_Click ended");
				}
			);
			currentTab = AvailbleTabs.atUser;
		}

		void ShowMessage(string message)
		{
			UIAlertView alert = new UIAlertView () { 
				Title = "alert title", Message = message//string.Format ("{0} clicks!", count++)
			};
			alert.AddButton("OK");
			alert.Show ();
		}

		public void Tutor1_Next_Click()
		{
			UIView.Animate(
				0.5,
				() =>
				{
					ContainerTutor1.Alpha = 0;
					ContainerTutor2.Alpha = 1;
				},
				() =>
				{
					Console.WriteLine("User_Click ended");
				}
			);
		}

		public void Tutor2_AllClear_Click()
		{
			CanClick = true;
			Map_Click();
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			//Console.WriteLine(@"PrepareForSegue in ViewController: " + (segue.DestinationViewController is Tutor2ViewController));
			if (segue.DestinationViewController is Tutor1ViewController) {
				(segue.DestinationViewController as Tutor1ViewController).Parent = this;
			}

			if (segue.DestinationViewController is Tutor2ViewController)
			{
				(segue.DestinationViewController as Tutor2ViewController).Parent = this;
			}

			if (segue.DestinationViewController is SignInViewController)
			{
				(segue.DestinationViewController as SignInViewController).Parent = this;
			}
		}
	}
}

