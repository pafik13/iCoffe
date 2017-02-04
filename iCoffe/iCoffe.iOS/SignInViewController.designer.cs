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

namespace tutCoffee.iOS
{
    [Register ("SignInViewController")]
    partial class SignInViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Email { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView EmailImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView EmailView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Password { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView PasswordImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PasswordView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SignIn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SignUp { get; set; }

        [Action ("SignButtonTouchDown:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SignButtonTouchDown (UIKit.UIButton sender);

        [Action ("SkipButtonTouchDown:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SkipButtonTouchDown (UIKit.UIButton sender);

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

            if (SignIn != null) {
                SignIn.Dispose ();
                SignIn = null;
            }

            if (SignUp != null) {
                SignUp.Dispose ();
                SignUp = null;
            }
        }
    }
}