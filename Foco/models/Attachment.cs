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
            if(string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException();
            this.content = content;
        }

        public Attachment(string title, string content)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException();
            this.content = content;
            this.title = title;
        }

        public string Title { get => title; set => title = value; }
        public string Content { get => content; set => content = value; }
        public AttachmentType Type { get => type; set => type = value; }

    }
}