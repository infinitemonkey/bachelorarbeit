using System;
using System.Linq;
using CocosSharp;
using Sidste.CrossFramework.Common.Configuration;
using Sideste.CrossFramework.Common;

namespace Sidste.CrossFramework.Common.Layers
{
    public class GameLayer : BaseLayer
    {
        public Configuration.LevelDefinition LevelDefinition { get; }

        private readonly string _levelKey;
        private int _score;
        private CCLabel _scoreLabel;

        public int ElapsedTime { get; set; }

        public GameLayer(string levelKey) : base(CCColor4B.Blue)
        {
            _levelKey = levelKey;
            LevelDefinition = string.IsNullOrWhiteSpace(_levelKey) 
                ? Configuration.LevelLayer.Levels.First() 
                : Configuration.LevelLayer.Levels.FirstOrDefault(x => x.Key == _levelKey);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            SetBackground(bounds, LevelDefinition.BackgroundImage);
            InitializeGameLayer(bounds);

            AddChild(new GameLogicLayer(LevelDefinition), 500);

            Schedule(t => ElapsedTime = ElapsedTime + (int)t, 1.0f);
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
            _scoreLabel = new CCLabel("0", "Bradley Hand", 36f)
            { 
                Color = CCColor3B.White,
                Position = new CCPoint(10, bounds.MaxY - 50),
                AnchorPoint = CCPoint.Zero
            };
            AddChild(_scoreLabel, 1000);

            var pauseButton = new CCMenuItemImage("pause_1", "pause_3", PauseGame) {Scale = 0.5f};
            var pauseMenu = new CCMenu(pauseButton) {Position = new CCPoint(bounds.MaxX - 50, bounds.MaxY - 50)};
            AddChild(pauseMenu, 1000);
        }

        private void PauseGame(object sender)
        {
            PauseGame();
        }

        public void SetScore(int score)
        {
            _score = score;
            _scoreLabel.Text = score.ToString();
            _scoreLabel.Color = score < 0 ? CCColor3B.Red : CCColor3B.White;
        }

        public void PauseGame()
        {
            Pause();
            foreach (CCNode child in Children)
            {
                child.Pause();
            }
            AddChild(new PauseLayer(), 2000);
        }

        public void GameOver(bool success)
        {
            Pause();
            foreach (CCNode child in Children)
            {
                child.Pause();
            }
            AddChild(new GameOverLayer(_score, success), 2000);
        }

        public void EndGame()
        {
            UnscheduleAll();
            BackToMenu();
        }

        public void NextLevel()
        {
            string nextLevelKey = Configuration.LevelLayer.Levels[Configuration.LevelLayer.Levels.IndexOf(LevelDefinition) + 1].Key;
            var gameLayer = GameLayer.CreateScene(GameView, nextLevelKey);
            GoToScene(gameLayer);
        }

        public void ToLevels()
        {
            var levelsLayer = LevelsLayer.CreateScene(GameView);
            GoToScene(levelsLayer);
        }

        private void BackToMenu()
        {
            var menuLayer = MenuLayer.CreateScene(GameView);
            GoToScene(menuLayer);
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
