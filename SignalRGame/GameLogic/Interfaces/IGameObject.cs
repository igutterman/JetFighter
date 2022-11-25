using SignalRGame.GameLogic.Physics;

namespace SignalRGame.GameLogic
{
    public interface IGameObject
    {
        public void Update(float elapsedTime);

        public void Rotate(float angle);

        public double AngleTo(IGameObject gameObject);

        public bool CollidesWith (IGameObject gameObject);
        void Clean();

        public float X { get; set; }
        public float Y { get; set; }

        public Vec2 Position { get; }

        public float Angle { get; set; }

        public float Velocity { get; set; }

        public List<Rectangle> Hitboxes { get;}

    }
}
