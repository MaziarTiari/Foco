using System.Windows;
using System.Windows.Controls;

namespace Foco.windows
{

    public enum ConfirmState { YES, NO };

    /// interaction logic for ConfirmWindow.xaml
    public partial class ConfirmWindow : Window
    {

        public delegate void ConfirmCallback(ConfirmState confirmState);

        private readonly ConfirmCallback confirmCallback;

        public ConfirmWindow( string title, string message, 
                              ConfirmCallback confirmCallback )
        {
            InitializeComponent();
            this.confirmCallback = confirmCallback;
            Owner = Application.Current.MainWindow;
            Title = title;
            Message.Text = message;
        }

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
            switch (((Button)sender).Tag)
            {
                case "Yes": confirmCallback(ConfirmState.YES); break;
                case "No": confirmCallback(ConfirmState.NO); break;
            }
        }

    }
}
