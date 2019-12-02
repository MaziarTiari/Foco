using Foco.controls;
using Foco.models;
using System;
using System.Windows;
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

        public BoardPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            int column = 0;
            foreach (State state in Enum.GetValues(typeof(State)))
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                BoardGrid.ColumnDefinitions.Add(columnDefinition);
                BoardLaneControl boardLaneControl = new BoardLaneControl(state);
                BoardGrid.Children.Add(boardLaneControl);
                Grid.SetColumn(boardLaneControl, column++);
            }
        }

        private void Update()
        {
            foreach (BoardLaneControl boardLaneControl in BoardGrid.Children)
            {
                boardLaneControl.Project = project;
            }
        }



    }
}
