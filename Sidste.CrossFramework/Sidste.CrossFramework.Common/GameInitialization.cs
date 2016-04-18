using System;
using System.Collections.Generic;
using CocosSharp;
using Sidste.CrossFramework.Common.Layers;
using CocosDenshion;

namespace Sidste.CrossFramework.Common
{
    public static class GameInitialization
    {
        public static void LoadGame(object sender, EventArgs e)
        {
            CCGameView gameView = sender as CCGameView;

            if (gameView != null)
            {
                var contentSearchPaths = new List<string>() { "Fonts", "Sounds" };
                CCSizeI viewSize = gameView.ViewSize;

                // Set world dimensions
                gameView.DesignResolution = new CCSizeI(viewSize.Width, viewSize.Height);

                // Determine whether to use the high or low def versions of our images
                // Make sure the default texel to content size ratio is set correctly
                // Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
                //if (width < viewSize.Width)
                //{
                //    contentSearchPaths.Add("Images/Hd");
                //    CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
                //}
                //else
                //{
                    contentSearchPaths.Add("Images/Ld");
                    CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
                //}

                gameView.ContentManager.SearchPaths = contentSearchPaths;

                CCAudioEngine.SharedEngine.PreloadEffect("pop");

                CCScene gameScene = new CCScene(gameView);
                gameScene.AddLayer(new MenuLayer());
                gameView.RunWithScene(gameScene);
            }
        }
    }
}