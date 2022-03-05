
using System;
using System.Collections.Generic;
using Google.Apis.Docs.v1.Data;

namespace MDTools
{

    public class GDocToMarkdown
    {
        private static readonly string[] HeadingTexts =
        {
            "!!!", // Heading level 0 doesn't exist
            "#",
            "##",
            "###",
            "####",
            "#####",
            "######",
        };

        public static bool md_useReferenceStyleLinks = true;
        public static bool md_useStrikethrough = true;
        public static bool md_useUnderline = true;

        private static IDictionary<string, List> ListProps;
        private static Dictionary<string, InlineObjectProperties> imageObjects;
        private static Dictionary<string, string> headingLinks;
        private static Dictionary<string, string> referenceLinks;
        public static string ConvertDocumentToMarkdown(Document doc)
        {
            string markdownString = "";

            headingLinks = new Dictionary<string, string>(); // from headings Ids in documents
            referenceLinks = new Dictionary<string, string>(); // all collected links

            imageObjects = new Dictionary<string, InlineObjectProperties>(); // From top level of Document object
            if (doc.InlineObjects != null)
            {
                foreach (var inlineObject in doc.InlineObjects)
                {
                    imageObjects.Add(inlineObject.Key, inlineObject.Value.InlineObjectProperties);
                }
            }

            Dictionary<string, ListState> listStates = new Dictionary<string, ListState>(); // From top level of Document object
            if (doc.Lists != null)
            {
                foreach (KeyValuePair<string, List> listProp in doc.Lists)
                {
                    listStates.TryAdd(listProp.Key, new ListState(listProp.Value.ListProperties));
                }
            }

            for (int e = 0; e < doc.Body.Content.Count; e++)
            {
                var element = doc.Body.Content[e];

                if (element.Paragraph != null
                    && element.Paragraph.Bullet != null
                )
                {
                    listStates.TryGetValue(element.Paragraph.Bullet.ListId, out var listState);
                    int level = element.Paragraph.Bullet.NestingLevel ?? 0;
                    markdownString += ProcessListItem(element.Paragraph, level, listState.GetNextItem(level),
                        listState.isBullet(level));
                }
                else if (element.Paragraph != null)
                {
                    markdownString += ProcessParagraph(element.Paragraph);
                }
                else if (element.Table != null)
                {
                    markdownString += ProcessTable(element.Table);
                }

                bool nextItemNotInList = (e + 1 >= doc.Body.Content.Count) ||
                                         (doc.Body.Content[e + 1].Paragraph?.Bullet == null);
                bool thisItemNotInList = element.Paragraph?.Bullet == null;
                if (thisItemNotInList || nextItemNotInList)
                {
                    // make sure there is a single blank line, except after items inside a list
                    markdownString.TrimEnd('\n'); // Remove them all
                    while (!markdownString.EndsWith("\n\n")) // add two back
                        markdownString += "\n";
                }
            }

            if (md_useReferenceStyleLinks)
                markdownString += AddLinkReferences();

            markdownString = markdownString.Replace((char)0x0b, '\n');
            //Console.WriteLine(markdownString);
            
            // Test via Markdig HTML conversion
            //var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            //Console.WriteLine(Markdown.ToHtml(markdownString, pipeline));
            // Normalize
           // MarkdownDocument md = Markdown.Parse(markdownString, pipeline);
            
            //var stWriter = new StringWriter();
            //var renderer = new NormalizeRenderer(stWriter);
            //pipeline.Setup(renderer);
            //var testString = renderer.Render(md).ToString();
            
            //Console.WriteLine(testString);

            return markdownString;
        }

        public static string ProcessParagraph(Paragraph para)
        {
            string md = "";
            if (para.Bullet != null)
            {
            }
            else if (para.ParagraphStyle.NamedStyleType.StartsWith("HEADING_") ||
                     para.ParagraphStyle.NamedStyleType.StartsWith("TITLE"))
            {
                md += ProcessHeading(para) + "\n";
            }
            else if (para.Elements != null)
            {
                md += ProcessParagraphElements(para.Elements);
            }

            return md;
        }

        public static string ProcessHeading(Paragraph heading)
        {
            int level = 1; // TITLE gets level 1
            if (heading.ParagraphStyle.NamedStyleType.StartsWith("HEADING_"))
                level = int.Parse(heading.ParagraphStyle.NamedStyleType["HEADING_".Length].ToString());
            
            string md = ProcessParagraphElements(heading.Elements);
            headingLinks.TryAdd(heading.ParagraphStyle.HeadingId, HeadingToAnchor(md));
            return $"{HeadingTexts[level]} {md}";
        }

        private static string HeadingToAnchor(string heading)
        {
            string anchor = "#";
            heading = heading.Trim()
                .ToLowerInvariant();
            while (heading.Contains("  ")) // condense consecutive spaces
                heading = heading.Replace("  ", " ");
            
            foreach (var character in heading)
            {
                if (character == ' ') anchor += '-';
                else if ("abcdefghijklmnopqrstuvwxyz-".Contains(character)) anchor += character;
            }
            return anchor;
        }
        public static string ProcessParagraphElements(IList<ParagraphElement> elements)
        {
            string md = "";
            foreach (var element in elements)
            {
                if (element.TextRun != null)
                {
                    md += ProcessTextRun(element.TextRun);
                }
                else if (element.HorizontalRule != null)
                {
                    md += ProcessHorizontalRule(element.HorizontalRule);
                } 
                else if (element.InlineObjectElement != null)
                {
                    md += ProcessInlineObject(element.InlineObjectElement);
                }

                //FootnoteReference (maybe)
            }

            return md;
        }

        public static string ProcessInlineObject(InlineObjectElement inline)
        {
            string url = "";
            string alt = "";
            if (imageObjects.TryGetValue(inline.InlineObjectId, out var props))
            {
                if (props.EmbeddedObject.Title != null) url = props.EmbeddedObject.Title;
                else if (props.EmbeddedObject.ImageProperties != null && props.EmbeddedObject.ImageProperties.ContentUri != null)
                    url = props.EmbeddedObject.ImageProperties.ContentUri;
                
                if (props.EmbeddedObject.Description != null) alt = props.EmbeddedObject.Description;
                
                alt = alt.Replace("\n", " "); // might need more sanitization so people can't accidentally break links
            }
            else url = inline.InlineObjectId;
            
            return $"![{alt}]({url})";
        }
        
        public static string ProcessTextRun(TextRun text)
        {
            string md = text.Content;
            if (text.TextStyle.Link != null)
            {
                string url = GetURL(text.TextStyle.Link);
                if (md_useReferenceStyleLinks)
                {
                    referenceLinks.TryAdd(text.Content, url);
                    md = $"[{md}]";
                } 
                else 
                    md = $"[{md}]({url})";
            }
            string formatMarker = GetFormatMarkerString(text.TextStyle);
            if (string.IsNullOrEmpty(formatMarker))
                return md;
            else 
                return FormatTextRun(formatMarker, md);
        }

        public static string GetURL(Link link)
        {
            if (link.Url != null)
            {
                return link.Url;
            }

            if (link.HeadingId != null)
            {
                if (headingLinks.TryGetValue(link.HeadingId, out string anchorLink))
                    return anchorLink;
                else
                    return "#-no-link-to-heading"; //shouldn't happen
            }

            if (link.BookmarkId != null)
            {
                return "#" + link.BookmarkId + "-bookmarklinks-not-supported";
            }
            return "#no-link-url-available";
        }
        private static string FormatTextRun(string marker, string text)
        {
            //Google Docs likes to put the end format marker after spaces, but this isn't allowed in markdown
            // so this swaps the end marker in in front of ending spaces.
            string endLineBreak = "";
            if (text.Split("\n").Length > 1) endLineBreak = "\n"; // Need to put any format markers before any line breaks
            string leadingSpaces = "";
            if (text[0] == ' ') leadingSpaces = " ";
            text = text.TrimStart();
            string trailingSpaces = "";
            if (text.Length > 0 && text[^1] == ' ') trailingSpaces = " ";
            text = text.TrimEnd();
            return $"{leadingSpaces}{marker}{text}{marker}{trailingSpaces}{endLineBreak}";
        }
        public static string ProcessHorizontalRule(HorizontalRule rule)
        {
            return "---\n";
        }

        public static string ProcessListItem(Paragraph item, int level, string itemPrefix, bool isBullet)
        {
            string indent = "".PadRight((level + 1) * 3);
            string md = "".PadRight(level * 3);;
            if (isBullet) md += "* ";
            else md += itemPrefix + ". "; //TODO pass format with itemPrefix
            
            md += ProcessParagraphElements(item.Elements);
            
            // Handle soft breaks in a GDoc list
            char softbreak = Convert.ToChar(0x0b);
            string[] subparas = md.Split(softbreak);
            if (subparas.Length >= 2)
            {
                md = subparas[0] + "\n\n";
                for (int i = 1; i < subparas.Length -1 ; i++)
                {
                    if(!string.IsNullOrWhiteSpace(subparas[i]))
                        md += indent + subparas[i] + "\n\n";
                }
                if(!string.IsNullOrWhiteSpace(subparas[^1]))
                    md += indent + subparas[^1] + "\n";
            }
            
            return md;
        }

        public static string ProcessTable(Table tbl)
        {
            string md = "";
            if (tbl.TableRows != null)
            {
                for(int r = 0; r < tbl.TableRows.Count; r++)
                {
                    TableRow row = tbl.TableRows[r];
                    if (r == 1) // draw pipe table heading separator
                    {
                        md += "|";
                        for (int c = 0; c < tbl.Columns; c++)
                        {
                            md += " --- |";
                        }

                        md += "\n";
                    }
                    md += "|"; // Open row
                    foreach (var cell in row.TableCells)
                    {
                        foreach (var thing in cell.Content)
                        {
                            if (thing.Paragraph != null)
                            {
                                md += $" { ProcessParagraph(thing.Paragraph).TrimEnd( '\r', '\n' )} |";
                            }
                        }
                    }

                    md += "\n"; // end of row
                }
            }

            return md;
        }

        public static string GetFormatMarkerString(TextStyle style)
        {
            if (style == null) return "";

            string marker = "";
            if (style.Bold == true) marker += "**";
            if (style.Italic == true) marker += "*";
            if (style.Strikethrough == true && md_useStrikethrough) marker += "~~";
            //if (style.Underline == true && md_useUnderline) name += "==";

            return marker;
        }

        public static string AddLinkReferences()
        {
            string md = "\n";
            foreach (var link in referenceLinks)
            {
                md += $"[{link.Key}]: {link.Value}\n";
            }

            return md;
        }
        
        public static string IndentName(Dimension indent)
        {
            if (indent == null) return "";
            return indent.Magnitude + " " + indent.Unit;
        }

 
    }
}