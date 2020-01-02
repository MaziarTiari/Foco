using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace Foco.windows
{
    /// <summary>
    /// Interaktionslogik für ProjectEditWindow.xaml
    /// </summary>
    public partial class ProjectEditWindow : Window
    {

        private static string[] colorStrings = new string[] { "#986526", "#946871", "#313131", "#694851", "#157578", "#DAEDAE", "#EEAEEA" }; // TODO Auf Farben von Nikolay warten

        public delegate void ProjectSaveCallack(string projectName, string projectColor);
        private ProjectSaveCallack projectSaveCallback;

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
                Border colorBorder = new Border();
                colorBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorString));
                colorBorder.Height = 20;
                colorBorder.Width = 200;
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
