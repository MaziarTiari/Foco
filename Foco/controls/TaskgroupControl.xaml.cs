using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Foco.models;
using Foco.pages;
using Foco.windows;
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
            TaskgroupHeader.Text = taskgroup.Title;
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

        private void OnLabelEdited(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                taskgroup.Title = text;
            else
                TaskgroupHeader.Text = taskgroup.Title;
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
                    if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && !string.IsNullOrWhiteSpace(TaskCreateEditor.Text))
                    {
                        Task task = new Task(TaskCreateEditor.Text);
                        TaskControl taskControl = new TaskControl(task, this);
                        TaskContainer.Children.Add(taskControl);
                        TaskContainerScroll.ScrollToBottom();
                        Taskgroup.Tasks.Add(task);
                        TaskCreateEditor.Text = null;
                        e.Handled = true;
                    }
                    break;
            }
        }

        // Benutzer klickte auf Löschen
        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow("Aufgabengruppe löschen", "Sind Sie sicher, dass Sie die Aufgabengruppe \"" + taskgroup.Title + "\" inkl. aller Aufgaben löschen möchten?", ConfirmDeleteCallback);
            confirmWindow.ShowDialog();
        }

        // Benutzer hat Löschen bestätigt
        private void ConfirmDeleteCallback(ConfirmState confirmState)
        {
            if (confirmState == ConfirmState.YES)
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
        }

        // Benutzer ändert Status
        private void OnStatusComboChanged(object sender, SelectionChangedEventArgs e)
        {
            taskgroup.State = (State)StateCombo.SelectedIndex;
        }

        // Benutzer ändert Priorität
        private void OnPrioComboChanged(object sender, SelectionChangedEventArgs e)
        {
            taskgroup.Prio = (Priority)PrioCombo.SelectedIndex;
        }

        // Benutzer ändert Deadline
        private void OnDeadlinePickerChanged(object sender, SelectionChangedEventArgs e)
        {
            taskgroup.Deadline = DeadlinePicker.SelectedDate == null ? DateTime.MinValue : (DateTime)DeadlinePicker.SelectedDate;
        }

        // Benutzer klickt auf Task
        public void OnTaskClicked(Task task)
        {
            if (taskgroupPage != null)
            {
                taskgroupPage.CurrentTask = task;
                foreach (TaskControl taskControl in TaskContainer.Children)
                    taskControl.Highlighted = (taskControl.Task == task);
            }
        }

        // Ein Task wurde (bereits) gelöscht
        public void OnTaskDeleted(Task task)
        {
            if (taskgroupPage != null && taskgroupPage.CurrentTask == task)
                taskgroupPage.CurrentTask = null;
        }

        // Benutzer klickt auf Info
        private void OnInfoClicked(object sender, RoutedEventArgs e)
        {
            if (ListPage != null)
                ListPage.MainWindow.ShowTaskgroup(taskgroup);
        }
    }
}
