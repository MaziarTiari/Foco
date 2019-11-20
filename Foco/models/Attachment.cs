namespace testFoco.models
{
    public class Attachment
    {
        protected string title;
        protected string description;
        public Attachment(string title)
        {
            this.Title = title;
        }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
    }
}