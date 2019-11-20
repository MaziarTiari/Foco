using System;

namespace testFoco.models
{
    public class CommentAttachment: Attachment
    {
        private DateTime date;
        public CommentAttachment(string comment)
        {
            Content = comment;
            this.date = DateTime.UtcNow;
            Type = AttachmentType.comment;
        }
        public DateTime Date => date;
    }
}
