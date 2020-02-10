using Foco.models;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Foco.windows
{
    /// interaction logic for AttachmentEditWindow.xaml
    public partial class AttachmentEditWindow : Window
    {

        public enum AttachmentEditWindowType { WebUrl, File }
        public delegate void InputCallback( InputState inputState, 
                                            string titleText, string linkText );

        private readonly InputCallback inputCallback;
        private readonly AttachmentEditWindowType type;

        public AttachmentEditWindow( string windowTitle, string defaultTitle, 
                                     string defaultLink, InputCallback inputCallback, 
                                     AttachmentEditWindowType type )
        {
            InitializeComponent();
            this.inputCallback = inputCallback;
            this.type = type;
            Owner = Application.Current.MainWindow;
            Title = windowTitle;
            if (type == AttachmentEditWindowType.File)
            {
                LinkLabel.Content = "Datei:";
            }
            else
            {
                LinkLabel.Content = "URL:";
                FileButton.Visibility = Visibility.Collapsed;
                LinkBox.Width = TitleBox.Width;
            }
            TitleBox.Text = defaultTitle;
            LinkBox.Text = defaultLink;
            TitleBox.Focus();
            TitleBox.CaretIndex = TitleBox.Text.Length;
        }

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            switch ((string)((Button)sender).Tag)
            {
                case "Save":
                    if (string.IsNullOrWhiteSpace(TitleBox.Text))
                    {
                        TitleBox.BorderBrush = new SolidColorBrush(Colors.Red);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(LinkBox.Text)
                        || (type == AttachmentEditWindowType.WebUrl 
                                && !Attachment.IsWebUrl(LinkBox.Text))
                        || (type == AttachmentEditWindowType.File 
                                && !File.Exists(LinkBox.Text)))
                    {
                        LinkBox.BorderBrush = new SolidColorBrush(Colors.Red);
                        return;
                    }
                    inputCallback(InputState.Save, TitleBox.Text, LinkBox.Text);
                    Close();
                    break;
                case "Cancel":
                    inputCallback(InputState.Cancel, TitleBox.Text, LinkBox.Text);
                    Close();
                    break;
            }
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            inputCallback(InputState.Close, TitleBox.Text, LinkBox.Text);
        }

        private void OnLoadFileButtonClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { 
                    Title = "Datei zum Anhängen auswählen"
                };
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LinkBox.Text = openFileDialog.FileName;
            }
            openFileDialog.Dispose();
        }
    }
}
