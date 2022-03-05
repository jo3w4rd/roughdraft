// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Markdig.Extensions.Abbreviations;
using Markdig.Renderers;
using Markdig.Renderers.GDoc;

namespace Markdig.GDoc.Extensions.Abbreviations
{
    /// <summary>
    /// A HTML renderer for a <see cref="AbbreviationInline"/>.
    /// </summary>
    /// <seealso cref="GDocObjectRenderer{TObject}" />
    public class GDocAbbreviationRenderer : GDocObjectRenderer<AbbreviationInline>
    {
        protected override void Write(GDocRenderer renderer, AbbreviationInline obj)
        {
            // <abbr title="Hyper Text Markup Language">HTML</abbr>
            var abbr = obj.Abbreviation;
            if (renderer.EnableHtmlForInline)
            {
                renderer.Write("<abbr").WriteAttributes(obj).Write(" title=\"").WriteEscape(ref abbr.Text).Write("\">");
            }
            renderer.Write(abbr.Label);
            if (renderer.EnableHtmlForInline)
            {
                renderer.Write("</abbr>");
            }
        }
    }
}