// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Markdig.Syntax.Inlines;

namespace Markdig.Renderers.GDoc.Inlines
{
    /// <summary>
    /// A HTML renderer for a <see cref="CodeInline"/>.
    /// </summary>
    /// <seealso cref="GDocObjectRenderer{TObject}" />
    public class CodeInlineRenderer : GDocObjectRenderer<CodeInline>
    {
        protected override void Write(GDocRenderer renderer, CodeInline obj)
        {
            if (renderer.EnableHtmlForInline)
            {
                renderer.Write("<code").WriteAttributes(obj).Write('>');
            }
            if (renderer.EnableHtmlEscape)
            {
                renderer.WriteEscape(obj.Content);
            }
            else
            {
                renderer.Write(obj.Content);
            }
            if (renderer.EnableHtmlForInline)
            {
                renderer.Write("</code>");
            }
        }
    }
}