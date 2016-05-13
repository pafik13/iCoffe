using System;
using System.Collections.Generic;
using UIKit;

using Foundation;

using SDWebImage;

using iCoffe.Shared;

namespace iCoffe.iOS
{
	public class GiftsTableSource: UITableViewSource
	{
		IList<GeolocObj> TableItems;
		string CellIdentifier = "GiftTableCell";
		UIViewController Controller;

		public GiftsTableSource (UIViewController controller, IList<GeolocObj> cObjs)
		{
			Controller = controller;
			TableItems = cObjs;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return (nint) TableItems.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Default, CellIdentifier);
			}
//			var image = new UIImageView ();
//			image.Image
			cell.ImageView.SetImage(new NSUrl(@"http://www.icon100.com/up/3997/72/36-Coffee.png")); 
			cell.TextLabel.Text = TableItems[indexPath.Row].Title;
			cell.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.4f);
			return cell;

		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			//throw new System.NotImplementedException ();
			if (Controller.ParentViewController.NavigationController != null) {
				EventDescViewController vc = Controller.Storyboard.InstantiateViewController ("EventDescVC") as EventDescViewController;
				vc.Obj = TableItems [indexPath.Row];
				Controller.ParentViewController.NavigationController.PushViewController (vc, true);
				Controller.ParentViewController.NavigationController.SetNavigationBarHidden(false, true);
			}

			tableView.DeselectRow (indexPath, false);
		}
	}
}

