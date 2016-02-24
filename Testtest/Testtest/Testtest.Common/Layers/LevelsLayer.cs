using System;
using System.Collections.Generic;
using CocosSharp;

namespace Testtest.Common.Layers
{
    public class LevelsLayer : CCLayerColor
    {
        public LevelsLayer()
            : base(CCColor4B.Orange)
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

        public static CCScene CreateScene(CCGameView gameView)
        {
            var scene = new CCScene(gameView);
            var layer = new LevelsLayer();

            scene.AddChild(layer);

            return scene;
        }
    }
}
