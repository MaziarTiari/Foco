using System;
using System.Collections.Generic;
namespace Foco.models
{
    public class Project
    {

        private string name;
        private List<Taskgroup> taskgroups;
        private string color;

        public Project(string name, string color)
        {
            Taskgroups = new List<Taskgroup>();
            Name = name;
            Color = color;
        }

        public string Name { get => name; set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(); name = value; } }
        public string Color { get => color; set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(); color = value; } }
        public List<Taskgroup> Taskgroups { get => taskgroups; set { if (value == null) throw new ArgumentNullException(); taskgroups = value; } }

    }
}