using System;
using System.Net.Security;

namespace SignalRGame.GameLogic.Physics
{
    public struct Vec2
    {
        public float x;
        public float y;

        public static Vec2 Rotate(Vec2 initial, float angle)
        {
            float x = initial.x * MathF.Cos(angle) - initial.y * MathF.Sin(angle);
            var y = initial.x * MathF.Sin(angle) + initial.y * MathF.Cos(angle);

            return new Vec2 { x = x, y = y };
        }

        /// <summary>
        /// https://gorillasun.de/blog/an-algorithm-for-polygon-intersections
        /// </summary>
        /// <param name="a1">Starting Point of first line</param>
        /// <param name="a2">Ending Point of first line</param>
        /// <param name="b1">Starting Point of second line</param>
        /// <param name="b2">Ending Point of second line</param>
        /// <returns></returns>
        public static bool Intersect(Vec2 a1, Vec2 a2, Vec2 b1, Vec2 b2)
        {
            if ((a1.x == a2.x && a1.y == a2.y) || (b1.x == b2.x && b1.y == b2.y))
            {
                return false;
            }

            var denominator = ((b2.y - b1.y) * (a2.x - a1.x) - (b2.x - b1.x) * (a2.y - a1.y));

            // Lines are parallel
            if (denominator == 0)
            {
                return false;
            }


            var ua = ((b2.x - b1.x) * (a1.y - b1.y) - (b2.y - b1.y) * (a1.x - b1.x)) / denominator;
            var ub = ((a2.x - a1.x) * (a1.y - b1.y) - (a2.y - a1.y) * (a1.x - b1.x)) / denominator;

            // is the intersection along the segments
            if (ua < 0 || ua > 1 || ub < 0 || ub > 1)
            {
                return false;
            }

            // Return a object with the x and y coordinates of the intersection
            //var x = a1.x + ua * (a2.x - a1.x);
            //var y = a1.y + ua * (a2.y - a1.y);
            return true;
        }

        public static Vec2 operator +(Vec2 a, Vec2 b)
        { 
            return new Vec2() { x = a.x + b.x, y = a.y + b.y };
        }

        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2() { x = b.x - a.x, y = b.y - a.y };
        }

        public static Vec2 operator -(Vec2 a) => new Vec2() { x = -a.x, y = -a.y };

        public static float AbsoluteAngle(Vec2 a)
        {
            return MathF.Atan2(a.x, a.y);
        }

        public static float AngleBetween(Vec2 a, Vec2 b)
        {
            var delta = AbsoluteAngle(b) - AbsoluteAngle(a);


            if (delta > MathF.PI)
                delta -= 2 * MathF.PI;


            if (delta < -MathF.PI)
                delta += 2 * MathF.PI;

            return delta;

        }

    }
}
