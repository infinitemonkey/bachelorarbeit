using System.Collections.Generic;
using CocosSharp;
using Sidste.CrossFramework.Common.Configuration;
using Sideste.CrossFramework.Common;

namespace Sidste.CrossFramework.Common.Layers
{
    public class GameLogicLayer : BaseLayer
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

            var levelKeyLabel = new CCLabel(_levelDefinition.Key, Fonts.MainFont, 48f)
            { 
                Color = CCColor3B.Black,
                Position = bounds.Center
            };
            AddChild(levelKeyLabel);

            var content = new CCLabel(_levelDefinition.Content, Fonts.MainFont, 30f)
            {
                PositionX = bounds.Center.X,
                PositionY = bounds.Center.Y - 50,
                Color = CCColor3B.Black
            };
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

