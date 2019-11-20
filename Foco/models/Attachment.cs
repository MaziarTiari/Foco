using System.Collections.Generic;

namespace testFoco.models
{
    public enum AttachmentType { link, comment}
    public class Attachment
    {
        protected AttachmentType type;
        protected string title;
        protected string content;
        public Attachment()
        {
        }
        public Attachment(string title)
        {
            this.Title = title;
        }
        public string Title { get => title; set => title = value; }
        public string Content { get => content; set => content = value; }
        public AttachmentType Type { get => type; set => type = value; }
    }
}