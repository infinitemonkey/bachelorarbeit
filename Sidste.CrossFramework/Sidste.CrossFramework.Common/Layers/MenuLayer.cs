using CocosSharp;

namespace Sidste.CrossFramework.Common.Layers
{
    public class MenuLayer : CCLayerColor
    {
        private readonly Configuration.Configuration _configuration;

        public MenuLayer() : base(CCColor4B.AliceBlue)
        {
            _configuration = Configuration.Configuration.Load();
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            CCAudioEngine.SharedEngine.PlayBackgroundMusic(_configuration.Menu.BackgroundSound, true);
            CCAudioEngine.SharedEngine.BackgroundMusicVolume = 0.5f;

            SetBackground(bounds, _configuration.Menu.BackgroundImage);

            CreateMenu(bounds);
            CreateLogo(bounds);
        }

        private void SetBackground(CCRect bounds, string backgroundImage)
        {
            var bg = new CCSprite(backgroundImage);
            bg.ContentSize = new CCSize(bounds.MaxX, bounds.MaxY);
            bg.Position = bounds.Center;
            AddChild(bg);

            var topOfscreen = bounds.Center.Offset(0f, bounds.MaxY/2f);
            var meteor = new CCParticleMeteor(topOfscreen.Offset(-200.0f, -200.0f));
            AddChild(meteor);

            var galaxy = new CCParticleGalaxy(topOfscreen.Offset(200.0f, -100.0f));
            AddChild(galaxy);
        }

        private void CreateMenu(CCRect bounds)
        {
            var menuItemStart = new CCMenuItemImage(_configuration.Menu.PlayButton.DefaultImage, _configuration.Menu.PlayButton.ClickImage, StartGame);
            menuItemStart.Scale = 1.4f;
            var menuItemLevels = new CCMenuItemImage(_configuration.Menu.LevelsButton.DefaultImage, _configuration.Menu.LevelsButton.ClickImage, StartLevels);
            menuItemLevels.Scale = 1.4f;
            var menuItemOptions = new CCMenuItemImage(_configuration.Menu.OptionsButton.DefaultImage, _configuration.Menu.OptionsButton.ClickImage, StartOptions);
            menuItemOptions.Scale = 1.4f;
            var menuItemTutorial = new CCMenuItemImage(_configuration.Menu.HelpButton.DefaultImage, _configuration.Menu.HelpButton.ClickImage, StartTutorial);
            menuItemTutorial.Scale = 1.4f;

            var menu = new CCMenu(menuItemStart, menuItemLevels, menuItemOptions, menuItemTutorial)
            {
                Position = new CCPoint(bounds.Size.Width / 2, bounds.Size.Height / 2),
                AnchorPoint = CCPoint.AnchorMiddle,
                ContentSize = new CCSize(bounds.Size.Width, bounds.Size.Height)
            };
            menu.AlignItemsInColumns(2, 2);
            AddChild(menu);

            var menuItemSound = new CCMenuItemToggle(ToggleSound, 
                new CCMenuItemImage(_configuration.Menu.SoundButton.DefaultImage, _configuration.Menu.SoundButton.DefaultImage, null), 
                new CCMenuItemImage(_configuration.Menu.SoundButton.ClickImage, _configuration.Menu.SoundButton.ClickImage, null));
            var menuBottom = new CCMenu(menuItemSound)
            {
                Position = new CCPoint(bounds.Size.Width / 2, bounds.MinY + 100)
            };
            AddChild(menuBottom);
        }

        private void CreateLogo(CCRect bounds)
        {
            CCSprite logo = new CCSprite("logo2");

            var moveUp = new CCMoveBy(1.0f, new CCPoint(0.0f, 50.0f));
            var moveDown = moveUp.Reverse();
            var moveSeq = new CCSequence(new CCEaseBackInOut(moveUp), new CCEaseBackInOut(moveDown));
            CCRepeatForever repeatedAction = new CCRepeatForever(moveSeq);
            logo.AnchorPoint = CCPoint.AnchorMiddle;
            logo.Position = new CCPoint(bounds.Size.Width / 2, bounds.Size.Height * 0.75f);
            logo.RunAction(repeatedAction);
            AddChild(logo);
        }

        private void ToggleSound(object sender)
        {
            if (CCAudioEngine.SharedEngine.BackgroundMusicPlaying)
            {
                CCAudioEngine.SharedEngine.PauseBackgroundMusic();
            }
            else
            {
                CCAudioEngine.SharedEngine.PlayBackgroundMusic(_configuration.Menu.BackgroundSound, true);
            }
        }

        private void StartGame(object sender)
        {
            var gameView = GameLayer.CreateScene(GameView, null); // TODO: get last played level
            GoToScene(gameView);
        }

        private void StartLevels(object sender)
        {
            var levelsView = LevelsLayer.CreateScene(GameView);
            GoToScene(levelsView);
        }

        private void StartOptions(object sender)
        {
            var optionsView = OptionsLayer.CreateScene(GameView);
            GoToScene(optionsView);
        }

        private void StartTutorial(object sender)
        {
            var tutorialView = TutorialLayer.CreateScene(GameView);
            GoToScene(tutorialView);
        }

        private void GoToScene(CCScene scene)
        {
            CCAudioEngine.SharedEngine.PlayEffect("pop");
            var transition = new CCTransitionProgressInOut(0.2f, scene);
            Director.ReplaceScene(transition);
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
