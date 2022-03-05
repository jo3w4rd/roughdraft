// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Markdig.Extensions.Tables;
using Markdig.Renderers;

namespace Markdig.GDoc.Extensions.Tables
{
    /// <summary>
    /// Extension that allows to use grid tables.
    /// </summary>
    /// <seealso cref="IMarkdownExtension" />
    public class GDocGridTableExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<GridTableParser>())
            {
                pipeline.BlockParsers.Insert(0, new GridTableParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is GDocRenderer gdocRenderer && !gdocRenderer.ObjectRenderers.Contains<GDocTableRenderer>())
            {
                gdocRenderer.ObjectRenderers.Add(new GDocTableRenderer());
            }
        }
    }
}