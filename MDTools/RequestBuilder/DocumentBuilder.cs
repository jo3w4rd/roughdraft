using System.Collections.Generic;
using System.Linq;
using Google.Apis.Docs.v1.Data;

namespace MDTools.RequestBuilder
{
    public class DocumentBuilder : StructuralElement
    {
        protected List<StructuralElement> elements;
        public bool AddNewLinesToParagraphs = true;
        public DocumentBuilder()
        {
            elements = new List<StructuralElement>();
        }
        public Paragraph AddParagraph(ParagraphStyle style)
        {
            var p = new Paragraph(style);
            p.AddNewLine = AddNewLinesToParagraphs;
            elements.Add(p);
            return p;
        }
        public Paragraph AddParagraph(ParagraphStyle style, string text, TextStyles textStyleName)
        {
            var p = new Paragraph(style);
            p.AddNewLine = AddNewLinesToParagraphs;
            p.AppendText(text, textStyleName);
            elements.Add(p);
            return p;
        }

        public DocumentList AddList()
        {
            var l = new DocumentList();
            elements.Add(l);
            return l;
        }

        public Table AddTable(int rows, int cols)
        {
            var table = new Table(rows, cols);
            elements.Add(table);
            return table;
        }

        public QuotedBlock AddBlockQuote()
        {
            var block = new QuotedBlock();
            elements.Add(block);
            return block;
        }
        
        public override (List<Request> requests, int length) RequestList(int insertionPoint = 1, bool useEoS = true)
        {
            var requests = new List<Request>(elements.Count);
            int requestLength = 0;
            foreach (var element in elements)
            {
                var reqs = element.RequestList(insertionPoint + requestLength, useEoS);
                requestLength += reqs.length;
                requests.AddRange(reqs.requests);
                
                //Add blank line between structural elements -- adds too many
                //requests.Add(RequestHelpers.MakeTextInsertRequest("\n", insertionPoint + requestLength, false));
                //requestLength += 1;
            }

            return (requests, requestLength);
        }
    }
}