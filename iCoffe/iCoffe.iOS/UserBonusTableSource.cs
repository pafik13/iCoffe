using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

using iCoffe.Shared;

namespace iCoffe.iOS
{
	public class UserBonusTableSource: UITableViewSource
	{
		//readonly UIViewController Controller;
		readonly IList<Gift> TableItems;
		string CellIdentifier = "UserBonusCell";

		public UserBonusTableSource (UIViewController controller, IList<Gift> gifts)
		{
			Controller = controller;
			TableItems = gifts;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return TableItems.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var bonusCell = tableView.DequeueReusableCell(CellIdentifier) as UserBonusRow ?? new UserBonusRow((NSString)CellIdentifier);
			bonusCell.SetRowData(UIImage.FromBundle("ic_bonus_logo_96pt"), TableItems[indexPath.Row].Name);
			bonusCell.BackgroundColor = UIColor.Clear; //UIColor.White.ColorWithAlpha(0.4f);
			return bonusCell;

		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			//throw new System.NotImplementedException ();
			//if (Controller.ParentViewController.NavigationController != null) {
			//	EventDescViewController vc = Controller.Storyboard.InstantiateViewController ("EventDescVC") as EventDescViewController;
			//	vc.Obj = TableItems [indexPath.Row];
			//	Controller.ParentViewController.NavigationController.PushViewController (vc, true);
			//	Controller.ParentViewController.NavigationController.SetNavigationBarHidden(false, true);
			//}

			tableView.DeselectRow(indexPath, false);
		}
	}
}

