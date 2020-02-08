﻿using Foco.models;
using Foco.controls;
using Foco.windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System;

namespace Foco.pages
{
    /// <summary>
    /// Interaktionslogik für ListPage.xaml
    /// </summary>
    public partial class ListPage : Page
    {
        
        public readonly State[] stateEnums = (State[])Enum.GetValues(typeof(State));
        private List<State> states = new List<State>();
        private readonly MainWindow mainWindow;
        private Project project;

        public ListPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            states.Add(State.Todo);
            states.Add(State.InProgress);
            states.Add(State.Validation);
            states.Add(State.Blocked);
            Update();
        }

        public Project Project { get => project; set { project = value; Update(); } }
        public MainWindow MainWindow => mainWindow;

        public List<State> States { get => states; set => states = value; }

        // Benutzer klickte auf Hinzufügen
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

        private void StateCheckboxKeyDown(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            string checkboxName = checkbox.Name;

            foreach(State state in stateEnums)
            {
                if (checkboxName == state.ToString())
                {
                    if (checkbox.IsChecked == true)
                    {
                        if (!States.Contains(state))
                            States.Add(state);
                    }
                    else
                    {
                        if (States.Contains(state))
                            States.Remove(state);
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
                    if(states.Contains(taskgroup.State))
                    {
                        TaskgroupControl taskgroupControl = new TaskgroupControl(taskgroup, this);
                        TaskgroupContainer.Children.Add(taskgroupControl);
                    }
                }
            }
        }

    }
}
