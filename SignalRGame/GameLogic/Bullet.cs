﻿namespace SignalRGame.GameLogic
{
    public class Bullet : GameObject
    {
        private const float bulletVelocity = 1;

        private float lifetime = 0;
        private float totalLifetime = 1000;

        public Bullet(float x, float y, float angle) : base(x, y, angle, bulletVelocity)
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
