// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using Markdig.Syntax.Inlines;
using System;

namespace Markdig.Renderers.GDoc.Inlines
{
    /// <summary>
    /// A HTML renderer for a <see cref="LinkInline"/>.
    /// </summary>
    /// <seealso cref="GDocObjectRenderer{TObject}" />
    public class LinkInlineRenderer : GDocObjectRenderer<LinkInline>
    {
        /// <summary>
        /// Gets or sets a value indicating whether to always add rel="nofollow" for links or not.
        /// </summary>
        [Obsolete("AutoRelNoFollow is obsolete. Please write \"nofollow\" into Property Rel.")]
        public bool AutoRelNoFollow
        {
            get
            {
                return Rel is not null && Rel.Contains("nofollow");
            }
            set
            {
                const string rel = "nofollow";
                if (value)
                {
                    if (string.IsNullOrEmpty(Rel))
                        Rel = rel;
                    else if (!Rel!.Contains(rel))
                        Rel += $" {rel}";
                }
                else if (!value && Rel is not null)
                {
                    Rel = Rel.Replace(rel, string.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the literal string in property rel for links
        /// </summary>
        public string? Rel { get; set; }

        protected override void Write(GDocRenderer renderer, LinkInline link)
        {
            if (renderer.EnableHtmlForInline)
            {
                renderer.Write(link.IsImage ? "<img src=\"" : "<a href=\"");
                renderer.WriteEscapeUrl(link.GetDynamicUrl != null ? link.GetDynamicUrl() ?? link.Url : link.Url);
                renderer.Write('"');
                renderer.WriteAttributes(link);
            }
            if (link.IsImage)
            {
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write(" alt=\"");
                }
                var wasEnableHtmlForInline = renderer.EnableHtmlForInline;
                renderer.EnableHtmlForInline = false;
                renderer.WriteChildren(link);
                renderer.EnableHtmlForInline = wasEnableHtmlForInline;
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write('"');
                }
            }

            if (renderer.EnableHtmlForInline && !string.IsNullOrEmpty(link.Title))
            {
                renderer.Write(" title=\"");
                renderer.WriteEscape(link.Title);
                renderer.Write('"');
            }

            if (link.IsImage)
            {
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write(" />");
                }
            }
            else
            {
                if (renderer.EnableHtmlForInline)
                {
                    if (!string.IsNullOrWhiteSpace(Rel))
                    {
                        renderer.Write($" rel=\"{Rel}\"");
                    }
                    renderer.Write('>');
                }
                renderer.WriteChildren(link);
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write("</a>");
                }
            }
        }
    }
}