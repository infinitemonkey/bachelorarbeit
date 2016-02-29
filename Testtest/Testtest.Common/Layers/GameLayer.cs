﻿using System;
using System.Collections.Generic;
using CocosSharp;

namespace Testtest.Common.Layers
{
    public class GameLayer : CCLayerGradient
    {
        private readonly string _levelKey;

        private CCParticleRain _rain;
        private CCLabel _scoreLabel;

        public GameLayer(string levelKey)
            : base(CCColor4B.Blue, new CCColor4B(127, 200, 205))
        {
            _levelKey = levelKey;
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

            var topOfscreen = bounds.Center.Offset(0f, bounds.MaxY/2f);
            _rain = new CCParticleRain(topOfscreen);
            _rain.Scale = 1.5f;
            this.AddChild(_rain);

            _scoreLabel = new CCLabel(String.Format("Score: {0}", 0), "Bradley Hand", 36f)
            { 
                Color = CCColor3B.White,
                Position = new CCPoint(55, bounds.MaxY - 20),
                HorizontalAlignment = CCTextAlignment.Center,
            };
            this.AddChild(_scoreLabel);

            var pauseButton = new CCMenuItemImage("pause_1", "pause_3", PauseGame);
            pauseButton.Scale = 0.5f;
            var pauseMenu = new CCMenu(pauseButton);
            pauseMenu.Position = new CCPoint(bounds.MaxX - 50, bounds.MaxY - 50);
            AddChild(pauseMenu);

            var levelKeyLabel = new CCLabel(_levelKey, "Bradley Hand", 36f)
            { 
                Color = CCColor3B.White,
                Position = new CCPoint(bounds.MaxX / 2, bounds.MaxY / 2),
                HorizontalAlignment = CCTextAlignment.Center,
            };
            this.AddChild(levelKeyLabel);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }

        void PauseGame(object stuff = null)
        {
            EndGame();
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
