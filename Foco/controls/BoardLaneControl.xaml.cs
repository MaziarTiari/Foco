using Foco.models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für BoardLaneControl.xaml
    /// </summary>
    public partial class BoardLaneControl : UserControl
    {

        private Project project;
        private State state;

        public Project Project { get => project; set { project = value; Update(); } }
        public State State { get => state; set { state = value; Update(); } }

        public BoardLaneControl(State state)
        {
            InitializeComponent();
            this.state = state;
            Update();
        }

        private void Update()
        {
            TaskgroupStack.Children.Clear();
            switch (state)
            {
                case State.Todo: TitleLabel.Content = "Warteschlange"; break;
                case State.InProgress: TitleLabel.Content = "In Bearbeitung"; break;
                case State.Done: TitleLabel.Content = "Fertig"; break;
            }
            if (project != null)
            {
                foreach (Taskgroup taskgroup in project.Taskgroups)
                {
                    if (taskgroup.State == state)
                    {
                        TaskgroupStack.Children.Add(new TaskgroupControl(taskgroup));
                    }
                }
            }
        }

        // Benutzer startet Drag eines TaskgroupControls
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is TaskgroupControl)
            {
                TaskgroupControl taskgroupControl = (TaskgroupControl)e.Source;
                taskgroupControl.Opacity = 0.3;
                DataObject dataObject = new DataObject(DataFormats.Xaml, taskgroupControl);
                DragDrop.DoDragDrop((DependencyObject)e.Source, dataObject, DragDropEffects.Move);
            }
        }

        // Benutzer droppt ein TaskgroupControl
        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.Xaml) is TaskgroupControl)
            {
                TaskgroupControl draggingTaskgroupControl = (TaskgroupControl)e.Data.GetData(DataFormats.Xaml);
                draggingTaskgroupControl.Opacity = 1.0;
                draggingTaskgroupControl.Taskgroup.State = state;
            }
        }

        // Benutzer verschiebt einen Drag über das BoardLaneControl
        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.Xaml) is TaskgroupControl)
            {

                TaskgroupControl draggingTaskgroupControl = (TaskgroupControl)e.Data.GetData(DataFormats.Xaml);
                int insertIndex = 0;
                StackPanel stackPanel = null;

                // DropEnter auf das StackPanel => Anhand von Mausposition den Index berechnen, Insert in das gehoverte Stackpanel
                if (e.Source is StackPanel)
                {
                    stackPanel = (StackPanel)e.Source;
                    double heightSum = 0;
                    Point absoluteMousePosition = new Point(System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y);
                    Point mousePosition = stackPanel.PointFromScreen(absoluteMousePosition);
                    if (stackPanel.Children.Count == 0)
                        insertIndex = -1;
                    else
                    {
                        foreach (TaskgroupControl taskgroupControl in stackPanel.Children)
                        {
                            double height = taskgroupControl.ActualHeight;
                            if (mousePosition.Y >= heightSum && mousePosition.Y <= heightSum + height)
                                break; // Mauszeiger liegt innerhalb dieses Elements, Abbruch weil Index gefunden
                            heightSum += height;
                            insertIndex++;
                        }
                    }
                }

                // DropEnter auf TaskgroupControl => Der Index entspricht dem des gehoverten Elements, Insert in das Parent des gehoverten Elements
                else if (e.Source is TaskgroupControl)
                {
                    TaskgroupControl hoveringTaskgroupControl = (TaskgroupControl)e.Source;
                    stackPanel = (StackPanel)hoveringTaskgroupControl.Parent;
                    insertIndex = stackPanel.Children.IndexOf(hoveringTaskgroupControl);
                }

                // aus dem alten Panel entfernen
                if (draggingTaskgroupControl.Parent != null)
                {
                    StackPanel oldPanel = (StackPanel)draggingTaskgroupControl.Parent;
                    oldPanel.Children.Remove(draggingTaskgroupControl); // aus dem alten Panel entfernen
                }

                try
                {
                    // versuche Insert an der Stelle vom berechneten Index
                    stackPanel.Children.Insert(insertIndex, draggingTaskgroupControl);
                }
                catch
                {
                    // wenn Index -1 (oder ungültig) ist das Panel leer, also Add statt Insert
                    stackPanel.Children.Add(draggingTaskgroupControl);
                }

            }
        }


    }
}