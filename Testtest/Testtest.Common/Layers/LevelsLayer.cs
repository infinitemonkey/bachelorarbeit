using System;
using System.Collections.Generic;
using CocosSharp;
using System.Linq;

namespace Testtest.Common.Layers
{
    public class LevelsLayer : CCLayerColor
    {
        private readonly Configuration _configuration;

        public LevelsLayer()
            : base(CCColor4B.Orange)
        {
            // Load and instantate your assets here
            _configuration = Configuration.Load();
            // Make any renderable node objects (e.g. sprites) children of this layer
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            CCRect bounds = VisibleBoundsWorldspace;

            var levelButtons = new List<CCMenuItem>();
            foreach (LevelDefinition level in _configuration.LevelLayer.Levels)
            {
                var levelLabel = new CCLabel(level.Text["de"], "Arial", 36);
                var levelButton = new CCMenuItemLabel(levelLabel, StartGame);
                levelButton.Name = level.Key;
                levelButtons.Add(levelButton);
            }
            var levelsMenu = new CCMenu(levelButtons.ToArray())
            {
                Position = new CCPoint(bounds.Size.Width / 2, bounds.Size.Height / 2),
                AnchorPoint = CCPoint.AnchorMiddle,
                ContentSize = new CCSize(bounds.Size.Width - 200, bounds.Size.Height)
            };
            var itemsInColumns = new List<uint>();
            for (var i = 0; i < _configuration.LevelLayer.Layout.Rows; i++)
            {
                itemsInColumns.Add(_configuration.LevelLayer.Layout.Cols);
            }
            levelsMenu.AlignItemsInColumns(itemsInColumns.ToArray());
            AddChild(levelsMenu);

            var backLabel = new CCLabel(_configuration.LevelLayer.BackButton.Text["de"], "Arial", 36);
            var backButton = new CCMenuItemLabel(backLabel, BackToMenu);
            var backMenu = new CCMenu(backButton)
            {
                Position = new CCPoint(_configuration.LevelLayer.BackButton.Position.X, 
                        _configuration.LevelLayer.BackButton.Position.Y)
            };
            AddChild(backMenu);

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
        }

        void StartGame(object levelButton)
        {
            var gameView = GameLayer.CreateScene(GameView, ((CCMenuItemLabel)levelButton).Name);
            var transition = new CCTransitionProgressInOut(0.2f, gameView);
            Director.ReplaceScene(transition);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }

        void BackToMenu(object backButton)
        {
            var menuLayer = MenuLayer.CreateScene(GameView);
            var transitionToGameOver = new CCTransitionProgressInOut(0.2f, menuLayer);
            Director.ReplaceScene(transitionToGameOver);
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
