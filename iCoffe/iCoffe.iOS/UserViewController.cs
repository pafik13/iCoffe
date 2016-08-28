using Foundation;
using System;
using System.Linq;
using System.CodeDom.Compiler;
using UIKit;
using System.Collections.Generic;
using iCoffe.Shared;
using CoreLocation;
using MapKit;

namespace iCoffe.iOS
{
	partial class UserViewController : UIViewController
	{
		UserBonusTableSource source;


		public UserViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
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
			UserMap.ShowsUserLocation = true;
			UserMap.DidUpdateUserLocation += (sender, e) =>
			{
				if (UserMap.UserLocation != null)
				{
					CLLocationCoordinate2D coords = UserMap.UserLocation.Coordinate;
					MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees(2), MilesToLongitudeDegrees(2, coords.Latitude));
					UserMap.Region = new MKCoordinateRegion(coords, span);
				}
			};

			ExitButton.Layer.CornerRadius = 8.0f;
			ExitButton.Layer.MasksToBounds = true;

		}

		public void UpdateUserBonuses()
		{
			if (Data.UserBonusOffers != null)
			{
				source = new UserBonusTableSource(this, Data.UserBonusOffers);

				GiftsTable.Source = source;
			}
		}

		public void UpdateUserInfo()
		{
			if (Data.UserInfo != null)
			{
				UserID.Text = string.IsNullOrEmpty(Data.UserInfo.Login) ? @"<No Login>" : Data.UserInfo.Login;
				UserName.Text = string.IsNullOrEmpty(Data.UserInfo.FullUserName) ? @"<No FullUserName>" : Data.UserInfo.Login;
				Points.Text = Data.UserInfo == null ? @"<No Points>" : Data.UserInfo.Points.ToString();
			}
		}

		partial void ExitButtonTouchDown (UIButton sender)
		{
			NSUserDefaults.StandardUserDefaults.SetBool(false, "isSigned");
			NSUserDefaults.StandardUserDefaults.Synchronize();
			ParentViewController.PerformSegue ("SignInSegue", ParentViewController);
		}

		public double MilesToLatitudeDegrees(double miles)
		{
			double earthRadius = 3960.0; // in miles
			double radiansToDegrees = 180.0 / Math.PI;
			return (miles / earthRadius) * radiansToDegrees;
		}

		public double MilesToLongitudeDegrees(double miles, double atLatitude)
		{
			double earthRadius = 3960.0; // in miles
			double degreesToRadians = Math.PI / 180.0;
			double radiansToDegrees = 180.0 / Math.PI;
			// derive the earth's radius at that point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (miles / radiusAtLatitude) * radiansToDegrees;
		}
	}
}
