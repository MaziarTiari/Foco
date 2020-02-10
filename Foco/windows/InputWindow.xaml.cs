using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Foco.windows
{

    public enum InputState { Save, Cancel, Close }

    /// interaction logic for InputWindow.xaml
    public partial class InputWindow : Window
    {

        public delegate void InputCallback(InputState inputState, string inputText);

        private readonly InputCallback inputCallback;
        private readonly bool allowWhitespaceOnSave;

        public InputWindow( string windowTitle, string labelText, string defaultText,
                            InputCallback inputCallback, bool allowWhitespaceOnSave = true )
        {
            InitializeComponent();
            this.inputCallback = inputCallback;
            this.allowWhitespaceOnSave = allowWhitespaceOnSave;
            Owner = Application.Current.MainWindow;
            Title = windowTitle;
            InputLabel.Content = labelText;
            InputBox.Text = defaultText;
            InputBox.Focus();
            InputBox.CaretIndex = InputBox.Text.Length;
        }

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            switch ((string)((Button)sender).Tag)
            {
                case "Save":
                    if (!allowWhitespaceOnSave 
                            && string.IsNullOrWhiteSpace(InputBox.Text))
                    {
                        InputBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        inputCallback(InputState.Save, InputBox.Text);
                        Close();
                    }
                    break;
                case "Cancel":
                    inputCallback(InputState.Cancel, InputBox.Text);
                    Close();
                    break;
            }
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            inputCallback(InputState.Close, InputBox.Text);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SaveButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
}
