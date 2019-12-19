using Foco.controls;
using Foco.models;
using System;
using System.Windows.Controls;

namespace Foco
{
    /// <summary>
    /// Interaktionslogik für BoardPage.xaml
    /// </summary>
    public partial class BoardPage : Page
    {

        private readonly MainWindow mainWindow;
        private Project project;

        public Project Project { get => project; set { project = value; Update(); } }
        public MainWindow MainWindow => mainWindow;

        public BoardPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            foreach (State state in Enum.GetValues(typeof(State)))
            {
                BoardLaneControl boardLaneControl = new BoardLaneControl(this, state);
                BoardStack.Children.Add(boardLaneControl);
            }
        }

        public void Update()
        {
            foreach (BoardLaneControl boardLaneControl in BoardStack.Children)
            {
                boardLaneControl.Project = project;
            }
        }



    }
}
