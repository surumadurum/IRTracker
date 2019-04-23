using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace IRTracker
{
    class UnidentifiedObject
    {
        public int[] position { get; set; }
        private EdgeDetector edgeDetector;

        public IDecoder decoder { get; set; } = new EqualDecoder();

        public delegate void OnObjectIdentifiedHandler(UnidentifiedObject obj, int ID);
        public event OnObjectIdentifiedHandler OnObjectIdentified;

        public delegate void OnObjectNotIdentifiedHandler(UnidentifiedObject obj, string error);
        public event OnObjectNotIdentifiedHandler OnObjectNotIdentified;

        private List<Stopwatch> frameStopwatches = new List<Stopwatch>();

        public UnidentifiedObject()
        {
            edgeDetector = new EdgeDetector();
            edgeDetector.OnEdgeDetected += EdgeDetectedCallback;
        }
        public void SignatureNewValue(bool val)
        {
            edgeDetector.NewValue(val);
        }

        void EdgeDetectedCallback(EdgeDetector.Edge edge)
        {
            Debug.WriteLine("[UnidentifiedObject] Edge detected: {0}", edge);

            // Stop all old stopwatches
            foreach (var stopwatch in frameStopwatches)
                stopwatch.Stop();

            Stopwatch newFrameStopwatch = new Stopwatch();
            newFrameStopwatch.Start();
            frameStopwatches.Add(newFrameStopwatch);

            CheckFrameComplete();
        }

        void CheckFrameComplete()
        {
            if(frameStopwatches.Count==Properties.Settings.Default.frameLength+1)
            {
                Debug.WriteLine("[UnidentifiedObject] frame complete");

                foreach (var stopwatch in frameStopwatches)
                    stopwatch.Stop();

                try
                {
                    int ID = decoder.Decode(frameStopwatches);

                    if (OnObjectIdentified.GetInvocationList().Length>0)
                        OnObjectIdentified(this, ID);
                }
                catch(InvalidDecoderConditionException ex)
                {
                    if (OnObjectNotIdentified.GetInvocationList().Length > 0)
                        OnObjectNotIdentified(this, ex.Message);
                }
            }
        }
    }
}
