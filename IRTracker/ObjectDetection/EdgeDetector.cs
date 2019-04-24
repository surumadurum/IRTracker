using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace IRTracker.ObjectDetection
{
    class EdgeDetector
    {
        public enum Edge 
        {
            RISING,
            FALLING
        };

        bool lastValue = false;

        public delegate void OnEdgeDetectedHandler(Edge edge);
        public event OnEdgeDetectedHandler OnEdgeDetected;

        public EdgeDetector()
        {

        }

        public void NewValue(bool val)
        {
            if (lastValue != val)
            {
                Debug.WriteLine("[EdgeDetector] Edge detected");

                OnEdgeDetected(Convert.ToInt32(lastValue) > Convert.ToInt32(val) ? Edge.FALLING : Edge.RISING);
                lastValue = val;                    
            }
        }


      

    }
}
