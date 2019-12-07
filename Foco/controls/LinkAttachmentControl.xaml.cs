using Foco.models;
using Microsoft.WindowsAPICodePack.Shell;
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
            ShellFile shellFile = ShellFile.FromFilePath(linkAttachment.Content);
            FileImg.Source = shellFile.Thumbnail.MediumBitmapSource;
        }

        private void OnControlClicked(object sender, MouseButtonEventArgs e)
        {
            linkAttachment.OpenUrl();
        }
        
    }
}
