using System.Collections.Generic;

namespace testFoco.models
{
    public class Attachment
    {
        protected string title;
        protected string description;
        private List<Comment> comments = new List<Comment>();
        public Attachment(string title)
        {
            this.Title = title;
        }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        protected List<Comment> Comments => comments;

        public void AddComment(string comment)
        {
            Comments.Add(new Comment("comment"));
        }
    }
}