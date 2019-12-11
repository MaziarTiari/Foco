using Foco.models;
using Foco.ui;
using Foco.windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Foco.pages
{
    /// <summary>
    /// Interaktionslogik für ListPage.xaml
    /// </summary>
    public partial class ListPage : Page
    {
        private readonly MainWindow mainWindow;
        private Project project;

        public ListPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            Update();
        }
        
        public Project Project { get => project; set { project = value; Update(); } }
        public MainWindow MainWindow => mainWindow;
        public int IndexOfLastTaskgroupControl => TaskgroupContainer.Children.Count - 1;

        public void RequestNewTaskgroupByListPage()
        {
            InputWindow inputWindow = new InputWindow("Aufgabengruppe erstellen:", "Name:", "", WindowCallback, false);
            inputWindow.ShowDialog();
        }

        private void WindowCallback(InputState inputState, string title)
        {
            if (inputState == InputState.Save)
            {
                CreateTaskgroup(title);
            }
        }

        private void CreateTaskgroup(string title)
        {
            if (!this.Project.Taskgroups.Exists(x => x.Title == title))
            {
                Taskgroup taskgroup = new Taskgroup(title);
                this.project.Taskgroups.Add(taskgroup);
                TaskgroupContainer.Children.Insert( IndexOfLastTaskgroupControl, new TaskgroupControl(taskgroup, this) );
            }
            else
            {
                MessageBox.Show("Dieser Titel existiert bereits in diesem Projekt!");
            }
        }

        public void Update()
        {
            TaskgroupContainer.Children.Clear();
            if (project != null && project.Taskgroups.Count > 0)
            {
                foreach (Taskgroup taskgroup in project.Taskgroups)
                {
                    TaskgroupControl taskgroupControl = new TaskgroupControl(taskgroup, this);
                    TaskgroupContainer.Children.Add(taskgroupControl);
                }
            }
            TaskgroupContainer.Children.Add(new TaskgroupCreateLabelControl(this));
        }
    }
}
