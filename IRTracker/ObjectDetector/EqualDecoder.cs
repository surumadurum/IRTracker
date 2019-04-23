using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRTracker
{
    class EqualDecoder : IDecoder
    {
        public int deadZone { get; set; } = 500;    //defines how different the periods of a frame can be to be regarded validly equal
        public int precision { get; set; } = 10;    //defines the precision in ms for creation of ID. eG average period time is 254 -> ID at precision 10 would be 25

        public int Decode(List<Stopwatch> stopwatches)
        {
#if DEBUG
            foreach (var stopwatch in stopwatches)
            {
                Debug.WriteLine("[EqualDecoder] frame: {0}", stopwatch.ElapsedMilliseconds);
            }
#endif
            stopwatches = stopwatches.GetRange(0, Properties.Settings.Default.frameLength); //strip off trailing watches

            int frameTimeRange = (int)(stopwatches.Max((x) => x.ElapsedMilliseconds) - stopwatches.Min((x) => x.ElapsedMilliseconds));

         
            if (frameTimeRange < deadZone)
            {
                int ID = (int)stopwatches.Average((x) => x.ElapsedMilliseconds)/precision;
                return ID;
            }
            else
                throw new InvalidDecoderConditionException(string.Format("period times differed by {0}ms", frameTimeRange));
        }
    }

   
}
