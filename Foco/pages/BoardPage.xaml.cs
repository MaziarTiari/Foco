using Foco.controls;
using Foco.models;
using System;
using System.Windows.Controls;

namespace Foco
{
    /// interaction logic for BoardPage.xaml
    public partial class BoardPage : Page
    {

        private readonly MainWindow mainWindow;
        private Project project;

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

        public Project Project 
        {
            get => project;
            set { project = value; Update(); }
        }
        public MainWindow MainWindow => mainWindow;

        public void Update()
        {
            foreach (BoardLaneControl boardLaneControl in BoardStack.Children)
            {
                boardLaneControl.Project = project;
            }
        }
    }
}
