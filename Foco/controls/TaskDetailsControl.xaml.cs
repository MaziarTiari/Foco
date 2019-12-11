using Foco.models;
using System.Windows.Controls;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für TaskDetailsControl.xaml
    /// </summary>
    public partial class TaskDetailsControl : UserControl
    {

        private Task task;

        public Task Task { get => task; set { task = value; Update(); } }

        public TaskDetailsControl(Task task)
        {
            InitializeComponent();
            Task = task;
        }

        private void Update()
        {
            AttachmentStack.Children.Clear();
            TaskDescriptionEditor.Text = null;
            if (task != null)
            {
                foreach (Attachment attachment in task.Attachments)
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
                TaskDescriptionEditor.Text = task.Description;
            }

        }

        private void OnAddAttachmentMenuItemClick(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
