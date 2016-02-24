﻿using System;
using CocosSharp;

namespace Testtest.Common.Layers
{
    public class TutorialLayer : CCLayerColor
    {
        CCLabel title, message, next;
        //CCSprite tutorialImage;
        string[] tutorials = {"Draw lines to same color \nbubbles to score points.",
            "Bubbles stop growing when chained",
            "Bigger the bubble, \nbigger the score",
            "Chain multiple bubbles \nfor score multiplier",
            "Have Fun!"};

        int currentTutorial = 0;
        string titleMessage;

        public TutorialLayer(bool startGameAfter) : base ()
        {
            titleMessage = "How to play: Part {0} of " + tutorials.Length;

            var touchListener = new CCEventListenerTouchAllAtOnce ();
            touchListener.OnTouchesEnded = (touches, ccevent) => {
                currentTutorial++;
                if(currentTutorial >= tutorials.Length)
                {
                    if(startGameAfter)
                    {
                        var mainGame = GameLayer.CreateScene(GameView);
                        var transition = new CCTransitionProgressInOut(0.2f, mainGame);
                        Director.ReplaceScene(transition);
                        RemoveAllChildren(true);
                    }
                    else
                    {
                        var menuLayer = MenuLayer.CreateScene(GameView);
                        var transition = new CCTransitionProgressInOut(0.2f, menuLayer);
                        Director.ReplaceScene(transition);
                        RemoveAllChildren(true);
                    }
                }
                else
                {
                    SetCurrentMessages();
                }
            };

            AddEventListener (touchListener, this);
            Color = new CCColor3B(127, 200, 205);
            Opacity = 255;
        }

        protected override void AddedToScene ()
        {
            base.AddedToScene ();

            var textColor = CCColor3B.White;
            CCRect bounds = VisibleBoundsWorldspace;

            title = new CCLabel(string.Empty, "Roboto-Light.ttf", 36, CCLabelFormat.SystemFont) {
                Position = new CCPoint(bounds.Size.Width / 2, bounds.Size.Height - 60),
                Color = textColor,
                HorizontalAlignment = CCTextAlignment.Center,
                VerticalAlignment = CCVerticalTextAlignment.Center,
                AnchorPoint = CCPoint.AnchorMiddle
            };

            AddChild (title);

            next = new CCLabel ("NEXT", "Roboto-Light.ttf", 36, CCLabelFormat.SystemFont) {
                Color = textColor,
                Position = new CCPoint(bounds.Size.Width - 60, 60),
                HorizontalAlignment = CCTextAlignment.Right,
                VerticalAlignment = CCVerticalTextAlignment.Center,
                AnchorPoint = CCPoint.AnchorMiddle
            };

            AddChild (next);

            message = new CCLabel(string.Empty, "Roboto-Light.ttf", 48, CCLabelFormat.SystemFont) {
                Position = new CCPoint (bounds.Size.Width / 2, bounds.Size.Height / 2),
                Color = textColor,
                HorizontalAlignment = CCTextAlignment.Center,
                VerticalAlignment = CCVerticalTextAlignment.Center,
                AnchorPoint = CCPoint.AnchorMiddle
            };

            AddChild(message);


            //CCRect visibleBounds = VisibleBoundsWorldspace;
            //CCPoint centerBounds = visibleBounds.Center;

            // Layout the positioning of sprites based on visibleBounds
            //tutorialImage.AnchorPoint = CCPoint.AnchorMiddle;
            //tutorialImage.Position = new CCPoint (centerBounds.X, centerBounds.Y - 125);


            //AddChild (tutorialImage);
            SetCurrentMessages();
        }

        private void SetCurrentMessages()
        {
            message.Text = tutorials [currentTutorial];
            title.Text = string.Format (titleMessage, currentTutorial + 1);
            //set image here

            next.Text = (currentTutorial + 1 <= tutorials.Length) ? "NEXT" : "START";
        }

        public static CCScene CreateScene(CCGameView gameView, bool startGameAfter = false)
        {
            var scene = new CCScene(gameView);
            var layer = new TutorialLayer(startGameAfter);

            scene.AddChild(layer);

            return scene;
        }
    }
}