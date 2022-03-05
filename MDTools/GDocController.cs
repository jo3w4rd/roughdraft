using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.Util.Store;
using Markdig;

namespace MDTools
{
    public class GDocController
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/docs.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { DocsService.Scope.Documents, DriveService.Scope.Drive };
        static string ApplicationName = "Google Docs API .NET Quickstart";

        private static UserCredential credential;
        public static void HandleCredential()
        {
            //TODO seems like we could load the saved token from the JSON file
           if(credential == null || credential.Token.IsExpired(SystemClock.Default)){
                using (var stream =
                    new FileStream(
                        "data.json",
                        FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";

                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;

                    Console.WriteLine("Credential file saved to: " + credPath);
                }
           }
        }

        public static void TestFetchDoc()
        {
            string testA = "https://docs.google.com/document/d/18_AQSF8xnXQeQFEvfSVe6ZkhLid76Lha_agkM3AjxD8/edit?usp=sharing";
            string testB = "https://docs.google.com/document/d/1L_hslO4QgJpeMxLu8_t8qBignL87S3w72h9s0iX_tUQ/edit?usp=sharing";
            string testC = "https://docs.google.com/document/d/1J8n83rPvy51v2q0hTD0zol3CpcH8Whu8RfmMAxgkTK8/edit?usp=sharing";
            string testD = "https://docs.google.com/document/d/1kD7ROEPBll-U88xF862vXjRa4CKi0snLyMBwwI9TpKE/edit";
            string testE =
                "https://docs.google.com/document/d/1Tjj_Mk6vQQ4eFN7xwJFRgAtdRXEu27WM-NiDAVyWyjw/edit#heading=h.gbcm3l729bfo";
            string testF = "https://docs.google.com/document/d/1FWREOrYSqSmKZjADvxwiVQb_KyxH1G3jOAtHIr8n-BY/edit#";
            string testG = "https://lh3.google.com/u/0/d/1Ky9Hn9B1MxcK-rOOrNmwxBh0lG8AXZo0=s2048";
            Document doc = FetchGDoc(testG);
            
            Console.WriteLine("The title of the doc is: {0}", doc.Title);
            Console.WriteLine(DocumentToDebugString(doc));
        }

        public static void TestConversionToMarkdown()
        {
            string testA = "https: //docs.google.com/document/d/18a8xV7ovXy-MqKKpxEWtuXGU4Et6eBbvEcnti88cx_g/edit#";
            string custInit =
                "https://docs.google.com/document/d/1IPEIDy5Ug447GHd4BBJ9xyw8pSbeC3Gc6rMygm6oHpg/edit?usp=sharing";
            string bullets =
                "https://docs.google.com/document/d/1teojDsBC8mgtvnSpGkCoAmUjophAJyElihzVv8MJcVk/edit?usp=sharing";
            
            Document doc = FetchGDoc(bullets);
            string md = GDocToMarkdown.ConvertDocumentToMarkdown(doc);
            Console.WriteLine(md);
            
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            Console.WriteLine(Markdown.ToHtml(md, pipeline));

        }

        public static void TestFolderList()
        {
            string FolderA = "https://drive.google.com/drive/folders/1JSJIAkWKi-x9SleiqTWttFkfvtMAswvx";
            string FolderB = "https://drive.google.com/drive/folders/1gKtJ6SfQyiMNwtjRpktHLGdLTISF6sib";
            string FolderC = "https://drive.google.com/drive/folders/1Cl7Sq_scOgXff55_HBxL5_zf6KcB5uxn";

            var results = GetGDocListFromFolder(FolderC);
            foreach (var fileInfo in results.Files)
            {
                Console.WriteLine(fileInfo.Id + ", " + fileInfo.Name);
            }
        }

        public static void TestBoth()
        {
            string FolderB = "https://drive.google.com/drive/folders/1gKtJ6SfQyiMNwtjRpktHLGdLTISF6sib";
            
            var results = GetGDocListFromFolder(FolderB);
            foreach (var fileInfo in results.Files)
            {
                Console.WriteLine(fileInfo.Id + ", " + fileInfo.Name);
                Document doc = FetchGDoc(fileInfo.Id);
                Console.WriteLine("The title of the doc is: {0}", doc.Title);
            }

        }
        
        private static string IDFromURL(string url)
        {
            // Leave untouched if the string isn't an absolute web URL
            if (!url.StartsWith("http"))
                return url;
            
            var parts = url.Split('/');

            if (url.Contains("folders"))
                return parts[^1]; // the ID is at the end for folder URLs

            return parts[^2]; // TODO verify that Id is always the second to last for GDocs!
        }
        public static Document FetchGDoc(string DocumentId)
        {
            HandleCredential();
            
            // Create Google Docs API service.
            var service = new DocsService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            DocumentsResource.GetRequest request = service.Documents.Get(IDFromURL(DocumentId));
            Document doc = request.Execute();

            return doc;
        }

        public static (List<(string Id, string Name)> Files, string PagingToken) // A tuple of a list of tuple (Id, Name) and a paging token
            GetGDocListFromFolder(string folderUrl, int limit = -1,  string nextPageToken = "")
        {
            HandleCredential();
            var result = new List<(string, string)>();
            // Create Drive API service.
            var driveService = new DriveService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = driveService.Files.List();
            listRequest.Q = "mimeType=\'application/vnd.google-apps.document\'";
            if(!string.IsNullOrWhiteSpace(folderUrl)) // limit to a given folder
                listRequest.Q += $" and \'{IDFromURL(folderUrl)}\' in parents";
            if(limit > 0) // limit to a number of files returned
                listRequest.PageSize = limit;
            listRequest.PageToken = nextPageToken; // token to get next page of results
            listRequest.Corpora = "allDrives"; // What to search: user, drive, allDrives
            listRequest.IncludeItemsFromAllDrives = true;
            listRequest.SupportsTeamDrives = true;
            listRequest.Fields = "nextPageToken, files(id, name)";
            
            // List files.
            var response = listRequest.Execute();
            
            IList<Google.Apis.Drive.v3.Data.File> files = response.Files;
            if (files != null && files.Count > 0) {
                foreach (var file in files) {
                    result.Add(new ValueTuple<string,string>(file.Id, file.Name));
                }
            }
            return (result, (response.IncompleteSearch ?? false) ? response.NextPageToken : "");
        }

        public static string DocumentRequest(BatchUpdateDocumentRequest requestBatch, string title)
        {
            HandleCredential();
            
            // Create Google Docs API service.
            var service = new DocsService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            Document newDoc = new Document {Title = title};
            newDoc = service.Documents.Create(newDoc).Execute();
            // Define request parameters.
            service.Documents.BatchUpdate(requestBatch, newDoc.DocumentId).Execute();
            return newDoc.DocumentId;
        }

        public static void TestUploadImage()
        {
            string path = "D:/Repos/MDTools/cyberpunk1.jpg";
            string mimeType = "image/jpeg";
            var id = UploadImage(path, mimeType);
            Console.WriteLine($"Uploaded Id = {id}");
        }
        public static string UploadImage(string filePath, string mimeType, string uploadFolder = "")
        {
            HandleCredential();
            var driveService = new DriveService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = Path.GetFileName(filePath),
                CopyRequiresWriterPermission = false,
            };
            
            var upload =
                new FilesResource.CreateMediaUpload(driveService, fileMetadata, stream, mimeType);
            upload.Upload();
            
            var perm = new Permission();
            perm.Type = "anyone";
            perm.Role = "reader";
            var setPermission = new PermissionsResource.CreateRequest(driveService, perm, upload.ResponseBody.Id);
            var permResult = setPermission.Execute();
            
            var getMetaDataRequest = new FilesResource.GetRequest(driveService, upload.ResponseBody.Id);
            getMetaDataRequest.Fields = "webContentLink";
            var imageMetadata = getMetaDataRequest.Execute();

            return imageMetadata.WebContentLink;
        }
        
        public static string DocumentToDebugString(Document doc)
        {
            string result = "";
            foreach (var se in doc.Body.Content)
            {
                result += $"({se.StartIndex}-{se.EndIndex}) ";
                if (se.Paragraph != null)
                {
                    // if (se.Paragraph.Bullet != null)
                    // {
                    //     result += $"{se.Paragraph.Bullet} ";
                    // }
                    foreach (var element in se.Paragraph.Elements)
                    {
                        if (element.TextRun != null)
                        {
                            result += $"{element.TextRun.Content}";
                        }
                    }
                }

                if (se.Table != null)
                {
                    foreach (var row in se.Table.TableRows)
                    {
                        result += $"Row {row.StartIndex}-{row.EndIndex}\n";
                        foreach (var cell in row.TableCells)
                        {
                            result += $"Cell ({cell.StartIndex}-{cell.EndIndex})\n";
                        }
                    }
                }
                //result += "\n";
            }
            return result;
        }
    }
}