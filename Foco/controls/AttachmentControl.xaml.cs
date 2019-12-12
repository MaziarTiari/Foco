using Foco.models;
using Microsoft.WindowsAPICodePack.Shell;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für AttachmentControl.xaml
    /// </summary>
    public partial class AttachmentControl : UserControl
    {

        private Attachment attachment;

        public Attachment Attachment { get => attachment; set { attachment = value; Update(); } }

        public AttachmentControl(Attachment attachment)
        {
            InitializeComponent();
            Attachment = attachment;
        }

        private void Update()
        {
            TitleText.Text = attachment.Title;
            ContentText.Text = attachment.Link;
            if (!attachment.IsWebUrl())
            {
                // Anhang ist normale Datei: einfach das Thumbnail auslesen
                ShellFile shellFile = ShellFile.FromFilePath(attachment.Link);
                FileImg.Source = shellFile.Thumbnail.MediumBitmapSource;
            }
            else
            {
                // Anhang ist URL: dummy.html anlegen und davon das Thumbnail auslesen
                string dummyPath = Path.GetTempPath() + "foco_dummy.html";
                File.WriteAllText(dummyPath, ""); // erstellt und schließt
                ShellFile shellFile = ShellFile.FromFilePath(dummyPath);
                FileImg.Source = shellFile.Thumbnail.MediumBitmapSource;
                File.Delete(dummyPath);
            }
        }

        private void OnControlClicked(object sender, MouseButtonEventArgs e)
        {
            attachment.OpenUrl();
        }

    }
}
