using Foco.models;
using Microsoft.WindowsAPICodePack.Shell;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Foco.windows;
using static Foco.windows.AttachmentEditWindow;
using System.Net;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für AttachmentControl.xaml
    /// </summary>
    public partial class AttachmentControl : UserControl
    {

        private const string GOOGLE_API_LINK = "http://www.google.com/s2/favicons?domain=";

        private Attachment attachment;
        private readonly TaskDetailsControl taskDetailsControl;

        public Attachment Attachment { get => attachment; set { attachment = value; Update(); } }

        public AttachmentControl(TaskDetailsControl taskdetailsControl, Attachment attachment)
        {
            InitializeComponent();
            this.taskDetailsControl = taskdetailsControl;
            Attachment = attachment;
            EditButton.Visibility = Visibility.Hidden;
            DeleteButton.Visibility = Visibility.Hidden;
        }

        private void Update()
        {
            TitleText.Content = attachment.Title;
            if (!attachment.IsWebUrl())
            {
                // Anhang ist normale Datei: einfach das Thumbnail auslesen
                ShellFile shellFile = ShellFile.FromFilePath(attachment.Link);
                FileImg.Source = shellFile.Thumbnail.SmallBitmapSource;
            }
            else
            {
                try
                {
                    // Optimal: Favicon der Website mithilfe von Googles API auslesen
                    WebClient webClient = new WebClient();
                    byte[] faviconData = webClient.DownloadData(GOOGLE_API_LINK + attachment.Link);
                    MemoryStream memoryStream = new MemoryStream(faviconData);
                    BitmapImage bitmapImage = new BitmapImage();
                    memoryStream.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.EndInit();
                    FileImg.Source = bitmapImage;
                }
                catch
                {
                    // Geht nicht? Dann temporäre foco_dummy.html anlegen und davon das File Icon
                    string dummyPath = Path.GetTempPath() + "foco_dummy.html";
                    File.WriteAllText(dummyPath, ""); // erstellt und schließt
                    ShellFile shellFile = ShellFile.FromFilePath(dummyPath);
                    FileImg.Source = shellFile.Thumbnail.SmallBitmapSource;
                    File.Delete(dummyPath);
                }

            }
        }

        private void OnControlClicked(object sender, MouseButtonEventArgs e)
        {
            attachment.OpenUrl();
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            EditButton.Visibility = Visibility.Visible;
            DeleteButton.Visibility = Visibility.Visible;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            EditButton.Visibility = Visibility.Hidden;
            DeleteButton.Visibility = Visibility.Hidden;
        }

        private void OnEditClicked(object sender, RoutedEventArgs e)
        {
            AttachmentEditWindow attachmentEditWindow;
            if (attachment.IsWebUrl())
                attachmentEditWindow = new AttachmentEditWindow("Webadresse editieren", attachment.Title, attachment.Link, OnAttachmentEditedCallback, AttachmentEditWindowType.WebUrl);
            else
                attachmentEditWindow = new AttachmentEditWindow("Dateipfad editieren", attachment.Title, attachment.Link, OnAttachmentEditedCallback, AttachmentEditWindowType.File);
            attachmentEditWindow.ShowDialog();
        }

        private void OnAttachmentEditedCallback(InputState inputState, string title, string link)
        {
            if (inputState == InputState.Save)
            {
                attachment.Title = title;
                attachment.Link = link;
                Update();
            }
        }

        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirmWindow = new ConfirmWindow("Anhang löschen", "Sind Sie sicher, dass Sie den Anhang \"" + attachment.Title + "\" löschen möchten?", OnAttachmentDeletedCallback);
            confirmWindow.ShowDialog();
        }

        private void OnAttachmentDeletedCallback(ConfirmState confirmState)
        {
            if (confirmState == ConfirmState.YES)
            {
                taskDetailsControl.DeleteAttachment(attachment);
            }
        }
    }
}
