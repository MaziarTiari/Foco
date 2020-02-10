using Foco.models;
using Foco.windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Foco.controls
{
    // Interaktionslogik for BoardGroupControl.xaml
    public partial class BoardGroupControl : UserControl
    {
        private readonly Taskgroup taskgroup;
        private readonly BoardPage boardPage;

        public BoardGroupControl(BoardPage boardPage, Taskgroup taskgroup)
        {
            if (taskgroup == null || boardPage == null)
                throw new ArgumentNullException();
            InitializeComponent();
            this.taskgroup = taskgroup;
            this.boardPage = boardPage;
            Update();
        }

        public Taskgroup Taskgroup { get => taskgroup; }

        private void Update()
        {
            DeleteButton.Visibility = Visibility.Hidden;
            InfoButton.Visibility = Visibility.Hidden;
            NameLabel.Text = taskgroup.Title;
            switch (taskgroup.Prio)
            {
                case Priority.High:
                    PriorityBorder.Background = new SolidColorBrush(
                        Color.FromRgb(210,93,93)
                        );
                    PriorityLabel.Content = "Hoch";
                    break;
                case Priority.Mid:
                    PriorityBorder.Background = new SolidColorBrush(Colors.Orange);
                    PriorityLabel.Content = "Normal";
                    break;
                case Priority.Low:
                    PriorityBorder.Background = new SolidColorBrush(
                        Color.FromRgb(96, 212, 134)
                        );
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

        private void MouseEnteredBoard(object sender, MouseEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Visible;
            InfoButton.Visibility = Visibility.Visible;
        }

        private void MouseLeavedBoard(object sender, MouseEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Hidden;
            InfoButton.Visibility = Visibility.Hidden;
        }

        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow(
                "Aufgabengruppe löschen",
                "Sind Sie sicher, dass Sie die Aufgabengruppe \"" +
                taskgroup.Title + "\" inkl. aller Aufgaben löschen möchten?",
                ConfirmDeleteCallback
                );
            confirmWindow.ShowDialog();
        }

        private void ConfirmDeleteCallback(ConfirmState confirmState)
        {
            if (confirmState == ConfirmState.YES)
            {
                boardPage.Project.Taskgroups.Remove(taskgroup);
                boardPage.Update();
            }
        }

        private void OnInfoClicked(object sender, RoutedEventArgs e)
        {
            boardPage.MainWindow.ShowTaskgroup(taskgroup);
        }

        #pragma warning disable IDE0051 // called by XAML
        private void EditedTitleHandler(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                taskgroup.Title = text;
            else
                NameLabel.Text = taskgroup.Title;
        }
        #pragma warning restore IDE0051 
    }
}
