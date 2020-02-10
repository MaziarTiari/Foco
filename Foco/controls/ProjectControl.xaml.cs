using Foco.models;
using Foco.windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Foco.controls
{
    /// interaction logic for ProjectControl.xaml
    public partial class ProjectControl : UserControl
    {

        private readonly Project project;
        private readonly GoalControl goalControl;

        public Project Project => project;

        public ProjectControl(GoalControl goalControl)
        {
            this.goalControl = goalControl;
            InitializeComponent();
            Update();
        }

        public ProjectControl( GoalControl goalControl,
                               Project project ) : this(goalControl)
        {
            this.project = project;
            Update();
        }

        private void Update()
        {
            DeleteButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Hidden;
            if (project != null)
            {
                NameLabel.Content = project.Name;
                NameLabel.Background = new SolidColorBrush(
                        (Color) ColorConverter.ConvertFromString("#30232A")
                    );
                ProjectBorder.Background = new SolidColorBrush(
                        (Color) ColorConverter.ConvertFromString(project.Color)
                    );
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
            }
            else
            {
                NameLabel.Content = "";
                NameLabel.Background = new SolidColorBrush(Colors.White);
                ProjectBorder.Background = new SolidColorBrush(Colors.White);
                TasksLabel.Content = "+ Projekt hinzufügen";
            }
        }

        private void OnEditClicked(object sender, RoutedEventArgs e)
        {
            ProjectEditWindow projectEditWindow = new ProjectEditWindow(
                    "Projekt bearbeiten", project.Name, project.Color, ConfirmEditedCallback
                );
            projectEditWindow.ShowDialog();
        }


        public void ConfirmEditedCallback(string projectName, string projectColor)
        {
            project.Name = projectName;
            project.Color = projectColor;
            Update();
        }

        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow(
                    "Projekt löschen", "Sind sie sicher, dass sie das Projekt \"" +
                    project.Name + "\" inkl. aller Aufgaben löschen möchten?",
                    ConfirmDeleteCallback
                );
            confirmWindow.ShowDialog();
        }


        private void ConfirmDeleteCallback(ConfirmState confirmState)
        {
            if (confirmState == ConfirmState.YES)
            {
                goalControl.Goal.Projects.Remove(project);
                goalControl.Update();
            }
        }

        private void OnControlClicked(object sender, MouseButtonEventArgs e)
        {
            if (project == null)
            {
                string color;
                if (goalControl.Goal.Projects.Count > 0)
                {
                    int index = goalControl.Goal.Projects.Count - 1;
                    string lastProjectColor = goalControl.Goal.Projects[index].Color;
                    color = FindNextUnusedColor(lastProjectColor);
                }
                else
                {
                    color = ProjectEditWindow.colorStrings[0];
                }
                ProjectEditWindow projectEditWindow = new ProjectEditWindow(
                        "Projekt erstellen", "", color, ConfirmedCreateCallback
                    );
                projectEditWindow.ShowDialog();

            }
            else
            {
                goalControl.HomePage.MainWindow.ShowProject(project);
            }
        }

        private string FindNextUnusedColor(string lastProjectColor)
        {
            string[] colors = ProjectEditWindow.colorStrings;
            for (int i = 0; i < colors.Length; i++)
            {
                if (ProjectEditWindow.colorStrings[i] == lastProjectColor)
                {
                    return colors[++i % colors.Length];
                }
            }
            return colors[0];
        }

        public void ConfirmedCreateCallback(string projectName, string projectColor)
        {
            Project project = new Project(projectName, projectColor);
            goalControl.Goal.Projects.Add(project);
            goalControl.Update();
        }

        private void MouseEnteredControl(object sender, MouseEventArgs e)
        {
            if (project != null)
            {
                DeleteButton.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Visible;
            }
        }

        private void MouesLeavedControl(object sender, MouseEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Hidden;
        }
    }
}