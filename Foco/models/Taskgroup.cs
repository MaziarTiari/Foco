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
        private State state;
        private List<Task> tasks;
        private List<Attachment> attachments;
        private DateTime deadline;

        public Taskgroup(string title)
        {
            Title = title;
            State = State.Todo;
            Prio = Priority.Mid;
            Tasks = new List<Task>();
            Attachments = new List<Attachment>();
            Deadline = DateTime.MinValue; // bedeutet keine Deadline
        }

        public Priority Prio { get => prio; set => prio = value; }
        public State State { get => state; set => state = value; }
        public string Title { get => title; set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(); title = value; } }
        public DateTime Deadline { get => deadline; set { if (value == null) throw new ArgumentNullException(); deadline = value; } }
        public List<Task> Tasks { get => tasks; set { if (value == null) throw new ArgumentNullException(); tasks = value; } }
        public List<Attachment> Attachments { get => attachments; set { if (value == null) throw new ArgumentNullException(); attachments = value; } }

        public Task GetTaskByName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("message", nameof(name));
            if (tasks.Count == 0) return null;
            return tasks.Find(x => x.Title == name);
        }

    }
}