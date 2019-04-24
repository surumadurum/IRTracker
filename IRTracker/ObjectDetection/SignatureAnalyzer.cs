using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace IRTracker.ObjectDetection
{
    class SignatureAnalyzer
    {
        public List<IdentifiedObject> identifiedObjects = new List<IdentifiedObject>(); //holds the positions of  identified objects
        private List<UnidentifiedObject> unidentifiedObjects = new List<UnidentifiedObject>(); //yet to be identified objects

        private List<UnidentifiedObject> currentFrameUnidentifiedObjectsWithBlob = new List<UnidentifiedObject>();

        /// <summary>
        /// When a blob (e.g. an IR-LED) is detected (LED ON) in a picture/video frame, this method should be called.
        /// Do this for all detected blobs.
        /// </summary>
        /// <param name="position">position of blob</param>
        public void BlobDetectedAt(Vector2 position)
        {
            var unidentifiedObj = FindCorrespondingUnidentifiedObjectOrCreateNew(position);
            unidentifiedObj.SignatureNewValue(true);

            //move it to our list of objects that received a blob in current frame
            currentFrameUnidentifiedObjectsWithBlob.Add(unidentifiedObj);
        }

        /// <summary>
        /// Call this after all blobs of current frame have been called in with BlobDetectedAt.
        /// Necessary to detect the LED OFF states
        /// </summary>
        public void FinalizeCurrentFrame()
        {

            //get all unidentified objects without blob in current frame (LED OFF)
            IEnumerable<UnidentifiedObject> currentFrameWithoutBlob = unidentifiedObjects.Except(currentFrameUnidentifiedObjectsWithBlob);

            //send OFF state to edge detector
            foreach (var obj in currentFrameWithoutBlob)
                obj.SignatureNewValue(false);

            currentFrameUnidentifiedObjectsWithBlob.Clear();
        }

        private UnidentifiedObject FindCorrespondingUnidentifiedObjectOrCreateNew(Vector2 position)
        {
            UnidentifiedObject @object = unidentifiedObjects.Find((item) => item.position.Equals(position));
            if(@object == null)
            {
                @object = new UnidentifiedObject(position);
                @object.OnObjectIdentified += ObjectIdentifiedCallback;
                @object.OnObjectNotIdentified += ObjectNotIdentifiedCallback;
                @object.OnRethrowEdge += RethrowEdgeCallback;
                unidentifiedObjects.Add(@object);
                
            }
            return @object;
        }

       void ObjectIdentifiedCallback(UnidentifiedObject obj, int ID)
        {
            Console.WriteLine("[SignatureAnalyzer] Object identified:{0} at ({1}|{2})", ID,obj.position.x,obj.position.y);

            

            IdentifiedObject @object = identifiedObjects.Find((item)=> item.ID == ID);
            if (@object == null)
            {
                identifiedObjects.Add(new IdentifiedObject(ID, obj.position));
            }
            else
                @object.latestPosition = obj.position;

            unidentifiedObjects.Remove(obj);
        }

        void ObjectNotIdentifiedCallback(UnidentifiedObject obj, string error)
        {
            Console.WriteLine("[SignatureAnalyzer] Object NOT identified:{0}", obj.GetHashCode());
            unidentifiedObjects.Remove(obj);
        }

        /// <summary>
        /// This callback will be called from UnidentifiedObject to rethrow the 5th edge from the end of previous message beacon
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="edge"></param>
        void RethrowEdgeCallback(Vector2 pos, EdgeDetector.Edge edge)
        {
            //TODO so far only rising edge supported
            BlobDetectedAt(pos);
        }
    }
}
