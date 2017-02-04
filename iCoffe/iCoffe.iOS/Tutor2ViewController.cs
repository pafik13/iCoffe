using Foundation;
using System;
using UIKit;

namespace tutCoffee.iOS
{
    public partial class Tutor2ViewController : UIViewController
    {
		public ViewController Parent;

		public Tutor2ViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = UIColor.White.ColorWithAlpha((nfloat)0.0f);
			Rules.BackgroundColor = UIColor.White.ColorWithAlpha((nfloat)0.4f);

			AllClear.Layer.CornerRadius = 8.0f;
			AllClear.Layer.MasksToBounds = true;

			Console.WriteLine(@"ViewDidLoad in Tutor2ViewController: " + (ParentViewController is ViewController));
		}

		partial void AllClearTouchDown(UIButton sender)
		{
			//throw new NotImplementedException();
			if (Parent != null) {
				Parent.Tutor2_AllClear_Click();
			}		
		}
	}
}