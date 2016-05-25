using System.Collections.Generic;
using CocosSharp;
using Sideste.CrossFramework.Common;

namespace Sidste.CrossFramework.Common.Layers
{
    public class PauseLayer : BaseLayer
    {
        private GameLayer GameLayer { get { return (GameLayer)Parent; } }
            
        public PauseLayer() : base(CCColor4B.Black)
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
            drawNode.DrawRect(shape, new CCColor4B(0, 0, 0, 150));

            CCLabel pauseLabel = new CCLabel("GAME PAUSED.", "Arial", 60) {Position = bounds.Center};
            AddChild(pauseLabel);

            CCLabel resumeLabel = new CCLabel("TOUCH TO RESUME.", "Arial", 30);
            resumeLabel.PositionX = bounds.Center.X;
            resumeLabel.PositionY = bounds.Center.Y - 50;
            AddChild(resumeLabel);

            var scaleUp = new CCScaleBy(1, 1.3f);
            var scaleDown = scaleUp.Reverse();
            var moveSeq = new CCSequence(new CCEaseBackInOut(scaleUp), new CCEaseBackInOut(scaleDown));
            CCRepeatForever repeatedAction = new CCRepeatForever(moveSeq);
            resumeLabel.RunAction(repeatedAction);

            AddChild(MenuHelper.CreateHomeButton(bounds, BackToMenu));

            Opacity = 180;

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
                GameLayer.Resume();
                GameLayer.RemoveChild(this);
            }
        }
    }
}
