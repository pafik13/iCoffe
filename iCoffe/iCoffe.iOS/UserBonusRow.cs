using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace tutCoffee.iOS
{
	public class UserBonusRow : UITableViewCell
	{
		readonly UIView Content;
		readonly UIImageView Icon;
		readonly UIImageView Logo;
		readonly UILabel Name;

		public UserBonusRow(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Gray;

			Logo = new UIImageView();

			Name = new UILabel()
			{
				Font = UIFont.FromName("Courier-Bold", 14f),
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center,
				Lines = 3,
			};

			Icon = new UIImageView()
			{
				Image = UIImage.FromBundle("ic_bonus_red_48pt"),
			};

			Content = new UIView()
			{
				BackgroundColor = UIColor.White.ColorWithAlpha(0.25f)
			};
			Content.Layer.CornerRadius = 8.0f;
			Content.Layer.MasksToBounds = true;

			Content.AddSubviews(new UIView[] { Logo, Name, Icon });

			ContentView.AddSubview(Content);
		}

		public void SetRowData(UIImage logo, string name)
		{
			Logo.Image = logo;
			Name.Text = name;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			Content.Frame = new CGRect(0, 0, ContentView.Bounds.Width, 84);
			Logo.Frame = new CGRect(0, 0, 84, 84);
			Name.Frame = new CGRect(89, 28, ContentView.Bounds.Width - 49 - 89, 30);
			Icon.Frame = new CGRect(ContentView.Bounds.Width - 44, 30, 24, 24);
		}
	}
}

