using Google.Apis.Docs.v1.Data;

namespace MDTools
{
    public class GDocRequest
    {
        public Request InsertContent;
        public Request SetStyle;

        public GDocRequest()
        {
            //InsertContent = new Request();
        }
        public GDocRequest(Request insertContent, Request setStyle)
        {
            this.InsertContent = insertContent;
            this.SetStyle = setStyle;
        }
        public GDocRequest(Request insertContent)
        {
            this.InsertContent = insertContent;
        }

    }
}