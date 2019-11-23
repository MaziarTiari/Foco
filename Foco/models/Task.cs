using System;
using System.Collections.Generic;

namespace Foco.models
{
    public class Task
    {
        
        private string title;
        private string description;
        private bool done = false;
        private List<Attachment> attachments = new List<Attachment>();
        
        public Task(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException();
            this.Title = title;
            this.Done = false;
        }

        public string Description { get => description; set => description = value; }
        public bool Done { get => done; set => done = value; }
        public string Title { get => title; set => title = value; }
        public List<Attachment> Attachments { get => attachments; set => attachments = value; }

    }
}