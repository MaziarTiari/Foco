using System;

namespace Foco.models
{
    public class CommentAttachment : Attachment
    {

        private DateTime date;

        public CommentAttachment(string comment, DateTime date) : base(comment)
        {
            Date = date;
            Type = AttachmentType.Comment;
        }

        public DateTime Date { get => date; set { if (value == null) throw new ArgumentNullException(); date = value; } }

    }
}
