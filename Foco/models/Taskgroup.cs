using System;
using System.Collections.Generic;

public enum Priority { low, mid, hight }
public enum State { Todo, InProgress, Done }
namespace testFoco.models
{
    public class Taskgroup
    {
        private string title;
        private Priority prio;
        private State state = State.Todo;
        private List<Task> tasks = new List<Task>();
        private List<Attachment> attachments = new List<Attachment>();

        public Taskgroup(string title)
        {
            this.title = title;
        }
        public Priority Prio { get => prio; set => prio = value; }
        public State State { get => state; set => state = value; }
        public List<Task> Tasks => tasks;
        public void AddTask(Task task)
        {
            tasks.Add(task);
        }
        public Task GetTaskByName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("message", nameof(name));
            if (tasks.Count == 0) return null;
            return tasks.Find(x => x.Title == name);
        }
    }
}