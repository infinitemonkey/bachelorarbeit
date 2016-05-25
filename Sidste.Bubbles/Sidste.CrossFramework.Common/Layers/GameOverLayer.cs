using System.Collections.Generic;
using CocosSharp;
using Sideste.CrossFramework.Common;

namespace Sidste.CrossFramework.Common.Layers
{
    public class GameOverLayer : BaseLayer
    {
        private GameLayer GameLayer { get { return (GameLayer)Parent; } }
            
        private bool _isSuccess;
        private int _score;

        public GameOverLayer(int score, bool isSuccess) : base(CCColor4B.Black)
        {
            _isSuccess = isSuccess;
            _score = score;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            var drawNode = new CCDrawNode();
            drawNode.Opacity = 50;
            AddChild(drawNode);
            var shape = new CCRect(0, bounds.Center.Y - 120, bounds.MaxX, 200);
            drawNode.DrawRect(shape, new CCColor4B(0, 0, 0, 150));

            CCLabel pauseLabel = new CCLabel(_isSuccess ? "Score: " + _score : "GAME OVER.", "Arial", 60)
            {
                Position = bounds.Center 
            };
            AddChild(pauseLabel);

            CCLabel resumeLabel = new CCLabel("TOUCH TO RETRY.", "Arial", 30)
            {
                PositionX = bounds.Center.X,
                PositionY = bounds.Center.Y - 50
            };
            AddChild(resumeLabel);

            var scaleUp = new CCScaleBy(1, 1.3f);
            var scaleDown = scaleUp.Reverse();
            var moveSeq = new CCSequence(new CCEaseBackInOut(scaleUp), new CCEaseBackInOut(scaleDown));
            CCRepeatForever repeatedAction = new CCRepeatForever(moveSeq);
            resumeLabel.RunAction(repeatedAction);

            AddChild(MenuHelper.CreateEndGameNavigation(bounds, BackToMenu, ToLevels, NextLevel));

            Opacity = 180;

            var touchListener = new CCEventListenerTouchAllAtOnce { OnTouchesEnded = OnTouchesEnded };
            AddEventListener(touchListener, this);
        }

        private void BackToMenu(object sender)
        {
            GameLayer.EndGame();
        }

        private void ToLevels(object sender)
        {
            GameLayer.ToLevels();
        }

        private void NextLevel(object sender)
        {
            GameLayer.NextLevel();
        }

        private void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                var gameLayer = GameLayer.CreateScene(GameView, GameLayer.LevelDefinition.Key);
                var transitionToMenu = new CCTransitionProgressInOut(0.2f, gameLayer);
                Director.ReplaceScene(transitionToMenu);
            }
        }
    }
}