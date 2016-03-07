using System.Collections.Generic;
using CocosSharp;

namespace Sidste.CrossFramework.Common.Layers
{
    public class OptionsLayer : CCLayerColor
    {
        public OptionsLayer() : base(CCColor4B.Magenta)
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            var title = new CCLabel("Options", "Bradley Hand", 50f);
            title.Position = bounds.Center;
            title.RunAction(new CCRepeatForever(new CCRotateBy(1f, 360f)));
            AddChild(title);

            AddChild(MenuHelper.CreateHomeButton(bounds, BackToMenu));
        }

        private void BackToMenu(object sender)
        {
            var menuLayer = MenuLayer.CreateScene(GameView);
            var transitionToGameOver = new CCTransitionProgressInOut(0.2f, menuLayer);
            Director.ReplaceScene(transitionToGameOver);
        }

        public static CCScene CreateScene(CCGameView gameView, bool startGameAfter = false)
        {
            var scene = new CCScene(gameView);
            var layer = new OptionsLayer();

            scene.AddChild(layer);

            return scene;
        }
    }
}
