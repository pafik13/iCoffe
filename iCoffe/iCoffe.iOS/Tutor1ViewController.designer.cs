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
    [Register ("Tutor1ViewController")]
    partial class Tutor1ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Next { get; set; }

        [Action ("NextTouchDown:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void NextTouchDown (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Next != null) {
                Next.Dispose ();
                Next = null;
            }
        }
    }
}