using System;
using System.Collections.Generic;
using CocosSharp;

namespace Testtest.Common.Layers
{
    public class GameLayer : CCLayerGradient
    {
        private readonly Configuration.Configuration _configuration;
        private readonly string _levelKey;

        private CCParticleRain _rain;
        private CCLabel _scoreLabel;

        public GameLayer(string levelKey)
            : base(CCColor4B.Blue, new CCColor4B(127, 200, 205))
        {
            _levelKey = levelKey;
            _configuration = Configuration.Configuration.Load();
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            var topOfscreen = bounds.Center.Offset(0f, bounds.MaxY/2f);
            _rain = new CCParticleRain(topOfscreen) {Scale = 1.5f};
            AddChild(_rain);

            _scoreLabel = new CCLabel(String.Format("Score: {0}", 0), "Bradley Hand", 36f)
            { 
                Color = CCColor3B.White,
                Position = new CCPoint(55, bounds.MaxY - 20),
                HorizontalAlignment = CCTextAlignment.Center
            };
            AddChild(_scoreLabel);

            var pauseButton = new CCMenuItemImage("pause_1", "pause_3", PauseGame) {Scale = 0.5f};
            var pauseMenu = new CCMenu(pauseButton) {Position = new CCPoint(bounds.MaxX - 50, bounds.MaxY - 50)};
            AddChild(pauseMenu);

            var levelKeyLabel = new CCLabel(_levelKey, "Bradley Hand", 36f)
            { 
                Color = CCColor3B.White,
                Position = new CCPoint(bounds.MaxX / 2, bounds.MaxY / 2),
                HorizontalAlignment = CCTextAlignment.Center
            };
            AddChild(levelKeyLabel);

            var touchListener = new CCEventListenerTouchAllAtOnce { OnTouchesEnded = OnTouchesEnded };
            AddEventListener(touchListener, this);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }

        void PauseGame(object sender)
        {
            var pauseLayer = new PauseLayer();
            AddChild(pauseLayer);
            //EndGame();
            //GameView.Paused = !GameView.Paused;
        }

        void EndGame()
        {
            // Stop scheduled events as we transition to game over scene
            UnscheduleAll();
            BackToMenu();
        }

        private void BackToMenu()
        {
            var menuLayer = MenuLayer.CreateScene(GameView);
            var transitionToGameOver = new CCTransitionProgressInOut(0.2f, menuLayer);
            Director.ReplaceScene(transitionToGameOver);
        }

        public static CCScene CreateScene(CCGameView gameView, string levelKey)
        {
            var scene = new CCScene(gameView);
            var layer = new GameLayer(levelKey);

            scene.AddChild(layer);

            return scene;
        }
    }
}
