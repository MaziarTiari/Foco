using System.Collections.Generic;

namespace testFoco.models
{
    public class Task
    {
        private string title;
        private string description;
        private bool done = false;
        private List<Attachment> attachments = new List<Attachment>();
        public Task(string title)
        {
            this.Title = title;
        }

        public string Description { get => description; set => description = value; }
        public bool Done { get => done; set => done = value; }
        public string Title { get => title; set => title = value; }

    }
}