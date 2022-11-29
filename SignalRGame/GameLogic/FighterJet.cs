using SignalRGame.GameLogic.Physics;

using System.Text.Json.Serialization;


namespace SignalRGame.GameLogic
{

    public class FighterJet : GameObject
    {
        public string playerID { get; set; }
        public List<Bullet> Bullets { get; private set; } = new List<Bullet>();
        public FighterJet(float x, float y, float angle)
            : base(x, y, angle, 0.5f)
        {
            Bullets = new List<Bullet>();

            Hitboxes.Add(new Rectangle(X, Y, 10, 10, angle));

        }

        //added to use object initializer for sending dummy data to client
        public FighterJet() : base () { }


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

        public override void Clean()
        {
            // Remove bullets marked for deletion.
            var newBullets = new List<Bullet>();
            foreach (Bullet bullet in Bullets)
            {
                if (!bullet.MarkForDeletion)
                    newBullets.Add(bullet);
                
            }

            Bullets = newBullets;
        }
    }
}
