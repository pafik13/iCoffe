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
    [Register ("BonusViewController")]
    partial class BonusViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView BonusDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BonusPrice { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView CafeAddress { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView CafeImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView CafeLogo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CafeName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MiddleLeftView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MiddleRightView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TopView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Want { get; set; }

        [Action ("WantTouchDown:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void WantTouchDown (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (BonusDescription != null) {
                BonusDescription.Dispose ();
                BonusDescription = null;
            }

            if (BonusPrice != null) {
                BonusPrice.Dispose ();
                BonusPrice = null;
            }

            if (CafeAddress != null) {
                CafeAddress.Dispose ();
                CafeAddress = null;
            }

            if (CafeImage != null) {
                CafeImage.Dispose ();
                CafeImage = null;
            }

            if (CafeLogo != null) {
                CafeLogo.Dispose ();
                CafeLogo = null;
            }

            if (CafeName != null) {
                CafeName.Dispose ();
                CafeName = null;
            }

            if (MiddleLeftView != null) {
                MiddleLeftView.Dispose ();
                MiddleLeftView = null;
            }

            if (MiddleRightView != null) {
                MiddleRightView.Dispose ();
                MiddleRightView = null;
            }

            if (TopView != null) {
                TopView.Dispose ();
                TopView = null;
            }

            if (Want != null) {
                Want.Dispose ();
                Want = null;
            }
        }
    }
}