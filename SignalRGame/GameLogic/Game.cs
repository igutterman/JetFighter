using System.Collections.Concurrent;
using System.Diagnostics;

namespace SignalRGame.GameLogic
{
    public class Game
    {
        private CancellationTokenSource cts;

        private ConcurrentBag<FighterJet> jets;

        public ConcurrentDictionary<string, FighterJet> players;

        public Game()
        {
            cts = new CancellationTokenSource();
            jets = new ConcurrentBag<FighterJet>();
            players = new ConcurrentDictionary<string, FighterJet>();
        }

        public FighterJet AddJet(float x, float y, float angle, int jetID)
        {
            var jet = new FighterJet(x, y, angle, jetID);

            jets.Add(jet);

            return jet;

        }

        public int GetPlayerCount()
        {
            return players.Count;
        }


        public bool AddPlayer(string playerID)
        {

            if (players.Count == 0)
            {
                FighterJet jet = AddJet(100, 100, 1.57f, 1);
                players.TryAdd(playerID, jet);

                return true;
            } else if (players.Count == 1)
            {
                FighterJet jet = AddJet(900, 900, 4.71f, 2);
                players.TryAdd(playerID, jet);
                return true;
            } else
            {
                return false;
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
                                System.Diagnostics.Debug.WriteLine("collision");
                                ;
                                // mark jet as hit by jetOther
                                // something else will have to clean up the jets
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
