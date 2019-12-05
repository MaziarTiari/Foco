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

        // eigentlich unnötig, da man direkt auf die Liste zugreifen darf?
        public void AddProject(Project project)
        {
            if (projects.Exists(x => x.Name == project.Name)) throw new DuplicateWaitObjectException();
            else projects.Add(project);
        }

        public Project GetProjectByName(string name)
        {
            if (projects.Count == 0) return null;
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("message", nameof(name));
            return projects.Find(x => x.Name == name);
        }

    }
}