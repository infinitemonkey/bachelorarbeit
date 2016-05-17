using System;
using System.Collections.Generic;
using CocosSharp;

namespace Sideste.CrossFramework.Common
{
    public class Bubble : CCDrawNode
    {
        private int _bubblePoints = 50;
        private float _bubbleMax;
        private float _growthTime; 
        private CCColor4B _color;
        private CCColor4F _colorF;
        private CCScaleBy _scale;
        private CCMoveBy _move1, _move2;
        private CCRepeatForever _repeatedAction;
        private int _maxGrowthTime;

        private static CCColor4B _color0 = new CCColor4B (119, 208, 101, 255);
        private static CCColor4B _color1 = new CCColor4B (180, 85, 182, 255);
        private static CCColor4B _color2 = new CCColor4B (255, 255, 255, 255);//white
        private static CCColor4B _color3 = new CCColor4B (44, 62, 80, 255);
        private static CCColor4B _color4 = new CCColor4B (255, 255, 75, 255);//yellow

        public int Points
        {
            get { return (int)(_bubblePoints * this.ScaleX); }
        }
            
        public static int UniversalId;
        public int Id { get; }

        public CCColor4B ColorId 
        {
            get { return _color; }
        }

        public CCColor4F ColorF 
        {
            get { return _colorF; }
        }

        public bool IsFrozen { get; set; }

        public Bubble(int maxColors, int maxGrowthTime)
        {
            Id = UniversalId++;
            Scale = 0.5f;
            switch (CCRandom.Next(0, maxColors))
            {
                case 0:
                    _color = _color0;
                    break;
                case 1:
                    _color = _color1;
                    break;
                case 2:
                    _color = _color2;
                    break;
                case 3:
                    _color = _color3;
                    break;
                case 4:
                    _color = _color4;
                    break;
            }

            _maxGrowthTime = maxGrowthTime;
            _colorF = new CCColor4F(_color);
            var size = CCRandom.Next(25, 50);
            this.ContentSize = new CCSize(size, size);
            this.DrawSolidCircle(this.Position, (float)size, _color);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            _bubbleMax = CCRandom.Next(4, 6);
            _growthTime = CCRandom.Next(3, _maxGrowthTime);
            _scale = new CCScaleBy(_growthTime, _bubbleMax);
            this.AddAction(_scale);

            _move1 = new CCMoveBy(.2f, new CCPoint(4, 4));
            _move2 = new CCMoveBy(.24f, new CCPoint(-4, -4));
            _repeatedAction = new CCRepeatForever(_move1, _move2);
        }

        public void Freeze(int count)
        {
            IsFrozen = true;
            this.StopAllActions();
            try
            {
                //CCAudioEngine.SharedEngine.PlayEffect("sounds/ring" + count, false);
            }
            catch
            {
            }
        }

        private void PopAnimation(CCLayer layer)
        {
            //play pop sound here
            this.StopAllActions ();
            var pop = new CCParticleExplosion(this.Position);
            pop.EndColor = new CCColor4F(CCColor3B.Yellow);
            pop.AutoRemoveOnFinish = true;
            pop.BlendAdditive = true;
            pop.Life = 1.5F;
            pop.EmissionRate = 80;
            pop.StartColor = new CCColor4F(_color);
            layer.AddChild(pop);
            //CCAudioEngine.SharedEngine.PlayEffect ("sounds/pop");
        }

        public void ForcePop(CCLayer layer)
        {
            PopAnimation(layer);
            this.RemoveFromParent(true);
        }

        public bool Pop(CCLayer layer)
        {
            if (this.NumberOfRunningActions == 1 
                && this.ScaleX + this.ScaleY > _bubbleMax - 1.25) 
            {
                RunAction(_repeatedAction);
            }

            if (this.ScaleX + this.ScaleY < _bubbleMax)
                return false;

            PopAnimation(layer);

            this.RemoveFromParent(true);
            return true;
        }

        public bool ContainsPoint(CCPoint toTest)
        {
            // Is the point inside the circle? Sum the squares of the x-difference and
            // y-difference from the centre, square-root it, and compare with the radius.
            // (This is Pythagoras' theorem.)

            var dX = Math.Abs(toTest.X - this.Position.X);
            var dY = Math.Abs(toTest.Y - this.Position.Y);

            var sumOfSquares = dX * dX + dY * dY;

            int distance = (int) Math.Sqrt(sumOfSquares);

            return (this.ScaledContentSize.Width + 15 >= distance);
        }
    }
}
