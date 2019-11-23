using System;
using System.Collections.Generic;
namespace Foco.models
{
    public class Project
    {
        
        private string name;
        private List<Taskgroup> taskgroups = new List<Taskgroup>();
        private string color;

        public Project(string name, string color)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(color))
                throw new ArgumentNullException();
            this.Name = name;
            this.color = color;
        }

        public string Name { get => name; set => name = value; }
        public string Color { get => color; set => color = value; }
        public List<Taskgroup> Taskgroups { get => taskgroups; set => taskgroups = value; }

    }
}