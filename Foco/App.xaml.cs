using Foco.models;
using Foco.sqlite;
using System.Collections.Generic;
using System.Windows;

namespace Foco
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {

        private const string DB_PATH = "foco.sqlite";

        private DatabaseManager databaseManager;
        private List<Goal> goals;
        private bool isConnected;

        // die App wird vom Benutzer gestartet
        private void OnAppStart(object sender, StartupEventArgs e)
        {
            databaseManager = new DatabaseManager(DB_PATH);
            if ((isConnected = databaseManager.Connect()) == true && (goals = databaseManager.LoadAll()) != null)
            {
                new MainWindow(goals).Show();
            }
            else
            {
                MessageBox.Show("Es kann nicht mit der Datenbank kommuniziert werden.\nDie Anwendung wird nun beendet.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        // die App wird vom Benutzer beendet
        private void OnAppExit(object sender, ExitEventArgs e)
        {
            if (isConnected && !databaseManager.SaveAll(goals))
            {
                MessageBox.Show("Es kann nicht mit der Datenbank kommuniziert werden.\nIhre Änderungen konnten nicht gespeichert werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
