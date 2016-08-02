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
    [Register ("EventDescViewController")]
    partial class EventDescViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView Addresses { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView Description { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView EventImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView EventNameImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EventNameLabel { get; set; }

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
            if (Addresses != null) {
                Addresses.Dispose ();
                Addresses = null;
            }

            if (Description != null) {
                Description.Dispose ();
                Description = null;
            }

            if (EventImage != null) {
                EventImage.Dispose ();
                EventImage = null;
            }

            if (EventNameImage != null) {
                EventNameImage.Dispose ();
                EventNameImage = null;
            }

            if (EventNameLabel != null) {
                EventNameLabel.Dispose ();
                EventNameLabel = null;
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