using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace IRTracker
{
    interface IDecoder
    {
        int Decode(List<Stopwatch> stopwatches);
    }
}
