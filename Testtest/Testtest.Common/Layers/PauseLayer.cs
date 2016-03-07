using System.Collections.Generic;
using CocosSharp;

namespace Testtest.Common.Layers
{
    public class PauseLayer : CCLayerColor
    {
        public PauseLayer() : base(CCColor4B.Green)
        {
            
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            CCLabel pauseLabel = new CCLabel("GAME PAUSED.", "Arial", 60) {Position = bounds.Center};
            AddChild(pauseLabel);

            var touchListener = new CCEventListenerTouchAllAtOnce { OnTouchesEnded = OnTouchesEnded };
            AddEventListener(touchListener, this);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
                Parent.RemoveChild(this);
            }
        }
    }
}
