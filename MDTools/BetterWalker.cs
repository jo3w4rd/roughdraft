using System;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using MDTools.RequestBuilder;

namespace MDTools
{
    public class BetterWalker
    {
        public void WalkAST(MarkdownDocument document)
        {
            //Blocks
            foreach (var node in document/*.Descendants<Block>()*/)
            {
                //Console.WriteLine($"{node.GetType()} {node.Span}");
                if (node.GetType() == typeof(HeadingBlock))
                {
                    var block = (HeadingBlock) node;
                    string heading =$"H{block.Level}. ";
                    WalkInlines(block.Inline, ref heading);
                    Console.WriteLine(heading);
                }
                else if (node.GetType() == typeof(ParagraphBlock))
                {
                    var block = (ParagraphBlock) node;
                    //var para = builder.AddParagraph(MDTools.RequestBuilder.ParagraphStyle.NORMAL_TEXT);
                    string para = "P. ";
                    WalkInlines(block.Inline, ref para);
                    Console.WriteLine(para);
                }
                else if (node.GetType() == typeof(QuoteBlock))
                {
                    var block = (QuoteBlock) node;
                    //var para = builder.AddParagraph(MDTools.RequestBuilder.ParagraphStyle.NORMAL_TEXT);
                    string para = "Q.";
                    foreach (var quoteLine in block.QuoteLines)
                    {
                        //para.AppendText($"{quoteLine.QuoteChar} {quoteLine}");
                        //Console.WriteLine($"{quoteLine.QuoteChar} {quoteLine}");
                        para += $"{quoteLine.QuoteChar} {quoteLine}";
                    }
                    Console.WriteLine(para);

                }
                else if (node.GetType() == typeof(CodeBlock))
                {
                    var block = (CodeBlock) node;
                    //var para = builder.AddParagraph(MDTools.RequestBuilder.ParagraphStyle.NORMAL_TEXT);
                    string para = "";
                    foreach (var line in block.Lines)
                    {
                        //para.AppendText($"    {line.ToString()}\n");
                        para += $"    {line.ToString()}\n";
                    }
                    //para.SetTextStyle(TextStyles.Code, 1, para.Length);
                    Console.WriteLine(para);
                }
                else if (node.GetType() == typeof(FencedCodeBlock))
                {
                    var block = (FencedCodeBlock) node;
                    //var para = builder.AddParagraph(MDTools.RequestBuilder.ParagraphStyle.NORMAL_TEXT);
                    string para = "";
                    string openfence = new string(block.FencedChar, block.OpeningFencedCharCount);
                    if (block.Info != null)
                        openfence += " " + block.Info;
                    //para.AppendText(openfence + "\n");
                    para += openfence + "\n";
                    foreach (var line in block.Lines)
                    {
                        //para.AppendText(line.ToString() + "\n");
                        para += line.ToString() + "\n";
                    }
                    string closefence = new string(block.FencedChar, block.ClosingFencedCharCount);
                    //para.AppendText(closefence + "\n");
                    para += closefence + "\n";
                    //para.SetTextStyle(TextStyles.Code, 1, para.Length);
                    Console.WriteLine(para);
                }
                else if (node.GetType() == typeof(HtmlBlock))
                {
                    var block = (HtmlBlock) node;
                    //var para = builder.AddParagraph(MDTools.RequestBuilder.ParagraphStyle.NORMAL_TEXT);
                    
                    foreach (var line in block.Lines)
                    {
                       // para.AppendText($"    {line.ToString()}\n");
                    }
                    //para.SetTextStyle(TextStyles.Code, 1, para.Length);

                }
                else if (node.GetType() == typeof(ListBlock))
                {
                    var block = (ListBlock) node;
                    if (block.Parent != null && block.Parent.GetType() == typeof(ListItemBlock))
                        continue; // sublists show up in descendants, but we process them as children of list items

                    //var list = builder.AddList();
                    string list = "Start List\n";
                    foreach (var child in block.Descendants<ListItemBlock>())
                    {
                        int level = 0;
                        var parent = child.Parent;
                        while (parent != null && parent != block)
                        {
                            level++;
                            parent = parent.Parent;
                        }
                        //var listItem = list.AddItem(level / 2, ((ListBlock) child.Parent).IsOrdered);
                        list += new string(' ', 4 * level / 2);
                        list += ((ListBlock) child.Parent).IsOrdered ? "1." : "*";
                        foreach (var thing in child)
                        {
                            if (thing.GetType() == typeof(ParagraphBlock))
                            {
                                //WalkInlines(((ParagraphBlock)thing).Inline, listItem.P);
                                WalkInlines(((ParagraphBlock)thing).Inline, ref list);
                            }
                        }

                        list += "\n";

                    }
                    Console.WriteLine(list);

                }
                // else if (node.GetType() == typeof(ListItemBlock))
                // { // Handled by the ListBlock
                // }
                else if (node.GetType() == typeof(Markdig.Extensions.Tables.Table))
                {
                    var block = (Markdig.Extensions.Tables.Table) node;
                    //var para = builder.AddParagraph(MDTools.RequestBuilder.ParagraphStyle.NORMAL_TEXT);
                    Console.WriteLine($"{block}");

                }
                else
                {
                    //Console.WriteLine($"Didn't handle: {node.GetType()}");
                }
            }

            // foreach (var node in document.Descendants<Inline>())
            // {
            //     Console.WriteLine($"Inline types in doc: {node.GetType()}");
            // }
        }

        private void WalkSublist(ListItemBlock block, DocumentList list, int level)
        {
            
            //var item = list.AddItem(level, false);
            string item = "";
            foreach (var child in block.Descendants<Block>())
            {
                if (child.GetType() == typeof(ListItemBlock))
                {
                    var li = child as ListItemBlock;
                    WalkSublist(li, list, level + 1);
                }
                else if (child.GetType() == typeof(ParagraphBlock))
                {
                    var paraBlock = child as ParagraphBlock;
                    
                    //WalkInlines(paraBlock.Inline, item.P);
                    WalkInlines(paraBlock.Inline, ref item);
                    //Console.WriteLine($"{new string('-', level)}Item: {item.P.Text}");
                }
            }
        }
        private void WalkInlines(ContainerInline container, ref string para)
        {
            foreach (var inline in container)
            {
                ProcessInlineObject(inline, ref para);
            }
            
            //Console.WriteLine($">{para.Text}");
        }
        
        private void ProcessInlineObject(Inline inline, ref string para)
        {
            //Console.WriteLine(inline.GetType() + ", " + inline);
            if (inline.GetType() == typeof(LiteralInline))
            {
                ProcessLiteralInline((LiteralInline) inline,  ref para);
            } 
            else if (inline.GetType() == typeof(LinkInline))
            {
                ProcessLinkInline((LinkInline) inline,  ref para);
            } 
            else if (inline.GetType() == typeof(EmphasisInline))
            {
                ProcessEmphasisInline((EmphasisInline)inline,  ref para);
            } 
            else if (inline.GetType() == typeof(LineBreakInline))
            {
                ProcessLinebreakInline((LineBreakInline) inline,  ref para);
            } 
            else if (inline.GetType() == typeof(CodeInline))
            {
                 ProcessCodeInline((CodeInline) inline,  ref para);
            }
            else
            {
                Console.WriteLine("Unhandled inline: " + inline.GetType() + ", " + inline);
            }
        }
        
        public void ProcessAutolinkInline(AutolinkInline inline, ref string para)
        {
            //leaf
            //para.AppendLink(inline.Url, TextStyles.Link, inline.Url);
            para += $"[{inline.Url}]({inline.Url})";
        }
        public void ProcessCodeInline(CodeInline inline, ref string para)
        {
            //leaf
            string delim = new string(inline.Delimiter, inline.DelimiterCount);
            string content = $"{delim}{inline.Content}{delim}";
            //para.AppendText(content, TextStyles.Code);
            para += content;
        }
        public void ProcessHtmlEntityInline(HtmlEntityInline inline, ref string para)
        {
            //leaf
            //para.AppendText(inline.Transcoded.ToString());
            para += inline.Transcoded.ToString();
        }
        public void ProcessHtmlInline(HtmlInline inline)
        {
            //leaf
           Console.WriteLine($"Not impl. HTML inline {inline}");
        }
        public void ProcessLinebreakInline(LineBreakInline inline, ref string para)
        {
            //para.AppendText("\n");
            para += "\n";
        }
        public void ProcessEmphasisInline(EmphasisInline inline, ref string para)
        {
            string delimiter = new string(inline.DelimiterChar, inline.DelimiterCount);
            bool includeDelimiter = IncludeDelimiterCharacters(inline.DelimiterChar);
            para += delimiter;

            int start = para.Length;
            //if(includeDelimiter) para.AppendText(delimiter);
            foreach (var child in inline)
            {
                ProcessInlineObject(child, ref para);
            }
            //if(includeDelimiter) para.AppendText(delimiter);
            int end = para.Length;

            var styleType = GetStyleFromDelimiter(delimiter);
            //para.SetTextStyle(styleType, start, end);
            para += delimiter;
        }
        public void ProcessLinkInline(LinkInline inline, ref string para)
        {
            //container
            //para.AppendLink(inline.FirstChild.ToString(), TextStyles.Link, inline.Url);
            para += $"[";
            foreach (var child in inline)
            {
                ProcessInlineObject(child, ref para);
            }
            para += $"]({inline.Url})";
            //Add Title text and alt text
        }
        public void ProcessLiteralInline(LiteralInline inline, ref string para)
        {
           // Console.WriteLine("LITERAL: " + inline.Content.Length + ": " + inline.Content.ToString());
            if (string.IsNullOrEmpty(inline.Content.ToString())) //Possible problem here -- why is literal empty? seems related to tables, which aren't handled yet
            {
                Console.WriteLine("Empty literal");
            }
            else
            {
                //para.AppendText(inline.Content.ToString());
                para += inline.Content.ToString();
            }
        }
        

        public static TextStyles GetStyleFromDelimiter(string delimiter)
        {
            switch (delimiter)
            {
                case "*":
                case "_":
                    return TextStyles.Italic;
                case "**": 
                case "__":
                    return TextStyles.Bold;
                case "`":
                    return TextStyles.Code;
                case "~~":
                    return TextStyles.StrikeThrough;
                case "~":
                    return TextStyles.Subscript;
                case "^":
                    return TextStyles.Superscript;
                case "++":
                    return TextStyles.Inserted;
                case "==":
                    return TextStyles.Marked;
                default:
                    return TextStyles.Normal;
            }
        }

        // When GDoc doesn't have a named style, we still need the delimiters in the text
        // For bold and italic, this isn't necessary
        private static bool IncludeDelimiterCharacters(char delimiter)
        {
            return "`~^+=".Contains(delimiter);
        }

    }
}