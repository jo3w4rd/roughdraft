using System.Collections.Generic;
using Google.Apis.Docs.v1.Data;

namespace MDTools.RequestBuilder
{
    /// <summary>
    /// A QuotedBlock stores the properties and content of a markdown BlockQuote.
    /// </summary>
    /// <remarks>
    /// Complicating matters is that markdown allows elements like headings and lists inside a block quote.
    /// You can even nest block quotes in markdown. However nested block quotes aren't supported in this tool.
    /// I don't know of a pressing need for them in technical documentation.
    /// What currently happens is that the nested block quote "steals" any following lines from the block
    /// it is nested in.
    /// </remarks>
    public class QuotedBlock : DocumentBuilder
    {
        private BlockQuoteType type;
        public QuotedBlock(BlockQuoteType type = BlockQuoteType.Standard)
        {
            this.type = type;
        }

        public (Color shade, Color border) GetColorForType(BlockQuoteType quoteType)
        {
            var shade = new Color();
            shade.RgbColor = new RgbColor();
            var border = new Color();
            border.RgbColor = new RgbColor();
            switch (quoteType)
            {
                case BlockQuoteType.Important: 
                case BlockQuoteType.Caution:
                    // light red
                    shade.RgbColor.Red = 239f/256f;
                    shade.RgbColor.Green = 223f/256f;
                    shade.RgbColor.Blue = 222f/256f;
                    border.RgbColor.Red = 230f/256f;
                    border.RgbColor.Green = 206f/256f;
                    border.RgbColor.Blue = 209f/256f;
                    return (shade, border);
                case BlockQuoteType.Warning:
                    // light yellow
                    shade.RgbColor.Red = 251f/256f;
                    shade.RgbColor.Green = 248f/256f;
                    shade.RgbColor.Blue = 228f/256f;
                    border.RgbColor.Red = 248f/256f;
                    border.RgbColor.Green = 236f/256f;
                    border.RgbColor.Blue = 206f/256f;
                    return (shade, border);
                case BlockQuoteType.Note:
                case BlockQuoteType.Tip:
                    // light blue
                    shade.RgbColor.Red = 221f/256f;
                    shade.RgbColor.Green = 236f/256f;
                    shade.RgbColor.Blue = 246f/256f;
                    border.RgbColor.Red = 196f/256f;
                    border.RgbColor.Green = 230f/256f;
                    border.RgbColor.Blue = 240f/256f;
                    return (shade, border);
                case BlockQuoteType.Standard:
                default:
                    // light grey
                    shade.RgbColor.Red = 245f/256f;
                    shade.RgbColor.Green = 245f/256f;
                    shade.RgbColor.Blue = 245f/256f;
                    border.RgbColor.Red = 200f/256f;
                    border.RgbColor.Green = 200f/256f;
                    border.RgbColor.Blue = 200f/256f;
                    return (shade, border);
            }
        }

        private void FindQuoteType()
        {
            foreach (var element in elements)
            {
                if (element.GetType() == typeof(Paragraph))
                {
                    var para = (Paragraph) element;
                    if (para.Text.Trim().StartsWith("[!NOTE]"))
                    {
                        type = BlockQuoteType.Note;
                    }
                    else if (para.Text.Trim().StartsWith("[!TIP]"))
                    {
                        type = BlockQuoteType.Tip;
                    }
                    else if (para.Text.Trim().StartsWith("[!WARNING]"))
                    {
                        type = BlockQuoteType.Warning;
                    }
                    else if (para.Text.Trim().StartsWith("[!CAUTION]"))
                    {
                        type = BlockQuoteType.Caution;
                    }
                    else if (para.Text.Trim().StartsWith("[!IMPORTANT]"))
                    {
                        type = BlockQuoteType.Important;
                    }
                }
            }
        }
        
        /// <summary>
        /// Add the Quote character to the front.
        /// </summary>
        /// <param name="insertionPoint"></param>
        /// <param name="useEoS"></param>
        /// <returns></returns>
        public override (List<Request> requests, int length) RequestList(int insertionPoint = 1, bool useEoS = true)
        {
            FindQuoteType();
            var requests = new List<Request>();
            requests.Add(RequestHelpers.MakeTextInsertRequest(">>>\n", insertionPoint, false));
            var quoteReqInfo = base.RequestList(insertionPoint + 4, useEoS);
            requests.AddRange(quoteReqInfo.requests);
            requests.Add(RequestHelpers.MakeTextInsertRequest(">>>", -1, true));
            var quoteColor = GetColorForType(this.type);
            requests.Add(RequestHelpers.MakeParagraphBackgroundUpdateRequest(quoteColor.shade, quoteColor.border, insertionPoint,
                insertionPoint + quoteReqInfo.length + 8));
            return (requests, quoteReqInfo.length + 8);
        }
    }
    
    public enum BlockQuoteType{Tip, Note, Warning, Caution, Important, Standard}
}