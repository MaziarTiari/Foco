using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Foco.windows
{
    /// <summary>
    /// Interaktionslogik für ProjectEditWindow.xaml
    /// </summary>
    public partial class ProjectEditWindow : Window
    {

        private static readonly string[] colorStrings = new string[] { "#A0A0A0", "#EB2524", "#F47B10", "#F8E40A", "#436A0C", "#86E004", "#1BDBE9", "#144EE1", "#C70EE3", "#E90FD4" };

        public delegate void ProjectSaveCallack(string projectName, string projectColor);
        private readonly ProjectSaveCallack projectSaveCallback;

        public ProjectEditWindow(string windowTitle, string projectName, string projectColor, ProjectSaveCallack projectSaveCallback)
        {
            InitializeComponent();
            this.projectSaveCallback = projectSaveCallback;
            Title = windowTitle;
            Owner = System.Windows.Application.Current.MainWindow;
            NameBox.Text = projectName;
            NameBox.Focus();
            NameBox.CaretIndex = NameBox.Text.Length;
            foreach (string colorString in colorStrings)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                Border colorBorder = new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorString)),
                    Height = 20,
                    Width = 130
                };
                comboBoxItem.Content = colorBorder;
                comboBoxItem.Tag = colorString;
                ColorCombo.Items.Add(comboBoxItem);
                if (projectColor == colorString)
                    ColorCombo.SelectedItem = comboBoxItem; // vorhandene Farbe vorselektieren
            }
            if (ColorCombo.SelectedIndex == -1)
                ColorCombo.SelectedIndex = 0; // nicht gefunden, dann erstes Element vorselektieren
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
                projectSaveCallback(NameBox.Text, ((ComboBoxItem)ColorCombo.SelectedItem).Tag.ToString());
            }

        }

        // Benutzer hat auf Cancel geklickt
        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
