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
		UITextView Addresses { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView Contacts { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView EventImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView EventNameImage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel EventNameLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView MiddleLeftView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView MiddleRightView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView OperTimes { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton TakeGiftButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView TopView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Addresses != null) {
				Addresses.Dispose ();
				Addresses = null;
			}
			if (Contacts != null) {
				Contacts.Dispose ();
				Contacts = null;
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
			if (OperTimes != null) {
				OperTimes.Dispose ();
				OperTimes = null;
			}
			if (TakeGiftButton != null) {
				TakeGiftButton.Dispose ();
				TakeGiftButton = null;
			}
			if (TopView != null) {
				TopView.Dispose ();
				TopView = null;
			}
		}
	}
}
