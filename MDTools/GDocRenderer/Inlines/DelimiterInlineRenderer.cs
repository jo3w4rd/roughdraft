// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.GDoc.Inlines
{
    /// <summary>
    /// A HTML renderer for a <see cref="DelimiterInline"/>.
    /// </summary>
    /// <seealso cref="GDocObjectRenderer{TObject}" />
    public class DelimiterInlineRenderer : GDocObjectRenderer<DelimiterInline>
    {
        protected override void Write(GDocRenderer renderer, DelimiterInline obj)
        {
            renderer.WriteEscape(obj.ToLiteral());
            renderer.WriteChildren(obj);
        }
    }
}