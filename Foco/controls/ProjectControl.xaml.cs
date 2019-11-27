using Foco.models;
using Foco.windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Foco.ui
{
    /// <summary>
    /// Interaktionslogik für ProjectControl.xaml
    /// </summary>
    public partial class ProjectControl : UserControl
    {

        private readonly Project project;
        private readonly GoalControl goalControl;

        public ProjectControl(GoalControl goalControl)
        {
            this.goalControl = goalControl;
            InitializeComponent();
            Update();
        }

        public ProjectControl(GoalControl goalControl, Project project) : this(goalControl)
        {
            this.project = project;
            Update();
        }

        private void Update()
        {
            if (project != null)
            {
                NameLabel.Content = project.Name;
                ProjectBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(project.Color));
                int countAll = 0, countDone = 0;
                foreach (Taskgroup taskgroup in project.Taskgroups)
                {
                    foreach (Task task in taskgroup.Tasks)
                    {
                        countAll++;
                        countDone += task.Done ? 1 : 0;
                    }
                }
                TasksLabel.Content = countDone + " / " + countAll + " Aufgaben";
                DeleteButton.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Visible;
            }
            else
            {
                NameLabel.Content = "Hinzufügen";
                ProjectBorder.Background = new SolidColorBrush(Colors.White);
                TasksLabel.Content = "";
                DeleteButton.Visibility = Visibility.Hidden;
                EditButton.Visibility = Visibility.Hidden;
            }
        }

        // Benutzer hat auf Editieren geklickt
        private void OnEditClicked(object sender, RoutedEventArgs e)
        {
            ProjectEditWindow projectEditWindow = new ProjectEditWindow("Projekt bearbeiten", project.Name, project.Color, EditedCallback);
            projectEditWindow.ShowDialog();
        }


        // Benutzer hat beim Editieren gespeichert
        public void EditedCallback(string projectName, string projectColor)
        {
            project.Name = projectName;
            project.Color = projectColor;
            Update();
        }

        // Benutzer hat auf Löschen geklickt
        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow("Projekt löschen", "Sind sie sicher, dass sie das Projekt \"" + project.Name + "\" inkl. aller Aufgaben löschen möchten?", ConfirmDeleteCallback);
            confirmWindow.ShowDialog();
        }


        // Benutzer hat Löschen bestätigt oder abgebrochen
        private void ConfirmDeleteCallback(ConfirmState confirmState)
        {
            if (confirmState == ConfirmState.YES)
            {
                goalControl.Goal.Projects.Remove(project);
                goalControl.Update();
            }
        }

        // Benutzer hat auf Control geklickt
        private void OnClicked(object sender, MouseButtonEventArgs e)
        {
            if (project == null)
            {
                ProjectEditWindow projectEditWindow = new ProjectEditWindow("Projekt erstellen", "", "white", CreatedCallback);
                projectEditWindow.ShowDialog();
            }
            else
            {
                // TODO Projekt in View zeigen
            }
        }

        // Benutzer hat beim Erstellen gespeichert
        public void CreatedCallback(string projectName, string projectColor)
        {
            Project project = new Project(projectName, projectColor);
            goalControl.Goal.Projects.Add(project);
            goalControl.Update();
        }

    }
}
