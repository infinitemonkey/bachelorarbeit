using System;
using System.Collections.Generic;
using CocosSharp;
using Sidste.CrossFramework.Common.Configuration;

namespace Sideste.CrossFramework.Common
{
    public class BaseLayer : CCLayerGradient
    {
        protected readonly Configuration Configuration = Configuration.Load();

        public BaseLayer() : base(CCColor4B.AliceBlue, CCColor4B.Blue)
        {
        }

        public BaseLayer(CCColor4B color) : base(CCColor4B.AliceBlue, CCColor4B.Blue)
        {
        }

        public BaseLayer(CCColor4B startColor, CCColor4B endColor) : base(startColor, endColor)
        {
        }

        protected void GoToScene(CCScene scene)
        {
            CCAudioEngine.SharedEngine.PlayEffect("pop");
            var transition = new CCTransitionProgressInOut(0.2f, scene);
            Director.ReplaceScene(transition);
        }
    }
}
