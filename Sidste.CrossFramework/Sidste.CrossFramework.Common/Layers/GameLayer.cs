using System;
using System.Linq;
using CocosSharp;
using Sidste.CrossFramework.Common.Configuration;
using Sideste.CrossFramework.Common;

namespace Sidste.CrossFramework.Common.Layers
{
    public class GameLayer : BaseLayer
    {
        private readonly string _levelKey;

        private CCLabel _scoreLabel;

        private int _elapsedTime = 0;

        public GameLayer(string levelKey) : base(CCColor4B.Blue, new CCColor4B(127, 200, 205))
        {
            _levelKey = levelKey;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            LevelDefinition levelDefinition = string.IsNullOrWhiteSpace(_levelKey) 
                ? Configuration.LevelLayer.Levels.First() 
                : Configuration.LevelLayer.Levels.FirstOrDefault(x => x.Key == _levelKey);

            SetBackground(bounds, levelDefinition.BackgroundImage);
            InitializeGameLayer(bounds);

            AddChild(new GameLogicLayer(levelDefinition), 500);

            //test
            Schedule(t =>
                {
                    _elapsedTime = _elapsedTime + (int)t;
                    _scoreLabel.Text = "Score: " + _elapsedTime.ToString();
                }, 1.0f);
        }

        private void SetBackground(CCRect bounds, string backgroundImage)
        {
            var bg = new CCSprite(backgroundImage);
            bg.ContentSize = new CCSize(bounds.MaxX, bounds.MaxY);
            bg.Position = bounds.Center;
            AddChild(bg);
        }

        private void InitializeGameLayer(CCRect bounds)
        {
            _scoreLabel = new CCLabel(String.Format("Score: {0}", 0), "Bradley Hand", 36f)
            { 
                Color = CCColor3B.White,
                Position = new CCPoint(10, bounds.MaxY - 50),
                AnchorPoint = CCPoint.Zero
            };
            AddChild(_scoreLabel);

            var pauseButton = new CCMenuItemImage("pause_1", "pause_3", PauseGame) {Scale = 0.5f};
            var pauseMenu = new CCMenu(pauseButton) {Position = new CCPoint(bounds.MaxX - 50, bounds.MaxY - 50)};
            AddChild(pauseMenu);
        }

        private void PauseGame(object sender)
        {
            PauseGame();
        }

        public void PauseGame()
        {
            Pause();
            AddChild(new PauseLayer(), 1000);
        }

        public void EndGame()
        {
            // Stop scheduled events as we transition to game over scene
            UnscheduleAll();
            BackToMenu();
        }

        private void BackToMenu()
        {
            var menuLayer = MenuLayer.CreateScene(GameView);
            var transitionToMenu = new CCTransitionProgressInOut(0.2f, menuLayer);
            Director.ReplaceScene(transitionToMenu);
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
