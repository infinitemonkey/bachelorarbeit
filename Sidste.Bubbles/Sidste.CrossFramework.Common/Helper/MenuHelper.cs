using CocosSharp;
using System;

namespace Sidste.CrossFramework.Common
{
    public static class MenuHelper
    {
        public static CCMenu CreateHomeButton(CCRect bounds, Action<object> target)
        {
            var menuItemHome = new CCMenuItemImage("home_1", "home_3", "home_4", target) { Scale = 0.8f };
            return new CCMenu(menuItemHome)
            {
                Position = new CCPoint(bounds.Size.Width / 2, bounds.MinY + 100)
            };
        }

        public static CCMenu CreateEndGameNavigation(CCRect bounds, Action<object> homeTarget, 
            Action<object> levelsTarget, Action<object> nextLevelTarget)
        {
            var menuItemHome = new CCMenuItemImage("home_1", "home_3", "home_4", homeTarget) { Scale = 0.8f };
            var menuItemLevels = new CCMenuItemImage("levels_1", "levels_3", "levels_4", levelsTarget) { Scale = 0.8f };
            var menuItemNextLevel = new CCMenuItemImage("play_1", "play_3", "play_4", nextLevelTarget) { Scale = 0.8f };
            var menu = new CCMenu(menuItemHome, menuItemLevels, menuItemNextLevel)
            {
                Position = new CCPoint(bounds.Size.Width / 2, bounds.MinY + 100)
            };
            menu.AlignItemsHorizontally();
            return menu;
        }
    }
}