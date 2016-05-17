using CocosSharp;
using System;

namespace Sidste.CrossFramework.Common
{
    public static class MenuHelper
    {
        public static CCMenu CreateHomeButton(CCRect bounds, Action<object> target)
        {
            var menuItemStart = new CCMenuItemImage("home_1", "home_3", "home_4", target) { Scale = 0.8f };
            return new CCMenu(menuItemStart)
            {
                Position = new CCPoint(bounds.Size.Width / 2, bounds.MinY + 100)
            };
        }
    }
}