using System.Collections.Generic;
using CocosSharp;

namespace Sidste.CrossFramework.Common.Layers
{
    public class PauseLayer : CCLayerColor
    {
        private GameLayer GameLayer { get { return (GameLayer)Parent; } }
            
        public PauseLayer() : base(CCColor4B.Aquamarine)
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            var drawNode = new CCDrawNode();
            drawNode.Opacity = 50;
            AddChild(drawNode);
            var shape = new CCRect(0, bounds.Center.Y - 120, bounds.MaxX, 200);
            drawNode.DrawRect(shape, new CCColor4B(0, 0, 0, 100));

            CCLabel pauseLabel = new CCLabel("GAME PAUSED.", "Arial", 60) {Position = bounds.Center};
            AddChild(pauseLabel);

            CCLabel resumeLabel = new CCLabel("TOUCH TO RESUME.", "Arial", 30);
            resumeLabel.PositionX = bounds.Center.X;
            resumeLabel.PositionY = bounds.Center.Y - 50;
            AddChild(resumeLabel);

            AddChild(MenuHelper.CreateHomeButton(bounds, BackToMenu));

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
