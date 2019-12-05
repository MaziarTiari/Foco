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
        public MainWindow MainWindow => mainWindow;
        private Project project;
        TaskgroupControl taskgroupControl;
        private const int maxTaskgroupWidth = 750;
        private int taskgroupWidth;

        public ListPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            Update();
        }
        public Project Project { get => project; set => project = value; }
        public int TaskgroupWidth { get => taskgroupWidth; set => taskgroupWidth = value; }

        public void CreateTaskgroupGui(object sender, RoutedEventArgs e)
        {
            InputWindow inputWindow = new InputWindow("Aufgabengruppe erstellen:", "Name:", "", CreatedCallback, false);
            inputWindow.ShowDialog();
        }

        private void CreatedCallback(InputState inputState, string title)
        {
            if (inputState == InputState.Save)
            {
                int tgCount = project.Taskgroups.Count;
                project.Taskgroups.Add(new Taskgroup(title));
                this.taskgroupControl = new TaskgroupControl(title, this);
                if (tgCount == 2) 
                    this.taskgroupWidth = maxTaskgroupWidth / 2;
                if (tgCount >= 3)
                    this.taskgroupWidth = maxTaskgroupWidth / 3;
                taskgroupControl.Width = this.taskgroupWidth;
                TaskgroupContainer.Children.Add(taskgroupControl);
            }
        }

        public void Update()
        {
            TaskgroupContainer.Children.Clear();
            if (project != null && project.Taskgroups.Count > 0)
            {
                if (project.Taskgroups.Count > 3)
                    this.taskgroupWidth = maxTaskgroupWidth / 3;
                else
                    this.taskgroupWidth = maxTaskgroupWidth / project.Taskgroups.Count;
                foreach (Taskgroup taskgroup in project.Taskgroups)
                {
                    TaskgroupControl taskgroupControl = new TaskgroupControl(taskgroup.Title, this);
                    taskgroupControl.Width = this.taskgroupWidth;
                    TaskgroupContainer.Children.Add(taskgroupControl);
                }
            }
        }
    }
}
