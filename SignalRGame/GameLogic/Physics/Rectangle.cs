namespace SignalRGame.GameLogic.Physics
{
    public class Rectangle
    {
        public Vec2 Position;
        private float width, height;
        private float angle;

        public Rectangle(float x, float y, float width, float height, float angle)
        {
            Position = new Vec2 { x = x, y = y };
            this.width = width;
            this.height = height;
            this.angle = angle;
        }

        public void SetPos(float x, float y)
        {
            Position.x = x; 
            Position.y = y;
        }

        public void SetAngle(float angle)
        {
            this.angle = angle; 
        }

        public Vec2 TL
        {
            get 
            {
                return Position +
                    Vec2.Rotate(new Vec2() { x =  -width / 2, y = height / 2 }, angle);
            }
        }

        public Vec2 TR
        {
            get
            {
                return Position +
                    Vec2.Rotate(new Vec2() { x = width / 2, y = height / 2 }, angle);
            }
        }

        public Vec2 BL
        {
            get
            {
                return Position +
                    Vec2.Rotate(new Vec2() { x = -width / 2, y = -height / 2 }, angle);
            }
        }

        public Vec2 BR
        {
            get
            {
                return Position +
                    Vec2.Rotate(new Vec2() { x = width / 2, y = -height / 2 }, angle);
            }
        }


        public bool Intersects(Rectangle other)
        { 
            // This will double count.
            // Fine for the moment though.
            var a = new List<Vec2> {TL,TR, BR,BL, TL};
            var b = new List<Vec2> { other.TL, other.TR, other.BR, other.BL, other.TL };

            for (int i = 0; i < a.Count - 1; i++)
            {
                for (int j = 0; j < b.Count - 1; j++)
                {
                    if (Vec2.Intersect(a[i], a[i + 1], b[i], b[i + 1]))
                        return true;

                }
            }
            return false;
        }

        /// <summary>
        /// Method to check whether a point lies inside this rectangle.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>True if point is inside the rectangle</returns>
        public bool Inside(Vec2 point)
        {
            var a = new List<Vec2> { TL, TR, BR, BL, TL };

            for (int i = 0; i < a.Count - 1; i++)
            {
                if (Vec2.AngleBetween(a[i +1] - a[i], point - a[i]) < 0)
                    return false;
            }
            return true;
        }
    }
}
