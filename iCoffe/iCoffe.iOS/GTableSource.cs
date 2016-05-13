using System;
using System.Collections.Generic;
using UIKit;

using iCoffe.Shared;

namespace iCoffe.iOS
{
	public class GTableSource: UITableViewSource
	{
		IList<Gift> TableItems;
		string CellIdentifier = "GTableCell";
		UIViewController Controller;

		public GTableSource (UIViewController controller, IList<Gift> gifts)
		{
			Controller = controller;
			TableItems = gifts;
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
			cell.ImageView.Image = UIImage.FromBundle ("redhat.png");
			cell.TextLabel.Text = TableItems[indexPath.Row].Name;
			cell.BackgroundColor = UIColor.White.ColorWithAlpha ((nfloat)0.4f);
			return cell;

		}

//		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
//		{
//			//throw new System.NotImplementedException ();
//			if (Controller.ParentViewController.NavigationController != null) {
//				EventDescViewController vc = Controller.Storyboard.InstantiateViewController ("EventDescVC") as EventDescViewController;
//				vc.Obj = TableItems [indexPath.Row];
//				Controller.ParentViewController.NavigationController.PushViewController (vc, true);
//				Controller.ParentViewController.NavigationController.SetNavigationBarHidden(false, true);
//			}
//		}
	}
}

