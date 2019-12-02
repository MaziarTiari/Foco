﻿using Foco.models;
using Foco.pages;
using Foco.windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Foco.ui
{
    /// <summary>
    /// Interaktionslogik für GoalControl.xaml
    /// </summary>
    public partial class GoalControl : UserControl
    {

        private readonly Goal goal;
        private readonly HomePage homePage;
        public Goal Goal { get => goal; }
        public HomePage HomePage { get => homePage; }

        public GoalControl(HomePage homePage)
        {
            this.homePage = homePage;
            InitializeComponent();
            Update();
        }

        public GoalControl(HomePage homePage, Goal goal) : this(homePage)
        {
            this.goal = goal;
            Update();
        }

        public void Update()
        {
            ProjectWrap.Children.Clear();
            EditButton.Visibility = Visibility.Hidden;
            DeleteButton.Visibility = Visibility.Hidden;
            if (goal != null)
            {
                NameLabel.Content = goal.Title;
                foreach (Project project in goal.Projects)
                {
                    ProjectControl projectControl = new ProjectControl(this, project);
                    ProjectWrap.Children.Add(projectControl);
                }
                ProjectControl addProjectControl = new ProjectControl(this);
                ProjectWrap.Children.Add(addProjectControl);
            }
            else
            {
                NameLabel.Content = "Hinzufügen";
            }
        }

        // Benutzer hat auf Editieren geklickt
        private void OnEditClicked(object sender, RoutedEventArgs e)
        {
            GoalEditWindow goalEditWindow = new GoalEditWindow("Ziel bearbeiten", goal.Title, EditedCallback);
            goalEditWindow.ShowDialog();
        }

        // Benutzer hat beim Editieren gespeichert
        private void EditedCallback(string goalName)
        {
            goal.Title = goalName;
            Update();
        }

        // Benutzer hat auf ein Goal geklickt
        private void OnGoalClick(object sender, MouseButtonEventArgs e)
        {
            if (goal == null)
            {
                GoalEditWindow goalEditWindow = new GoalEditWindow("Ziel erstellen", "", CreatedCallback);
                goalEditWindow.ShowDialog();
            }
        }

        // Benutzer hat beim Erstellen gespeichert
        private void CreatedCallback(string goalName)
        {
            Goal goal = new Goal(goalName);
            homePage.Goals.Add(goal);
            homePage.Update();
        }

        // Benutzer hat auf Löschen geklickt
        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow("Ziel löschen", "Sind sie sicher, dass sie das Ziel \"" + goal.Title + "\" inkl. aller Projekte löschen möchten?", ConfirmDeleteCallback);
            confirmWindow.ShowDialog();
        }

        // Benutzer hat Löschen bestätigt
        private void ConfirmDeleteCallback(ConfirmState confirmState)
        {
            if (confirmState == ConfirmState.YES)
            {
                homePage.Goals.Remove(goal);
                homePage.Update();
            }
        }

        // Benutzer beginnt Hover
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (goal != null)
            {
                DeleteButton.Visibility = Visibility.Visible;
                EditButton.Visibility = Visibility.Visible;
            }
        }

        // Benutzer endet Hover
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Hidden;
        }

    }
}
