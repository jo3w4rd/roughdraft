using System;
using System.Collections.Generic;
using Google.Apis.Docs.v1.Data;

namespace MDTools.RequestBuilder
{
    public class Paragraph : StructuralElement
    {
        private string _text = "";
        public string Text // public for debugging only, should be private
        {
            get { return _text;}
            set { _text = value; }
        }

        public bool AddNewLine = true;
        public int Length => Text.Length;
        private List<StyleRun> _textStyleRuns;
        private List<StyleRun> TextStyleRuns
        {
            get
            {
                if (_textStyleRuns == null)
                    _textStyleRuns = new List<StyleRun>();
                return _textStyleRuns;
            }
        }
        private ParagraphStyle Style;
        private List<InlineObject> InlineObjects;

        public Paragraph() { Style = ParagraphStyle.NORMAL_TEXT;}
        public Paragraph(ParagraphStyle style) {Style = style; }
        
        public (int start, int end) AppendText(string text)
        {
            int start = Text.Length;
            Text += text;
            int end = Text.Length;
            return (start, end);
        }
        
        public (int start, int end) AppendText(string text, TextStyles textStyleName)
        {
            var range = AppendText(text);
            if(textStyleName != TextStyles.Normal)
                SetTextStyle(textStyleName, range.start, range.end);
            return range;
        }
        //Used for Autolinks because they can't have internal formatting
        public (int start, int end) AppendLink(string text, TextStyles textStyleName, string url)
        {
            var range = AppendText(text);
            var linkStyle = new Style(TextStyles.Link, url);
            SetTextStyle(linkStyle, range.start, range.end);
            return range;
        }
        
        // Used for standard links, which can have emphasis inside the link text
        public void MakeLink(int start, int end, string url)
        {
            var linkStyle = new Style(TextStyles.Link, url);
            SetTextStyle(linkStyle, start, end);
        }

        
        // public (int start, int end) AppendLink(string text, TextStyles textStyleName, string url)
        // {
        //     int currentLength = Text.Length;
        //     Text += text;
        //     var linkStyle = new Style(TextStyles.Link, url);
        //     SetTextStyle(linkStyle, currentLength, currentLength + text.Length);
        //     int end = Text.Length;
        //     return (currentLength, end);
        // }

        public void SetTextStyle(TextStyles textStyleName, int start, int end)
        {
            if(start < 0 || start > Text.Length || end < 0 || end > Text.Length + 1)
                throw new ArgumentException($"Applying style outside text run start: {start}, end: {end}, len: {Text.Length}");

            if (textStyleName != TextStyles.Normal)
            {
                var style = new Style(textStyleName);
                var sRun = new StyleRun(style, start, end);
                TextStyleRuns.Add(sRun);
            }
        }
        
        public void SetTextStyle(Style textStyle, int start, int end)
        {
            if(start < 0 || start > Text.Length || end < 0 || end > Text.Length + 1)
                throw new ArgumentException($"Applying style outside text run start: {start}, end: {end}, len: {Text.Length}");
        
            var sRun = new StyleRun(textStyle, start, end);
            TextStyleRuns.Add(sRun);
        }

        /// <summary>
        /// Builds the request list for the paragraph text, paragraph style, if any, and the text styles of any runs of text
        /// that have styles (emphasis, links).
        /// </summary>
        /// <param name="insertionPoint">The insertion point of the first character of the paragraph. All styles must specify
        /// a start index based on this point.</param>
        /// <param name="useEoS">Whether to use the EndOfSegment location for the text insertion. (Probably unnecessary
        /// since we have to calculate exact ranges for styles and tables anyway.)</param>
        /// <returns>A tuple containing the list of requests and the insertion length, which is the number of characters in the
        /// paragraph (minus any characters like leading tabs that are stripped out of list items), +1 for inline images,
        /// and +1 for tables (I think).</returns>
        public override (List<Request> requests, int length) RequestList(int insertionPoint, bool useEoS = true)
        {
            var reqs = new List<Request>();
            var insertionLength = 0;
            
            //The paragraph text
            if (!string.IsNullOrEmpty(Text))
            {
                var insert = RequestHelpers.MakeTextInsertRequest(this.Text, useEoS ? -1 : insertionPoint, AddNewLine); 
                insertionLength = insert.InsertText.Text.Length; // A new line character can be added by MakeTextInsertRequest, so use final length 
                reqs.Add(insert);
            }
            else
                return (reqs,insertionLength);

            // The style spans for things like bold, italic, and links (which are a style)
            if (TextStyleRuns != null)
            {
                foreach (var run in TextStyleRuns)
                {
                    if(!run.style.IsEmpty)
                    {
                        Console.WriteLine($"{run.start}-{run.end}: {insertionPoint}-{insertionLength}");
                        reqs.Add(RequestHelpers.MakeTextUpdateRequest(run, insertionPoint));
                    }
                }
            }

            // The paragraph style. Skipped for normal text since it is the default.
            if(Style != ParagraphStyle.NORMAL_TEXT)
                reqs.Add(RequestHelpers.MakeParagraphStyleUpdateRequest(Style, insertionPoint, insertionLength + insertionPoint));

            return (reqs, insertionLength);
        }
        
        /// <summary>
        /// Returns the GDoc string for the specified style enum value.
        /// </summary>
        /// <param name="paraStyle">The enumeration style item.</param>
        /// <returns>The string to use in the GDoc request.</returns>
        public static string GetParaStyleName(ParagraphStyle paraStyle)
        {
            switch (paraStyle)
            {
                case ParagraphStyle.TITLE:
                    return "HEADING_1";
                case ParagraphStyle.HEADING_1:
                    return "HEADING_1";
                case ParagraphStyle.HEADING_2:
                    return "HEADING_2";
                case ParagraphStyle.HEADING_3:
                    return "HEADING_3";
                case ParagraphStyle.HEADING_4:
                    return "HEADING_4";
                case ParagraphStyle.HEADING_5:
                    return "HEADING_5";
                case ParagraphStyle.HEADING_6:
                    return "HEADING_6";
                case ParagraphStyle.NORMAL_TEXT:
                    return "NORMAL_TEXT";
                default:
                    return "NORMAL_TEXT";
            }
        }
        
    }
}