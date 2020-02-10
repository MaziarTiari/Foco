using Foco.models;
using Foco.controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Foco.pages
{

    // interaction logic for HomePage.xaml
    public partial class HomePage : Page
    {

        private readonly MainWindow mainWindow;
        public MainWindow MainWindow => mainWindow;
        public List<Goal> Goals => mainWindow.Goals;

        public HomePage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            Update();
        }

        public void Update()
        {
            HomeStackpanel.Children.Clear();
            foreach (Goal goal in mainWindow.Goals)
            {
                GoalControl goalControl = new GoalControl(this, goal);
                HomeStackpanel.Children.Add(goalControl);
            }
        }

        private void OnAddGoalClicked(object sender, RoutedEventArgs e)
        {
            string title = "Neues Ziel";
            int i = 1;
            while (Goals.Exists(x => x.Title == title))
                title = title.Split(' ')[0] + " " + title.Split(' ')[1] + " " + (++i);
            Goal goal = new Goal(title);
            GoalControl goalControl = new GoalControl(this, goal);
            HomeStackpanel.Children.Add(goalControl);
            Goals.Add(goal);
            goalControl.NameLabel.BeginEditing();
            HomeScrollViewer.ScrollToBottom();
        }

    }
 }

