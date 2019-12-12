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
            TaskTextBox.Text = task.Title;
            AttachmentInfoText.Text = task.Attachments.Count > 0 ? task.Attachments.Count + "📎 " : null;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            SaveOrDeleteTask();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                case Key.Return:
                    SaveOrDeleteTask();
                    break;
            }
        }

        public void DeleteTaskMouseEvent(object sender, RoutedEventArgs e)
        {
            DeleteTask();
        }

        private void SaveOrDeleteTask()
        {
            if (!string.IsNullOrWhiteSpace(TaskTextBox.Text))
                task.Title = TaskTextBox.Text;
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

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            taskgroupControl.OnTaskFocused(task);
        }

    }
}