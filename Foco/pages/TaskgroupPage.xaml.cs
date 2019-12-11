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

        public Taskgroup Taskgroup { get => taskgroup; set { taskgroup = value; Update(); } }
        public Task CurrentTask { get => currentTask; set { currentTask = value; ShowTaskDetails(); } }

        public TaskgroupPage(Taskgroup taskgroup)
        {
            InitializeComponent();
            this.taskgroup = taskgroup;
            taskgroupControl = new TaskgroupControl(taskgroup, this);
            taskDetailsControl = new TaskDetailsControl(null);
            ContentStack.Children.Add(taskgroupControl);
            ContentStack.Children.Add(taskDetailsControl);
        }

        private void Update()
        {
            taskgroupControl.Taskgroup = taskgroup;
            taskDetailsControl.Task = null;
        }

        private void ShowTaskDetails()
        {
            taskDetailsControl.Task = currentTask;
        }

    }
}
