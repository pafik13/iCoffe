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
    [Register ("UserViewController")]
    partial class UserViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ExitButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView GiftsTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Points { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UserID { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView UserInfo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView UserMap { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UserName { get; set; }

        [Action ("ExitButtonTouchDown:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ExitButtonTouchDown (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ExitButton != null) {
                ExitButton.Dispose ();
                ExitButton = null;
            }

            if (GiftsTable != null) {
                GiftsTable.Dispose ();
                GiftsTable = null;
            }

            if (Points != null) {
                Points.Dispose ();
                Points = null;
            }

            if (UserID != null) {
                UserID.Dispose ();
                UserID = null;
            }

            if (UserInfo != null) {
                UserInfo.Dispose ();
                UserInfo = null;
            }

            if (UserMap != null) {
                UserMap.Dispose ();
                UserMap = null;
            }

            if (UserName != null) {
                UserName.Dispose ();
                UserName = null;
            }
        }
    }
}