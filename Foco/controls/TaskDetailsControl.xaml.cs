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
            if (task != null)
            {
                ContentGrid.Visibility = Visibility.Visible;
                TaskDescriptionEditor.Text = task.Description;
                foreach (Attachment attachment in task.Attachments)
                    AttachmentStack.Children.Add(new AttachmentControl(this, attachment));
            }
            else
            {
                ContentGrid.Visibility = Visibility.Hidden;
                TaskDescriptionEditor.Text = null;
            }
        }

        private void OnAddAttachmentMenuItemClick(object sender, RoutedEventArgs e)
        {
            AttachmentEditWindow attachmentEditWindow;
            switch (((MenuItem)sender).Tag.ToString())
            {
                case "URL": attachmentEditWindow = new AttachmentEditWindow("Webadresse anhängen", "", "https://", OnAttachmentCreatedCallback, AttachmentEditWindowType.WebUrl); break;
                case "FILE": attachmentEditWindow = new AttachmentEditWindow("Lokale Datei anhängen", "", "", OnAttachmentCreatedCallback, AttachmentEditWindowType.File); break;
                default: return;
            }
            attachmentEditWindow.ShowDialog();
        }

        private void OnAttachmentCreatedCallback(InputState inputState, string title, string link)
        {
            if (inputState == InputState.Save)
            {
                Attachment attachment = new Attachment(title, link);
                task.Attachments.Add(attachment);
                AttachmentStack.Children.Add(new AttachmentControl(this, attachment));
            }
        }

        public void DeleteAttachment(Attachment attachment)
        {
            task.Attachments.Remove(attachment);
            Update();
        }

    }
}
