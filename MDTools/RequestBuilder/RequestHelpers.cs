using System;
using System.Collections.Generic;
using System.IO;
using Google.Apis.Docs.v1.Data;
using Range = Google.Apis.Docs.v1.Data.Range;

namespace MDTools.RequestBuilder
{
    public class RequestHelpers
    {
        //Helpers
        public static Request MakeTextInsertRequest(string text, int insertionPoint = 1, bool addNewline = true)
        {
            var textReq = new Request();
            textReq.InsertText = addNewline ? new InsertTextRequest {Text = text + (char) 0x0a} : new InsertTextRequest {Text = text};
            
            if (insertionPoint == -1)
            {
                textReq.InsertText.EndOfSegmentLocation = new EndOfSegmentLocation();
            }
            else
            {
                textReq.InsertText.Location = new Location {Index = insertionPoint};
            }

            return textReq;
        }

        public static Request MakeTextUpdateRequest(StyleRun run, int insertionPoint)
        {
            var style = new Request
            {
                UpdateTextStyle = new UpdateTextStyleRequest
                {
                    TextStyle = run.style.GetTextStyle(),
                    Fields = run.style.GetFieldList(),
                    Range = new Range {StartIndex = run.start + insertionPoint, EndIndex = run.end + insertionPoint}
                }
            };

            return style;
        }

        public static Request MakeParagraphStyleUpdateRequest(ParagraphStyle paraStyle, int start = 1, int end = 1)
        {
            var style = new Request
            {
                UpdateParagraphStyle = new UpdateParagraphStyleRequest
                {
                    ParagraphStyle = new Google.Apis.Docs.v1.Data.ParagraphStyle
                    {
                        NamedStyleType = Paragraph.GetParaStyleName(paraStyle)
                    },
                    Range = new Range {StartIndex = start, EndIndex =  end},
                    Fields = "namedStyleType"
                }
            };

            return style;
        }

        public static Request MakeInsertImageRequest(string path, int insertionPoint = -1, string folder = "")
        {
            var mimeType = MimeTypeForExtension(Path.GetExtension(path));

            // Synchronous -- we need the file ID before going on...
            var imageURL = GDocController.UploadImage(path, mimeType, folder);
            //TODO: need better error checking as this can fail...
            if (string.IsNullOrEmpty(imageURL))
            {
                throw new IOException($"Failed to upload {path}.");
            }

            var imageRequest = new Request();
            imageRequest.InsertInlineImage = new InsertInlineImageRequest();
            imageRequest.InsertInlineImage.Uri = imageURL;
            
            if (insertionPoint <= 0)
            {
                imageRequest.InsertInlineImage.EndOfSegmentLocation = new EndOfSegmentLocation();
            }
            else
            {
                imageRequest.InsertInlineImage.Location = new Location();
                imageRequest.InsertInlineImage.Location.Index = insertionPoint;
            }

            return imageRequest;
        }

        private static string MimeTypeForExtension(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".png":
                    return "image/png";
                case ".apng":
                    return "image/apng";
                case ".avif":
                    return "image/avif";
                case ".jpg":
                case "jpeg":
                case "jpe":
                case "jif":
                case "jfif":
                case "pjpeg":
                case "pjp":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                //case ".svg":
                //    return "image/svg-xml";
                default:
                    throw new ArgumentException($"Unsupported image extension: {fileExtension}");
            }
        }
        
        public static Request MakeParagraphBackgroundUpdateRequest(Color background, Color border, int start = 1, int end = 1)
        {
            var style = new Request
            {
                UpdateParagraphStyle = new UpdateParagraphStyleRequest
                {
                    Range = new Range {StartIndex = start, EndIndex =  end},
                    Fields = "shading, borderTop,borderRight,borderBottom,borderLeft"
                }
            };
            style.UpdateParagraphStyle.ParagraphStyle = new Google.Apis.Docs.v1.Data.ParagraphStyle
            {
                Shading = new Shading {BackgroundColor = new OptionalColor {Color = background}},
                BorderTop = MakeBorderObject(border),
                BorderBottom = MakeBorderObject(border),
                BorderLeft = MakeBorderObject(border),
                BorderRight = MakeBorderObject(border)
            };

            return style;
        }

        private static ParagraphBorder MakeBorderObject(Color color)
        {
            var border = new ParagraphBorder
            {
                Color = new OptionalColor {Color = color},
                DashStyle = "SOLID",
                Padding = new Dimension {Magnitude = 2, Unit = "PT"},
                Width = new Dimension {Magnitude = 1, Unit = "PT"}
            };
            return border;
        }
        // Can't use the same preset in consecutive indent levels or list is collapsed.
        private static string GetNumberedPreset(int level)
        {
            if (level % 2 == 0)
                return "NUMBERED_DECIMAL_ALPHA_ROMAN";
            else
                return "NUMBERED_DECIMAL_ALPHA_ROMAN_PARENS";
        }
        private static string GetBulletedPreset(int level)
        {
            if (level % 2 == 0)
                return "BULLET_DISC_CIRCLE_SQUARE";
            else
                return "BULLET_DIAMOND_CIRCLE_SQUARE";
        }
        public static Request MakeCreateBulletRequest(int level, bool ordered, int start, int end)
        {
            var bullet = new Request
            {
                CreateParagraphBullets = new CreateParagraphBulletsRequest
                {
                    Range = new Range {StartIndex = start, EndIndex = end},
                    BulletPreset = ordered ? GetNumberedPreset(level) : GetBulletedPreset(level)
                }
            };
            return bullet;
        }
        public static Request MakeDeleteBulletRequest(int start, int end)
        {
            var bullet = new Request
            {
                DeleteParagraphBullets = new DeleteParagraphBulletsRequest()
                {
                    Range = new Range {StartIndex = start, EndIndex = end}
                }
            };
            return bullet;
        }

        public static Request MakeTableInsertRequest(int rows, int cols, int insertionPoint = -1)
        {
            var tableReq = new Request {InsertTable = new InsertTableRequest {Columns = cols, Rows = rows}};
            if (insertionPoint < 0)
            {
                tableReq.InsertTable.EndOfSegmentLocation = new EndOfSegmentLocation();
            }
            else
            {
                tableReq.InsertTable.Location = new Location {Index = insertionPoint};
            }
            return tableReq;
        }
        
        public static (int start, int end, int length) CalculateTextRange(List<Request> requestList)
        {
            int start = Int32.MaxValue;
            int length = 0;
            foreach (var req in requestList)
            {
                if (req.InsertText == null) continue;
                if (req.InsertText.Location != null && req.InsertText.Location.Index < start) start = req.InsertText.Location.Index ?? 1;
                length += req.InsertText.Text.Length;
            }

            int end = start + length;;
            return (start, end, length);
        }
        
        // Only works if the modifiers apply only to the preceding text insert, which isn't always the case.
        // This might be okay for lists
        // It also "expands" text style setters to entire previous text run (which is probably okay for this purpose)
        
        public static void SetRanges(List<Request> reqs)
        {
            int start = 1;
            int end = 1;
            foreach (var item in reqs)
            {
                if (item.InsertText != null)
                {
                    start = end;
                    end += item.InsertText.Text.Length;
                    item.InsertText.Location.Index = start;
                } 
                else if (item.CreateParagraphBullets != null)
                {
                    item.CreateParagraphBullets.Range.StartIndex = start;
                    item.CreateParagraphBullets.Range.EndIndex = end;
                }
                else if (item.InsertInlineImage != null)
                {
                    item.InsertInlineImage.Location.Index += start;

                }
                else if (item.UpdateParagraphStyle != null)
                {
                    item.UpdateParagraphStyle.Range.StartIndex = start;
                    item.UpdateParagraphStyle.Range.EndIndex = end;
                }
                else if (item.UpdateTextStyle != null)
                {
                    item.UpdateTextStyle.Range.StartIndex = start;
                    item.UpdateTextStyle.Range.EndIndex = end;
                }
            }
        }
        //An alternative to SetRanges is to only shift start, end, and location based on the blocks insertion point
        public static int ShiftRanges(List<Request> reqs, int initialOffset)
        {
            int offset = initialOffset;
            int totalLength = 0;
            bool inList = false;
            int strippedTabs = 0;
            foreach (var item in reqs)
            {
                if (item.InsertText != null)
                {
                    ToEnd(item);
                    //Don't count tabs because they are used to indicate list indent and are stripped by GDoc
                    //before next request is executed.
                    var temp = item.InsertText.Text.Replace(DocumentList.Tab.ToString(),"");
                    strippedTabs = item.InsertText.Text.Length - temp.Length;
                    totalLength += temp.Length;
                } 
                else if (item.CreateParagraphBullets != null)
                {
                    item.CreateParagraphBullets.Range.StartIndex += offset;
                    item.CreateParagraphBullets.Range.EndIndex += offset;
                }
                else if (item.InsertInlineImage != null)
                {
                    item.InsertInlineImage.Location.Index += offset;
                    totalLength += 1;
                }
                else if (item.UpdateParagraphStyle != null)
                {
                    item.UpdateParagraphStyle.Range.StartIndex += offset;
                    item.UpdateParagraphStyle.Range.EndIndex += offset;
                }
                else if (item.UpdateTextStyle != null)
                {
                    item.UpdateTextStyle.Range.StartIndex += offset + strippedTabs;
                    item.UpdateTextStyle.Range.EndIndex += offset + strippedTabs;
                }

            }
            //Console.WriteLine($"Offset {offset} Length {totalLength}");
            return totalLength;
        }
        
        private static void ToEnd(Request textReq)
        {
            textReq.InsertText.Location = null;
            textReq.InsertText.EndOfSegmentLocation = new EndOfSegmentLocation();
        }

    }
}