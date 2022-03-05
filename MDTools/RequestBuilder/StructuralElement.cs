using System.Collections.Generic;
using Google.Apis.Docs.v1.Data;

namespace MDTools.RequestBuilder
{
    public abstract class StructuralElement
    {
        /// <summary>
        /// This function returns a list of the Google Document Request objects needed to build the
        /// StructuralElement when sent to the Docs API.
        /// </summary>
        /// <remarks>
        /// InsertText requests use the EndOfSegment location by default.
        ///
        /// Although Google Evangelists recommend inserting text at the beginning and building the document backward
        /// to simplify range tracking, it DOES NOT WORK. The problem is that you cannot insert an indented
        /// list item at the beginning of the document -- it is treated as a first-level list item with an extra tab.
        /// </remarks>
        /// <param name="insertionPoint">The location in the doc where the insertion begins.</param>
        /// <returns>A tuple containing the list of requests in the order they should appear and the length by which the
        /// insertion point should be incremented for the next StructuralElement. Do not include the initial insertionPoint
        /// in the length value, it must be the difference between the old and the new insertion point.</returns>
        public abstract (List<Request> requests, int length) RequestList(int insertionPoint, bool useEoS = true);
    }
}