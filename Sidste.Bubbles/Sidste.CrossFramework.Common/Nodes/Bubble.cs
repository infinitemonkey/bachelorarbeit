using System;
using CocosSharp;

namespace Sidste.CrossFramework.Common.Nodes
{
    public class Bubble : CCDrawNode
    {
        private int _bubblePoints = 50;
        private float _bubbleMax;
        private float _growthTime; 
        private readonly CCColor4B _color;
        private CCScaleBy _scale;
        private CCMoveBy _move1, _move2;
        private CCRepeatForever _repeatedAction;
        private readonly int _maxGrowthTime;

        private static readonly CCColor4B Color0 = new CCColor4B (119, 208, 101, 255);
        private static readonly CCColor4B Color1 = new CCColor4B (180, 85, 182, 255);
        private static readonly CCColor4B Color2 = new CCColor4B (255, 255, 255, 255);//white
        private static readonly CCColor4B Color3 = new CCColor4B (44, 62, 80, 255);
        private static readonly CCColor4B Color4 = new CCColor4B (255, 255, 75, 255);//yellow

        public int Points => (int)(_bubblePoints * ScaleX);

        private static int _universalId;
        public int Id { get; }

        public CCColor4B ColorId => _color;

        public CCColor4F ColorF { get; }

        public bool IsFrozen { get; private set; }

        public Bubble(int maxColors, int maxGrowthTime)
        {
            Id = _universalId++;
            Scale = 0.5f;
            switch (CCRandom.Next(0, maxColors))
            {
                case 0:
                    _color = Color0;
                    break;
                case 1:
                    _color = Color1;
                    break;
                case 2:
                    _color = Color2;
                    break;
                case 3:
                    _color = Color3;
                    break;
                case 4:
                    _color = Color4;
                    break;
            }

            _maxGrowthTime = maxGrowthTime;
            ColorF = new CCColor4F(_color);
            var size = CCRandom.Next(25, 50);
            ContentSize = new CCSize(size, size);
            DrawSolidCircle(Position, size, _color);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            _bubbleMax = CCRandom.Next(4, 6);
            _growthTime = CCRandom.Next(3, _maxGrowthTime);
            _scale = new CCScaleBy(_growthTime, _bubbleMax);
            AddAction(_scale);

            _move1 = new CCMoveBy(.2f, new CCPoint(4, 4));
            _move2 = new CCMoveBy(.24f, new CCPoint(-4, -4));
            _repeatedAction = new CCRepeatForever(_move1, _move2);
        }

        public void Freeze(int count)
        {
            IsFrozen = true;
            StopAllActions();
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
            StopAllActions();
            var pop = new CCParticleExplosion(this.Position)
            {
                EndColor = new CCColor4F(CCColor3B.Yellow),
                AutoRemoveOnFinish = true,
                BlendAdditive = true,
                Life = 1.5F,
                EmissionRate = 80,
                StartColor = new CCColor4F(_color)
            };
            layer.AddChild(pop);
            //CCAudioEngine.SharedEngine.PlayEffect ("sounds/pop");
        }

        public void ForcePop(CCLayer layer)
        {
            PopAnimation(layer);
            RemoveFromParent();
        }

        public bool Pop(CCLayer layer)
        {
            if (NumberOfRunningActions == 1 
                && ScaleX + ScaleY > _bubbleMax - 1.25) 
            {
                RunAction(_repeatedAction);
            }

            if (ScaleX + ScaleY < _bubbleMax)
                return false;

            PopAnimation(layer);

            RemoveFromParent();
            return true;
        }

        public bool ContainsPoint(CCPoint toTest)
        {
            // Is the point inside the circle? Sum the squares of the x-difference and
            // y-difference from the centre, square-root it, and compare with the radius.
            // (This is Pythagoras' theorem.)

            var dX = Math.Abs(toTest.X - Position.X);
            var dY = Math.Abs(toTest.Y - Position.Y);

            var sumOfSquares = dX * dX + dY * dY;

            int distance = (int) Math.Sqrt(sumOfSquares);

            return ScaledContentSize.Width + 15 >= distance;
        }
    }
}
