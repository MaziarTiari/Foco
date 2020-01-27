using System;
using System.Collections.Generic;

namespace Foco.models
{
    public class Task
    {

        private string title;
        private string description;
        private bool done;
        private List<Attachment> attachments;

        public Task(string title)
        {
            Title = title;
            Done = false;
            Description = "Beschreibung " + title;
            Attachments = new List<Attachment>();
        }

        public string Description { get => description; set => description = value; }
        public bool Done { get => done; set => done = value; }
        public string Title { get => title; set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(); title = value; } }
        public List<Attachment> Attachments { get => attachments; set { if (value == null) throw new ArgumentNullException(); attachments = value; } }

    }
}