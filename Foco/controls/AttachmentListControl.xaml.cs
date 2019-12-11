using Foco.models;
using System.Windows.Controls;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für AttachmentListControl.xaml
    /// </summary>
    public partial class AttachmentListControl : UserControl
    {

        private readonly Taskgroup taskgroup;

        public AttachmentListControl(Taskgroup taskgroup)
        {
            InitializeComponent();
            this.taskgroup = taskgroup;
            Update();
        }

        private void Update()
        {
            AttachmentStack.Children.Clear();
            foreach (Attachment attachment in taskgroup.Attachments)
            {
                switch (attachment.Type)
                {
                    case AttachmentType.Link:
                        AttachmentStack.Children.Add(new LinkAttachmentControl((LinkAttachment)attachment));
                        break;
                    case AttachmentType.Comment:
                        // TODO
                        break;
                }
            }
        }

        private void OnAddAttachmentMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
