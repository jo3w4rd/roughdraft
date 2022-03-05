using System;
using Google.Apis.Docs.v1.Data;

namespace MDTools
{
    public class ListState
    {
        public int[] counts;
        public ListProperties props;

        public bool isBullet(int level)
        {
            return string.IsNullOrEmpty(props.NestingLevels[level].GlyphType) || props.NestingLevels[level].GlyphType == "GLYPH_TYPE_UNSPECIFIED";
        }

        public ListState(ListProperties props)
        {
            this.props = props;
            counts = new int[props.NestingLevels.Count];
            for (var i = 0; i < props.NestingLevels.Count; i++)
                counts[i] = props.NestingLevels[i].StartNumber ?? 0;
        }

        public string Peek(int level)
        {
            return GetListPrefix(counts[level], props.NestingLevels[level].GlyphType);
        }
        public string GetNextItem(int level)
        {
            return GetListPrefix(counts[level]++, props.NestingLevels[level].GlyphType);
        }
        
        private static readonly string[] upper_roman =
        {
            "I", "II", "III", "IV", "V", "VI", "VII"
        };
        private static readonly string[] roman =
        {
            "i", "ii", "iii", "iv", "v", "vi", "vii"
        };

        public static string GetListPrefix(int ordinal, string glyphType)
        {
            switch (glyphType)
            {
                case "DECIMAL":
                    return ordinal.ToString();
                case "ALPHA":
                    return Convert.ToString((char)('a' + ordinal - 1));
                case "UPPER_ALPHA":
                    return Convert.ToString((char)('A' + ordinal - 1));
                case "UPPER_ROMAN":
                    if (ordinal <= roman.Length)
                        return upper_roman[ordinal - 1];
                    else
                        return upper_roman[^1]; // Your list is too long anyway
                case "ROMAN":
                    if (ordinal <= roman.Length)
                        return roman[ordinal - 1];
                    else
                        return roman[^1]; // Your list is too long anyway
                default:
                    return "-";
            }
        }
    }
}