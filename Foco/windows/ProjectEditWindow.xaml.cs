using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace Foco.windows
{
    /// <summary>
    /// Interaktionslogik für ProjectEditWindow.xaml
    /// </summary>
    public partial class ProjectEditWindow : Window
    {

        public delegate void ProjectSaveCallack(string projectName, string projectColor);

        private Color color;
        private ProjectSaveCallack projectSaveCallback;

        public ProjectEditWindow(string windowTitle, string projectName, string projectColor, ProjectSaveCallack projectSaveCallback)
        {
            InitializeComponent();
            this.projectSaveCallback = projectSaveCallback;
            Title = windowTitle;
            color = (Color)ColorConverter.ConvertFromString(projectColor);
            Owner = System.Windows.Application.Current.MainWindow;
            ColorBox.Background = new SolidColorBrush(color);
            NameBox.Text = projectName;
            NameBox.Focus();
            NameBox.CaretIndex = NameBox.Text.Length;
        }

        // Benutzer hat auf "Wählen" geklickt
        private void OnChooseColorClicked(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                color = Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                ColorBox.Background = new SolidColorBrush(color);
            }
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
                Close();
                projectSaveCallback(NameBox.Text, new ColorConverter().ConvertToString(color));
            }

        }

        // Benutzer hat auf Cancel geklickt
        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
