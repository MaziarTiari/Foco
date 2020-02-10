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

        private const string helpUrl
            = "https://github.com/MaziarTiari/Foco";

        private readonly HomePage homePage;
        private readonly BoardPage boardPage;
        private readonly ListPage listPage;
        private readonly CalendarPage calendarPage;
        private readonly TaskgroupPage taskgroupPage;
        private readonly List<Goal> goals;

        public MainWindow(List<Goal> goals)
        {
            InitializeComponent();
            this.goals = goals;
            homePage = new HomePage(this);
            boardPage = new BoardPage(this);
            listPage = new ListPage(this);
            calendarPage = new CalendarPage(this);
            taskgroupPage = new TaskgroupPage(null);
            PageFrame.Content = HomePage;
            SetViewButtonsVisibilities(Visibility.Hidden);
            Title = "foco | Startseite";
        }

        public List<Goal> Goals => goals;
        public HomePage HomePage => homePage;
        public BoardPage BoardPage => boardPage;
        public ListPage ListPage => listPage;
        public CalendarPage CalendarPage => calendarPage;

        public void ShowProject(Project project)
        {
            BoardPage.Project = project;
            ListPage.Project = project;
            CalendarPage.Project = project;
            PageFrame.Content = BoardPage; // default view
            SetViewButtonsVisibilities(Visibility.Visible);
            DrawButtonBorder(BoardBorder);
            Title = "foco | " + project.Name;
        }

        public void ShowTaskgroup(Taskgroup taskgroup)
        {
            taskgroupPage.Taskgroup = taskgroup;
            PageFrame.Content = taskgroupPage;
            DrawButtonBorder(null);
        }

        private void OnMenuButtonClick(object sender, RoutedEventArgs e)
        {
            Button menuButton = (Button)sender;
            switch (menuButton.Tag)
            {
                case "Home":
                    PageFrame.Content = HomePage;
                    HomePage.Update();
                    SetViewButtonsVisibilities(Visibility.Hidden);
                    Title = "foco | Startseite";
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
                    CalendarPage.SetCurrentDate();
                    CalendarPage.Update();
                    PageFrame.Content = CalendarPage;
                    DrawButtonBorder(CalendarBorder);
                    break;
                case "Help":
                    System.Diagnostics.Process.Start(helpUrl);
                    break;
            }
        }

        // draw border around active navbar button
        private void DrawButtonBorder(Border border)
        {
            BoardBorder.BorderThickness = new Thickness(0);
            ListBorder.BorderThickness = new Thickness(0);
            CalendarBorder.BorderThickness = new Thickness(0);
            if (border != null)
                border.BorderThickness = new Thickness(2);
        }

        // in home view project-view-buttons in nav bar hides
        private void SetViewButtonsVisibilities(Visibility visibility)
        {
            BoardBorder.Visibility = visibility;
            ListBorder.Visibility = visibility;
            CalendarBorder.Visibility = visibility;
            WelcomeMessage.Visibility = visibility == Visibility.Hidden 
                                        ? Visibility.Visible 
                                        : Visibility.Hidden;
        }

    }
}
