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
        private readonly CalenderPage calendarPage;
        private readonly TaskgroupPage taskgroupPage;
        private readonly List<Goal> goals;

        public List<Goal> Goals => goals;
        public HomePage HomePage => homePage;
        public BoardPage BoardPage => boardPage;
        public ListPage ListPage => listPage;
        public CalenderPage CalenderPage => calendarPage;

        public MainWindow(List<Goal> goals)
        {
            InitializeComponent();
            this.goals = goals;
            homePage = new HomePage(this);
            boardPage = new BoardPage(this);
            listPage = new ListPage(this);
            calendarPage = new CalenderPage(this);
            taskgroupPage = new TaskgroupPage(null);
            PageFrame.Content = HomePage;
            MenuGrid.Visibility = Visibility.Hidden;
        }

        public void ShowProject(Project project)
        {
            BoardPage.Project = project;
            ListPage.Project = project;
            CalenderPage.Project = project;
            PageFrame.Content = BoardPage; // default Ansicht
            DrawButtonBorder(BoardBorder);
            MenuGrid.Visibility = Visibility.Visible;
        }

        public void ShowTaskgroup(Taskgroup taskgroup)
        {
            taskgroupPage.Taskgroup = taskgroup;
            PageFrame.Content = taskgroupPage;
            DrawButtonBorder(null);
        }

        // wird aufgerufen, wenn ein Button in der Menüleiste gedrückt wird
        private void OnMenuButtonClick(object sender, RoutedEventArgs e)
        {
            Button menuButton = (Button)sender;
            switch (menuButton.Tag)
            {
                case "Home":
                    MenuGrid.Visibility = Visibility.Hidden;
                    PageFrame.Content = HomePage;
                    HomePage.Update();
                    break;
                case "Board":
                    BoardPage.Update();
                    PageFrame.Content = BoardPage;
                    DrawButtonBorder(BoardBorder);
                    break;
                case "List":
                    ListPage.Update();
                    PageFrame.Content = ListPage;
                    DrawButtonBorder(ListBorder);
                    break;
                case "Calendar":
                    CalenderPage.setCurrentDate();
                    CalenderPage.Update();
                    PageFrame.Content = CalenderPage;
                    DrawButtonBorder(CalendarBorder);
                    break;
            }
        }

        // zeichnet den Border um den Button der aktiven Sicht
        private void DrawButtonBorder(Border border)
        {
            BoardBorder.BorderThickness = new Thickness(0);
            ListBorder.BorderThickness = new Thickness(0);
            CalendarBorder.BorderThickness = new Thickness(0);
            if(border != null)
                border.BorderThickness = new Thickness(2);
        }

    }
}
