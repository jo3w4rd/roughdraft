using System;
using System.Collections.Generic;
using System.Threading;
using Google.Apis.Docs.v1.Data;

namespace MDTools.RequestBuilder
{
    public class DocumentList : StructuralElement
    {
        public static readonly char Tab = (char)0x09;
        public static readonly char NewLine = (char)0x0a;
        public static readonly char LineFeed = (char)0x0b;

        private List<ListItem> items;
        public DocumentList()
        {
            items = new List<ListItem>();
        }

        public ListItem AddItem(int level, bool ordered)
        {
            var li = new ListItem(level, ordered);
            items.Add(li);
            return li;
        }

        //Add same type of item as last, defaults to unordered
        public ListItem AddItem(string text)
        {
            return AddItem(0, false, text);
        }
        public ListItem AddItem(int level, bool ordered, string text)
        {
            ListItem li = AddItem(level, ordered);
            li.P.AppendText(text);
            return li;
        }

        public ListItem AddItem(int level, bool ordered, string text, TextStyles style)
        {
            ListItem li = AddItem(level, ordered);
            li.P.AppendText(text, style);
            return li;
        }

        //public int AddSubimage(){} // TODO -- this is an image on its own, not one inside item text.

        public override (List<Request> requests, int length) RequestList(int insertionPoint, bool useEoS = true)
        {
            var reqChain = new List<Request>();
            var intraListInsertionPoint = 0;
            foreach (var item in items)
            {
                var reqs = item.RequestList(insertionPoint + intraListInsertionPoint, useEoS);
                reqs.requests.Add(RequestHelpers.MakeCreateBulletRequest(item.Level, item.Ordered, insertionPoint + intraListInsertionPoint+1, insertionPoint + intraListInsertionPoint+1)); // Only need a point inside paragraph, don't need exact span
                reqChain.AddRange(reqs.requests);
                intraListInsertionPoint += reqs.length;
            }
            return (reqChain, intraListInsertionPoint);
        }
    }
}