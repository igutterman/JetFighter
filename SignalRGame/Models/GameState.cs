using SignalRGame.GameLogic;
using System.Collections.Concurrent;


namespace SignalRGame.Models
{
    public class GameState
    {

        public IList<FighterJet> Jets { get; set; } = new List<FighterJet>();


        public GameState(IList<FighterJet> jets)
        {
            Jets = jets;
        }

        //delete after testing?
        public GameState()
        {

        }





        //Methods for client testing
        public Bullet GenerateDummyBullet()
        {
            Random random = new Random();

            Bullet bullet = new Bullet
            {
                X = random.Next(0, 1001),
                Y = random.Next(0, 1001),
                Angle = (float)random.NextDouble() * ((MathF.PI) - -(MathF.PI)) + -MathF.PI,
                Velocity = 1
            };

            return bullet;
        }



        public GameState GenerateDummyState()
        {
            Random random = new Random();




            FighterJet jetOne = new FighterJet
            {
                X = random.Next(0, 1001),
                Y = random.Next(0, 1001),
                Angle = (float)random.NextDouble() * ((MathF.PI) - -(MathF.PI)) + -MathF.PI,
                Velocity = 0.5f,
                jetID = 1,
                

            };


            FighterJet jetTwo = new FighterJet
            {
                X = random.Next(0, 1001),
                Y = random.Next(0, 1001),
                Angle = (float)random.NextDouble() * ((MathF.PI) - -(MathF.PI)) + -MathF.PI,
                Velocity = 0.5f,
                jetID = 2,

            };

            jetOne.Bullets.Add(GenerateDummyBullet());
            jetOne.Bullets.Add(GenerateDummyBullet());

            jetTwo.Bullets.Add(GenerateDummyBullet());
            jetTwo.Bullets.Add(GenerateDummyBullet());

            GameState gameState = new GameState
            {
                Jets = new List<FighterJet>() { jetOne, jetTwo }
   
            };

            return gameState;


        }


    }
}
