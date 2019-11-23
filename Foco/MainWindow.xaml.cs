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
        private List<Goal> goals;
        private DatabaseManager databaseManager;

        // wird aufgerufen, wenn das MainWindow erstellt wird
        public MainWindow()
        {
            InitializeComponent();
            homePage = new HomePage();
            boardPage = new BoardPage();
            listPage = new ListPage();
            calendarPage = new CalendarPage();
            PageFrame.Content = homePage;

            // TODO Pfad später evtl. in Konfiguration o.ä. auslagern
            databaseManager = new DatabaseManager("foco.sqlite");

            if (!databaseManager.Connect() || (goals = databaseManager.LoadAll()) == null)
            {
                MessageBox.Show("Es kann nicht mit der Datenbank kommuniziert werden.\nDie Anwendung wird nun beendet.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

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
