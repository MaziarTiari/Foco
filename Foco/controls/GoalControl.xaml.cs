using Foco.models;
using Foco.pages;
using Foco.windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für GoalControl.xaml
    /// </summary>
    public partial class GoalControl : UserControl
    {

        private readonly Goal goal;
        private readonly HomePage homePage;
        public Goal Goal => goal;
        public HomePage HomePage => homePage;

        public GoalControl(HomePage homePage, Goal goal)
        {
            InitializeComponent();
            if (goal == null || homePage == null)
                throw new ArgumentNullException();
            this.goal = goal;
            this.homePage = homePage;
            Update();
        }

        public void Update()
        {
            ProjectWrap.Children.Clear();
            DeleteButton.Visibility = Visibility.Hidden;
            NameLabel.Text = goal.Title;
            foreach (Project project in goal.Projects)
            {
                ProjectControl projectControl = new ProjectControl(this, project);
                ProjectWrap.Children.Add(projectControl);
            }
            ProjectControl addProjectControl = new ProjectControl(this);
            ProjectWrap.Children.Add(addProjectControl);
        }

        // Benutzer hat Label editiert
        #pragma warning disable IDE0051 // wird eigentlich durch XAML aufgerufen
        private void OnLabelEdited(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                goal.Title = text;
            else
                NameLabel.Text = goal.Title;
        }
        #pragma warning restore IDE0051

        // Benutzer hat auf Löschen geklickt
        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow("Ziel löschen", "Sind Sie sicher, dass Sie das Ziel \"" + goal.Title + "\" inkl. aller Projekte löschen möchten?", ConfirmDeleteCallback);
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

        private void OnMouseEnter(object sender, MouseEventArgs e) => DeleteButton.Visibility = Visibility.Visible;
        private void OnMouseLeave(object sender, MouseEventArgs e) => DeleteButton.Visibility = Visibility.Hidden;

    }
}
