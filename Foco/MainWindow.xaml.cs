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

        private readonly HomePage homePage;
        private readonly BoardPage boardPage;
        private readonly ListPage listPage;
        private readonly CalendarPage calendarPage;
        private readonly List<Goal> goals;
        private DatabaseManager databaseManager;

        public List<Goal> Goals => goals;
        public HomePage HomePage => homePage;
        public BoardPage BoardPage => boardPage;
        public ListPage ListPage => listPage;
        public CalendarPage CalendarPage => calendarPage;

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
            listPage = new ListPage(this);
            calendarPage = new CalendarPage();
            taskgroupPage = new TaskgroupPage(null);

            PageFrame.Content = HomePage;
            HomePage.Update();

        }

        public void ShowProject(Project project)
        {
            boardPage.Project = project;
            PageFrame.Content = boardPage;
            ListPage.Project = project;
            PageFrame.Content = ListPage;
            List.Visibility = Visibility.Visible;
            Board.Visibility = Visibility.Visible;
            Calender.Visibility = Visibility.Visible;
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
                    PageFrame.Content = HomePage;
                    HomePage.Update();
                    Board.Visibility = Visibility.Hidden;
                    Calender.Visibility = Visibility.Hidden;
                    List.Visibility = Visibility.Hidden;
                    break;
                case "Board":
                    PageFrame.Content = BoardPage;
                    break;
                case "List":
                    PageFrame.Content = ListPage;
                    break;
                case "Calendar":
                    PageFrame.Content = CalendarPage;
                    break;
            }
        }
    }
}
