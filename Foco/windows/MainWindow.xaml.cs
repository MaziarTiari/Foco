using Foco.models;
using Foco.pages;
using System.Collections.Generic;
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
        private readonly TaskgroupPage taskgroupPage;
        private readonly List<Goal> goals;

        public List<Goal> Goals => goals;
        public HomePage HomePage => homePage;
        public BoardPage BoardPage => boardPage;
        public ListPage ListPage => listPage;
        public CalendarPage CalendarPage => calendarPage;

        public MainWindow(List<Goal> goals)
        {
            InitializeComponent();
            this.goals = goals;
            homePage = new HomePage(this);
            boardPage = new BoardPage(this);
            listPage = new ListPage(this);
            calendarPage = new CalendarPage();
            taskgroupPage = new TaskgroupPage(null);
            PageFrame.Content = HomePage;
        }

        public void ShowProject(Project project)
        {
            boardPage.Project = project;
            ListPage.Project = project;
            PageFrame.Content = BoardPage; // default Ansicht
            List.Visibility = Visibility.Visible;
            Board.Visibility = Visibility.Visible;
            Calender.Visibility = Visibility.Visible;
        }

        public void ShowTaskgroup(Taskgroup taskgroup)
        {
            taskgroupPage.Taskgroup = taskgroup;
            PageFrame.Content = taskgroupPage;
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
                    BoardPage.Update();
                    PageFrame.Content = BoardPage;
                    break;
                case "List":
                    ListPage.Update();
                    PageFrame.Content = ListPage;
                    break;
                case "Calendar":
                    PageFrame.Content = CalendarPage;
                    break;
            }
        }

    }
}
