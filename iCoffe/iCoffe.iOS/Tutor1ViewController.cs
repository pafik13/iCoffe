using Foundation;
using System;
using UIKit;

namespace iCoffe.iOS
{
    public partial class Tutor1ViewController : UIViewController
    {
		public ViewController Parent;

        public Tutor1ViewController (IntPtr handle) : base (handle)
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

			Next.Layer.CornerRadius = 8.0f;
			Next.Layer.MasksToBounds = true;

			Console.WriteLine(@"ViewDidLoad in Tutor1ViewController: " + (ParentViewController is ViewController));
		}

		partial void NextTouchDown(UIButton sender)
		{
			//throw new NotImplementedException();
			if (Parent != null) {
				Parent.Tutor1_Next_Click();
			}
		}
	}
}