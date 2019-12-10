using Foco.models;
using Microsoft.WindowsAPICodePack.Shell;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für LinkAttachmentControl.xaml
    /// </summary>
    public partial class LinkAttachmentControl : UserControl
    {

        private LinkAttachment linkAttachment;

        public LinkAttachment LinkAttachment { get => linkAttachment; set { linkAttachment = value; Update(); } }

        public LinkAttachmentControl(LinkAttachment linkAttachment)
        {
            InitializeComponent();
            LinkAttachment = linkAttachment;
        }

        private void Update()
        {
            TitleText.Text = linkAttachment.Title;
            ContentText.Text = linkAttachment.Content;
            if (!linkAttachment.IsWebUrl())
            {
                // Anhang ist normale Datei: einfach das Thumbnail auslesen
                ShellFile shellFile = ShellFile.FromFilePath(linkAttachment.Content);
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
            linkAttachment.OpenUrl();
        }

    }
}
