// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System.Globalization;
using Markdig.Syntax;

namespace Markdig.Renderers.GDoc
{
    /// <summary>
    /// An HTML renderer for a <see cref="HeadingBlock"/>.
    /// </summary>
    /// <seealso cref="GDocObjectRenderer{TObject}" />
    public class HeadingRenderer : GDocObjectRenderer<HeadingBlock>
    {
        private static readonly string[] HeadingTexts = {
            "HEADING_1",
            "HEADING_2",
            "HEADING_3",
            "HEADING_4",
            "HEADING_5",
            "HEADING_6",
        };

        protected override void Write(GDocRenderer renderer, HeadingBlock obj)
        {
            int index = obj.Level - 1;
            string headingText = ((uint)index < (uint)HeadingTexts.Length)
                ? HeadingTexts[index]
                : "h" + obj.Level.ToString(CultureInfo.InvariantCulture);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("<").Write(headingText).WriteAttributes(obj).Write('>');
            }

            renderer.WriteLeafInline(obj);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("</").Write(headingText).WriteLine(">");
            }

            renderer.EnsureLine();
        }
    }
}