using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using SDiag = System.Diagnostics;

using UIKit;

using Foundation;
using CoreLocation;

using iCoffe.Shared;

namespace iCoffe.iOS
{
	public enum AvailbleTabs
	{
		atNone,
		atGifts,
		atMap,
		atUser
	}

	public partial class ViewController : UIViewController
	{
		MessageOverlay loadingOverlay;

		// Consts
		public const string C_WAS_BONUS_DESCRIPTION = @"C_WAS_BONUS_DESCRIPTION";
		public const string C_IS_USER_SIGN_IN = @"C_IS_USER_SIGN_IN";
		public const string C_DEFAULT_PREFS = @"I_COFFEE";
		public const string C_ACCESS_TOKEN = @"ACCESS_TOKEN";
		public const string C_IS_NEED_TUTORIAL = @"C_IS_NEED_TUTORIAL";
		public const string C_IS_NEED_UPDATE_USERINFO = @"C_IS_NEED_UPDATE_USERINFO";

		bool CanClick;

		//int count = 1;

		UIColor Selected = UIColor.FromRGB(250, 10, 59);
		UIColor NonSelected = UIColor.White.ColorWithAlpha (0.4f);

		AvailbleTabs currentTab;
		GiftsViewController BonusesVC;
		MapViewController MapVC;
		UserViewController UserVC;

		LocationManager Manager;

		// Data Service
		private CancellationTokenSource CancelSource;
		private CancellationToken CancelToken;

		// User Desc
		CancellationTokenSource CSUserDesc;
		CancellationToken CTUserDesc;

		public ViewController (IntPtr handle) : base (handle)
		{
			Manager = new LocationManager();
			Manager.StartLocationUpdates();

			//ContainerTutor1.Alpha = 0;
			//ContainerTutor2.Alpha = 0;
			//ContainerGifts.Alpha = 0;
			//ContainerMap.Alpha = 1;
			//ContainerUser.Alpha = 0;
		}

		#region Public Methods
		public void HandleLocationChanged(object sender, LocationUpdatedEventArgs e)
		{
			// Handle foreground updates
			//CLLocation location = e.Location;

			//LblAltitude.Text = location.Altitude + " meters";
			//LblLongitude.Text = location.Coordinate.Longitude.ToString();
			//LblLatitude.Text = location.Coordinate.Latitude.ToString();
			//LblCourse.Text = location.Course.ToString();
			//LblSpeed.Text = location.Speed.ToString();

			Console.WriteLine("foreground updated");
		}

		//This will keep going in the background and the foreground
		public void PrintLocation(object sender, LocationUpdatedEventArgs e)
		{
			CLLocation location = e.Location;
			Console.WriteLine("Altitude: " + location.Altitude + " meters");
			Console.WriteLine("Longitude: " + location.Coordinate.Longitude);
			Console.WriteLine("Latitude: " + location.Coordinate.Latitude);
			Console.WriteLine("Course: " + location.Course);
			Console.WriteLine("Speed: " + location.Speed);
		}
		#endregion

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			if (CTUserDesc.CanBeCanceled && CSUserDesc != null)
			{
				CSUserDesc.Cancel();
			}

			CSUserDesc = new CancellationTokenSource();
			CTUserDesc = CSUserDesc.Token;

			var dueTime = TimeSpan.FromSeconds(30);
			var interval = TimeSpan.FromSeconds(5);

			// TODO: Add a CancellationTokenSource and supply the token here instead of None.
			Rest.RunPeriodicAsync(OnTick, dueTime, interval, CTUserDesc);

			///
			/// Main code
			///
			NavigationController.SetNavigationBarHidden (true, false);
			bool isSigned = NSUserDefaults.StandardUserDefaults.BoolForKey ("isSigned");
			bool isSkiped = NSUserDefaults.StandardUserDefaults.BoolForKey ("isSkiped");

			if (!isSigned && !isSkiped)
			{
				PerformSegue("SignInSegue", this);
			}
			else
			{
				bool isNeedTutorial = NSUserDefaults.StandardUserDefaults.BoolForKey(C_IS_NEED_TUTORIAL);
				if (isNeedTutorial)
				{
					CanClick = false;
					ContainerTutor1.Alpha = 1;
					ContainerTutor2.Alpha = 0;
					ContainerGifts.Alpha = 0;
					ContainerMap.Alpha = 0;
					ContainerUser.Alpha = 0;

					// Data Update
					//UpdateGlobalData();
					NSUserDefaults.StandardUserDefaults.SetBool(false, C_IS_NEED_TUTORIAL);
				}
				else {
					CanClick = true;

					if (currentTab == AvailbleTabs.atNone) {
						Map_Click();
					}

					//NSUserDefaults.StandardUserDefaults.SetBool(false, C_WAS_BONUS_DESCRIPTION);

					bool isNeedUpdateUserinfo = NSUserDefaults.StandardUserDefaults.BoolForKey(C_IS_NEED_UPDATE_USERINFO);

					if (isNeedUpdateUserinfo)
					{
						if (UserVC != null)
						{
							UserVC.UpdateUserInfo();
						}
					}

					NSUserDefaults.StandardUserDefaults.SetBool(false, C_IS_NEED_UPDATE_USERINFO);
				}
				NSUserDefaults.StandardUserDefaults.Synchronize();
			}
		}

		void OnTick()
		{
			// TODO: Your code here
			SDiag.Debug.Print("OnTick. Date: {0}, Thread: {1}", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
			string accessToken = NSUserDefaults.StandardUserDefaults.StringForKey(C_ACCESS_TOKEN);
			SDiag.Debug.Print("accessToken " + accessToken);
			Rest.GetUserInfo(accessToken);
			InvokeOnMainThread(() =>
			{
				UserVC.UpdateUserInfo();
			});		
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			if (CTUserDesc.CanBeCanceled && CSUserDesc != null)
			{
				CSUserDesc.Cancel();
			}

			if (CancelToken.CanBeCanceled && CancelSource != null)
			{
				CancelSource.Cancel();
			}
		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			NSUserDefaults.StandardUserDefaults.SetBool(false, "isSkiped");
			NSUserDefaults.StandardUserDefaults.Synchronize();

			ContainerTutor1.Alpha = 0;
			ContainerTutor2.Alpha = 0;
			ContainerGifts.Alpha = 0;
			ContainerMap.Alpha = 0;
			ContainerUser.Alpha = 0;

			UIGraphics.BeginImageContext(vMain.Frame.Size);
			//TODO: if this is an iphone5, then use an iphone5 pic (the default 2x picture would not have the same aspect ratio)
			UIImage i = UIImage.FromFile("fon");
			i = i.Scale(vMain.Frame.Size);

			vMain.BackgroundColor = UIColor.FromPatternImage(i);

			vGifts.BackgroundColor = NonSelected;
			vMap.BackgroundColor = NonSelected;
			vUser.BackgroundColor = NonSelected;

			ContainerGifts.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.2f);

			foreach (var item in ChildViewControllers) {
				if (item is UserViewController) {
					UserVC = (item as UserViewController);
				}
				if (item is GiftsViewController)
				{
					BonusesVC = (item as GiftsViewController);
				}
				if (item is MapViewController)
				{
					MapVC = (item as MapViewController);
				}
			}

			// Location
			Manager.LocationUpdated += PrintLocation;
			UIApplication.Notifications.ObserveDidEnterBackground((sender, args) =>
			{
				Manager.LocationUpdated -= HandleLocationChanged;

				if (CancelToken.CanBeCanceled && CancelSource != null)
				{
					CancelSource.Cancel();
				}

				if (loadingOverlay != null)
				{
					InvokeOnMainThread(() => loadingOverlay.Hide());
				}
			});

			//MapVC.UserLocationUpdated += (object sender, UserLocationUpdatedEventArgs e) =>
			Manager.LocationUpdated += (sender, e) => 
			{
				if (CancelToken.CanBeCanceled && CancelSource != null)
				{
					if (loadingOverlay != null)
					{
						InvokeOnMainThread(() => loadingOverlay.Hide());
					}
					CancelSource.Cancel();
				}

				CancelSource = new CancellationTokenSource();
				CancelToken = CancelSource.Token;
				var task = UpdateGlobalDataAsync(CancelToken, e.Location.Coordinate.Latitude, e.Location.Coordinate.Longitude, 40);
			};
		}

		public void UpdateGlobalData()
		{
			ThreadPool.QueueUserWorkItem(state =>
			{
				string accessToken = NSUserDefaults.StandardUserDefaults.StringForKey(C_ACCESS_TOKEN);
				int radius = 5; // in km

				SDiag.Debug.Print("Radius " + radius);
				SDiag.Debug.Print("accessToken " + accessToken);

				Data.BonusOffers = Rest.GetBonusOffers(accessToken, 54.974362, 73.418061, radius);
				SDiag.Debug.Print("Data.BonusOffers: " + Data.BonusOffers.Count);

				Data.Cafes = Rest.GetCafes(accessToken, 54.974362, 73.418061, radius);
				SDiag.Debug.Print("Data.Cafes: " + Data.Cafes.Count);

				Data.UserBonusOffers = Rest.GetUserBonusOffers(accessToken);
				SDiag.Debug.Print("Data.UserBonusOffers: " + Data.UserBonusOffers.Count);

				if (BonusesVC != null)
				{
					InvokeOnMainThread(() => BonusesVC.UpdateBonuses());
				}

				if (MapVC != null)
				{
					InvokeOnMainThread(() => MapVC.UpdateAnnotations());
				}

				if (UserVC != null)
				{
					InvokeOnMainThread(() =>
					{
						UserVC.UpdateUserInfo();
						UserVC.UpdateUserBonuses();
					});
				}

				SDiag.Debug.Print("GetCafesAndBonusOffers ended.");

				if (loadingOverlay != null)
				{
					InvokeOnMainThread(() => loadingOverlay.Hide());
				}
			});
		}

		async Task UpdateGlobalDataAsync(CancellationToken cancellationToken, double lat, double lon, int rad)
		{
			SDiag.Debug.Print("UpdateGlobalDataAsync started. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

			loadingOverlay = new MessageOverlay(UIScreen.MainScreen.Bounds, @"Обновление данных, ждите...");
			View.Add(loadingOverlay);

			SDiag.Debug.Print("Radius " + rad.ToString());
			string accessToken = NSUserDefaults.StandardUserDefaults.StringForKey(C_ACCESS_TOKEN);
			SDiag.Debug.Print("accessToken " + accessToken);
			Data.BonusOffers = new List<BonusOffer>();
			Data.Cafes = new List<Cafe>();
			Data.UserInfo = new UserInfo() { FullUserName = @"<нет данных>", Login = @"<нет данных>", Points = -1 };
			Data.UserBonusOffers = new List<BonusOffer>();

			UpdateScreens();

			var offers = await Rest.GetBonusOffersAsync(accessToken, lat, lon, rad);
			SDiag.Debug.Print("UpdateGlobalDataAsync running. Offers. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

			var cafes = await Rest.GetCafesAsync(accessToken, lat, lon, rad);
			SDiag.Debug.Print("UpdateGlobalDataAsync running. Cafes Thread: {0}", Thread.CurrentThread.ManagedThreadId);

			var offrersCafeIds = offers.Select(i => i.CafeId).Distinct().ToArray();
			var offrersCafes = await Rest.GetCafesAsync(accessToken, offrersCafeIds);
			cafes.AddRange(offrersCafes);

			var userInfo = await Rest.GetUserInfoAsync(accessToken);
			SDiag.Debug.Print("UpdateGlobalDataAsync running. UserInfo. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

			var userBonuses = await Rest.GetUserBonusOffersAsync(accessToken);
			SDiag.Debug.Print("UpdateGlobalDataAsync running. UserBonusOffer. Thread: {0}", Thread.CurrentThread.ManagedThreadId);

			var userBonusesCafeIds = userBonuses.Select(i => i.CafeId).Distinct().ToArray();
			var cafesIds = cafes.Select(i => i.Id).Distinct().ToArray();
			var userCafes = await Rest.GetCafesAsync(accessToken, userBonusesCafeIds.Except(cafesIds).ToArray());
			cafes.AddRange(userCafes);
			SDiag.Debug.Print("UpdateGlobalDataAsync running. Cafes for UserBonusOffer. Thread: {0}", Thread.CurrentThread.ManagedThreadId);


			if (cancellationToken.IsCancellationRequested)
			{
				if (loadingOverlay != null)
				{
					InvokeOnMainThread(() => loadingOverlay.Hide());
				}
				// do something here as task was cancelled mid flight maybe just
				return;
			}

			Data.BonusOffers = offers;
			Data.Cafes = cafes;
			Data.UserInfo = userInfo;
			Data.UserBonusOffers = userBonuses;

			UpdateScreens();

			if (loadingOverlay != null)
			{
				InvokeOnMainThread(() => loadingOverlay.Hide());
			}

			SDiag.Debug.Print("UpdateGlobalDataAsync stopped. Thread: {0}", Thread.CurrentThread.ManagedThreadId);
		}

		void UpdateScreens()
		{
			if (BonusesVC != null)
			{
				InvokeOnMainThread(() => BonusesVC.UpdateBonuses());
			}

			if (MapVC != null)
			{
				InvokeOnMainThread(() => MapVC.UpdateAnnotations());
			}

			if (UserVC != null)
			{
				InvokeOnMainThread(() =>
				{
					UserVC.UpdateUserInfo();
					UserVC.UpdateUserBonuses();
				});
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
			if (!CanClick) return;

			//BonusesVC.UpdateBonuses();

			ContainerTutor1.Alpha = 0;
			ContainerTutor2.Alpha = 0;

			UIView.Animate (
				0.5,
				() => {
					ContainerGifts.Alpha = 1;
					ivGifts.Image = UIImage.FromBundle("ic_bonus_red_48pt");
					BonusLabel.TextColor = Selected; //UIColor.Red;

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

			//MapVC.UpdateAnnotations();

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
					MapLabel.TextColor = Selected; //UIColor.Red;

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
				//UserVC.ViewWillAppear(false);
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
					UserLabel.TextColor = Selected; //UIColor.Red;
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
			if (segue.DestinationViewController is Tutor1ViewController)
			{
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

