using System;
using System.Collections.Generic;
using Google.Apis.Docs.v1.Data;

namespace MDTools.RequestBuilder
{
    /// <summary>
    /// Contains the data for a text style. 
    /// </summary>
    /// <remarks>
    /// A span can have multiple style runs applied to it. The data needed to make a request includes:
    /// a list of style names (bild, italic), a list of font families, a list of the fields that are set
    /// by a request, and a Url for links (which are implemented as a style in GDoc).
    /// 
    /// (The multiple style aspect might be unnecessary and thus removed to simplify the class.)
    ///</remarks>
    public class Style : IEquatable<Style>
    {
        private List<TextStyles> textStyles;
        private List<string> FontFamilies;
        private List<string> Fields;
        private string Url;

        private Style()
        {
            textStyles = new List<TextStyles>();
            Fields = new List<string>();
        }
        
        /// <summary>
        /// Creates a new style object from a single style name (and a link URL, if applicable). 
        /// </summary>
        /// <param name="textStyle"></param>
        /// <param name="url"></param>
        /// <exception cref="ArgumentException">If using the Link style without setting a Url.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If passing in a text style name that this method doesn't
        /// have code to handle.</exception>
        public Style(TextStyles textStyle, string url = "")
        {
            if (textStyle == TextStyles.Link && string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Must set Url when creating link.");
            
            textStyles = new List<TextStyles>();
            Fields = new List<string>();
            textStyles.Add(textStyle);

            switch(textStyle)
            {
                case TextStyles.Bold:
                    Fields.Add(FieldForStyle(TextStyles.Bold));
                    break;
                case TextStyles.Italic:
                    Fields.Add(FieldForStyle(TextStyles.Italic));
                    break;
                case TextStyles.Code:
                    Fields.Add(FieldForStyle(TextStyles.Code));
                    FontFamilies = new List<string>();
                    FontFamilies.Add("Courier New");
                    break;
                case TextStyles.Link:
                    Fields.Add(FieldForStyle(TextStyles.Link));
                    Url = url;
                    break;
                case TextStyles.StrikeThrough:
                case TextStyles.Subscript:
                case TextStyles.Superscript:
                case TextStyles.Inserted:
                case TextStyles.Marked:
                    Fields.Add(FieldForStyle(TextStyles.StrikeThrough)); // All the underline currently
                    break;
                case TextStyles.Normal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(textStyle), textStyle, "Unrecognized text style");
            }
            
        }

        public bool IsEmpty
        {
            get { return textStyles.Count == 0; }
        }
        private string FieldForStyle(TextStyles style)
        {
            switch(style)
            {
                case TextStyles.Bold:
                    return "bold";
                case TextStyles.Italic:
                    return "italic";
                case TextStyles.Code:
                    return "weightedFontFamily";
                case TextStyles.Link:
                    return "link";
                case TextStyles.StrikeThrough:
                case TextStyles.Subscript:
                case TextStyles.Superscript:
                case TextStyles.Inserted:
                case TextStyles.Marked:
                    return "underline";
                case TextStyles.Normal:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, "Unrecognized text style");
            }
        }

        public string GetFieldList()
        {
            var fieldList = "";
            foreach (var field in Fields)
            {
                fieldList += field + ",";
            }
            return fieldList.TrimEnd(',');

        }
        public TextStyle GetTextStyle()
        {
            var textStyle = new TextStyle();
            foreach (var styleType in textStyles)
            {
                switch(styleType)
                {
                    case TextStyles.Bold:
                        textStyle.Bold = true;
                        break;
                    case TextStyles.Italic:
                        textStyle.Italic = true;
                        break;
                    case TextStyles.Code:
                        if(textStyle.WeightedFontFamily == null)
                            textStyle.WeightedFontFamily = new WeightedFontFamily();
                        textStyle.WeightedFontFamily.FontFamily = FontListToString();
                        break;
                    case TextStyles.Link:
                        if (textStyle.Link == null)
                            textStyle.Link = new Link();
                        else
                            Console.WriteLine("Warning overlapping links -- that shouldn't be possible in Markdown!");
                        textStyle.Link.Url = Url;
                        break;
                    case TextStyles.StrikeThrough:
                        textStyle.Strikethrough = true;
                        break;
                    case TextStyles.Subscript:
                    case TextStyles.Superscript:
                    case TextStyles.Inserted:
                    case TextStyles.Marked:
                        textStyle.Underline = true;
                        break;
                    case TextStyles.Normal:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(styleType), styleType, "Unrecognized text style");
                }
            }
            return textStyle;
        }

        private string FontListToString()
        {
            var fonts = "";
            foreach (var fontFamily in FontFamilies)
            {
                fonts += fontFamily + ",";
            }
            return fonts.TrimEnd(',');
        }
        
        public Style Clone()
        {
            var clone = new Style();
            clone.textStyles.AddRange(this.textStyles);
            clone.Fields.AddRange(this.Fields);
            clone.FontFamilies.AddRange(this.FontFamilies);
            return clone;
        }

        public bool Equals(Style? style)
        {
            if (object.ReferenceEquals(this, style)) return true;
            if (this.Url != style.Url) return false;
            if (this.textStyles != style.textStyles) return false;
            if (this.FontFamilies != style.FontFamilies) return false;
            if (this.Fields != style.Fields) return false;

            return true;        }

        public override bool Equals(object obj)
        {
            var style = obj as Style;
            if (style == null) return false;
            if (object.ReferenceEquals(this, style)) return true;
            if (this.Url != style.Url) return false;
            if (this.textStyles != style.textStyles) return false;
            if (this.FontFamilies != style.FontFamilies) return false;
            if (this.Fields != style.Fields) return false;

            return true;
        }
        
        public static bool operator ==(Style left, Style right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }
        public static bool operator !=(Style left, Style right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return !object.ReferenceEquals(right, null);
            }
            return !left.Equals(right);
        }
    }
}