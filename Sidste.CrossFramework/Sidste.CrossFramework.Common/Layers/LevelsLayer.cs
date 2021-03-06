﻿using System.Collections.Generic;
using CocosSharp;
using Sidste.CrossFramework.Common.Configuration;
using System.Linq;
using Sideste.CrossFramework.Common;

namespace Sidste.CrossFramework.Common.Layers
{
    public class LevelsLayer : BaseLayer
    {
        public LevelsLayer() : base(CCColor4B.Blue)
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            RenderLevels(bounds);
            RenderBackButton();
        }

        private void RenderBackButton()
        {
            DefaultButton backButtonDefinition = Configuration.LevelLayer.BackButton;
            var backButton = new CCMenuItemImage(backButtonDefinition.DefaultImage, backButtonDefinition.ClickImage, BackToMenu);
            var backMenu = new CCMenu(backButton)
            {
                Position = new CCPoint(backButtonDefinition.Position.X, backButtonDefinition.Position.Y)
            };
            AddChild(backMenu);

            var backLabel = new CCLabel(backButtonDefinition.Text["de"], Fonts.MainFont, 36);
            backLabel.Position = backButton.PositionWorldspace;
            AddChild(backLabel);
        }

        private void RenderLevels(CCRect bounds)
        {
            var levelButtons = new List<CCMenuItem>();
            foreach (LevelDefinition level in Configuration.LevelLayer.Levels)
            {
                var levelButton = new CCMenuItemImage(level.DefaultImage, level.ClickImage, StartGame)
                {
                    Name = level.Key
                };
                levelButtons.Add(levelButton);
            }
            var levelsMenu = new CCMenu(levelButtons.ToArray())
            {
                Position = new CCPoint(bounds.Size.Width / 2, bounds.Size.Height / 2),
                AnchorPoint = CCPoint.AnchorMiddle,
                ContentSize = new CCSize(bounds.Size.Width - 100, bounds.Size.Height),
            };
            var itemsInColumns = new List<uint>();
            for (var i = 0; i < Configuration.LevelLayer.Layout.Rows; i++)
            {
                itemsInColumns.Add(Configuration.LevelLayer.Layout.Cols);
            }
            levelsMenu.AlignItemsInColumns(itemsInColumns.ToArray());
            AddChild(levelsMenu);

            foreach (CCMenuItem levelButton in levelButtons)
            {
                var levelText = Configuration.LevelLayer.Levels.FirstOrDefault(x => x.Key == levelButton.Name).Text["de"];
                var levelLabel = new CCLabel(levelText, Fonts.MainFont, 48)
                {
                    Position = levelButton.PositionWorldspace
                };
                AddChild(levelLabel);
            }
        }

        private void StartGame(object sender)
        {
            string levelKey = ((CCMenuItem)sender).Name;
            var gameView = GameLayer.CreateScene(GameView, levelKey);
            GoToScene(gameView, "pop");
        }

        private void BackToMenu(object sender)
        {
            var menuLayer = MenuLayer.CreateScene(GameView);
            GoToScene(menuLayer, Configuration.LevelLayer.BackButton.ClickSound);
        }

        private void GoToScene(CCScene scene, string effectName)
        {
            CCAudioEngine.SharedEngine.PlayEffect(effectName);
            var transitionToGameOver = new CCTransitionProgressInOut(0.2f, scene);
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
