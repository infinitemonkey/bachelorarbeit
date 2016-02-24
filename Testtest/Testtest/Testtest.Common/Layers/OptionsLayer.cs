using System;
using System.Collections.Generic;
using CocosSharp;

namespace Testtest.Common.Layers
{
    public class OptionsLayer : CCLayerColor
    {
        public OptionsLayer()
            : base(CCColor4B.Magenta)
        {
            // Load and instantate your assets here

            // Make any renderable node objects (e.g. sprites) children of this layer
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

            var title = new CCLabel("Options", "Bradley Hand", 50f);
            title.Position = bounds.Center;
            title.RunAction(new CCRepeatForever(new CCRotateBy(1f, 360f)));
            AddChild(title);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
                BackToMenu();
            }
        }

        private void BackToMenu()
        {
            var menuLayer = MenuLayer.CreateScene(GameView);
            var transitionToGameOver = new CCTransitionProgressInOut(0.2f, menuLayer);
            Director.ReplaceScene(transitionToGameOver);
            RemoveAllChildren(true);
        }

        public static CCScene CreateScene(CCGameView gameView, bool startGameAfter = false)
        {
            var scene = new CCScene(gameView);
            var layer = new OptionsLayer();

            scene.AddChild(layer);

            return scene;
        }
    }
}
