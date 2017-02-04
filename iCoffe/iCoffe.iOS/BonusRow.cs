using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace tutCoffee.iOS
{
	public class BonusRow: UITableViewCell
	{
		readonly UIView Content;
		readonly UIImageView Icon;
		readonly UIImageView Logo;
		readonly UILabel Name;

		public BonusRow(NSString cellId) : base (UITableViewCellStyle.Default, cellId)
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
				BackgroundColor = UIColor.White.ColorWithAlpha(0.25f),
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
			Content.Frame = new CGRect(0, 0, ContentView.Bounds.Width, 102);
			Logo.Frame = new CGRect(0, 0, 102, 102);
			Name.Frame = new CGRect(107, 37, ContentView.Bounds.Width - 67 - 107, 30);
			Icon.Frame = new CGRect(ContentView.Bounds.Width - 62, 30, 42, 42);
		}
	}
}

