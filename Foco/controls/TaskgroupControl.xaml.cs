using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Foco.models;
using Foco.pages;
using Task = Foco.models.Task;

namespace Foco.ui
{
    /// <summary>
    /// Interaktionslogik für TaskgroupControl.xaml
    /// </summary>
    public partial class TaskgroupControl : UserControl
    {
        private readonly ListPage listPage;
        private readonly TaskgroupPage taskgroupPage;
        private Taskgroup taskgroup;

        public Taskgroup Taskgroup { get => taskgroup; set { if (value == null) value = new Taskgroup("Dummy"); taskgroup = value; Update(); } }
        public ListPage ListPage => listPage;
        public TaskgroupPage TaskgroupPage => taskgroupPage;

        private TaskgroupControl(Taskgroup taskgroup)
        {
            InitializeComponent();
            Taskgroup = taskgroup;
        }

        private void Update()
        {
            TaskgroupHeader.Content = taskgroup.Title;
            PrioCombo.SelectedIndex = (int)taskgroup.Prio;
            StateCombo.SelectedIndex = (int)taskgroup.State;
            if (taskgroup.Deadline == DateTime.MinValue)
                DeadlinePicker.SelectedDate = null;
            else
                DeadlinePicker.SelectedDate = taskgroup.Deadline;
            TaskContainer.Children.Clear();
            LoadTaskControls();
        }

        public TaskgroupControl(Taskgroup taskgroup, ListPage listPage) : this(taskgroup)
        {
            this.listPage = listPage;
        }

        public TaskgroupControl(Taskgroup taskgroup, TaskgroupPage taskgroupPage) : this(taskgroup)
        {
            this.taskgroupPage = taskgroupPage;
        }

        /**
         * Alle verfügbare Tasks sollen in die TaskgroupControl geladen werden
         */
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

        /**
         * Bie Tätigung der Enter-Taste mit gedrückter Schift-Taste wird eine neue Zeile erstellt,
         * ohne die Schift-Taste gedrückt bedeutet Enter: fertig mit schreiben -> neue Task erstellen
         */
        private void TaskEditorPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                    {
                        CreateNewTask(TaskCreateEditor.Text);
                        TaskCreateEditor.Text = null;
                        e.Handled = true;
                    }
                    break;
            }
        }

        /**
         * Eine neue Task erstellen
         */
        public void CreateNewTask(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return;
            Task task = new Task(title);
            TaskControl taskControl = new TaskControl(task, this);
            TaskContainer.Children.Add(taskControl);
            Taskgroup.Tasks.Add(task);
        }

        /**
         * Lösche TaskgroupControl
         */
        public void DeleteTaskgroupControl(object sender, RoutedEventArgs e)
        {
            if (ListPage != null)
            {
                ListPage.TaskgroupContainer.Children.Remove(this);
                ListPage.Project.Taskgroups.Remove(this.Taskgroup);
            }
            else if (TaskgroupPage != null)
            {
                // TODO
            }

        }

        // Benutzer klickte auf Control
        private void OnTaskgroupClicked(object sender, MouseButtonEventArgs e)
        {
            if (ListPage != null)
            {
                ListPage.MainWindow.ShowTaskgroup(taskgroup);
            }
        }

        private void OnStatusComboChanged(object sender, SelectionChangedEventArgs e)
        {
            taskgroup.State = (State)StateCombo.SelectedIndex;
        }

        private void OnPrioComboChanged(object sender, SelectionChangedEventArgs e)
        {
            taskgroup.Prio = (Priority)PrioCombo.SelectedIndex;
        }

        private void OnDeadlinePickerChanged(object sender, SelectionChangedEventArgs e)
        {
            taskgroup.Deadline = DeadlinePicker.SelectedDate == null ? DateTime.MinValue : (DateTime)DeadlinePicker.SelectedDate;
        }

        public void OnTaskFocused(Task task)
        {
            if (taskgroupPage != null)
                taskgroupPage.CurrentTask = task;
        }

    }
}
