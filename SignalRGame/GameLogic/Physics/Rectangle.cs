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
                return Vec2.Add(Position,
                    Vec2.Rotate(new Vec2() { x =  -width / 2, y = height / 2 }, angle));
            }
        }

        public Vec2 TR
        {
            get
            {
                return Vec2.Add(Position,
                    Vec2.Rotate(new Vec2() { x = width / 2, y = height / 2 }, angle));
            }
        }

        public Vec2 BL
        {
            get
            {
                return Vec2.Add(Position,
                    Vec2.Rotate(new Vec2() { x = -width / 2, y = -height / 2 }, angle));
            }
        }

        public Vec2 BR
        {
            get
            {
                return Vec2.Add(Position,
                    Vec2.Rotate(new Vec2() { x = width / 2, y = -height / 2 }, angle));
            }
        }


        public bool Intersects(Rectangle other)
        { 
            // This will double count.
            // Fine for the moment though.
            var a = new List<Vec2> {TL,TR, BL,BR, TL};
            var b = new List<Vec2> { other.TL, other.TR, other.BL, other.BR, other.TL };

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
    }
}
