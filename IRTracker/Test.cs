using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Diagnostics;

namespace IRTracker
{
    class Test
    {
        Stopwatch sndwatch = new Stopwatch();
        SignatureAnalyzer signature = new SignatureAnalyzer();

        Stopwatch[] stopwatches = new[] { new Stopwatch(), new Stopwatch(), new Stopwatch() };

        public void Run()
        {
            //signature.BlobDetectedAt(new Vector2(10, 10));
            //signature.BlobDetectedAt(new Vector2(20, 20));
            //signature.FinalizeCurrentFrame();
            //signature.BlobDetectedAt(new Vector2(20, 20));
            //signature.BlobDetectedAt(new Vector2(30, 30));
            //signature.FinalizeCurrentFrame();

            sndwatch.Start();

            foreach (var stopwatch in stopwatches)
                stopwatch.Start();

            while (true)
            {
                //foreach (var obj in signature.identifiedObjects)
                //    Console.WriteLine("{0}: {1}\t{2}", obj.ID, obj.latestPosition.x, obj.latestPosition.y);

                Thread.Sleep(1);

                //Stopwatches are way more accurate than Timers...

                if (stopwatches[0].ElapsedMilliseconds >= 640)
                {
                    Timer_Elapsed(null, null);
                    stopwatches[0].Restart();
                }

                if (stopwatches[1].ElapsedMilliseconds >= 64)
                {
                    Timer2_Elapsed(null, null);
                    stopwatches[1].Restart();
                }

                if (stopwatches[2].ElapsedMilliseconds >= 16)
                {
                    Shutter_Elapsed(null, null);
                    stopwatches[2].Restart();
                }

            }
        }

        int state2 = 1;
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)

        {
            state2 = 1 - state2;
            //  Console.WriteLine(sndwatch.ElapsedMilliseconds);
            sndwatch.Restart();
        }

        private void Shutter_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (state == 1)
                signature.BlobDetectedAt(new Vector2(20, 20));
            if (state2 == 1)
                signature.BlobDetectedAt(new Vector2(30, 30));

            signature.FinalizeCurrentFrame();
        }

        int state = 1;
        private void Timer2_Elapsed(object sender, ElapsedEventArgs e)
        {
            state = 1 - state;
        }
    }
}
