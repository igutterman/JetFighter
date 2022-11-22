using SignalRGame.GameLogic.Physics;

namespace SignalRGame.GameLogic
{
    public class GameObject : IGameObject
    {
        public GameObject(float x, float y, float angle, float velocity)
        {
            X = x;
            Y = y;
            Angle = angle;
            Velocity = velocity;

        }

        private List<Rectangle> hitboxes = new List<Rectangle>();

        public float X { get; set; }
        public float Y { get; set; }

        public float Angle { get; set; }

        public float Velocity { get; set; }
        public List<Rectangle> Hitboxes { get => hitboxes; }

        public bool MarkForDeletion { get; set; }

        public void Rotate(float angle)
        {
            Angle += angle;

            if (Angle > MathF.PI)
                Angle -= 2 * MathF.PI;


            if (Angle < -MathF.PI)
                Angle += 2 * MathF.PI;
        }

        public virtual void Update(float elapsedTime)
        {
            X += MathF.Sin(Angle) * Velocity;
            Y += MathF.Sin(Angle) * Velocity;

            foreach (var hitbox in Hitboxes)
            {
                // All hitboxes are centered on object for the moment.
                hitbox.SetPos(X, Y);
                hitbox.SetAngle(Angle);
            }
        }

        public double AngleTo(IGameObject gameObject)
        {
            var delta = MathF.Atan2(gameObject.X - X, gameObject.Y - Y) - Angle;

            if (delta > MathF.PI)
                delta -= 2 * MathF.PI;


            if (delta < -MathF.PI)
                delta += 2 * MathF.PI;

            return delta;


        }

        public bool CollidesWith(IGameObject gameObject)
        {
            foreach(var hitboxa in Hitboxes)
            {
                foreach(var hitboxb in gameObject.Hitboxes)
                {
                    if (hitboxa.Intersects(hitboxb))
                        return true;
                }
            }
            return false;
        }
    }
}
