using System;
using System.Collections.Generic;
using Google.Apis.Docs.v1.Data;

namespace MDTools.RequestBuilder
{
    public class ListItem : StructuralElement
    {
        public Paragraph P;
        public int Level;
        public bool Ordered;

        public int Length{
            get
            {
                return P.Length;
            }
        }
        public ListItem(int level, bool ordered)
        {
            P = new Paragraph();
            Level = level;
            Ordered = ordered;
            P.AppendText(new string(DocumentList.Tab, Level));
        }
        
        // Add a "paragraph" with a soft line break -- has no bullet
        public (int start, int end) AddSubparagraph(string text)
        {
            return P.AppendText(DocumentList.LineFeed + text, TextStyles.Normal);
        }

        public override (List<Request> requests, int length) RequestList(int insertionPoint, bool useEoS = true)
        {
            var itemReqs = P.RequestList(insertionPoint, useEoS);
            return (itemReqs.requests, itemReqs.length - Level);
        }
    }
}