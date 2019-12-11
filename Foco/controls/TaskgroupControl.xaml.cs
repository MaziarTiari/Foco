using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private Taskgroup taskgroup;

        public TaskgroupControl(Taskgroup taskgroup, ListPage listPage)
        {
            InitializeComponent();
            this.listPage = listPage;
            this.Taskgroup = taskgroup;
            TaskgroupHeader.Content = this.taskgroup.Title;
            LoadTaskControls();
        }

        public Taskgroup Taskgroup { get => taskgroup; set => taskgroup = value; }

        public ListPage ListPage => listPage;

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
            this.Height =+ taskControl.Height;
            Taskgroup.Tasks.Add(task);
        }

        /**
         * Lösche TaskgroupControl
         */
        public void DeleteTaskgroupControl(object sender, MouseButtonEventArgs e)
        {
            ListPage.TaskgroupContainer.Children.Remove(this);
            ListPage.Project.Taskgroups.Remove(this.Taskgroup);
        }
    }
}
