using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Foco.models;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {

        private readonly TaskgroupControl taskgroupControl;
        private readonly Task task;
        private bool isHighlighted;

        public bool Highlighted { get => isHighlighted; set { isHighlighted = value; Update(); } }
        public Task Task => task;

        public TaskControl(Task task, TaskgroupControl taskgroupControl)
        {
            InitializeComponent();
            this.taskgroupControl = taskgroupControl;
            this.task = task;
            DeleteButton.Visibility = Visibility.Hidden;
            Update();
        }

        public void Update()
        {
            TaskCheckBox.IsChecked = task.Done;
            EditableTaskLabel.Text = task.Title;
            AttachmentInfoText.Text = task.Attachments.Count > 0 ? task.Attachments.Count + "📎 " : null;
            ControlContainer.Background = isHighlighted ? new SolidColorBrush(Color.FromArgb(50, 0, 0, 0)) : null;
        }

        // Benutzer drückt auf Löschen
        public void DeleteTaskMouseEvent(object sender, RoutedEventArgs e)
        {
            DeleteTask();
        }

        // Benutzer schließt Editieren ab
        private void OnLabelEdited(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                task.Title = text;
            else
                DeleteTask();
        }

        private void DeleteTask()
        {
            taskgroupControl.TaskContainer.Children.Remove(this);
            taskgroupControl.Taskgroup.Tasks.Remove(task);
            taskgroupControl.OnTaskDeleted(task);
        }

        private void OnCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            task.Done = TaskCheckBox.IsChecked == true;
        }

        private void OnControlClicked(object sender, MouseButtonEventArgs e)
        {
            taskgroupControl.OnTaskClicked(task);
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Visible;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Hidden;
        }
    }
}