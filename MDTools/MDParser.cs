using System;
using System.IO;
using Google.Apis.Services;
using Markdig;
using Markdig.GDoc.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Syntax;

namespace MDTools
{
    public class MDParser
    {
        public static void ParseMarkdown(string markdown, IClientService service = null)
        {
            var pipeline = new MarkdownPipelineBuilder().UseGDocPipeTables().Build();
            MarkdownDocument md = Markdown.Parse(markdown, pipeline);
            
            var stWriter = new StringWriter();
            var renderer = new GDocRenderer(stWriter, service);
            pipeline.Setup(renderer);
            var testString = renderer.Render(md).ToString();
            
            //Console.WriteLine(testString);
        }
    }
    
}