using System;
using System.IO;
using System.Text;

namespace MDTools
{
    public class GDocWriter : TextWriter
    {
        private TextWriter writer;
        public override Encoding Encoding { get; }
        
        protected GDocWriter(TextWriter writer)
        {
            if (writer is null) throw new ArgumentNullException();
            this.writer = writer;
            this.writer.NewLine = "\n";
        }
    }
}