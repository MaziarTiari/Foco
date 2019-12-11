using Foco.controls;
using Foco.models;
using System.Windows.Controls;

namespace Foco.pages
{
    /// <summary>
    /// Interaktionslogik für TaskgroupPage.xaml
    /// </summary>
    public partial class TaskgroupPage : Page
    {

        private Taskgroup taskgroup;

        public Taskgroup Taskgroup { get => taskgroup; set { taskgroup = value; Update(); } }

        public TaskgroupPage(Taskgroup taskgroup)
        {
            InitializeComponent();
        }

        private void Update()
        {
            if (taskgroup == null)
                return;
            ContentStack.Children.Clear();
            ContentStack.Children.Add(new AttachmentListControl(taskgroup));
        }

    }
}
