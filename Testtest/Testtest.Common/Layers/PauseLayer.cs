using System.Collections.Generic;
using CocosSharp;

namespace Testtest.Common.Layers
{
    public class PauseLayer : CCLayerColor
    {
        private GameLayer GameLayer { get { return (GameLayer)Parent; } }
            
        public PauseLayer() : base(CCColor4B.Green)
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            CCLabel pauseLabel = new CCLabel("GAME PAUSED.", "Arial", 60) {Position = bounds.Center};
            AddChild(pauseLabel);

            CCLabel resumeLabel = new CCLabel("TOUCH TO RESUME.", "Arial", 30);
            resumeLabel.PositionX = bounds.Center.X;
            resumeLabel.PositionY = bounds.Center.Y - 50;
            AddChild(resumeLabel);

            var menuItemStart = new CCMenuItemImage("home_1", "home_3", "home_4", BackToMenu) { Scale = 0.8f };
            var menuBottom = new CCMenu(menuItemStart)
            {
                Position = new CCPoint(bounds.Size.Width / 2, bounds.MinY + 100)
            };
            AddChild(menuBottom);

            Opacity = 150;

            var touchListener = new CCEventListenerTouchAllAtOnce { OnTouchesEnded = OnTouchesEnded };
            AddEventListener(touchListener, this);
        }

        private void BackToMenu(object sender)
        {
            GameLayer.EndGame();
        }

        private void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                Parent.Resume();
                Parent.RemoveChild(this);
            }
        }
    }
}
