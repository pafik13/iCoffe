using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using iCoffe.Shared;

namespace iCoffe.iOS
{
	partial class SignInViewController : UIViewController
	{
		public const string C_USER = @"C_USER";

		public SignInViewController (IntPtr handle) : base (handle)
		{
		}

		#region LifeCycle

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			NSUserDefaults.StandardUserDefaults.SetString(@"q12345", @"lp@m.ru");
			NSUserDefaults.StandardUserDefaults.SetString(Data.SerializeUser(new User() { Email = @"lp@m.ru", FirstName = @"Pavel", LastName = @"Lyubin", City = @"Moscow" }), @"lp@m.ru" + C_USER);
			NSUserDefaults.StandardUserDefaults.SetString(@"q12345", @"ii@m.ru");
			NSUserDefaults.StandardUserDefaults.SetString(Data.SerializeUser(new User() { Email = @"ii@m.ru", FirstName = @"Ivan", LastName = @"Ivanov", City = @"Tver'" }), @"ii@m.ru" + C_USER);
			NSUserDefaults.StandardUserDefaults.SetString(@"q12345", @"pp@m.ru");
			NSUserDefaults.StandardUserDefaults.SetString(Data.SerializeUser(new User() { Email = @"pp@m.ru", FirstName = @"Petr", LastName = @"Petrov", City = @"Tula" }),@"pp@m.ru" + C_USER);
			NSUserDefaults.StandardUserDefaults.SetString(@"q12345", @"ss@m.ru");
			NSUserDefaults.StandardUserDefaults.SetString(Data.SerializeUser(new User() { Email = @"ss@m.ru", FirstName = @"Sidr", LastName = @"Sidrov", City = @"Pushino" }), @"ss@m.ru" + C_USER);
			NSUserDefaults.StandardUserDefaults.Synchronize();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			UIGraphics.BeginImageContext(View.Frame.Size);
			//TODO: if this is an iphone5, then use an iphone5 pic (the default 2x picture would not have the same aspect ratio)
			UIImage i = UIImage.FromFile("fon");
			i = i.Scale(View.Frame.Size);

			View.BackgroundColor = UIColor.FromPatternImage(i);

			EmailView.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.4f);
			PasswordView.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.4f);
		}

		#endregion

		partial void SignButtonTouchDown (UIButton sender)
		{
			//bool isSigned = NSUserDefaults.StandardUserDefaults.BoolForKey ("isSigned");
			if (string.IsNullOrEmpty(Email.Text))
			{
				ShowMessage (@"Пустой E-mail!");
			}
			else
			{
				if (string.IsNullOrEmpty(Password.Text))
				{
					ShowMessage(@"Пустой Password!");
				}
				else
				{
					string storedPassword = NSUserDefaults.StandardUserDefaults.StringForKey(Email.Text);
					if (!string.IsNullOrEmpty(storedPassword))
					{
						if (storedPassword.CompareTo(Password.Text) == 0)
						{
							string storedUserInfo = NSUserDefaults.StandardUserDefaults.StringForKey(Email.Text + C_USER);
							NSUserDefaults.StandardUserDefaults.SetString(storedUserInfo, C_USER);
							NSUserDefaults.StandardUserDefaults.SetBool(true, "isSigned");
							NSUserDefaults.StandardUserDefaults.Synchronize();
							//(ParentViewController as ViewController).Map_Click();
							DismissViewController(true, null);
						}
					}
					else
					{
						ShowMessage(@"Пользователь с таким Email и паролем ОТСУТСТВУЕТ!");
					}
				}
			}
		}

		void ShowMessage(string message)
		{
			var msgBox = UIAlertController.Create ("Ошибка", message, UIAlertControllerStyle.Alert);
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
