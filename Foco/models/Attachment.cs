using System;

namespace Foco.models
{

    public enum AttachmentType { Link, Comment } // Reihenfolge bitte nicht ändern!

    public abstract class Attachment
    {

        protected AttachmentType type;
        protected string title;
        protected string content;

        public Attachment(string content)
        {
            Content = content;
        }

        public Attachment(string title, string content)
        {
            Content = content;
            Title = title;
        }

        public string Title { get => title; set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(); title = value; } }
        public string Content { get => content; set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(); content = value; } }
        public AttachmentType Type { get => type; set => type = value; }

    }
}