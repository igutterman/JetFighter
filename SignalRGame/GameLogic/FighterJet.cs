﻿using SignalRGame.GameLogic.Physics;
using System.Collections.Concurrent;
using System.Text.Json.Serialization;


namespace SignalRGame.GameLogic
{

    public class FighterJet : GameObject
    {
        //public string playerID { get; set; }

        public int jetID { get; set; }
        public ConcurrentBag<Bullet> Bullets { get; private set; } = new ConcurrentBag<Bullet>();
        public FighterJet(float x, float y, float angle, int jetID)
            : base(x, y, angle, 1.0f)
        {
            Bullets = new ConcurrentBag<Bullet>();

            Hitboxes.Add(new Rectangle(X, Y, 10, 10, angle));
            this.jetID = jetID;
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
            var newBullets = new ConcurrentBag<Bullet>();
            foreach (Bullet bullet in Bullets)
            {
                if (!bullet.MarkForDeletion)
                    newBullets.Add(bullet);
                
            }

            Bullets = newBullets;
        }
    }
}
