using Foco.models;
using Foco.windows;
using System.Windows;
using System.Windows.Controls;
using static Foco.windows.AttachmentEditWindow;

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
                TaskDescriptionEditor.Text = task.Description;
                foreach (Attachment attachment in task.Attachments)
                {
                    AttachmentStack.Children.Add(new AttachmentControl(attachment));
                }
            }
        }

        private void OnAddAttachmentMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (task == null)
                return;
            switch (((MenuItem)sender).Tag.ToString())
            {
                case "URL":
                    AttachmentEditWindow attachmentEditWindow = new AttachmentEditWindow("Webadresse anhängen", "", "https://", OnAttachmentCreatedCallback, AttachmentEditWindowType.WebUrl);
                    attachmentEditWindow.ShowDialog();
                    break;
                case "FILE":
                    break;
            }
        }

        private void OnAttachmentCreatedCallback(InputState inputState, string title, string link)
        {
            if (inputState == InputState.Save)
            {
                Attachment attachment = new Attachment(title, link);
                task.Attachments.Add(attachment);
                AttachmentStack.Children.Add(new AttachmentControl(attachment));
            }
        }

    }
}
