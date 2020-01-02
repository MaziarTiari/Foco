using System;
using System.Collections.Generic;
namespace Foco.models
{
    public class Goal
    {
        private string title;
        private List<Project> projects;

        public Goal(string title)
        {
            Projects = new List<Project>();
            Title = title;
        }
        public string Title { get => title; set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(); title = value; } }
        public List<Project> Projects { get => projects; set { if (value == null) throw new ArgumentNullException(); projects = value; } }
    }
}