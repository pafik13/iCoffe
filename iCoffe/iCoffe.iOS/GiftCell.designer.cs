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
	[Register ("GiftCell")]
	partial class GiftCell
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView Image { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel TextLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Image != null) {
				Image.Dispose ();
				Image = null;
			}
			if (TextLabel != null) {
				TextLabel.Dispose ();
				TextLabel = null;
			}
		}
	}
}
