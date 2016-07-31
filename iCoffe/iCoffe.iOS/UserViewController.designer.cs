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
        UIKit.UIImageView CityImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CityLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CityView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ExitButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView GiftsTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView UserAvatar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView UserInfo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UserName { get; set; }


        [Action ("ExitButtonTouchDown:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ExitButtonTouchDown (UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CityImage != null) {
                CityImage.Dispose ();
                CityImage = null;
            }

            if (CityLabel != null) {
                CityLabel.Dispose ();
                CityLabel = null;
            }

            if (CityView != null) {
                CityView.Dispose ();
                CityView = null;
            }

            if (ExitButton != null) {
                ExitButton.Dispose ();
                ExitButton = null;
            }

            if (GiftsTable != null) {
                GiftsTable.Dispose ();
                GiftsTable = null;
            }

            if (UserAvatar != null) {
                UserAvatar.Dispose ();
                UserAvatar = null;
            }

            if (UserInfo != null) {
                UserInfo.Dispose ();
                UserInfo = null;
            }

            if (UserName != null) {
                UserName.Dispose ();
                UserName = null;
            }
        }
    }
}