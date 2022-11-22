using SignalRChat.GameLogic.Physics;

namespace SignalRChat.GameLogic
{
    public interface IGameObject
    {
        public void Update(float elapsedTime);

        public void Rotate(float angle);

        public double AngleTo(IGameObject gameObject);

        public bool CollidesWith (IGameObject gameObject);

        public float X { get; set; }
        public float Y { get; set; }

        public float Angle { get; set; }

        public float Velocity { get; set; }

        public List<Rectangle> Hitboxes { get;}

    }
}
