using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using SDWebImage;

using iCoffe.Shared;

namespace iCoffe.iOS
{
	partial class EventDescViewController : UIViewController
	{
		public GeolocObj Obj;
		public int ObjId = -1;

		public EventDescViewController (IntPtr handle) : base (handle)
		{

		}

		#region View Lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			UIGraphics.BeginImageContext(View.Frame.Size);
			//TODO: if this is an iphone5, then use an iphone5 pic (the default 2x picture would not have the same aspect ratio)
			UIImage i = UIImage.FromFile("fon");
			i = i.Scale(View.Frame.Size);

			View.BackgroundColor = UIColor.FromPatternImage(i);

			if (Obj == null) {
				if (ObjId != -1) {
					Obj = Data.GetObj (ObjId);
				}
			}

			// View
			TopView.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.4f);

			MiddleLeftView.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.0f);
			EventDesc.BackgroundColor = UIColor.White.ColorWithAlpha((nfloat)0.4f);

			MiddleRightView.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.0f);
			Addresses.BackgroundColor = UIColor.White.ColorWithAlpha((nfloat)0.4f);


			//Contacts.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.4f);

			EventNameImage.SetImage(
				url: new NSUrl (Obj.Label), 
				placeholder: UIImage.FromBundle ("29_icon.png")
			);
			//imageLoader.DisplayImage(@"http://geolocwebapi.azurewebsites.net/images/Traveler.jpg", eventLogo);

			//imageLoader.DisplayImage(@"http://cdn01.travelerscoffee.ru/files/joints/photos/41cfd6cd60ac93a0ff4748694c1f01b68794a33c%D0%A4%D0%BB%D0%B0%D0%B3%D0%BC%D0%B0%D0%BD%20(1).jpg", eventImage);
			
			EventNameLabel.Text = Obj.Title != string.Empty ? Obj.Title : @"<No Title>";

			if (!string.IsNullOrEmpty (Obj.ImageURL)) {
				EventImage.SetImage (
					//url: new NSUrl (@"https://encrypted-tbn1.gstatic.com/images?q=tbn:ANd9GcTN0xl3izJ71Pvcy6eP2xkE2_XetRArOYIojIqLIvtuZ4bL40DiAw")
					//url: new NSUrl (@"https://cdn01.travelerscoffee.ru/files/main_slider/1461595992atyrau_web.jpg")
					url: new NSUrl (Obj.ImageURL) 
				);
			} else {
				EventImage.SetImage (new NSUrl (@"https://pixabay.com/static/uploads/photo/2015/02/18/12/17/coffee-640647_960_720.jpg"));
			}

			if (string.IsNullOrEmpty(Obj.Descr)) {
				EventDesc.Text = Obj.Descr;
			}

			Addresses.Text = Obj.Adress != string.Empty ? Obj.Adress : @"<No address>";

			//OperTimes.Text = @"<No OperTimes>";

			//Contacts.Text = Obj.Descr;
			Want.Layer.CornerRadius = 8.0f;
			Want.Layer.MasksToBounds = true;
		}

		#endregion

		partial void WantTouchDown(UIButton sender)
		{
			//throw new NotImplementedException();
			ShowMessage(@"Приобретено!");
		}

		void ShowMessage(string message)
		{
			var msgBox = UIAlertController.Create("Поздравляем", message, UIAlertControllerStyle.Alert);
			msgBox.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
			PresentViewController(msgBox, true, null);
		}
	}
}
