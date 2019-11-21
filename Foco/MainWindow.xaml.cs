using Foco.pages;
using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();
            homePage = new HomePage();
            boardPage = new BoardPage();
            listPage = new ListPage();
            calendarPage = new CalendarPage();
            PageFrame.Content = homePage;
        }

        private void OnMenuButtonClick(object sender, RoutedEventArgs e)
        {
            Button menuButton = (Button)sender;
            switch (menuButton.Tag)
            {
                case "Home":
                    PageFrame.Content = homePage;
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
