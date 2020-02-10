using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Foco.models;
using Foco.pages;
using Foco.windows;
using Task = Foco.models.Task;

namespace Foco.controls
{
    /// interaction logic for TaskgroupControl.xaml
    public partial class TaskgroupControl : UserControl
    {
        private readonly ListPage listPage;
        private readonly TaskgroupPage taskgroupPage;
        private Taskgroup taskgroup;

        private TaskgroupControl(Taskgroup taskgroup)
        {
            InitializeComponent();
            Taskgroup = taskgroup;
        }

        public TaskgroupControl(Taskgroup taskgroup,
                                ListPage listPage) : this(taskgroup)
        {
            this.listPage = listPage;
        }

        public TaskgroupControl(Taskgroup taskgroup,
                                TaskgroupPage taskgroupPage) : this(taskgroup)
        {
            this.taskgroupPage = taskgroupPage;
            InfoButton.Visibility = Visibility.Collapsed;
            DeleteButton.Visibility = Visibility.Collapsed;
        }

        public Taskgroup Taskgroup
        {
            get => taskgroup;
            set
            {
                if (value == null)
                    value = new Taskgroup("Dummy");
                taskgroup = value;
                Update();
            }
        }
        public TaskControl HighlightedTask
        {
            get
            {
                foreach (TaskControl taskControl in TaskContainer.Children)
                    if (taskControl.Highlighted) return taskControl;
                return null;
            }
        }
        public ListPage ListPage => listPage;
        public TaskgroupPage TaskgroupPage => taskgroupPage;

        private void Update()
        {
            TaskgroupHeader.Text = taskgroup.Title;
            PrioCombo.SelectedIndex = (int)taskgroup.Prio;
            StateCombo.SelectedIndex = (int)taskgroup.State;
            if (taskgroup.Deadline == DateTime.MinValue)
                DeadlinePicker.SelectedDate = null;
            else
                DeadlinePicker.SelectedDate = taskgroup.Deadline;
            TaskContainer.Children.Clear();
            LoadTaskControls();
            if (Taskgroup.Tasks.Count == 1)
                OnTaskClicked(Taskgroup.Tasks[0]);
        }

        #pragma warning restore IDE0051
        private void LoadTaskControls()
        {
            if (Taskgroup.Tasks.Count > 0)
            {
                foreach (Task task in Taskgroup.Tasks)
                {
                    TaskControl taskControl = new TaskControl(task, this);
                    TaskContainer.Children.Add(taskControl);
                }
            }
        }

        #pragma warning disable IDE0051 // called by XAML
        private void OnLabelEdited(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                taskgroup.Title = text;
            else
                TaskgroupHeader.Text = taskgroup.Title;
        }


        private void OnTaskCreateEditorKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(TaskCreateEditor.Text))
            {
                if (!(Keyboard.IsKeyDown(Key.LeftShift)
                        || Keyboard.IsKeyDown(Key.RightShift)))
                {
                    Task task = new Task(TaskCreateEditor.Text);
                    TaskControl taskControl = new TaskControl(task, this);
                    TaskContainer.Children.Add(taskControl);
                    TaskContainerScroll.ScrollToBottom();
                    var tasks = Taskgroup.Tasks as List<Task>;
                    tasks.Add(task);
                    if (tasks.Count == 1)
                        OnTaskClicked(task);
                    TaskCreateEditor.Text = null;
                    e.Handled = true;
                }
            }
        }

        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow(
                    "Aufgabengruppe löschen",
                    "Sind Sie sicher, dass Sie die Aufgabengruppe \"" +
                    taskgroup.Title + "\" inkl. aller Aufgaben löschen möchten?",
                    ConfirmedDeleteCallback
                );
            confirmWindow.ShowDialog();
        }

        private void ConfirmedDeleteCallback(ConfirmState confirmState)
        {
            if (confirmState == ConfirmState.YES)
            {
                if (ListPage != null)
                {
                    ListPage.TaskgroupContainer.Children.Remove(this);
                    ListPage.Project.Taskgroups.Remove(this.Taskgroup);
                }
            }
        }

        private void StateComboChanged(object sender, SelectionChangedEventArgs e)
        {
            taskgroup.State = (State)StateCombo.SelectedIndex;
            if(ListPage != null)
                ListPage.Update();
        }

        private void PriorityChanged(object sender, SelectionChangedEventArgs e)
        {
            taskgroup.Prio = (Priority)PrioCombo.SelectedIndex;
        }

        private void DeadlinePickerChanged(object sender, SelectionChangedEventArgs e)
        {
            taskgroup.Deadline = DeadlinePicker.SelectedDate == null
                                 ? DateTime.MinValue
                                 : (DateTime)DeadlinePicker.SelectedDate;
        }

        public void OnTaskClicked(Task task)
        {
            if (taskgroupPage != null)
            {
                taskgroupPage.CurrentTask = task;
                foreach (TaskControl taskControl in TaskContainer.Children)
                    taskControl.Highlighted = (taskControl.Task == task);
            }
        }

        // task (already) deleted
        public void OnTaskDeleted(Task task)
        {
            if (taskgroupPage != null && taskgroupPage.CurrentTask == task)
                taskgroupPage.CurrentTask = null;
        }

        private void OnInfoClicked(object sender, RoutedEventArgs e)
        {
            if (ListPage != null)
                ListPage.MainWindow.ShowTaskgroup(taskgroup);
        }

    }
}
