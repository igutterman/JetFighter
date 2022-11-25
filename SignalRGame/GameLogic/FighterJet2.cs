using SignalRGame.GameLogic.Physics;

namespace SignalRGame.GameLogic
{
    public class FighterJet2 : FighterJet
    {
        public List<Bullet> Bullets { get; private set; }
        public FighterJet2(float x, float y, float angle)
            : base(x, y, angle)
        {
            Bullets = new List<Bullet>();
            
            Velocity = 0;
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
