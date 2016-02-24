using System;
using UIKit;
using Testtest.Common;

namespace Testtest.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (GameView != null)
            {
                // Set loading event to be called once game view is fully initialised
                GameView.ViewCreated += GameInitialization.LoadGame;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (GameView != null)
                GameView.Paused = true;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (GameView != null)
                GameView.Paused = false;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

