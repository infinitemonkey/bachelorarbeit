using System.Collections.Generic;
using CocosSharp;

namespace Testtest.Common.Layers
{
    public class MenuLayer : CCLayerColor
    {
        public MenuLayer() : base(CCColor4B.AliceBlue)
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            var touchListener = new CCEventListenerTouchAllAtOnce {OnTouchesEnded = OnTouchesEnded};
            AddEventListener(touchListener, this);

            CreateMenu(bounds);
            CreateLogo(bounds);
        }

        void CreateMenu(CCRect bounds)
        {
            var menuItemStart = new CCMenuItemImage("play_1", "play_3", "play_4", StartGame);
            var menuItemLevels = new CCMenuItemImage("levels_1", "levels_3", "levels_4", StartLevels);
            var menuItemOptions = new CCMenuItemImage("options_1", "options_3", "options_4", StartOptions);
            var menuItemTutorial = new CCMenuItemImage("tutorial_1", "tutorial_3", "tutorial_4", StartTutorial);

            var menu = new CCMenu(menuItemStart, menuItemLevels, menuItemOptions, menuItemTutorial)
            {
                Position = new CCPoint(bounds.Size.Width / 2, bounds.Size.Height / 2),
                AnchorPoint = CCPoint.AnchorMiddle,
                ContentSize = new CCSize(bounds.Size.Width - 200, bounds.Size.Height)
            };
            menu.AlignItemsInColumns(2, 2);
            //menu.AlignItemsVertically(50);
            AddChild(menu);

            var menuItemSound = new CCMenuItemToggle(ToggleSound, 
                new CCMenuItemImage("sound_2", "sound_2", null), 
                new CCMenuItemImage("sound_4", "sound_4", null));
            var menuBottom = new CCMenu(menuItemSound)
            {
                Position = new CCPoint(bounds.Size.Width / 2, bounds.MinY + 100)
            };
            AddChild(menuBottom);
        }

        void CreateLogo(CCRect bounds)
        {
            CCSprite logo = new CCSprite("logo");

            var moveUp = new CCMoveBy (1.0f, new CCPoint (0.0f, 50.0f));
            var moveDown = moveUp.Reverse();
            var moveSeq = new CCSequence (new CCEaseBackInOut (moveUp), new CCEaseBackInOut (moveDown));
            CCRepeatForever repeatedAction = new CCRepeatForever(moveSeq);
            logo.AnchorPoint = CCPoint.AnchorMiddle;
            logo.Position = new CCPoint(bounds.Size.Width / 2, bounds.Size.Height * 0.75f);
            logo.RunAction(repeatedAction);
            AddChild(logo);
        }

        void ToggleSound(object sender)
        {
            // toggle sound
        }

        void StartGame(object sender)
        {
            var gameView = GameLayer.CreateScene(GameView, "none"); // TODO: get last played level
            var transition = new CCTransitionProgressInOut(0.2f, gameView);
            Director.ReplaceScene(transition);
        }

        void StartLevels(object sender)
        {
            var levelsView = LevelsLayer.CreateScene(GameView);
            var transition = new CCTransitionProgressInOut(0.2f, levelsView);
            Director.ReplaceScene(transition);
        }

        void StartOptions(object sender)
        {
            var optionsView = OptionsLayer.CreateScene(GameView);
            var transition = new CCTransitionProgressInOut(0.2f, optionsView);
            Director.ReplaceScene(transition);
        }

        void StartTutorial(object sender)
        {
            var tutorialView = TutorialLayer.CreateScene(GameView);
            var transition = new CCTransitionProgressInOut(0.2f, tutorialView);
            Director.ReplaceScene(transition);
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
            }
        }

        public static CCScene CreateScene(CCGameView gameView)
        {
            var scene = new CCScene(gameView);
            var layer = new MenuLayer();

            scene.AddChild(layer);

            return scene;
        }
    }
}
