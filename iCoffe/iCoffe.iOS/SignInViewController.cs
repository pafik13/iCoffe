using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using iCoffe.Shared;

namespace iCoffe.iOS
{
	public partial class SignInViewController : UIViewController
	{
		public const string C_USER = @"C_USER";

		public ViewController Parent;

		public string EmailText;
		public string PasswordText;

		public SignInViewController (IntPtr handle) : base (handle)
		{
		}

		#region LifeCycle

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			//NSUserDefaults.StandardUserDefaults.SetString(@"q12345", @"lp@m.ru");
			//NSUserDefaults.StandardUserDefaults.SetString(Data.SerializeUser(new User() { Email = @"lp@m.ru", FirstName = @"Pavel", LastName = @"Lyubin", City = @"Moscow" }), @"lp@m.ru" + C_USER);
			//NSUserDefaults.StandardUserDefaults.SetString(@"q12345", @"ii@m.ru");
			//NSUserDefaults.StandardUserDefaults.SetString(Data.SerializeUser(new User() { Email = @"ii@m.ru", FirstName = @"Ivan", LastName = @"Ivanov", City = @"Tver'" }), @"ii@m.ru" + C_USER);
			//NSUserDefaults.StandardUserDefaults.SetString(@"q12345", @"pp@m.ru");
			//NSUserDefaults.StandardUserDefaults.SetString(Data.SerializeUser(new User() { Email = @"pp@m.ru", FirstName = @"Petr", LastName = @"Petrov", City = @"Tula" }),@"pp@m.ru" + C_USER);
			//NSUserDefaults.StandardUserDefaults.SetString(@"q12345", @"ss@m.ru");
			//NSUserDefaults.StandardUserDefaults.SetString(Data.SerializeUser(new User() { Email = @"ss@m.ru", FirstName = @"Sidr", LastName = @"Sidrov", City = @"Pushino" }), @"ss@m.ru" + C_USER);
			//NSUserDefaults.StandardUserDefaults.Synchronize();
			if (string.IsNullOrEmpty(EmailText) && string.IsNullOrEmpty(PasswordText)) return;

			Email.Text = EmailText;
			Password.Text = PasswordText;
			Sign();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			EmailText = string.Empty;
			PasswordText = string.Empty;

			UIGraphics.BeginImageContext(View.Frame.Size);
			//TODO: if this is an iphone5, then use an iphone5 pic (the default 2x picture would not have the same aspect ratio)
			UIImage i = UIImage.FromFile("fon");
			i = i.Scale(View.Frame.Size);

			View.BackgroundColor = UIColor.FromPatternImage(i);

			EmailView.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.4f);
			PasswordView.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.4f);

			SignIn.Layer.CornerRadius = 8.0f;
			SignIn.Layer.MasksToBounds = true;

			SignUp.Layer.CornerRadius = 8.0f;
			SignUp.Layer.MasksToBounds = true;
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			if (segue.DestinationViewController is SignUpController)
			{
				(segue.DestinationViewController as SignUpController).Parent = this;
			}
		}

		#endregion

		partial void SignButtonTouchDown (UIButton sender)
		{
			Sign();
		}

		void Sign()
		{
			if (string.IsNullOrEmpty(Email.Text))
			{
				ShowMessage(@"Ошибка", @"Введите, пожалуйста, <E-mail>");
			}
			else
			{
				if (string.IsNullOrEmpty(Password.Text))
				{
					ShowMessage(@"Ошибка", @"Введите, пожалуйста, <Пароль>");
				}
				else
				{
					string accessToken = Rest.GetAccessToken(Email.Text, Password.Text);
					if (!string.IsNullOrEmpty(accessToken))
					{
						//ShowMessage(@"Новый AccessToken:", accessToken);
						bool isSignedLater = NSUserDefaults.StandardUserDefaults.BoolForKey(Email.Text);
						NSUserDefaults.StandardUserDefaults.SetString(accessToken, ViewController.C_ACCESS_TOKEN);
						NSUserDefaults.StandardUserDefaults.SetBool(true, "isSigned");
						NSUserDefaults.StandardUserDefaults.SetBool(!isSignedLater, ViewController.C_IS_NEED_TUTORIAL);
						NSUserDefaults.StandardUserDefaults.Synchronize();

						if (Parent != null)
						{
							Parent.Map_Click();
						}
						DismissViewController(true, null);
					}
					else
					{
						ShowMessage(@"Ошибка", @"Неизвестная ошибка!");
					}
				}
			}
		}

		void ShowMessage(string title, string message)
		{
			var msgBox = UIAlertController.Create (title, message, UIAlertControllerStyle.Alert);
			msgBox.AddAction (UIAlertAction.Create ("OK", UIAlertActionStyle.Default, null));
			PresentViewController (msgBox, true, null); 
		}

		partial void SkipButtonTouchDown (UIButton sender)
		{
			NSUserDefaults.StandardUserDefaults.SetBool(true, "isSkiped");
			NSUserDefaults.StandardUserDefaults.Synchronize();
			DismissViewController(true, null);
			//throw new NotImplementedException ();
		}
	}
}
