// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Markdig.Extensions.Abbreviations;
using Markdig.Renderers;

namespace Markdig.GDoc.Extensions.Abbreviations
{
    /// <summary>
    /// Extension to allow abbreviations.
    /// </summary>
    /// <seealso cref="IMarkdownExtension" />
    public class AbbreviationExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.BlockParsers.AddIfNotAlready<AbbreviationParser>();
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is GDocRenderer gDocRenderer && !gDocRenderer.ObjectRenderers.Contains<GDocAbbreviationRenderer>())
            {
                // Must be inserted before CodeBlockRenderer
                gDocRenderer.ObjectRenderers.Insert(0, new GDocAbbreviationRenderer());
            }
        }
    }
}