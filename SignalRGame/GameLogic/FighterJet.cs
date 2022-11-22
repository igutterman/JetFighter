using SignalRChat.GameLogic.Physics;

namespace SignalRChat.GameLogic
{
    public class FighterJet : GameObject
    {
        public List<Bullet> Bullets { get; private set; }
        public FighterJet(float x, float y, float angle)
            : base(x, y, angle, 0.5f)
        {
            Bullets = new List<Bullet>();
            
            Hitboxes.Add(new Rectangle(X, Y, 10, 10, angle));

        }

        public void FireBullet()
        {
            Bullets.Add(new Bullet(X, Y, Angle));
        }

        public override void Update(float elapsedTime)
        {
            foreach (Bullet bullet in Bullets)
            {
                bullet.Update(elapsedTime);
            }    

            base.Update(elapsedTime);
        }
    }
}
