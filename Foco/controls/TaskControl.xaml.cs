using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Foco.models;

namespace Foco.ui
{
    /// <summary>
    /// Interaktionslogik für TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        private readonly TaskgroupControl taskgroupControl;
        private Task task;
        private readonly int index;

        public TaskControl(Task task, TaskgroupControl taskgroupControl)
        {
            InitializeComponent();
            this.taskgroupControl = taskgroupControl;
            this.task = task;
            TaskTextBox.Text = task.Title;
            this.index = taskgroupControl.TaskContainer.Children.Count;
        }

        public TaskgroupControl TaskgroupControl => taskgroupControl;

        public int Index => index;

        public Task Task { get => task; set => task = value; }

        private void TaskControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskTextBox.Text))
                Task.Title = TaskTextBox.Text;
            else
                DeleteTask();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            var key = e.Key;
            switch (key)
            {
                case Key.Return:
                    if (! string.IsNullOrWhiteSpace(TaskTextBox.Text))
                    {
                        Task.Title = TaskTextBox.Text;
                        TaskgroupControl.Taskgroup.Tasks[Index] = Task;
                    }
                    else
                    {
                        DeleteTask();
                    }
                    break;
                case Key.Escape:
                    if (string.IsNullOrWhiteSpace(this.TaskTextBox.Text))
                    {
                        DeleteTask();
                    }
                    break;
            }
        }

        public void DeleteTaskMouseEvent(object sender, MouseButtonEventArgs e)
        {
            DeleteTask();
        }

        private void DeleteTask()
        {
            TaskgroupControl.TaskContainer.Children.Remove(this);
            taskgroupControl.Taskgroup.Tasks.Remove(Task);
        }
    }
}