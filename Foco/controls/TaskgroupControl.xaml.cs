using Foco.models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für TaskgroupControl.xaml
    /// </summary>
    public partial class TaskgroupControl : UserControl
    {

        private readonly Taskgroup taskgroup;

        public Taskgroup Taskgroup { get => taskgroup; }

        public TaskgroupControl(Taskgroup taskgroup)
        {
            if (taskgroup == null)
                throw new ArgumentNullException();
            InitializeComponent();
            this.taskgroup = taskgroup;
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
                DeadlineLabel.Content = taskgroup.Deadline.ToString("dd.MM.yyyy HH:mm");
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

    }
}
