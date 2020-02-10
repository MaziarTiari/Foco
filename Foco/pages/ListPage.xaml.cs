using Foco.models;
using Foco.controls;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System;

namespace Foco.pages
{
    /// interaction lofigc for ListPage.xaml
    public partial class ListPage : Page
    {
        
        public readonly State[] stateEnums = (State[])Enum.GetValues(typeof(State));
        private List<State> displayedStates = new List<State>();
        private readonly MainWindow mainWindow;
        private Project project;

        public ListPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            setDefaultStatestoDisplay();
            Update();
        }

        public Project Project 
        {
            get => project; 
            set { project = value; Update(); } 
        }
        public MainWindow MainWindow => mainWindow;

        private void setDefaultStatestoDisplay()
        {
            foreach(State state in stateEnums)
            {
                if (state != State.Done)
                    displayedStates.Add(state);
            }
        }

        private void OnAddTaskgroupClicked(object sender, RoutedEventArgs e)
        {
            string title = "Neue Gruppe";
            int i = 1;
            while (Project.Taskgroups.Exists(x => x.Title == title))
                title = title.Split(' ')[0] + " " + title.Split(' ')[1] + " " + (++i);
            Taskgroup taskgroup = new Taskgroup(title);
            project.Taskgroups.Add(taskgroup);
            TaskgroupControl taskgroupControl = new TaskgroupControl(taskgroup, this);
            TaskgroupContainer.Children.Add(taskgroupControl);
            taskgroupControl.TaskgroupHeader.BeginEditing();
            TaskgroupScroll.ScrollToBottom();
        }

        private void OnStateCheckboxKeyDown(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            string checkboxName = checkbox.Name;

            foreach(State state in stateEnums)
            {
                if (checkboxName == state.ToString())
                {
                    if (checkbox.IsChecked == true)
                    {
                        if (! displayedStates.Contains(state))
                            displayedStates.Add(state);
                    }
                    else
                    {
                        if (displayedStates.Contains(state))
                            displayedStates.Remove(state);
                    }
                }
            }
            Update();
        }

        public void Update()
        {
            TaskgroupContainer.Children.Clear();
            if (project != null && project.Taskgroups.Count > 0)
            {
                foreach (Taskgroup taskgroup in project.Taskgroups)
                {
                    if(displayedStates.Contains(taskgroup.State))
                    {
                        TaskgroupControl taskgroupControl = new TaskgroupControl(
                                taskgroup, this
                            );
                        TaskgroupContainer.Children.Add(taskgroupControl);
                    }
                }
            }
        }

    }
}
