using System;
using System.Collections.Generic;
namespace testFoco.models
{
    public class Project
    {
        private string name;
        private List<Taskgroup> taskgroups;
        private string color;

        public Project(string name, string color)
        {
            this.Name = name;
            this.color = color;
        }

        public string Name { get => name; set => name = value; }
        public string Color { get => color; set => color = value; }
        List<Taskgroup> Taskgroups { get => taskgroups; set => taskgroups = value; }
    }
}