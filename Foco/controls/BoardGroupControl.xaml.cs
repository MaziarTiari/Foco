using Foco.models;
using Foco.windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für BoardGroupControl.xaml
    /// </summary>
    public partial class BoardGroupControl : UserControl
    {

        private readonly Taskgroup taskgroup;
        private readonly BoardPage boardPage;

        public Taskgroup Taskgroup { get => taskgroup; }

        public BoardGroupControl(BoardPage boardPage, Taskgroup taskgroup)
        {
            if (taskgroup == null || boardPage == null)
                throw new ArgumentNullException();
            InitializeComponent();
            this.taskgroup = taskgroup;
            this.boardPage = boardPage;
            Update();
        }

        private void Update()
        {
            DeleteButton.Visibility = Visibility.Hidden;
            InfoButton.Visibility = Visibility.Hidden;
            NameLabel.Text = taskgroup.Title;
            switch (taskgroup.Prio)
            {
                case Priority.High:
                    PriorityBorder.Background = new SolidColorBrush(Color.FromRgb(210,93,93));
                    PriorityLabel.Content = "Hoch";
                    break;
                case Priority.Mid:
                    PriorityBorder.Background = new SolidColorBrush(Colors.Orange);
                    PriorityLabel.Content = "Normal";
                    break;
                case Priority.Low:
                    PriorityBorder.Background = new SolidColorBrush(Color.FromRgb(96, 212, 134));
                    PriorityLabel.Content = "Niedrig";
                    break;
            }
            if (taskgroup.Deadline == DateTime.MinValue)
            {
                DeadlineBorder.Visibility = Visibility.Hidden;
            }
            else
            {
                DeadlineBorder.Visibility = Visibility.Visible;
                DeadlineLabel.Content = taskgroup.Deadline.ToString("dd.MM.yyyy");
            }
            int countAll = 0, countDone = 0;
            foreach (Task task in taskgroup.Tasks)
            {
                countAll++;
                countDone += task.Done ? 1 : 0;
            }
            TasksLabel.Content = countDone + " / " + countAll + " Aufgaben";
        }

        // Benutzer beginnt Hover
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Visible;
            InfoButton.Visibility = Visibility.Visible;
        }

        // Benutzer endet Hover
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Hidden;
            InfoButton.Visibility = Visibility.Hidden;
        }

        // Benutzer drückt auf löschen
        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow("Aufgabengruppe löschen", "Sind Sie sicher, dass Sie die Aufgabengruppe \"" + taskgroup.Title + "\" inkl. aller Aufgaben löschen möchten?", ConfirmDeleteCallback);
            confirmWindow.ShowDialog();
        }

        // Benutzer hat Löschen bestätigt oder abgebrochen
        private void ConfirmDeleteCallback(ConfirmState confirmState)
        {
            if (confirmState == ConfirmState.YES)
            {
                boardPage.Project.Taskgroups.Remove(taskgroup);
                boardPage.Update();
            }
        }

        // Benutzer hat auf Info geklickt
        private void OnInfoClicked(object sender, RoutedEventArgs e)
        {
            boardPage.MainWindow.ShowTaskgroup(taskgroup);
        }

        // Benutzer hat Label bearbeitet
        #pragma warning disable IDE0051 // wird eigentlich durch XAML aufgerufen
        private void OnLabelEdited(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                taskgroup.Title = text;
            else
                NameLabel.Text = taskgroup.Title;
        }
        #pragma warning restore IDE0051 

    }
}
