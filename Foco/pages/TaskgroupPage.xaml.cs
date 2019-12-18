using Foco.controls;
using Foco.models;
using Foco.ui;
using System.Windows.Controls;

namespace Foco.pages
{
    /// <summary>
    /// Interaktionslogik für TaskgroupPage.xaml
    /// </summary>
    public partial class TaskgroupPage : Page
    {

        private Taskgroup taskgroup;
        private Task currentTask;
        private readonly TaskgroupControl taskgroupControl;
        private readonly TaskDetailsControl taskDetailsControl;

        public Taskgroup Taskgroup { get => taskgroup; set { taskgroup = value; UpdateTaskgroup(); } }
        public Task CurrentTask { get => currentTask; set { currentTask = value; ShowTaskDetails(); } }

        public TaskgroupPage(Taskgroup taskgroup)
        {
            InitializeComponent();
            this.taskgroup = taskgroup;
            taskgroupControl = new TaskgroupControl(taskgroup, this);
            taskDetailsControl = new TaskDetailsControl(null);
            ContentGrid.Children.Add(taskgroupControl);
            ContentGrid.Children.Add(taskDetailsControl);
            Grid.SetColumn(taskgroupControl, 0);
            Grid.SetColumn(taskDetailsControl, 1);
            Grid.SetRow(taskgroupControl, 0);
            Grid.SetRow(taskDetailsControl, 0);
        }

        private void UpdateTaskgroup()
        {
            taskgroupControl.Taskgroup = taskgroup;
            taskgroupControl.OnTaskClicked(taskgroup.Tasks.Count > 0 ? taskgroup.Tasks[0] : null);
        }

        private void ShowTaskDetails()
        {
            taskDetailsControl.Task = currentTask;
        }

    }
}
