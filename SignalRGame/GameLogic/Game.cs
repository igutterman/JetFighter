﻿using System.Collections.Concurrent;
using System.Diagnostics;

namespace SignalRGame.GameLogic
{
    public class Game
    {
        private CancellationTokenSource cts;

        private ConcurrentBag<FighterJet> jets;

        public Game(float canvasWidth, float canvasHeight)
        {

            cts = new CancellationTokenSource();
            jets = new ConcurrentBag<FighterJet>();
        }

        public FighterJet AddJet(float x, float y, float angle)
        {
            var jet = new FighterJet(x, y, angle);

            jets.Add(jet);

            return jet;

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

        private const double preferredTickPeriod = 40;

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

                        foreach(var bullet in jetOther.Bullets)
                        {
                            if (jet.CollidesWith(bullet))
                            {
                                // mark jet as hit by jetOther
                                // something else will have to clean up the jets
                            }
                        }
                    }
                }

                if (diff < preferredTickPeriod)
                    Thread.Sleep((int)(preferredTickPeriod - diff));

            }

        }
    }
}
