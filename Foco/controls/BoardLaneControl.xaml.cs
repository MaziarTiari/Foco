using Foco.models;
using System.Windows.Controls;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für BoardLaneControl.xaml
    /// </summary>
    public partial class BoardLaneControl : UserControl
    {

        private Project project;
        private State state;

        public Project Project { get => project; set { project = value; Update(); } }
        public State State { get => state; set { state = value; Update(); } }

        public BoardLaneControl(State state)
        {
            InitializeComponent();
            this.state = state;
            Update();
        }

        private void Update()
        {
            TaskgroupStack.Children.Clear();
            switch (state)
            {
                case State.Todo: TitleText.Text = "Todo"; break;
                case State.InProgress: TitleText.Text = "In Bearbeitung"; break;
                case State.Done: TitleText.Text = "Fertig"; break;
            }
            if (project != null)
            {
                foreach (Taskgroup taskgroup in project.Taskgroups)
                {
                    if (taskgroup.State == state)
                    {
                        TaskgroupStack.Children.Add(new TaskgroupControl(taskgroup));
                    }
                }
            }
        }

    }
}
