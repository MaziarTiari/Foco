using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Foco.windows
{

    public enum InputState { Save, Cancel, Close }

    /// <summary>
    /// Interaktionslogik für InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {

        public delegate void InputCallback(InputState inputState, string inputText);

        private readonly InputCallback inputCallback;
        private readonly bool allowWhitespaceOnSave;

        public InputWindow(string windowTitle, string labelText, string defaultText, InputCallback inputCallback, bool allowWhitespaceOnSave = true)
        {
            InitializeComponent();
            this.inputCallback = inputCallback;
            this.allowWhitespaceOnSave = allowWhitespaceOnSave;
            Owner = Application.Current.MainWindow;
            Title = windowTitle;
            InputLabel.Content = labelText;
            NameBox.Text = defaultText;
            NameBox.Focus();
            NameBox.CaretIndex = NameBox.Text.Length;
        }

        // Benutzer hat auf Button geklickt
        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            switch ((string)((Button)sender).Tag)
            {
                case "Save":
                    if (!allowWhitespaceOnSave && string.IsNullOrWhiteSpace(NameBox.Text))
                    {
                        NameBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        inputCallback(InputState.Save, NameBox.Text);
                        Close();
                    }
                    break;
                case "Cancel":
                    inputCallback(InputState.Cancel, NameBox.Text);
                    Close();
                    break;
            }
        }

        // Benutzer schließt Fenster
        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            inputCallback(InputState.Close, NameBox.Text);
        }

    }
}
