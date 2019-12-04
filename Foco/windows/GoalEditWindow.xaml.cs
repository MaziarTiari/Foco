using System.Windows;
using System.Windows.Media;

namespace Foco.windows
{
    /// <summary>
    /// Interaktionslogik für GoalEditWindow.xaml
    /// </summary>
    public partial class GoalEditWindow : Window
    {

        public delegate void GoalSaveCallack(string goalName);

        private readonly GoalSaveCallack goalSaveCallback;

        public GoalEditWindow(string windowTitle, string goalName, GoalSaveCallack goalSaveCallback)
        {
            InitializeComponent();
            this.goalSaveCallback = goalSaveCallback;
            Owner = Application.Current.MainWindow;
            Title = windowTitle;
            NameBox.Text = goalName;
            NameBox.Focus();
            NameBox.CaretIndex = NameBox.Text.Length;
        }

        // Benutzer hat auf Speichern geklickt
        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                NameBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                goalSaveCallback(NameBox.Text);
                Close();
            }
        }

        // Benutzer hat auf Cancel geklickt
        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
