using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


namespace Foco.windows
{
    /// interaction logic for DatepickerWindow.xaml
    public partial class DatepickerWindow : Window
    {
        public delegate void InputCallback(InputState inputState, DateTime selectedDate);

        private readonly InputCallback inputCallback;
        public DatepickerWindow( string windowTitle, string labelText,
                                 InputCallback inputCallback )
        {
            InitializeComponent();
            this.inputCallback = inputCallback;
            Owner = Application.Current.MainWindow;
            Title = windowTitle;
            InputLabel.Content = labelText;
        }

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            switch ((string)((Button)sender).Tag)
            {
                case "save":
                    inputCallback(InputState.Save, this.DateTimePicker.SelectedDate.Value);
                    Close();
                    break;
                case "cancel":
                    Close();
                    break;
            }
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            return;
        }
    }
}
