namespace SignalRGame.GameLogic
{
    public class Bullet : GameObject
    {
        private const float bulletVelocity = 2;

        private float lifetime = 0;
        private float totalLifetime = 3000;

        public int drawState { get; set; } = 0;

        //bulletVelocity should come from options
        public Bullet(float x, float y, float angle, GameConfigOptions options) : base(x, y, angle, options.bulletSpeed, options)
        {
        }


        //added to use object initializer for sending dummy data to client
        public Bullet()
        {
        }

        public override void Update(float elapsedTime)
        {
            lifetime += elapsedTime;

            if (lifetime > totalLifetime)
                MarkForDeletion = true;

            if (drawState > 0 && drawState < 20)
            {
                drawState += 1;
            }

            if (drawState >= 20)
            {
                MarkForDeletion = true;
            }

            if (drawState == 0)
            {
                base.Update(elapsedTime);
            }
            
        }

        public override void Clean()
        {
            return;
        }
    }
}
