using System;
using System.Collections.Generic;
using Google.Apis.Docs.v1.Data;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using MDTools.RequestBuilder;
using Paragraph = MDTools.RequestBuilder.Paragraph;
using Table = MDTools.RequestBuilder.Table;
using TableCell = Markdig.Extensions.Tables.TableCell;
using TableRow = Markdig.Extensions.Tables.TableRow;

namespace MDTools
{
    public class MarkdownConvertor
    {
        private DocumentBuilder builder;

        public MarkdownConvertor()
        {
            builder = new DocumentBuilder();
        }

        public (List<Request> requests, int insertLength) Convert(MarkdownDocument document)
        {
            WalkAST(document, builder);
            
            return builder.RequestList(1);
        }
        
        private void WalkAST(ContainerBlock documentFragment, DocumentBuilder builder)
        {
            //Blocks
            foreach (var node in documentFragment/*.Descendants<Block>()*/)
            {
                Console.WriteLine($"{node.GetType()} {node.Span}");
                if (node.GetType() == typeof(HeadingBlock))
                {
                    var block = (HeadingBlock) node;
                    var headingPara = builder.AddParagraph((MDTools.RequestBuilder.ParagraphStyle) block.Level);
                    WalkInlines(block.Inline, headingPara);
                    //Console.WriteLine($"{block.Level} {block.HeaderChar} {block.Inline.FirstChild}");
                }
                else if (node.GetType() == typeof(ParagraphBlock))
                {
                    var block = (ParagraphBlock) node;
                    var para = builder.AddParagraph(MDTools.RequestBuilder.ParagraphStyle.NORMAL_TEXT);
                    WalkInlines(block.Inline, para);
                }
                else if (node.GetType() == typeof(QuoteBlock))
                {
                    var block = (QuoteBlock) node;
                    var blockQuote = builder.AddBlockQuote();
                    
                    // This "works", but unquotes the block need a non-hacky way to prefix all lines with the quote character...
                    // Solution for now is to delimit the block quote with ">>>" like a code fence 
                    WalkAST(block, blockQuote);
                        
                }
                else if (node.GetType() == typeof(CodeBlock))
                {
                    var block = (CodeBlock) node;
                    var para = builder.AddParagraph(MDTools.RequestBuilder.ParagraphStyle.NORMAL_TEXT);
                    
                    for(int i = 0; i < block.Lines.Count; i++)
                    {
                        var line = block.Lines.Lines[i];
                        para.AppendText($"    {line.ToString()}\n");
                    }
                    para.SetTextStyle(TextStyles.Code, 1, para.Length);
                }
                else if (node.GetType() == typeof(FencedCodeBlock))
                {
                    var block = (FencedCodeBlock) node;
                    var para = builder.AddParagraph(MDTools.RequestBuilder.ParagraphStyle.NORMAL_TEXT);

                    string openfence = new string(block.FencedChar, block.OpeningFencedCharCount);
                    if (block.Info != null)
                        openfence += " " + block.Info;
                    para.AppendText(openfence + "\n");
                    foreach (var line in block.Lines)
                    {
                        para.AppendText(line.ToString());
                        
                    }
                    string closefence = new string(block.FencedChar, block.ClosingFencedCharCount);
                    para.AppendText(closefence); // End of paragraph gets no new line.
                    para.SetTextStyle(TextStyles.Code, 1, para.Length);
                }
                else if (node.GetType() == typeof(HtmlBlock))
                {
                    var block = (HtmlBlock) node;
                    var para = builder.AddParagraph(MDTools.RequestBuilder.ParagraphStyle.NORMAL_TEXT);
                    
                    foreach (var line in block.Lines)
                    {
                        para.AppendText($"    {line.ToString()}\n");
                    }
                    para.SetTextStyle(TextStyles.Code, 1, para.Length);

                }
                else if (node.GetType() == typeof(ListBlock))
                {
                    var block = (ListBlock) node;
                    if (block.Parent != null && block.Parent.GetType() == typeof(ListItemBlock))
                        continue; // sublists show up in descendants, but we process them as children of list items

                    var list = builder.AddList();
                    foreach (var child in block.Descendants<ListItemBlock>())
                    {
                        int level = 0;
                        var parent = child.Parent;
                        while (parent != null && parent != block)
                        {
                            level++;
                            parent = parent.Parent;
                        }
                        var listItem = list.AddItem(level / 2, ((ListBlock) child.Parent).IsOrdered);
                        foreach (var thing in child)
                        {
                            if (thing.GetType() == typeof(ParagraphBlock))
                            {
                                WalkInlines(((ParagraphBlock)thing).Inline, listItem.P);
                            }
                        }
                        
                    }

                }
                // else if (node.GetType() == typeof(ListItemBlock))
                // { // Handled by the ListBlock
                // }
                else if (node.GetType() == typeof(Markdig.Extensions.Tables.Table))
                {
                    var mdTable = (Markdig.Extensions.Tables.Table) node;
                    var table = builder.AddTable(mdTable.Count, mdTable.ColumnDefinitions.Count - 1);
                    
                    for (int row = 0; row < mdTable.Count; row++)
                    {
                        var mdRow = (TableRow)mdTable[row];

                        Console.WriteLine($"Is the header: {mdRow.IsHeader} with {mdRow.Count} cells");
                        for (int col = 0; col < mdRow.Count; col++)
                        {
                            var mdCell = (TableCell) mdRow[col];
                            var cell = table.GetCell(row, col);
                            cell.AddNewLinesToParagraphs = false;
                            WalkAST(mdCell, cell);
                        }
                    }

                }
                else
                {
                    Console.WriteLine($"Didn't handle: {node.GetType()}");
                }
            }

            // foreach (var node in document.Descendants<Inline>())
            // {
            //     Console.WriteLine($"Inline types in doc: {node.GetType()}");
            // }
        }

        private void WalkInlines(ContainerInline container, Paragraph para)
        {
            foreach (var inline in container)
            {
                ProcessInlineObject(inline, para);
            }
            
            Console.WriteLine($">{para.Text}");
        }
        
        private void ProcessInlineObject(Inline inline, Paragraph para)
        {
            Console.WriteLine(inline.GetType() + ", " + inline);
            if (inline.GetType() == typeof(LiteralInline))
            {
                ProcessLiteralInline((LiteralInline) inline,  para);
            } 
            else if (inline.GetType() == typeof(LinkInline))
            {
                ProcessLinkInline((LinkInline) inline,  para);
            } 
            else if (inline.GetType() == typeof(EmphasisInline))
            {
                ProcessEmphasisInline((EmphasisInline)inline,  para);
            } 
            else if (inline.GetType() == typeof(LineBreakInline))
            {
                ProcessLinebreakInline((LineBreakInline) inline,  para);
            } 
            else if (inline.GetType() == typeof(CodeInline))
            {
                 ProcessCodeInline((CodeInline) inline,  para);
            }
            else
            {
                Console.WriteLine("Unhandled inline: " + inline.GetType() + ", " + inline);
            }
        }
        
        public void ProcessAutolinkInline(AutolinkInline inline, Paragraph para)
        {
            //leaf
            para.AppendLink(inline.Url, TextStyles.Link, inline.Url);
        }
        public void ProcessCodeInline(CodeInline inline, Paragraph para)
        {
            //leaf
            string delim = new string(inline.Delimiter, inline.DelimiterCount);
            string content = $"{delim}{inline.Content}{delim}";
            para.AppendText(content, TextStyles.Code);
        }
        public void ProcessHtmlEntityInline(HtmlEntityInline inline, Paragraph para)
        {
            //leaf
            para.AppendText(inline.Transcoded.ToString());
        }
        public void ProcessHtmlInline(HtmlInline inline)
        {
            //leaf
           Console.WriteLine($"Not impl. HTML inline {inline}");
        }
        public void ProcessLinebreakInline(LineBreakInline inline, Paragraph para)
        {
            para.AppendText("\n");
        }
        public void ProcessEmphasisInline(EmphasisInline inline, Paragraph para)
        {
            string delimiter = new string(inline.DelimiterChar, inline.DelimiterCount);
            bool includeDelimiter = IncludeDelimiterCharacters(inline.DelimiterChar);
            
            int start = para.Length;
            if(includeDelimiter) para.AppendText(delimiter);
            foreach (var child in inline)
            {
                ProcessInlineObject(child, para);
            }
            if(includeDelimiter) para.AppendText(delimiter);
            int end = para.Length;

            var styleType = GetStyleFromDelimiter(delimiter);
            para.SetTextStyle(styleType, start, end);
        }
        public void ProcessLinkInline(LinkInline inline, Paragraph para)
        {
            int start = para.Length;
            //container
            foreach (var child in inline)
            {
                ProcessInlineObject(child, para);
            }

            int end = para.Length;
            //para.AppendLink(inline.FirstChild.ToString(), TextStyles.Link, inline.Url);
            if (inline.IsImage)
            {
                
            }
            else
            {
                para.MakeLink(start, end, inline.Url);
            }

            //Add Title text and alt text
        }
        public void ProcessLiteralInline(LiteralInline inline, Paragraph para)
        {
           Console.WriteLine("LITERAL: " + inline.Content.Length + ": " + inline.Content.ToString() + " of type " + inline.GetType());
            if (string.IsNullOrEmpty(inline.Content.ToString())) //Possible problem if we get here -- something isn't implemented
            {
                Console.WriteLine("Empty literal");
            }
            else
            {
                para.AppendText(inline.Content.ToString());
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