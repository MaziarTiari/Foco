using System;
using System.Collections.Generic;

namespace Foco.models
{

    public enum Priority { Low, Mid, High } // Reihenfolge bitte nicht ändern!
    public enum State { Todo, InProgress, Done } // Reihenfolge bitte nicht ändern!

    public class Taskgroup
    {
        
        private string title;
        private Priority prio;
        private State state = State.Todo;
        private List<Task> tasks = new List<Task>();
        private List<Attachment> attachments = new List<Attachment>();
        private DateTime deadline;

        public Taskgroup(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException();
            this.title = title;
        }
        
        public Priority Prio { get => prio; set => prio = value; }
        public State State { get => state; set => state = value; }
        public string Title { get => title; set => title = value; }
        public DateTime Deadline { get => deadline; set => deadline = value; }
        public List<Task> Tasks { get => tasks; set => tasks = value; }
        public List<Attachment> Attachments { get => attachments; set => attachments = value; }

        public Task GetTaskByName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("message", nameof(name));
            if (tasks.Count == 0) return null;
            return tasks.Find(x => x.Title == name);
        }

    }
}