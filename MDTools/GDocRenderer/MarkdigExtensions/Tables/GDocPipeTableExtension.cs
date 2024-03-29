// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Markdig.Extensions.Tables;
using Markdig.Parsers.Inlines;
using Markdig.Renderers;

namespace Markdig.GDoc.Extensions.Tables
{
    /// <summary>
    /// Extension that allows to use pipe tables.
    /// </summary>
    /// <seealso cref="IMarkdownExtension" />
    public class GDocPipeTableExtension : IMarkdownExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipeTableExtension"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public GDocPipeTableExtension(PipeTableOptions? options = null)
        {
            Options = options ?? new PipeTableOptions();
        }

        /// <summary>
        /// Gets the options.
        /// </summary>
        public PipeTableOptions Options { get; }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            // Pipe tables require precise source location
            pipeline.PreciseSourceLocation = true;
            if (!pipeline.BlockParsers.Contains<PipeTableBlockParser>())
            {
                pipeline.BlockParsers.Insert(0, new PipeTableBlockParser());
            }
            var lineBreakParser = pipeline.InlineParsers.FindExact<LineBreakInlineParser>();
            if (!pipeline.InlineParsers.Contains<PipeTableParser>())
            {
                pipeline.InlineParsers.InsertBefore<EmphasisInlineParser>(new PipeTableParser(lineBreakParser!, Options));
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