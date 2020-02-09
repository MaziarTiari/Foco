using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Foco.models;
using Foco.windows;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {

        private readonly TaskgroupControl taskgroupControl;
        private readonly Task task;
        private bool isHighlighted;

        public bool Highlighted { get => isHighlighted; set { isHighlighted = value; Update(); } }
        public Task Task => task;

        public TaskControl(Task task, TaskgroupControl taskgroupControl)
        {
            InitializeComponent();
            this.taskgroupControl = taskgroupControl;
            this.task = task;
            DeleteButton.Visibility = Visibility.Hidden;
            Update();
        }

        public void Update()
        {
            TaskCheckBox.IsChecked = task.Done;
            EditableTaskLabel.Text = task.Title;
            AttachmentInfoText.Text = task.Attachments.Count > 0 ? task.Attachments.Count + "📎 " : null;
            ControlContainer.Background = isHighlighted ? new SolidColorBrush(Color.FromArgb(50, 0, 0, 0)) : null;
        }

        // Benutzer drückt auf Löschen
        public void DeleteTaskMouseEvent(object sender, RoutedEventArgs e)
        {
            if( String.IsNullOrEmpty(Task.Description) && Task.Attachments.Count == 0)
                DeleteTask();
            else
            {
                ShowDeleteConfirmWindow();
            }
        }

        private void ShowDeleteConfirmWindow()
        {
            ConfirmWindow confirmWindow = new ConfirmWindow(
            "Alle Anhänge löschen", "Sind Sie sich sicher, dass Sie diese Aufgabe inkl. aller Anhänge endgültig löschen wollen?",
            DeleteTaskCallback);
            confirmWindow.ShowDialog();
        }

        private void DeleteTaskCallback(ConfirmState confirmState)
        {
            if(confirmState == ConfirmState.YES)
                DeleteTask();
            else
            {
                if(string.IsNullOrWhiteSpace(EditableTaskLabel.Text))
                {
                    if(Task.Attachments.Count > 0)
                    {
                        Task.Title = Task.Attachments[0].Title;
                    }
                    else
                    {
                        Task.Title = "Ohne Titel";
                    }
                    Update();
                }
            }
        }


        // Benutzer schließt Editieren ab
#pragma warning disable IDE0051 // wird eigentlich durch XAML aufgerufen
        private void OnLabelEdited(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                task.Title = text;
            else
            {
                if (Task.Attachments.Count > 0 || Task.Description != "")
                {
                    ShowDeleteConfirmWindow();
                }
                else
                    DeleteTask();
            }
        }
        #pragma warning restore IDE0051

        private void DeleteTask()
        {
            taskgroupControl.TaskContainer.Children.Remove(this);
            taskgroupControl.Taskgroup.Tasks.Remove(task);
            taskgroupControl.OnTaskDeleted(task);
        }

        private void OnCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            task.Done = TaskCheckBox.IsChecked == true;
        }

        private void OnControlClicked(object sender, MouseButtonEventArgs e)
        {
            taskgroupControl.OnTaskClicked(task);
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Visible;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Hidden;
        }
    }
}