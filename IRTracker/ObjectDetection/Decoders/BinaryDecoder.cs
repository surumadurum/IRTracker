using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRTracker.ObjectDetection


{
    class BinaryDecoder : IDecoder
    {
        //TODO put precision settings to properties

        public int precision { get; set; } = Properties.Settings.Default.sampleTime * 2;    //defines the precision in ms 

        private int lengthStartBit = Properties.Settings.Default.sampleTime * Properties.Settings.Default.startBitFactor;
        private int lengthZeroBit = Properties.Settings.Default.sampleTime * Properties.Settings.Default.logicZeroFactor;
        private int lengthOneBit = Properties.Settings.Default.sampleTime * Properties.Settings.Default.logicOneFactor;


        public int Decode(List<int> bitLengths)
        {
            if (bitLengths[0] / precision != lengthStartBit / precision)
                throw new InvalidDecoderConditionException("wrong startbit length");
            else
            {
                UInt16 decoded = 0;
                for (int i = 0; i < Properties.Settings.Default.frameLength-1; i++)
                {
                    if(bitLengths[i+1] / precision == lengthOneBit / precision)
                        decoded |= (UInt16)(1 <<  Properties.Settings.Default.frameLength-2 - i);

                }
                //TODO parity bit check
                return decoded; 
            }
        }

        public int Decode(List<Stopwatch> stopwatches)
        {
            List<int> bitLengths = new List<int>();
            stopwatches.ForEach((item) => bitLengths.Add((int)item.ElapsedMilliseconds));

            return Decode(bitLengths);
        }
    }
}
