using Foco.models;
using Foco.pages;
using Foco.sqlite;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Foco
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private HomePage homePage;
        private BoardPage boardPage;
        private ListPage listPage;
        private CalendarPage calendarPage;
        private TaskgroupPage taskgroupPage;
        private List<Goal> goals;
        private DatabaseManager databaseManager;

        public List<Goal> Goals => goals;

        // wird aufgerufen, wenn das MainWindow erstellt wird
        public MainWindow()
        {
            InitializeComponent();

            // TODO Pfad später evtl. in Konfiguration o.ä. auslagern
            databaseManager = new DatabaseManager("foco.sqlite");

            if (!databaseManager.Connect() || (goals = databaseManager.LoadAll()) == null)
            {
                MessageBox.Show("Es kann nicht mit der Datenbank kommuniziert werden.\nDie Anwendung wird nun beendet.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }

            homePage = new HomePage(this);
            boardPage = new BoardPage(this);
            listPage = new ListPage();
            calendarPage = new CalendarPage();
            taskgroupPage = new TaskgroupPage(null);

            PageFrame.Content = homePage;
            homePage.Update();

        }

        public void ShowProject(Project project)
        {
            boardPage.Project = project;
            PageFrame.Content = boardPage;
        }

        public void ShowTaskgroup(Taskgroup taskgroup)
        {
            taskgroupPage.Taskgroup = taskgroup;
            PageFrame.Content = taskgroupPage;
        }

        // wird aufgerufen, wenn das MainWindow geschlossen wird
        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (databaseManager.SaveAll(goals))
            {
                databaseManager.Disconnect();
            }
            else
            {
                MessageBox.Show("Es kann nicht mit der Datenbank kommuniziert werden.\nDie Anwendung kann solange nicht beendet werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
            }
        }

        // wird aufgerufen, wenn ein Button in der Menüleiste gedrückt wird
        private void OnMenuButtonClick(object sender, RoutedEventArgs e)
        {
            Button menuButton = (Button)sender;
            switch (menuButton.Tag)
            {
                case "Home":
                    PageFrame.Content = homePage;
                    homePage.Update();
                    break;
                case "Board":
                    PageFrame.Content = boardPage;
                    break;
                case "List":
                    PageFrame.Content = listPage;
                    break;
                case "Calendar":
                    PageFrame.Content = calendarPage;
                    break;
            }
        }

    }
}
