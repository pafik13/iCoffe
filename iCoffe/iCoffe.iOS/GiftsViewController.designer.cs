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
    [Register ("GiftsViewController")]
    partial class GiftsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView Table { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Table != null) {
                Table.Dispose ();
                Table = null;
            }
        }
    }
}