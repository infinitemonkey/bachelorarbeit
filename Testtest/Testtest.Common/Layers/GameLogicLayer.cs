using System;
using CocosSharp;
using System.Collections.Generic;
using Testtest.Common.Layers;
using Testtest.Common.Configuration;

namespace Testtest.Common
{
    public class GameLogicLayer : CCLayer
    {
        private GameLayer GameLayer { get { return (GameLayer)Parent; } }
        private LevelDefinition _levelDefinition;

        public GameLogicLayer(LevelDefinition levelDefinition)
        {
            _levelDefinition = levelDefinition;

            // Parse your level content here
            var x = levelDefinition.Content;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();


            // Test code --------------------------
            CCRect bounds = VisibleBoundsWorldspace;
            var topOfscreen = bounds.Center.Offset(0f, bounds.MaxY/2f);
            var rain = new CCParticleRain(topOfscreen) {Scale = 1.5f};
            AddChild(rain);

            var levelKeyLabel = new CCLabel(_levelDefinition.Key, "Bradley Hand", 36f)
                { 
                    Color = CCColor3B.White,
                    Position = bounds.Center
                };
            var moveUp = new CCMoveBy(1.0f, new CCPoint (0.0f, 50.0f));
            var moveDown = moveUp.Reverse();
            var moveSeq = new CCSequence(new CCEaseBackInOut(moveUp), new CCRotateBy(1.0f, 360.0f), new CCEaseBackInOut(moveDown));
            CCRepeatForever repeatedAction = new CCRepeatForever(moveSeq);
            levelKeyLabel.RunAction(repeatedAction);
            AddChild(levelKeyLabel);

            var content = new CCLabel(_levelDefinition.Content, "Bradley Hand", 30f);
            content.PositionX = bounds.Center.X;
            content.PositionY = bounds.Center.Y - 50;
            AddChild(content);
            // ----------------------------------------


            var touchListener = new CCEventListenerTouchAllAtOnce {OnTouchesEnded = OnTouchesEnded};
            AddEventListener(touchListener, this);
        }

        private void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }

        private void PauseGame()
        {
            GameLayer.PauseGame();
        }

        private void EndGame()
        {
            GameLayer.EndGame();
        }
    }
}

