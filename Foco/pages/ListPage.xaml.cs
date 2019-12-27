using Foco.models;
using Foco.ui;
using Foco.windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        }

    }
}
