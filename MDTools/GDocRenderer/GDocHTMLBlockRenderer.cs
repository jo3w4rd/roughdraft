// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Markdig.Syntax;

namespace Markdig.Renderers.GDoc
{
    /// <summary>
    /// A GDoc renderer for a <see cref="HtmlBlock"/>.
    /// </summary>
    /// <seealso cref="GDocObjectRenderer{TObject}" />
    public class GDocHTMLBlockRenderer : GDocObjectRenderer<HtmlBlock>
    {
        protected override void Write(GDocRenderer renderer, HtmlBlock obj)
        {
            renderer.WriteLeafRawLines(obj, true, false);
        }
    }
}