using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRTracker.ObjectDetection
{
    class IdentifiedObject
    {
        public int ID;
        public Vector2 latestPosition
        {
            get { return positionHistory.Last(); }
            set { positionHistory.Add(value); }
        }
        public List<Vector2> positionHistory = new List<Vector2>();

        public IdentifiedObject(int ID,Vector2 position)
        {
            this.ID = ID;
            this.latestPosition = position;
        }
    }
}
