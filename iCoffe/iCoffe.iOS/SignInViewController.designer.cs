// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iCoffe.iOS
{
	[Register ("SignInViewController")]
	partial class SignInViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Email { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView EmailImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView EmailView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField Password { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView PasswordImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView PasswordView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton SignButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton SkipButton { get; set; }

		[Action ("SignButtonTouchDown:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void SignButtonTouchDown (UIButton sender);

		[Action ("SkipButtonTouchDown:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void SkipButtonTouchDown (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (Email != null) {
				Email.Dispose ();
				Email = null;
			}
			if (EmailImage != null) {
				EmailImage.Dispose ();
				EmailImage = null;
			}
			if (EmailView != null) {
				EmailView.Dispose ();
				EmailView = null;
			}
			if (Password != null) {
				Password.Dispose ();
				Password = null;
			}
			if (PasswordImage != null) {
				PasswordImage.Dispose ();
				PasswordImage = null;
			}
			if (PasswordView != null) {
				PasswordView.Dispose ();
				PasswordView = null;
			}
			if (SignButton != null) {
				SignButton.Dispose ();
				SignButton = null;
			}
			if (SkipButton != null) {
				SkipButton.Dispose ();
				SkipButton = null;
			}
		}
	}
}
