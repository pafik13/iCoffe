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
    [Register ("SignUpController")]
    partial class SignUpController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Back { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Confirm { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ConfirmView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Email { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView EmailView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Password { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PasswordView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Register { get; set; }

        [Action ("BackTouchDown:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BackTouchDown (UIKit.UIButton sender);

        [Action ("RegisterTouchDown:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RegisterTouchDown (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Back != null) {
                Back.Dispose ();
                Back = null;
            }

            if (Confirm != null) {
                Confirm.Dispose ();
                Confirm = null;
            }

            if (ConfirmView != null) {
                ConfirmView.Dispose ();
                ConfirmView = null;
            }

            if (Email != null) {
                Email.Dispose ();
                Email = null;
            }

            if (EmailView != null) {
                EmailView.Dispose ();
                EmailView = null;
            }

            if (Password != null) {
                Password.Dispose ();
                Password = null;
            }

            if (PasswordView != null) {
                PasswordView.Dispose ();
                PasswordView = null;
            }

            if (Register != null) {
                Register.Dispose ();
                Register = null;
            }
        }
    }
}