using Foco.models;
using Foco.ui;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Foco.pages
{

    /// <summary>
    /// Interaktionslogik für HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {

        private readonly MainWindow mainWindow;
        public MainWindow MainWindow => mainWindow;
        public List<Goal> Goals => mainWindow.Goals;

        public HomePage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        public void Update()
        {
            HomeStackpanel.Children.Clear();
            foreach (Goal goal in mainWindow.Goals)
            {
                GoalControl goalControl = new GoalControl(this, goal);
                HomeStackpanel.Children.Add(goalControl);
            }
            HomeStackpanel.Children.Add(new GoalControl(this));
        }
    }
 }

