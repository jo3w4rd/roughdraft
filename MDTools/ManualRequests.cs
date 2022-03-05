using System;
using System.Collections.Generic;
using Google.Apis.Docs.v1.Data;
using MDTools;
using MDTools.RequestBuilder;
using Range = Google.Apis.Docs.v1.Data.Range;

namespace MDTools
{
    public class ManualRequests
    {
        public static void StyledTextBatchRequest()
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

            var response = GDocController.DocumentRequest(body, "Styled Text Batch Request");
        }

        public static void ListBatchRequest()
        {
            var requestList = new List<Request>();


            var style1 = new Request();
            style1.UpdateTextStyle = new UpdateTextStyleRequest();
            style1.UpdateTextStyle.TextStyle = new TextStyle();
            style1.UpdateTextStyle.TextStyle.Bold = true;
            style1.UpdateTextStyle.Fields = "bold";
            style1.UpdateTextStyle.Range = new Range();
            style1.UpdateTextStyle.Range.StartIndex = 3;
            style1.UpdateTextStyle.Range.EndIndex = 23;

            var textReq1 = RequestHelpers.MakeTextInsertRequest("1ardy boys went to town.", 1);
            var textReq2 =
                RequestHelpers.MakeTextInsertRequest(
                    "2ardy boys went to town." + (char) 0x0b + "IArdy boys went to town.", 1);
            var textReq3 = RequestHelpers.MakeTextInsertRequest("3ardy boys went to town.", 1);
            var textReqa = RequestHelpers.MakeTextInsertRequest("\tAardy boys went to town.", 1);
            var textReqb =
                RequestHelpers.MakeTextInsertRequest(
                    "\tBardy boys went to town." + (char) 0x0b + "IBrdy boys went to town.", 1);
            var textReqc = RequestHelpers.MakeTextInsertRequest("\tCardy boys went to town.", 1);
            var textReq4 =
                RequestHelpers.MakeTextInsertRequest(
                    "4ardy boys went to town." + (char) 0x0b + "ICrdy boys went to town.", 1);
            var textReq5 = RequestHelpers.MakeTextInsertRequest("5ardy boys went to town.", 1);
            var textReq6 = RequestHelpers.MakeTextInsertRequest("6ardy boys went to town.", 1);

            var bullet = MakeBulletRequest(1, 296, "NUMBERED_DECIMAL_ALPHA_ROMAN");
            var bullet1 = MakeBulletRequest(1, 74, "NUMBERED_DECIMAL_ALPHA_ROMAN");

            var bulleta = MakeBulletRequest(74, 174, "BULLET_DISC_CIRCLE_SQUARE");
            var bullet4 = MakeBulletRequest(174, 296, "NUMBERED_DECIMAL_ALPHA_ROMAN");


            var reqList = new List<Request>();
            reqList.Add(textReq6); //Put text requests and text style, even non-bullet para styles in reverse order
            reqList.Add(textReq5);
            reqList.Add(textReq4);
            reqList.Add(textReqc);
            reqList.Add(textReqb);
            reqList.Add(textReqa);
            reqList.Add(textReq3);
            reqList.Add(textReq2);
            reqList.Add(textReq1);
            reqList.Add(style1);
            reqList.Add(bullet); // Put full list create bullets request next, covering full list
            reqList.Add(bullet1); // Not required for first sequence, but doesn't break things
            reqList.Add(bulleta); // Put sub list create bullet requests after full list bullet request
            reqList.Add(
                bullet4); // Do need one for each list-level sequence, even if it *should* be the same as the full list and hence already set

            // Use tab \t to indent the list level
            // Use (char)0x0b to enter a soft line break
            // Can only set bulleted vs ordered with presets

            BatchUpdateDocumentRequest body
                = new BatchUpdateDocumentRequest {Requests = reqList};
            var response = GDocController.DocumentRequest(body, "List Batch Request");
        }

        private static void ToEnd(Request textReq)
        {
            textReq.InsertText.Location = null;
            textReq.InsertText.EndOfSegmentLocation = new EndOfSegmentLocation();
        }

        private static Request MakeBulletRequest(int start, int end, string preset)
        {
            var bullet = new Request
            {
                CreateParagraphBullets = new CreateParagraphBulletsRequest
                {
                    Range = new Range {StartIndex = start, EndIndex = end},
                    BulletPreset = preset //"NUMBERED_DECIMAL_ALPHA_ROMAN";// "BULLET_DISC_CIRCLE_SQUARE";
                }
            };
            return bullet;


        }

        public static void PrintRanges(List<Request> reqs)
        {
            foreach (var item in reqs)
            {
                if (item.InsertText != null)
                {
                    Console.WriteLine(
                        $"TextInsert: {item.InsertText.Location.Index} len:{item.InsertText.Text.Length}");
                }
                else if (item.CreateParagraphBullets != null)
                {
                    Console.WriteLine(
                        $"SetBullet: {item.CreateParagraphBullets.Range.StartIndex}-{item.CreateParagraphBullets.Range.EndIndex}");
                }
                else if (item.DeleteParagraphBullets != null)
                {
                    Console.WriteLine(
                        $"DelBullet: {item.DeleteParagraphBullets.Range.StartIndex}-{item.DeleteParagraphBullets.Range.EndIndex}");
                }
                else if (item.InsertInlineImage != null)
                {
                    Console.WriteLine($"ImageInsert: {item.InsertInlineImage.Location.Index}-");
                }
                else if (item.UpdateParagraphStyle != null)
                {
                    Console.WriteLine(
                        $"ParaStyle: {item.UpdateParagraphStyle.Range.StartIndex}-{item.UpdateParagraphStyle.Range.EndIndex}");
                }
                else if (item.UpdateTextStyle != null)
                {
                    Console.WriteLine(
                        $"TextStyle: {item.UpdateTextStyle.Range.StartIndex}-{item.UpdateTextStyle.Range.EndIndex}");
                }
            }
        }

        public static void DocumentListRequest()
        {
            DocumentBuilder builder = new DocumentBuilder();

            var list = builder.AddList();
            list.AddItem(0, true, "1");
            list.AddItem(0, true, "2");
            list.AddItem(1, true, "3");
            list.AddItem(1, true, "4");
            list.AddItem(2, true, "5");
            list.AddItem(2, true, "6");
            list.AddItem(3, true, "7");
            list.AddItem(3, true, "8");
            list.AddItem(4, true, "9");
            list.AddItem(4, true, "a");
            list.AddItem(5, true, "b");
            list.AddItem(5, true, "c");


            var reqs = builder.RequestList().requests;
            //PrintRanges(reqs);
            BatchUpdateDocumentRequest body
                = new BatchUpdateDocumentRequest {Requests = reqs};

            var response = GDocController.DocumentRequest(body, "Document List Request");

        }

        public static void ListBatchRequest2()
        {
            var requestList = new List<Request>();


            int IP = 1;
            var textReq1 = RequestHelpers.MakeTextInsertRequest("1ardy boys went to town.", 1);
            ToEnd(textReq1);
            var bulletBa = RequestHelpers.MakeCreateBulletRequest(0,true, IP, IP);
            IP += textReq1.InsertText.Text.Length;
            var textReq2 = RequestHelpers.MakeTextInsertRequest("2ardy boys went to town.", 1);
            ToEnd(textReq2);
            var bulletBb = RequestHelpers.MakeCreateBulletRequest(0, true, IP , IP);
            IP += textReq2.InsertText.Text.Length;
            var textReq3 = RequestHelpers.MakeTextInsertRequest("3ardy boys went to town.", 1);
            ToEnd(textReq3);
            var bulletBc = RequestHelpers.MakeCreateBulletRequest(0, true, IP , IP);
            IP += textReq3.InsertText.Text.Length;
            var textReqa = RequestHelpers.MakeTextInsertRequest("\tAardy boys went to town.", 1);
            ToEnd(textReqa);
            var bulletB1 = RequestHelpers.MakeCreateBulletRequest(1, true, IP , IP);
            IP += textReqa.InsertText.Text.Length;
            var textReqb = RequestHelpers.MakeTextInsertRequest("\tBardy boys went to town.", 1);
            ToEnd(textReqb);
            var bulletB2 = RequestHelpers.MakeCreateBulletRequest(0, true, IP , IP);
            IP += textReqb.InsertText.Text.Length;
            var textReqc = RequestHelpers.MakeTextInsertRequest("\tCardy boys went to town.", 1);
            ToEnd(textReqc);
            var bulletB3 = RequestHelpers.MakeCreateBulletRequest(1, true, IP , IP);
            IP += textReqc.InsertText.Text.Length;
            var textReq4 = RequestHelpers.MakeTextInsertRequest("4ardy boys went to town.", 1);
            ToEnd(textReq4);
            var bulletBd = RequestHelpers.MakeCreateBulletRequest(0, true, IP , IP);
            IP += textReq4.InsertText.Text.Length;
            var textReq5 = RequestHelpers.MakeTextInsertRequest("5ardy boys went to town.", 1);
            ToEnd(textReq5);
            var bulletBe = RequestHelpers.MakeCreateBulletRequest(0, true, IP , IP);
            IP += textReq5.InsertText.Text.Length;
            var textReq6 = RequestHelpers.MakeTextInsertRequest("6ardy boys went to town.", 1);
            ToEnd(textReq6);
            var bulletBf = RequestHelpers.MakeCreateBulletRequest(0, true, IP , IP);

            int span1 = textReq1.InsertText.Text.Length + textReq2.InsertText.Text.Length +
                        textReq3.InsertText.Text.Length;
            int span2 = textReqa.InsertText.Text.Length + textReqb.InsertText.Text.Length +
                        textReqc.InsertText.Text.Length;
            int span3 = textReq4.InsertText.Text.Length + textReq5.InsertText.Text.Length +
                        textReq6.InsertText.Text.Length;
            
            var bullet = RequestHelpers.MakeCreateBulletRequest(0,false, 1, span1);
            var bullet1 = RequestHelpers.MakeCreateBulletRequest(1,true, span1 + 1, span1 + span2 - 3); 
            var bullet2 = RequestHelpers.MakeCreateBulletRequest(0,false, span1 + span2 + 1 - 3, span1 + span2 + span3 - 3); // Since tabs are removed, need to reduce span2 to compensate

            var bulletB = RequestHelpers.MakeCreateBulletRequest(0, false, 1, 1);
            var bulletN = RequestHelpers.MakeCreateBulletRequest(1,true, 1, 1); 

            var reqList = new List<Request>();

            reqList.Add(textReq1);
            reqList.Add(bulletBa);
            reqList.Add(textReq2);
            reqList.Add(bulletBb);
            reqList.Add(textReq3);
            reqList.Add(bulletBc);
            
            reqList.Add(textReqa);
            reqList.Add(bulletB1);
            reqList.Add(textReqb);
            reqList.Add(bulletB2);
            reqList.Add(textReqc);
            reqList.Add(bulletB3);

            reqList.Add(textReq4);
            reqList.Add(bulletBd);
            reqList.Add(textReq5);
            reqList.Add(bulletBe);
            reqList.Add(textReq6);
            reqList.Add(bulletBf);
            

            BatchUpdateDocumentRequest body
                = new BatchUpdateDocumentRequest {Requests = reqList};

            var response = GDocController.DocumentRequest(body, "List Batch Request 2");
        }

        private static string RandText()
        {
            Random rnd = new Random();
            int len = rnd.Next(1, 45);
            return new string('l', len);
        }
        public static void TableRequest()
        {
            int rows = 8;
            int cols = 9;
            int tableGutter = 4;
            int rowGutter = 1;
            int columnGutter = 2;

            int cellIp = 5;// + tableGutter + (rowGutter * (rows-1)) + (columnGutter * (cols-1)); 
            var requests = new List<Request>();
            var tableReq = RequestHelpers.MakeTableInsertRequest(rows, cols, 1);
            requests.Add(tableReq);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    string content = RandText();
                    requests.Add(RequestHelpers.MakeTextInsertRequest(content, cellIp, false));
                    cellIp += columnGutter + content.Length;
                }

                cellIp += rowGutter;
            }
            
            
            BatchUpdateDocumentRequest body
                = new BatchUpdateDocumentRequest {Requests = requests};

            var response = GDocController.DocumentRequest(body, "Table Request");

        }

        public static void ImageRequest()
        {
            string path = "D:/Repos/MDTools/cyberpunk1a.jpg";

            var requestList = new List<Request> {RequestHelpers.MakeInsertImageRequest(path)};
            BatchUpdateDocumentRequest body
                = new BatchUpdateDocumentRequest {Requests = requestList};

            var response = GDocController.DocumentRequest(body, "Insert Image");
        }
    }
}
