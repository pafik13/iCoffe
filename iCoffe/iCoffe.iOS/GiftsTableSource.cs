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
		readonly UIViewController Controller;
		readonly IList<BonusOffer> TableItems;
		string CellIdentifier = "BonusCell";

		public GiftsTableSource (UIViewController controller, IList<BonusOffer> offers)
		{
			Controller = controller;
			TableItems = offers;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return TableItems.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var bonusCell = tableView.DequeueReusableCell(CellIdentifier) as BonusRow ?? new BonusRow((NSString)CellIdentifier);
            var item = TableItems[indexPath.Row];
            bonusCell.SetRowData(UIImage.FromBundle("ic_bonus_logo_120pt"), string.IsNullOrEmpty(item.Title) ? "<unknow offer>" : item.Title);
			bonusCell.BackgroundColor = UIColor.Clear; //UIColor.White.ColorWithAlpha(0.4f);
			return bonusCell;

		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			//throw new System.NotImplementedException ();
			if (Controller.ParentViewController.NavigationController != null) {
				EventDescViewController vc = Controller.Storyboard.InstantiateViewController ("EventDescVC") as EventDescViewController;
                //vc.Obj = TableItems [indexPath.Row];
                vc.Bonus = TableItems[indexPath.Row];
                Controller.ParentViewController.NavigationController.PushViewController (vc, true);
				Controller.ParentViewController.NavigationController.SetNavigationBarHidden(false, true);
			}

			tableView.DeselectRow (indexPath, false);
		}
	}
}

