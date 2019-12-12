using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Foco.models;

namespace Foco.ui
{
    /// <summary>
    /// Interaktionslogik für TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        private readonly TaskgroupControl taskgroupControl;
        private readonly Task task;

        public TaskControl(Task task, TaskgroupControl taskgroupControl)
        {
            InitializeComponent();
            this.taskgroupControl = taskgroupControl;
            this.task = task;
            Update();
        }

        public void Update()
        {
            TaskCheckBox.IsChecked = task.Done;
            EditableTaskLabel.Text = task.Title;
            AttachmentInfoText.Text = task.Attachments.Count > 0 ? task.Attachments.Count + "📎 " : null;
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
        }

        private void OnCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            task.Done = TaskCheckBox.IsChecked == true;
        }

        private void OnControlClicked(object sender, MouseButtonEventArgs e)
        {
            taskgroupControl.OnTaskFocused(task);
        }
    }
}