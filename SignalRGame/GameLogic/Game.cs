using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace SignalRGame.GameLogic
{
    public class Game
    {
        private CancellationTokenSource cts;

        private ConcurrentBag<FighterJet> jets;

        public ConcurrentDictionary<string, FighterJet> players;

        public GameConfigOptions options;




        public Game(GameConfigOptions Options)
        {
            cts = new CancellationTokenSource();
            jets = new ConcurrentBag<FighterJet>();
            players = new ConcurrentDictionary<string, FighterJet>();
            options = Options;

        }

        public FighterJet AddJet(float x, float y, float angle, int jetID, GameConfigOptions options)
        {
            var jet = new FighterJet(x, y, angle, jetID, options);

            jets.Add(jet);

            return jet;

        }

        public int GetPlayerCount()
        {
            return players.Count;
        }


        public int AddPlayer(string playerID)
        {

            if (players.Count == 0)
            {
                FighterJet jet = AddJet(100, 100, 1.57f, 1, options);
                players.TryAdd(playerID, jet);

                return 1;
            } else if (players.Count == 1)
            {
                FighterJet jet = AddJet(900, 900, 4.71f, 2, options);
                players.TryAdd(playerID, jet);
                return 2;
            } else
            {
                return 0;
            }

            //return false;

        }



        public IList<FighterJet> GetJets()
        {
            return jets.ToList();
        }

        public void Pause()
        {
            cts.Cancel();
        }

        public void Start()
        {
            Thread t = new Thread(GameThread);
            t.Start(cts.Token);
        }

        private const double preferredTickPeriod = 20;

        private void GameThread(object e)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var ct = (CancellationToken)e;

            var previousTime = stopwatch.Elapsed.TotalMilliseconds;
            while (!ct.IsCancellationRequested)
            {
                var diff = stopwatch.Elapsed.TotalMilliseconds - previousTime;
                previousTime = stopwatch.Elapsed.TotalMilliseconds;


                foreach (var jet in jets)
                {
                    jet.Update((float) diff);
                }

                foreach (var jet in jets)
                {
                    if (!jet.MarkForDeletion)
                    { 

                        foreach (var jetOther in jets)
                        {
                            // Don't compare a jet against itself
                            if (jetOther == jet)
                                continue;

                            if (jet.CollidesWith(jetOther))
                            {
                                System.Diagnostics.Debug.WriteLine("collision");
                                ;
                            }

                            foreach(var bullet in jetOther.Bullets)
                            {
                                if (jet.CollidesWith(bullet))
                                {
                                    System.Diagnostics.Debug.WriteLine("collision with bullet");
                                    //jet.MarkForDeletion = true;
                                    bullet.MarkForDeletion = true;
                                    jet.TakesDamage(10);
                                }
                            }
                        }
                    }
                }

                if (OnSendState != null)
                {
                    OnSendState(jets.ToArray());

                    foreach (var jet in jets)
                    {
                        jet.Clean();
                    }
                }

                if (diff < preferredTickPeriod)
                    Thread.Sleep((int)(preferredTickPeriod - diff));

            }

        }

        public SendState OnSendState;

        public delegate void SendState(FighterJet[] jets);
    }
}
