using SignalRGame.GameLogic.Physics;
using System.Text.Json.Serialization;

namespace SignalRGame.GameLogic
{
    
    public abstract class GameObject : IGameObject
    {
        public GameObject(float x, float y, float angle, float velocity)
        {
            position = new Vec2();
            X = x;
            Y = y;
            Angle = angle;
            Velocity = velocity;

        }

        private List<Rectangle> hitboxes = new List<Rectangle>();

        private Vec2 position;

        public float X { get => position.x; set { position.x = value; } }
        public float Y { get => position.y; set { position.y = value; } }

        public float Angle { get; set; }

        public float Velocity { get; set; }

        [JsonIgnore]
        public List<Rectangle> Hitboxes { get => hitboxes; }

        public bool MarkForDeletion { get; set; }
        public Vec2 Position { get => position; }

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
            X += MathF.Cos(Angle) * Velocity * GameConfig.gameSpeed;
            Y += MathF.Sin(Angle) * Velocity * GameConfig.gameSpeed;


            // This doesn't account for collisions that happen over the edge of the canvas.
            // DP we want to do anything about that
            if (X > GameConfig.canvasWidth)
                X -= GameConfig.canvasWidth;

            if (Y > GameConfig.canvasHeight)
                Y -= GameConfig.canvasHeight;

            if (X < 0)
                X += GameConfig.canvasWidth;

            if (Y < 0)
                Y += GameConfig.canvasHeight;

            foreach (var hitbox in Hitboxes)
            {
                // All hitboxes are centered on object for the moment.
                hitbox.SetPos(X, Y);
                hitbox.SetAngle(Angle);
            }
        }

        public abstract void Clean();

        public double AngleTo(IGameObject gameObject)
        {
            return Vec2.AngleBetween(Position, gameObject.Position);
        }

        public bool CollidesWith(IGameObject gameObject)
        {
            foreach (var hitboxa in Hitboxes)
            {
                // check if the hitboxes intersect
                foreach (var hitboxb in gameObject.Hitboxes)
                {
                    if (hitboxa.Intersects(hitboxb))
                        return true;

                }
            }

            // check if each object is inside any of the hitboxes
            // useful for bullets and objects that are fully inside the other
            foreach (var hitboxa in Hitboxes)
            {
                if (hitboxa.Inside(gameObject.Position))
                    return true;
            }

            foreach (var hitboxb in gameObject.Hitboxes)
            {
                if (hitboxb.Inside(position))
                    return true;

            }

            return false;
        }


    }
}
