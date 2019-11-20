using System;
using System.Collections.Generic;
namespace testFoco.models
{
    public class Goal
    {
        private string title;
        private List<Project> projects = new List<Project>();
        public Goal(string title)
        {
            this.Title = title;
        }

        public string Title { get => title; set => title = value; }
        public List<Project> Projects => projects;
        public void AddProject(Project project)
        {
            if (project.Name == null || project.Name == " ") throw new ArgumentNullException();
            else if (projects.Exists(x => x.Name == project.Name)) throw new DuplicateWaitObjectException();
            else projects.Add(project);
        }
        public Project GetProjectByName(string name)
        {
            if(projects.Count == 0) return null;
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("message", nameof(name));
            return projects.Find(x => x.Name == name);
        }
    }
}