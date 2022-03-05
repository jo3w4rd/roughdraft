using Google.Apis.Drive.v3;

namespace MDTools.RequestBuilder
{
    public class StyleRun
    {
        public Style style;
        public int start;
        public int end;
        public int length{
            get { return end - start; }
        }

        public StyleRun(Style textStyle, int start, int end)
        {
            this.style = textStyle;
            this.start = start;
            this.end = end;
        }
    }
}