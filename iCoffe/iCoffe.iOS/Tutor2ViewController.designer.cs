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
    [Register ("Tutor2ViewController")]
    partial class Tutor2ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AllClear { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Rules { get; set; }

        [Action ("AllClearTouchDown:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AllClearTouchDown (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AllClear != null) {
                AllClear.Dispose ();
                AllClear = null;
            }

            if (Rules != null) {
                Rules.Dispose ();
                Rules = null;
            }
        }
    }
}