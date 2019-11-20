using System;

namespace testFoco.models
{
    public class Comment
    {
        private string content;
        private DateTime date;

        public Comment(string content)
        {
            this.Content = content;
            this.date = DateTime.UtcNow;
        }

        public string Content { 
            get => content;
            set
            {
                content = value;
                date = DateTime.UtcNow;
            }
        }

        public DateTime Date => date;
    }
}
