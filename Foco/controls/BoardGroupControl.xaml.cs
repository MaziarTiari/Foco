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
            EditButton.Visibility = Visibility.Hidden;
            NameLabel.Content = taskgroup.Title;
            switch (taskgroup.Prio)
            {
                case Priority.High:
                    PriorityBorder.Background = new SolidColorBrush(Colors.Red);
                    PriorityLabel.Content = "Hoch";
                    break;
                case Priority.Mid:
                    PriorityBorder.Background = new SolidColorBrush(Colors.Orange);
                    PriorityLabel.Content = "Normal";
                    break;
                case Priority.Low:
                    PriorityBorder.Background = new SolidColorBrush(Colors.Green);
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
            EditButton.Visibility = Visibility.Visible;
        }

        // Benutzer endet Hover
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Hidden;
        }

        // Benutzer drückt auf löschen
        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow("Aufgabengruppe löschen", "Sind sie sicher, dass sie die Aufgabengruppe \"" + taskgroup.Title + "\" inkl. aller Aufgaben löschen möchten?", ConfirmDeleteCallback);
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

        // Benutzer hat auf Editieren geklickt
        private void OnEditClicked(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        // Benutzer machte Doppelklick auf das Control
        private void OnControlDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            boardPage.MainWindow.ShowTaskgroup(taskgroup);
        }

    }
}
