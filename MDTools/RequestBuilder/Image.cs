using System.Collections.Generic;
using Google.Apis.Docs.v1.Data;

namespace MDTools.RequestBuilder
{
    public class Image : StructuralElement
    {
        public override (List<Request> requests, int length) RequestList(int insertionPoint, bool useEoS = true)
        {
            throw new System.NotImplementedException();
        }
    }
}