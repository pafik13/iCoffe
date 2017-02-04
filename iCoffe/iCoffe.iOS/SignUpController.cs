using Foundation;
using System;
using UIKit;

using tutCoffee.Shared;
using System.Net;

namespace tutCoffee.iOS
{
    public partial class SignUpController : UIViewController
    {
		public SignInViewController Parent;

        public SignUpController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			UIGraphics.BeginImageContext(View.Frame.Size);
			//TODO: if this is an iphone5, then use an iphone5 pic (the default 2x picture would not have the same aspect ratio)
			UIImage i = UIImage.FromFile("fon");
			i = i.Scale(View.Frame.Size);

			View.BackgroundColor = UIColor.FromPatternImage(i);

			EmailView.BackgroundColor = UIColor.White.ColorWithAlpha(0.4f);
			PasswordView.BackgroundColor = UIColor.White.ColorWithAlpha(0.4f);
			ConfirmView.BackgroundColor = UIColor.White.ColorWithAlpha(0.4f);

			Register.Layer.CornerRadius = 8.0f;
			Register.Layer.MasksToBounds = true;

			Back.Layer.CornerRadius = 8.0f;
			Back.Layer.MasksToBounds = true;
		}

		partial void RegisterTouchDown(UIButton sender)
		{
			//throw new NotImplementedException();
			if (IsInputAccepted())
			{
				TryRegister();
			}
		}

		private bool IsInputAccepted()
		{
			if (String.IsNullOrEmpty(Email.Text))
			{
				ShowMessage(@"Ошибка!", @"Введите, пожалуйста, <E-mail>.");
				return false;
			}

			if (String.IsNullOrEmpty(Password.Text))
			{
				ShowMessage(@"Ошибка!", @"Введите, пожалуйста, <Пароль>.");
				return false;
			}

			if (String.IsNullOrEmpty(Confirm.Text))
			{
				ShowMessage(@"Ошибка!", @"Введите, пожалуйста, <Подтверждение>.");
				return false;
			}

			if (String.Compare(Confirm.Text, Password.Text) != 0)
			{
				ShowMessage(@"Ошибка!", @"<Пароль> и <Подтверждение> не совпадают.");
				return false;
			}

			return true;
		}

		private void TryRegister()
		{
			var responseStatusCode = Rest.SignUp(Email.Text, Password.Text, Confirm.Text);
			if (responseStatusCode == HttpStatusCode.OK)
			{
				if (Parent != null) {
					Parent.EmailText = Email.Text;
					Parent.PasswordText = Password.Text;
					DismissViewController(true, null);
				}
			}
			else
			{
				ShowMessage(@"Ошибка!", @"Произошла ошибка. Попробуйте еще раз!");
			//	new AlertDialog.Builder(this)
			//		.SetTitle(Resource.String.error_caption)
			//		.SetMessage("Произошла ошибка. Попробуйте еще раз!")
			//		.SetCancelable(false)
			//		.SetPositiveButton(@"OK", (dialog, args) =>
			//		{
			//			if (dialog is Dialog)
			//			{
			//				((Dialog)dialog).Dismiss();
			//			}
			//		})
			//		.Show();
			}
		}

		partial void BackTouchDown(UIButton sender)
		{
			//throw new NotImplementedException();
			DismissViewController(true, null);
		}




		void ShowMessage(string title, string message)
		{
			var msgBox = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
			msgBox.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
			PresentViewController(msgBox, true, null);
		}
	}
}