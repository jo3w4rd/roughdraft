using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Requests;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.GDoc;
using Markdig.Syntax;
using MDTools.RequestBuilder;
using ParagraphStyle = MDTools.RequestBuilder.ParagraphStyle;
using Range = Google.Apis.Docs.v1.Data.Range;

namespace MDTools
{
    class Program
    {

        static void Main(string[] args) {
            //GDocController.TestFolderList();
            //GDocController.TestFetchDoc();
            //GDocController.TestBoth();
            //GDocController.TestConversionToMarkdown();
            //ConvertFolder();

            //var mdString = GDocToMarkdown.ConvertDocumentToMarkdown(doc); // where doc is a Google Document object

            //MDToMarkdown();
            //Test();
            //ManualRequests.StyledTextBatchRequest();
            //DocBuilder();
            Convertor();
            //ManualRequests.ListBatchRequest();
            //ManualRequests.ListBatchRequest2();
            //ManualRequests.DocumentListRequest();
            //BetterWalker();
            //ManualRequests.TableRequest();
            //MDtoHTML();
            //GDocController.TestUploadImage();
            //ManualRequests.ImageRequest();
        }

        public static void MDtoHTML()
        {
            string markdown = File.ReadAllText("d:\\Repos\\MDTools\\Binding.md");
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            MarkdownDocument md = Markdown.Parse(markdown, pipeline);
            var html = Markdown.ToHtml(markdown, pipeline);
            File.WriteAllText( "d:\\Repos\\MDTools\\out.html",html);
        }
        
        public static void MDToMarkdown()
        {
            //string markdown = File.ReadAllText("d:\\Repos\\Markdown.md");
            string markdown = File.ReadAllText("d:\\test.md");
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            MarkdownDocument md = Markdown.Parse(markdown, pipeline);
            //var renderer = new GDocRender();
            //pipeline.Setup(renderer);
            //renderer.Render(md);
            
            //Walk the AST
            var requestList = new List<GDocRequest>(md.Descendants().Count());
            foreach (var child in md.Descendants<Block>())
            {
                //Console.WriteLine(child.GetType());
                var childType = child.GetType();
                if(childType == typeof(HeadingBlock))
                    requestList.AddRange(((HeadingBlock)child).DoHeadingBlock());
                else if(childType == typeof(ParagraphBlock))
                    requestList.AddRange(((ParagraphBlock)child).DoParagraphBlock());
            }

            BatchUpdateDocumentRequest body 
                = new BatchUpdateDocumentRequest {Requests = new List<Request>()};
            for (int i = requestList.Count-1; i >= 0; i--)
            //for (int i = 0; i < requestList.Count; i++)
            {
                if(requestList[i].InsertContent != null)
                    body.Requests.Add(requestList[i].InsertContent);
                if(requestList[i].SetStyle != null)
                    body.Requests.Add(requestList[i].SetStyle);
            }

            //AddLocations(body);
            PrintBatchRequest(body);
            //var response = GDocController.DocumentRequest(body);
            //Console.WriteLine(reply.ETag);
            
        }
        static void AddLocations(BatchUpdateDocumentRequest batch)
        {
            var charCount = 0;
            var lengthOfLastInsert = 0;
            for (int i = 0; i < batch.Requests.Count; i++)
            {
                var req = batch.Requests[i];
                if (req.InsertText != null)
                {
                    charCount += lengthOfLastInsert;
                    var insertText = req.InsertText;
                    //insertText.Location.Index = charCount + 1;
                    req.InsertText.EndOfSegmentLocation = new EndOfSegmentLocation();
                    lengthOfLastInsert = insertText.Text.Length;
                }

                if (req.InsertTable != null)
                {
                    
                }

                if (req.InsertInlineImage != null)
                {
                    
                }

                if (req.UpdateParagraphStyle != null)
                {
                    req.UpdateParagraphStyle.Range.StartIndex += 0;
                    req.UpdateParagraphStyle.Range.EndIndex += lengthOfLastInsert-1;
                }

                if (req.UpdateTextStyle != null)
                {
                    req.UpdateTextStyle.Range.StartIndex += 0;
                    req.UpdateTextStyle.Range.EndIndex += lengthOfLastInsert-1;
                }
            }
        }

        static void PrintBatchRequest(BatchUpdateDocumentRequest batch)
        {
            for (int i = 0; i < batch.Requests.Count; i++)
            {
               //Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Start Request Info");
                var req = batch.Requests[i];
                var info = ReqInfo(req);
                Console.WriteLine($"{info.name}: loc {info.location}, len/start {info.length}, end {info.spanLength} == {info.info}");
                //Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< End Request Info");
            }
        }

        static (string name, string info, int location, int length, int spanLength) ReqInfo(Request req)
        {
            string name = "";
            string info = "";
            int length = 0;
            int location = 0;
            int spanLength = 0;
            if (req.InsertTable != null)
            {
                
            }  
            if (req.InsertText != null)
            {
                name = "InsertText";
                info = req.InsertText.Text;
                length = req.InsertText.Text.Length;
                spanLength = -1;
                location = req.InsertText.Location.Index ?? 0;
            }  
            if (req.CreateParagraphBullets != null)
            {
                
            }  
            if (req.InsertInlineImage != null)
            {
                
            }  
            if (req.UpdateParagraphStyle != null)
            {
                name = "UpdateParagraphStyle";
                info = req.UpdateParagraphStyle.ParagraphStyle.NamedStyleType;
                length = req.UpdateParagraphStyle.Range.StartIndex ?? 0;
                spanLength = req.UpdateParagraphStyle.Range.EndIndex ?? 0;
                location = -1;
            }  
            if (req.UpdateTextStyle != null)
            {
                name = "UpdateTextStyle";
                info = req.UpdateTextStyle.Fields.ToString();
                length = req.UpdateTextStyle.Range.StartIndex ?? 0;
                spanLength = req.UpdateTextStyle.Range.EndIndex ?? 0;
                location = -1;

            }  
            if (req.InsertTable != null)
            {
                
            }

            return (name, info, location, length, spanLength);
        }
        static void ConvertFolder()
        {
            var sourceFolderURL = "https://drive.google.com/drive/folders/1Cl7Sq_scOgXff55_HBxL5_zf6KcB5uxn";
            var mdDestinationFolder = "D:/UnityRepos/Addressables/AddressablesDemo/Packages/com.unity.addressables/Documentation~";

            var filesToFetch = GDocController.GetGDocListFromFolder(sourceFolderURL).Files;
            foreach (var fileInfo in filesToFetch)
            {
                Document docToConvert = GDocController.FetchGDoc(fileInfo.Id);
                string md = GDocToMarkdown.ConvertDocumentToMarkdown(docToConvert);
                var mdSavePath = Path.Combine(mdDestinationFolder, $"{fileInfo.Name}.md");
                File.WriteAllText(mdSavePath, md);
            }
        }

        public static void Test()
        {
            string markdown = File.ReadAllText("d:\\Repos\\Markdown.md");
            //string markdown = File.ReadAllText("d:\\test.md");
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            MarkdownDocument md = Markdown.Parse(markdown, pipeline);
            Console.WriteLine(md.ToHtml(pipeline));

            //var renderer = new GDocRender();
            //pipeline.Setup(renderer);
            //renderer.Render(md);
            
            //Walk the AST
            
            foreach (var child in md.Descendants<Block>())
            {
                
                var childType = child.GetType();
                int length = child.Span.Length;
                int start = child.Span.Start;
                int end = child.Span.End;
                string pt = markdown.Substring(start, (length > 10 ? 10 : length)).Replace("\n"," ");
                
                Console.WriteLine($"{childType}: ({start},{end}) = {length}: {pt}");
                if (childType.IsAssignableTo(typeof(ContainerBlock)))
                {
                    Walk(child as ContainerBlock);
                }

                if (childType == typeof(HeadingBlock))
                {
                    //var block = (HeadingBlock)child;
                    //Walk(block);
                }
                else if (childType == typeof(ParagraphBlock))
                {
                    
                }
            }
            
        }

        public static void Walk(ContainerBlock mdObj)
        {
            foreach (var child in mdObj)
            {
                var childType = child.GetType();
                int length = child.Span.Length;
                int start = child.Span.Start;
                int end = child.Span.End;
                
                Console.WriteLine($"    Child Obj: {childType}: ({start},{end}) = {length}");
            }
        }
        public static void zzzWalk(MarkdownObject mdObj)
        {
            foreach (var child in mdObj.Descendants())
            {

                var childType = child.GetType();
                int length = child.Span.Length;
                int start = child.Span.Start;
                int end = child.Span.End;
                
                Console.WriteLine($"    Child Obj: {childType}: ({start},{end}) = {length}");
                ChildWalk(child, "        ");
            }
        }
        public static void ChildWalk(MarkdownObject mdObj, string indent)
        {
            foreach (var child in mdObj.Descendants())
            {

                var childType = child.GetType();
                int length = child.Span.Length;
                int start = child.Span.Start;
                int end = child.Span.End;
                
                Console.WriteLine($"{indent}Child Obj: {childType}: ({start},{end}) = {length}");
                ChildWalk(child, indent + "    ");
            }
        }

        public static void ManualBatchRequest()
        {
            var requestList = new List<Request>();

            var textReq = new Request();
            textReq.InsertText = new InsertTextRequest();
            textReq.InsertText.Text = "Hardy boys went to town.";
            textReq.InsertText.Location = new Location();
            textReq.InsertText.Location.Index = 1;
            
            var style1 = new Request();
            style1.UpdateTextStyle = new UpdateTextStyleRequest();
            style1.UpdateTextStyle.TextStyle = new TextStyle();
            style1.UpdateTextStyle.TextStyle.Bold = true;
            style1.UpdateTextStyle.Fields = "bold";
            style1.UpdateTextStyle.Range = new Range();
            style1.UpdateTextStyle.Range.StartIndex = 3;
            style1.UpdateTextStyle.Range.EndIndex = 23;
            
            var style2 = new Request();
            style2.UpdateTextStyle = new UpdateTextStyleRequest();
            style2.UpdateTextStyle.TextStyle = new TextStyle();
            style2.UpdateTextStyle.TextStyle.Italic = true;
            style2.UpdateTextStyle.Fields = "italic";
            style2.UpdateTextStyle.Range = new Range();
            style2.UpdateTextStyle.Range.StartIndex = 7;
            style2.UpdateTextStyle.Range.EndIndex = 15;

            var style3 = new Request();
            style3.UpdateTextStyle = new UpdateTextStyleRequest();
            style3.UpdateTextStyle.TextStyle = new TextStyle();
            style3.UpdateTextStyle.TextStyle.Underline = true;
            style3.UpdateTextStyle.Fields = "underline";
            style3.UpdateTextStyle.Range = new Range();
            style3.UpdateTextStyle.Range.StartIndex = 12;
            style3.UpdateTextStyle.Range.EndIndex = 20;
            
            BatchUpdateDocumentRequest body 
                = new BatchUpdateDocumentRequest {Requests = new List<Request>()};
            body.Requests.Add(textReq);
            body.Requests.Add(style1);
            body.Requests.Add(style2);
            body.Requests.Add(style3);

            var response = GDocController.DocumentRequest(body, "Manual Batch Request");
        }
        
        public static void DocBuilder()
        {
            //string markdown = File.ReadAllText("d:\\Repos\\Markdown.md");
            string markdown = File.ReadAllText("d:\\test.md");
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            MarkdownDocument md = Markdown.Parse(markdown, pipeline);
            //var renderer = new GDocRender();
            //pipeline.Setup(renderer);
            //renderer.Render(md);

            var builder = new DocumentBuilder();
            //Walk the AST
            foreach (var child in md.Descendants<Block>())
            {
                //Console.WriteLine(child.GetType());
                var childType = child.GetType();
                if (childType == typeof(HeadingBlock))
                {
                    var heading = (HeadingBlock) child;
                    var headingType = (ParagraphStyle)heading.Level;
                    var parag = builder.AddParagraph(headingType, heading.Inline.FirstChild.ToString(), TextStyles.Normal);
                }
                else if (childType == typeof(ParagraphBlock))
                {
                    var paraMD = (ParagraphBlock) child;
                    var paraReq = builder.AddParagraph(ParagraphStyle.NORMAL_TEXT);
                    var text = paraMD;

                }
                //requestList.AddRange(((ParagraphBlock)child).DoParagraphBlock());
            }

            var requestList = builder.RequestList().requests;
            foreach (var request in requestList)
            {
                if (request.InsertText != null)
                {
                    if(request.InsertText.EndOfSegmentLocation != null)
                        Console.WriteLine($"EndOfSegmentLocation {request.InsertText.Text}");
                    else
                        Console.WriteLine($"Insert at {request.InsertText.Location.Index}: {request.InsertText.Text}");
                }

                if (request.UpdateTextStyle != null)
                {
                    Console.WriteLine($"TS: {request.UpdateTextStyle.Range.StartIndex}, {request.UpdateTextStyle.Range.EndIndex}: {request.UpdateTextStyle.Fields.ToString()}");
                }

                if (request.UpdateParagraphStyle != null)
                {
                    Console.WriteLine($"PS: {request.UpdateParagraphStyle.ParagraphStyle.NamedStyleType}");
                }
            }
            BatchUpdateDocumentRequest body 
                = new BatchUpdateDocumentRequest {Requests = requestList};
            

            var response = GDocController.DocumentRequest(body, "Manual Doc Builder Request");
            //Console.WriteLine(reply.ETag);
            
        }

        public static string RequestListToDebugString(List<Request> requests)
        {
            int IP = 1;
            string result = "";
            foreach (var se in requests)
            {
                if (se.CreateParagraphBullets != null)
                {
                    result +=
                        $"({se.CreateParagraphBullets.Range.StartIndex}-{se.CreateParagraphBullets.Range.EndIndex}) BulletStyle\n";
                }
                if (se.UpdateParagraphStyle != null)
                {
                    result += $"({se.UpdateParagraphStyle.Range.StartIndex}-{se.UpdateParagraphStyle.Range.EndIndex}) ParaStyle\n";
                }
                if (se.UpdateTextStyle != null)
                {
                    result += $"({se.UpdateTextStyle.Range.StartIndex}-{se.UpdateTextStyle.Range.EndIndex}) TextStyle\n";
                }

                if (se.InsertText != null)
                {
                    string text = se.InsertText.Text;
                    text = text.Replace(DocumentList.Tab, 't');
                    int tabblessLength = text.Length;
                    text = text.Replace(DocumentList.NewLine, 'n');
                    text = text.Replace(DocumentList.LineFeed, 'r');
                    result += $"({IP}-{IP+tabblessLength}) {text}\n";
                    IP += tabblessLength;
                }
            }
            return result;
        }
        public static void Convertor()
        {
            string filePath = "d:\\Repos\\MarkdownTest.md";
            string markdown = File.ReadAllText(filePath);
            //string markdown = File.ReadAllText("D:/Repos/markdig/src/Markdig.Tests/Specs/CommonMark.md");
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            MarkdownDocument md = Markdown.Parse(markdown, pipeline);

            var convertor = new MarkdownConvertor();
            var requestList = convertor.Convert(md);
            // BatchUpdateDocumentRequest body 
            //     = new BatchUpdateDocumentRequest {Requests = requestList.requests};
            // Console.WriteLine($"{RequestListToDebugString(requestList.requests)}");
            // var docID = GDocController.DocumentRequest(body, Path.GetFileName(filePath));
            // var theDoc = GDocController.FetchGDoc(docID);
            // Console.WriteLine(GDocController.DocumentToDebugString(theDoc));
            //             
            Console.WriteLine(md.ToHtml(pipeline));
        }

        public static void BetterWalker()
        {
            string markdown = File.ReadAllText("d:\\Repos\\MarkdownTest.md");
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            MarkdownDocument md = Markdown.Parse(markdown, pipeline);

            var convertor = new BetterWalker();
            convertor.WalkAST(md);

        }
    }
}