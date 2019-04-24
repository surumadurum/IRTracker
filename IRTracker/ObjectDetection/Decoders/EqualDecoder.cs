using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRTracker.ObjectDetection
{
    /// <summary>
    /// computes the ID from equal lengths of periods
    /// </summary>
    class EqualDecoder : IDecoder
    {
        public int deadZone { get; set; } = 40;    //defines how different the periods of a frame can be to be regarded validly equal
        public int precision { get; set; } = 32;    //defines the precision in ms for creation of ID. eG average period time is 254 -> ID at precision 10 would be 25

        public int Decode(List<int> bitLengths)
        {

            foreach (var stopwatch in bitLengths)
            {
                Debug.WriteLine("[EqualDecoder] frame: {0}", stopwatch);
            }

            bitLengths = bitLengths.GetRange(0, Properties.Settings.Default.frameLength); //strip off trailing watches

            int frameTimeRange = (int)(bitLengths.Max((x) => x) - bitLengths.Min((x) => x));

         
            if (frameTimeRange < deadZone)
            {
                //delete outlies and average the rest
                bitLengths = bitLengths.OrderBy((item)=>item).ToList();
                bitLengths.Remove(bitLengths.First());
                bitLengths.Remove(bitLengths.Last());
                int ID = (int)bitLengths.Average((x) => x)/precision;
                return ID;
            }
            else
                throw new InvalidDecoderConditionException(string.Format("period times differed by {0}ms", frameTimeRange));
        }

        public int Decode(List<Stopwatch> stopwatches)
        {
            List<int> bitLengths = new List<int>();
            stopwatches.ForEach((item) => bitLengths.Add((int)item.ElapsedMilliseconds));

            return Decode(bitLengths);
        }
    }

   
}
