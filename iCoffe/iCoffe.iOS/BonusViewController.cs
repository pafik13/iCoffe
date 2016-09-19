using Foundation;
using System;
using UIKit;

using SDWebImage;

using iCoffe.Shared;

namespace iCoffe.iOS
{
    public partial class BonusViewController : UIViewController
    {
		public BonusOffer Bonus;
		public Cafe Cafe;

        public BonusViewController (IntPtr handle) : base (handle)
        {
        }

		#region View Lifecycle

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			UIGraphics.BeginImageContext(View.Frame.Size);
			//TODO: if this is an iphone5, then use an iphone5 pic (the default 2x picture would not have the same aspect ratio)
			UIImage i = UIImage.FromFile("fon");
			i = i.Scale(View.Frame.Size);

			View.BackgroundColor = UIColor.FromPatternImage(i);

			Cafe = Data.GetCafe(Bonus.CafeId);

			// View
			TopView.BackgroundColor = UIColor.White.ColorWithAlpha(0.4f);

			MiddleLeftView.BackgroundColor = UIColor.White.ColorWithAlpha(0.0f);
			BonusDescription.BackgroundColor = UIColor.White.ColorWithAlpha(0.4f);

			MiddleRightView.BackgroundColor = UIColor.White.ColorWithAlpha(0.0f);
			CafeAddress.BackgroundColor = UIColor.White.ColorWithAlpha(0.4f);

			if (!string.IsNullOrEmpty(Cafe.LogoUrl))
			{
				CafeLogo.SetImage(
					url: new NSUrl(Cafe.LogoUrl),
					// TODO: Change to normal placeholder
					placeholder: UIImage.FromBundle("29_icon.png")
				);
			}

			CafeName.Text = Cafe.Name;

			if (!string.IsNullOrEmpty (Cafe.ImageUrl)) {
				CafeImage.SetImage (
					url: new NSUrl(Cafe.ImageUrl),
					// TODO: Change to normal placeholder
					placeholder: UIImage.FromBundle("29_icon.png")
				);
			}

			BonusPrice.Text = Bonus.Price.ToString();

			BonusDescription.Text = Bonus.Description;

			CafeAddress.Text = Cafe.FullAddress;

			Want.Layer.CornerRadius = 8.0f;
			Want.Layer.MasksToBounds = true;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			NSUserDefaults.StandardUserDefaults.SetBool(true, ViewController.C_WAS_BONUS_DESCRIPTION);
			NSUserDefaults.StandardUserDefaults.Synchronize();
		}

		#endregion

		void HandleSDWebImageCompletionHandler(UIImage image, NSError error, SDImageCacheType cacheType, NSUrl imageUrl)
		{
			Console.WriteLine(@"image:{0}; error:{1}; cacheType:{2}; imageUrl:{3}", image, error, cacheType, imageUrl);
		}


		partial void WantTouchDown(UIButton sender)
		{
			string accessToken = NSUserDefaults.StandardUserDefaults.StringForKey(ViewController.C_ACCESS_TOKEN);

			if (Rest.RequestOffer(accessToken, Bonus.Id))
			{
				Data.UserInfo.Points -= (int)Bonus.Price;
				Data.UserBonusOffers.Add(Bonus);
				//ShowMessage(@"Поздравляем", @"Приобретено!");
				NSUserDefaults.StandardUserDefaults.SetBool(true, ViewController.C_IS_NEED_UPDATE_USERINFO);
				NSUserDefaults.StandardUserDefaults.Synchronize();
				ShowOverlay(@"Поздравляем");
			}
			else
			{
				//ShowMessage(@"Неудача", @"Попробуйте позже");
				ShowOverlay(@"Неудача");
			}
		}

		void ShowMessage(string caption, string message)
		{
			var msgBox = UIAlertController.Create(caption, message, UIAlertControllerStyle.Alert);
			msgBox.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
			PresentViewController(msgBox, true, null);
		}

		void ShowOverlay(string message)
		{
			var overlay = new MessageOverlay(UIScreen.MainScreen.Bounds, message, false);
			View.AddSubview(overlay);
			System.Threading.ThreadPool.QueueUserWorkItem(state =>
			{
				System.Threading.Thread.Sleep(2000);

				if (overlay != null)
				{
					InvokeOnMainThread(() => overlay.Hide());
				}
			});
		}
    }
}