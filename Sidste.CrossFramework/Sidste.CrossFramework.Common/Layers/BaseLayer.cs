using System;
using System.Collections.Generic;
using CocosSharp;
using Sidste.CrossFramework.Common.Configuration;

namespace Sideste.CrossFramework.Common
{
    public class BaseLayer : CCLayerColor
    {
        protected readonly Configuration Configuration = Configuration.Load();

        public BaseLayer() : base(CCColor4B.AliceBlue)
        {
        }

        public BaseLayer(CCColor4B color) : base(CCColor4B.AliceBlue)
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            CCRect bounds = VisibleBoundsWorldspace;

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }
    }
}
