﻿using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using Sidste.CrossFramework.Common.Configuration;
using Sideste.CrossFramework.Common;
using Newtonsoft.Json;
using Sidste.CrossFramework.Common.Nodes;

namespace Sidste.CrossFramework.Common.Layers
{
    public class GameLogicLayer : BaseLayer
    {
        private GameLayer GameLayer => (GameLayer)Parent;

        private readonly GameConfiguration _gameConfiguration;

        private readonly CCDrawNode _line;
        private readonly List<Bubble> _visibleBubbles;
        private readonly List<Bubble> _burstedBubbles;
        private readonly List<Bubble> _frozenBubbles;
        private CCPoint _lastPoint;

        private int _redColorIncrement = 10;
        private int _redColorIncrementEnd = 20;
        private Bubble _hitBubble;
        private CCLabel _multiplierLabel;
        private CCLabel _countdown;
        private int _currentScore;

        private int ElapsedTime => GameLayer.ElapsedTime;

        private bool ShouldEndGame => ElapsedTime >= _gameConfiguration.MaxDuration
                                      || _currentScore >= _gameConfiguration.ScoreToReach;

        public GameLogicLayer(LevelDefinition levelDefinition) 
            : base(CCColor4B.Blue, new CCColor4B(127, 200, 205))
        {
            _gameConfiguration = JsonConvert.DeserializeObject<GameConfiguration>(levelDefinition.Content);

            _visibleBubbles = new List<Bubble>();
            _burstedBubbles = new List<Bubble>();
            _frozenBubbles = new List<Bubble>();
            Color = new CCColor3B(127, 200, 205);
            Opacity = 200;
            _line = new CCDrawNode {ZOrder = int.MaxValue};

            StartScheduling();
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            CCRect bounds = VisibleBoundsWorldspace;

            //Add line that we will use to draw later on
            AddChild(_line, 1);

            var touchListener = new CCEventListenerTouchAllAtOnce
            {
                OnTouchesEnded = OnTouchesEnded,
                OnTouchesMoved = OnTouchesMoved,
                OnTouchesBegan = OnTouchesBegan
            };
            AddEventListener(touchListener, this);

            _multiplierLabel = new CCLabel(string.Empty, Fonts.MainFont, 48, CCLabelFormat.SystemFont)
            {
                Position = new CCPoint(bounds.Size.Width - 60, 60),
                Color = CCColor3B.White,
                HorizontalAlignment = CCTextAlignment.Right,
                VerticalAlignment = CCVerticalTextAlignment.Center,
                AnchorPoint = CCPoint.AnchorMiddle
            };
            AddChild(_multiplierLabel, 1);

            _countdown = new CCLabel("60", Fonts.MainFont, 36, CCLabelFormat.SystemFont)
            {
                Position = new CCPoint(60, 60),
                Color = CCColor3B.White,
                HorizontalAlignment = CCTextAlignment.Right,
                VerticalAlignment = CCVerticalTextAlignment.Center,
                AnchorPoint = CCPoint.AnchorMiddle
            };
            AddChild(_countdown, 1);

            //add initial bubbles
            ScheduleOnce(t => _visibleBubbles.Add(AddBubble()), .25f);
            ScheduleOnce(t => _visibleBubbles.Add(AddBubble()), .25f);
            ScheduleOnce(t => _visibleBubbles.Add(AddBubble()), .25f);
            ScheduleOnce(t => _visibleBubbles.Add(AddBubble()), .25f);
        }

        private void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count <= 0)
                return;

            CCTouch touch = touches[0];

            List<Bubble> bubbles = _visibleBubbles.Where(x => x.ContainsPoint(touch.Location))
                                                  .OrderBy(x => x.Id).ToList();

            if (bubbles.Count == 0)
                return;

            _hitBubble = bubbles[0];
            if (_hitBubble == null)
                return;

            _hitBubble.Freeze(0);

            _frozenBubbles.Add(_hitBubble);

            _lastPoint = touch.Location;
            _line.Clear();
            _multiplierLabel.Text = string.Empty;
        }


        private void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count <= 0)
                return;

            if (_hitBubble == null)
                return;

            CCTouch touch = touches[0];

            _line.DrawLine(_lastPoint, touch.Location, 3, _hitBubble.ColorF);
            _lastPoint = touch.Location;

            bool hitOtherColor = _visibleBubbles.Any(x => x.ContainsPoint(touch.Location) 
                                                          && x.BubbleColor != _hitBubble.BubbleColor);

            if (hitOtherColor) 
            {
                foreach (Bubble bubble in _frozenBubbles) 
                {
                    bubble.ForcePop(this);
                    _visibleBubbles.Remove(bubble);
                }

                _line.Clear();
                _frozenBubbles.Clear();
                _hitBubble = null;
                _multiplierLabel.Text = string.Empty;
                return;
            }

            IEnumerable<Bubble> bubbles = _visibleBubbles.Where(
                x => x.ContainsPoint(touch.Location) 
                && x.BubbleColor == _hitBubble.BubbleColor
                && !x.IsFrozen).ToList();
                

            if (bubbles == null || !bubbles.Any())
                return;

            foreach (Bubble bubble in bubbles) 
            {
                _frozenBubbles.Add(bubble);
                bubble.Freeze(_frozenBubbles.Count);
            }

            if (_frozenBubbles.Count > 1) 
            {
                _multiplierLabel.SystemFontSize = 48 + _frozenBubbles.Count * 2;
                _multiplierLabel.Text = $"{_frozenBubbles.Count - 1}x";
            }

            if (_frozenBubbles.Count >= 6) 
            {
                CalculateScore();
                CCAudioEngine.SharedEngine.PlayEffect("sounds/highscore");
            }
        }

        private void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count <= 0)
                return;
            if (_hitBubble == null)
                return;

            CalculateScore();
        }

        private void CalculateScore()
        {
            int score = 0;
            int multiplier = _frozenBubbles.Count - 1;
            foreach (Bubble bubble in _frozenBubbles) 
            {
                score += bubble.Points;
                bubble.ForcePop(this);
                _visibleBubbles.Remove(bubble);
            }

            score *= multiplier;

            if (multiplier < 0)
            {
                score = 0;
            }
            else if (multiplier == 0) //  1 bubble
            {
                score = 20;
            }

            _line.Clear();
            _hitBubble = null;
            _frozenBubbles.Clear();
            UpdateScore(score);
            _multiplierLabel.Text = string.Empty;
        }

        private void StartScheduling()
        {
            Schedule(t => {
                if (ShouldEndGame) 
                {
                    GameLayer.SetScore(_currentScore);
                    GameLayer.GameOver(_currentScore >= _gameConfiguration.ScoreToReach);
                    return;
                }
                _visibleBubbles.Add(AddBubble());

                if (CCRandom.Next(0, 100) > 90)
                    _visibleBubbles.Add(AddBubble());

                float timeLeft = (_gameConfiguration.MaxDuration - ElapsedTime);
                if (timeLeft < 10 && CCRandom.Next(0, 100) > 30)
                    _visibleBubbles.Add(AddBubble());
            }, .5f);

            Schedule(t => CheckPop());

            Schedule(UpdateLayerGradient, 0.1f);
        }

        private Bubble AddBubble()
        {
            var bubble = new Bubble(_gameConfiguration.MaxColors, _gameConfiguration.GrowthTime);
            bubble.Position = PositionHelper.GetRandomPosition(bubble.ContentSize, VisibleBoundsWorldspace.Size);

            AddChild(bubble);

            return bubble;
        }

        private void CheckPop()
        {
            foreach (Bubble bubble in _visibleBubbles)
            {
                if (!bubble.Pop(this))
                    continue;

                _burstedBubbles.Add(bubble);
            }

            foreach (Bubble bubble in _burstedBubbles)
            {
                _visibleBubbles.Remove(bubble);
                UpdateScore(-10);
            }

            _burstedBubbles.Clear();
        }

        private void UpdateScore(int toAdd)
        {
            _currentScore += toAdd;
            GameLayer.SetScore(_currentScore);
        }

        private void UpdateLayerGradient(float dt)
        {
            float timeLeft = _gameConfiguration.MaxDuration - ElapsedTime;
            if (timeLeft < 0)
                timeLeft = 0;

            _countdown.Text = timeLeft.ToString();
            CCColor3B startColor = StartColor;

            int increment = _redColorIncrement;
            if (timeLeft < 10)
                increment = _redColorIncrementEnd;

            int newRedColor = startColor.R + increment;
            if (newRedColor <= byte.MinValue) 
            {
                newRedColor = 0;
                _redColorIncrement *= -1;
                _redColorIncrementEnd *= -1;
            } 
            else if (newRedColor >= byte.MaxValue) 
            {
                newRedColor = byte.MaxValue;
                _redColorIncrement *= -1;
                _redColorIncrementEnd *= -1;
            }

            startColor.R = (byte)newRedColor;

            StartColor = startColor;
        }
    }
}