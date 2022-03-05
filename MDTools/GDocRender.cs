using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Requests;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Range = System.Range;

namespace MDTools
{
    public enum GDocNamedStyles
    {
        TITLE, HEADING_1, HEADING_2, HEADING_3, HEADING_4, HEADING_5, HEADING_6, NORMAL_TEXT
    }

    // Only the first four are "standard" markdown
    // Link is because GDoc stores links inside a TextStyle structure
    public enum NamedTextStyles 
    {
        Normal, Bold, Italic, Code, StrikeThrough, Subscript, Superscript, Inserted, Marked, Link
    }
    
    public static class GDocRender
    {
        public static string GetParaStyleName(GDocNamedStyles paraStyle)
        {
            switch (paraStyle)
            {
                case GDocNamedStyles.TITLE:
                    return "HEADING_1";
                case GDocNamedStyles.HEADING_1:
                    return "HEADING_1";
                case GDocNamedStyles.HEADING_2:
                    return "HEADING_2";
                case GDocNamedStyles.HEADING_3:
                    return "HEADING_3";
                case GDocNamedStyles.HEADING_4:
                    return "HEADING_4";
                case GDocNamedStyles.HEADING_5:
                    return "HEADING_5";
                case GDocNamedStyles.HEADING_6:
                    return "HEADING_6";
                case GDocNamedStyles.NORMAL_TEXT:
                    return "NORMAL_TEXT";
                default:
                    return "NORMAL_TEXT";
            }
        }
        public static NamedTextStyles GetStyleFromDelimiter(string delimiter)
        {
            switch (delimiter)
            {
                case "*":
                case "_":
                    return NamedTextStyles.Italic;
                case "**": 
                case "__":
                    return NamedTextStyles.Bold;
                case "`":
                    return NamedTextStyles.Code;
                case "~~":
                    return NamedTextStyles.StrikeThrough;
                case "~":
                    return NamedTextStyles.Subscript;
                case "^":
                    return NamedTextStyles.Superscript;
                case "++":
                    return NamedTextStyles.Inserted;
                case "==":
                    return NamedTextStyles.Marked;
                default:
                    return NamedTextStyles.Normal;
            }
        }
 
        public static GDocRequest InsertTextRun(string text, NamedTextStyles styleType, int start, int end, string url = "", UpdateTextStyleRequest parentStyle = null)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Inserting empty text is not allowed");

            UpdateTextStyleRequest styleReq = null;
            if (styleType != NamedTextStyles.Normal)
            {
                if (parentStyle == null)
                {
                    styleReq = MakeTextStyle(styleType, start, end, url);
                }
                else
                {
                    styleReq = UpdateTextStyle(parentStyle, styleType, start, end, url);
                }
            }

            var req = new GDocRequest(
                new Request
                {
                    InsertText = new InsertTextRequest {Text = text, Location = new Location {Index = 1}}
                },
                new Request()
                {
                    UpdateTextStyle = styleReq
                }
                );
            return req;
        }

        public static GDocRequest UpdateParagraphStyle(GDocNamedStyles style)
        {
            return new()
            {   
                SetStyle = new Request{
                UpdateParagraphStyle = new UpdateParagraphStyleRequest
                {
                    ParagraphStyle = new ParagraphStyle {NamedStyleType = $"{GetParaStyleName(style)}"},
                    Range = new Google.Apis.Docs.v1.Data.Range {StartIndex = 1, EndIndex = 1},
                    Fields = "namedStyleType"
                }
                }
            };
        }

        public static UpdateTextStyleRequest MakeTextStyle(NamedTextStyles styleType, int start, int end,
            string url = "")
        {
            var textStyleReq = new UpdateTextStyleRequest();
           return UpdateTextStyle(textStyleReq, styleType, start, end, url);
        }

        private static string CombineFields(string a, string b)
        {
            if (a.Contains('*')) return b;
            if (b.Contains('*')) return a;
            return a + "," + b;
        }
        public static UpdateTextStyleRequest UpdateTextStyle(UpdateTextStyleRequest updateStyleReq, NamedTextStyles styleType, int start, int end, string url = "")
        {
            updateStyleReq.Range = new Google.Apis.Docs.v1.Data.Range {StartIndex = start, EndIndex = end};

            if (updateStyleReq.TextStyle == null)
            {
                updateStyleReq.TextStyle = new TextStyle();
            }
            switch (styleType)
            {
                    case NamedTextStyles.Bold:
                        updateStyleReq.TextStyle.Bold = true;
                        updateStyleReq.Fields = "bold";
                        break;
                    case NamedTextStyles.Code: // this is "extra" visual formatting for GDocs
                        updateStyleReq.TextStyle.WeightedFontFamily = new WeightedFontFamily
                            {
                                FontFamily = "Courier New"
                            };
                        updateStyleReq.Fields = "weightedFontFamily";
                        break;
                    case NamedTextStyles.Italic:
                        updateStyleReq.TextStyle.Italic = true;
                        updateStyleReq.Fields = "italic";
                        break;
                    case NamedTextStyles.Link: // GDoc implements links using a text style that contains the url
                        updateStyleReq.TextStyle.Link = new Link{Url = url};
                        updateStyleReq.Fields = "link";
                        break;
                    case NamedTextStyles.Inserted: // mark extended markdown styles with underline
                    case NamedTextStyles.Marked: 
                    case NamedTextStyles.Subscript:
                    case NamedTextStyles.Superscript:
                    case NamedTextStyles.StrikeThrough:
                        updateStyleReq.TextStyle.Underline = true;
                        updateStyleReq.Fields = "underline";
                        break;
                    case NamedTextStyles.Normal: // remove formatting
                        updateStyleReq.Fields = "*";
                        break;
                    default:
                        Console.WriteLine("Encountered unknown text style");
                        break;
            }
            
            return updateStyleReq;
        }
 
        public static GDocRequest ProcessAutolinkInline(AutolinkInline inline)
        {
            //leaf
            return new GDocRequest();
        }
        public static (int requestCount, int length)  ProcessCodeInline(CodeInline inline, ref List<GDocRequest> reqChain)
        {
            //leaf
            string delim = new string(inline.Delimiter, inline.DelimiterCount);
            string content = $"{delim}{inline.Content}{delim}";
            var code = InsertTextRun(content,NamedTextStyles.Code, 1, 1 + content.Length);
            reqChain.Add(code);
            return (1, content.Length);
        }
        public static GDocRequest ProcessHtmlEntityInline(HtmlEntityInline inline)
        {
            //leaf
            return new GDocRequest();
        }
        public static GDocRequest ProcessHtmlInline(HtmlInline inline)
        {
            //leaf
            return new GDocRequest();
        }
        public static (int requestCount, int length) ProcessLinebreakInline(LineBreakInline inline, ref List<GDocRequest> reqChain)
        {
            reqChain.Add(InsertTextRun(" ",NamedTextStyles.Normal, 1, 2));
            return (1, 1);
        }
        public static (int requestCount, int length) ProcessEmphasisInline(EmphasisInline inline, ref List<GDocRequest> reqChain)
        {
            //Console.WriteLine("EMPHASIS: " + inline.DelimiterChar + ", " + inline.DelimiterCount);
            int requestCount = 0;
            int length = 0;
            foreach (var child in inline)
            {
                var counts = ProcessInlineObject(child, ref reqChain);
                requestCount += counts.requestCount;
                length += counts.length;
            }
            string delim = new string(inline.DelimiterChar, inline.DelimiterCount);
            for (int i = 0; i < requestCount; i++)
            {
                var childReq = reqChain[reqChain.Count - requestCount];
                if (childReq.InsertContent.InsertText != null)
                {
                    if (childReq.SetStyle.UpdateTextStyle != null)
                    {
                        childReq.SetStyle.UpdateTextStyle = UpdateTextStyle(childReq.SetStyle.UpdateTextStyle,GetStyleFromDelimiter(delim), 1, 2, "");
                        childReq.SetStyle.UpdateTextStyle.Range.EndIndex =
                            childReq.InsertContent.InsertText.Text.Length + 1;
                    }
                    else
                    {
                        childReq.SetStyle.UpdateTextStyle = MakeTextStyle(GetStyleFromDelimiter(delim), 1, 2);
                        childReq.SetStyle.UpdateTextStyle.Range.EndIndex =
                            childReq.InsertContent.InsertText.Text.Length + 1;
                    }
                }
            }

            //Console.WriteLine("EMPHASIS CLOSE " + length);
            return (requestCount, length);
        }
        public static (int requestCount, int length) ProcessLinkInline(LinkInline inline, ref List<GDocRequest> reqChain)
        {
            //container
            Console.WriteLine("LINK: [" + ((LinkInline)inline).FirstChild + "](" + ((LinkInline)inline).Url + ")");
            int requestCount = 0;
            int length = 0;
            foreach (var child in inline)
            {
                var counts = ProcessInlineObject(child, ref reqChain);
                requestCount += counts.requestCount;
                length += counts.length;
            }

            for (int i = 0; i < requestCount; i++)
            {
                var childReq = reqChain[reqChain.Count - requestCount];
                if (childReq.InsertContent.InsertText != null)
                {
                    if (childReq.SetStyle.UpdateTextStyle != null)
                    {
                        childReq.SetStyle.UpdateTextStyle = UpdateTextStyle(childReq.SetStyle.UpdateTextStyle, NamedTextStyles.Link, 1, 2, inline.Url);
                        childReq.SetStyle.UpdateTextStyle.Range.EndIndex =
                            childReq.InsertContent.InsertText.Text.Length + 1;
                    }
                    else
                    {
                        childReq.SetStyle.UpdateTextStyle = MakeTextStyle(NamedTextStyles.Link, 1, 2, inline.Url);
                        childReq.SetStyle.UpdateTextStyle.Range.EndIndex =
                            childReq.InsertContent.InsertText.Text.Length + 1;
                    }
                }
            }

            //Add Title text and alt text
            Console.WriteLine("LINK CLOSE " + length);

            return (requestCount, length);
        }
        public static (int requestCount, int length) ProcessLiteralInline(LiteralInline inline, ref List<GDocRequest> reqChain)
        {
            Console.WriteLine("LITERAL: " + inline.Content.Length + ": " + inline.Content.ToString());
            if (string.IsNullOrEmpty(inline.Content.ToString())) //Possible problem here -- why is literal empty? seems related to tables, which aren't handled yet
            {
                Console.WriteLine("Empty literal");
                return (0, 0);
            }
            else
            {
                reqChain.Add(InsertTextRun(inline.Content.ToString(), NamedTextStyles.Normal, 1, inline.Content.Length + 1));
                return (1, inline.Content.Length);
            }
        }
        
        
        public static (int requestCount, int length) ProcessInlineObject(Inline inline, ref List<GDocRequest> reqChain)
        {
                //Console.WriteLine(inline.GetType() + ", " + inline);
                (int requestCount, int length) = (0, 0);
                if (inline.GetType() == typeof(LiteralInline))
                {
                    (requestCount, length) = ProcessLiteralInline((LiteralInline) inline, ref reqChain);
                } 
                else if (inline.GetType() == typeof(LinkInline))
                {
                    (requestCount, length) = ProcessLinkInline((LinkInline) inline, ref reqChain);
                } 
                else if (inline.GetType() == typeof(EmphasisInline))
                {
                    (requestCount, length) = ProcessEmphasisInline((EmphasisInline)inline, ref reqChain);
                } 
                else if (inline.GetType() == typeof(LineBreakInline))
                {
                    (requestCount, length) = ProcessLinebreakInline((LineBreakInline) inline, ref reqChain);
                } 
                else if (inline.GetType() == typeof(CodeInline))
                {
                    (requestCount, length) = ProcessCodeInline((CodeInline) inline, ref reqChain);
                }
                else
                {
                    Console.WriteLine(inline.GetType() + ", " + inline);
                }

                return (requestCount, length);
        }

        public static (int requestCount, int length) ProcessInlineContainer(ContainerInline inlineContainer, ref List<GDocRequest> reqChain)
        {
            int requests = 0;
            int contentLength = 0;
            foreach (var inline in inlineContainer)
            {
               var counts = ProcessInlineObject(inline, ref reqChain);
               requests += counts.requestCount;
               contentLength += counts.length;
            }
            
            return (requests, contentLength);
        }
        
        public static List<GDocRequest> DoHeadingBlock(this HeadingBlock head)
        {
            var reqChain = new List<GDocRequest>();

            var paracounts = ProcessInlineContainer(head.Inline, ref reqChain);
            reqChain.Add(UpdateParagraphStyle((GDocNamedStyles)head.Level));
            
            int trailingBreaks = 1;
            if (head.LinesAfter != null)
            {
                trailingBreaks += head.LinesAfter.Count;
            }
            string after = new string('\n', trailingBreaks);
            reqChain.Add(InsertTextRun(after, NamedTextStyles.Normal, 1,2));
            return reqChain;
        }

        public static List<GDocRequest> DoParagraphBlock(this ParagraphBlock para)
        {
            var reqChain = new List<GDocRequest>();
            reqChain.Add(UpdateParagraphStyle(GDocNamedStyles.NORMAL_TEXT));
            var paracounts = ProcessInlineContainer(para.Inline, ref reqChain);
            int trailingBreaks = 1;
            if (para.LinesAfter != null)
            {
                trailingBreaks += para.LinesAfter.Count;
            }
            string after = new string('\n', trailingBreaks);
            reqChain.Add(InsertTextRun(after, NamedTextStyles.Normal, 1,2));
            return reqChain;
        }
    }
}