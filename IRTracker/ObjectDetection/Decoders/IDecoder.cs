using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace IRTracker.ObjectDetection
{
    interface IDecoder
    {
        int Decode(List<int> stopwatches);
    }
}
