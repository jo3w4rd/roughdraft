// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.GDoc.Inlines
{
    /// <summary>
    /// A HTML renderer for a <see cref="LiteralInline"/>.
    /// </summary>
    /// <seealso cref="GDocObjectRenderer{TObject}" />
    public class LiteralInlineRenderer : GDocObjectRenderer<LiteralInline>
    {
        protected override void Write(GDocRenderer renderer, LiteralInline obj)
        {
            if (renderer.EnableHtmlEscape)
            {
                renderer.WriteEscape(ref obj.Content);
            }
            else
            {
                renderer.Write(ref obj.Content);
            }
        }
    }
}
