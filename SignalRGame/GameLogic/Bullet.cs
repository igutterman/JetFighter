namespace SignalRGame.GameLogic
{
    public class Bullet : GameObject
    {
        private const float bulletVelocity = 2;

        private float lifetime = 0;
        private float totalLifetime = 3000;

        //bulletVelocity should come from options
        public Bullet(float x, float y, float angle, GameConfigOptions options) : base(x, y, angle, options.jetSpeed, options)
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

            base.Update(elapsedTime);
        }

        public override void Clean()
        {
            return;
        }
    }
}
