using Markdig.Extensions.Tables;
using Markdig.Helpers;

namespace Markdig.GDoc.Extensions.Tables
{
    public static class MarkdownPipelineBuilderGDocExtensions
    {
        public static MarkdownPipelineBuilder UseGDocPipeTables(this MarkdownPipelineBuilder pipeline, PipeTableOptions? options = null)
        {
            {
                if (!pipeline.Extensions.Contains<GDocPipeTableExtension>())
                {
                    pipeline.Extensions.Add(new GDocPipeTableExtension(options));
                }
                return pipeline;
            }

        }
    }
}