using System;
using System.Collections.Generic;
using CocosSharp;

namespace Sideste.CrossFramework.Common
{
    public class PositionHelper
    {
        public static CCPoint GetRandomPosition(CCSize spriteSize, CCSize visibleBoundsWorldspaceSize)
        {
            var randomX = CCRandom.Next(40, (int)visibleBoundsWorldspaceSize.Width - 40);
            var randomY = CCRandom.Next(40, (int)visibleBoundsWorldspaceSize.Height - 40);

            return new CCPoint((float)randomX, (float)randomY);
        }
    }
}
