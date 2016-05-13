using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using iCoffe.Shared;

namespace iCoffe.iOS
{
	partial class GiftsViewController : UIViewController
	{
		GiftsTableSource source;

		public GiftsViewController (IntPtr handle) : base (handle)
		{
		}

		#region View Lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

//			string[] data = new string[]{"Blue", "Red", "Green", "Brown"};
			//this.ParentViewController.NavigationController.PushViewController
			source = new GiftsTableSource (this, Data.Objs);

			Table.Source = source;

			View.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.0f);
			Table.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.0f); 

			//Table.ContentInset = new UIEdgeInsets ((nfloat)0.0f, (nfloat)20.0f, (nfloat)0.0f, (nfloat)20.0f);
		}

		#endregion
	}
}
