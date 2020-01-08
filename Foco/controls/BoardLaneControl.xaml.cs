using Foco.models;
using System.Collections.Generic;
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

        private enum Sort { None, AlphanumericAscending, AlphanumericDescending, PriorityAscending, PriorityDescending, DeadlineAscending, DeadlineDescending }

        private Project project;
        private State state;
        private Sort sort;
        private readonly BoardPage boardPage;

        public Project Project { get => project; set { project = value; Update(); } }
        public State State { get => state; set { state = value; Update(); } }

        public BoardLaneControl(BoardPage boardPage, State state)
        {
            InitializeComponent();
            this.state = state;
            this.boardPage = boardPage;
            this.sort = Sort.None;
            Update();
        }

        private void Update()
        {
            TaskgroupStack.Children.Clear();
            switch (state)
            {
                case State.Todo: TitleLabel.Content = "Warteschlange"; break;
                case State.InProgress: TitleLabel.Content = "In Bearbeitung"; break;
                case State.Done: TitleLabel.Content = "Abgeschlossen"; break;
            }
            if (project != null)
            {
                foreach (Taskgroup taskgroup in project.Taskgroups)
                {
                    if (taskgroup.State == state)
                        TaskgroupStack.Children.Add(new BoardGroupControl(boardPage, taskgroup));
                }
                SortElements();
            }
        }

        private void SortElements()
        {
            if (sort == Sort.None) return;
            List<BoardGroupControl> boardGroupControls = new List<BoardGroupControl>();
            foreach (BoardGroupControl boardGroupControl in TaskgroupStack.Children)
                boardGroupControls.Add(boardGroupControl);
            switch (sort)
            {
                case Sort.PriorityAscending: boardGroupControls.Sort((x, y) => x.Taskgroup.Prio.CompareTo(y.Taskgroup.Prio)); break;
                case Sort.PriorityDescending: boardGroupControls.Sort((x, y) => y.Taskgroup.Prio.CompareTo(x.Taskgroup.Prio)); break;
                case Sort.AlphanumericAscending: boardGroupControls.Sort((x, y) => x.Taskgroup.Title.CompareTo(y.Taskgroup.Title)); break;
                case Sort.AlphanumericDescending: boardGroupControls.Sort((x, y) => y.Taskgroup.Title.CompareTo(x.Taskgroup.Title)); break;
                case Sort.DeadlineAscending: boardGroupControls.Sort((x, y) => x.Taskgroup.Deadline.CompareTo(y.Taskgroup.Deadline)); break;
                case Sort.DeadlineDescending: boardGroupControls.Sort((x, y) => y.Taskgroup.Deadline.CompareTo(x.Taskgroup.Deadline)); break;
                default: break;
            }
            TaskgroupStack.Children.Clear();
            foreach (BoardGroupControl boardGroupControl in boardGroupControls)
                TaskgroupStack.Children.Add(boardGroupControl);
        }

        // Benutzer startet Drag eines BoardGroupControls
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is BoardGroupControl boardGroupControl)
            {
                boardGroupControl.Opacity = 0.3;
                DataObject dataObject = new DataObject(DataFormats.Xaml, boardGroupControl);
                DragDrop.DoDragDrop((DependencyObject)e.Source, dataObject, DragDropEffects.Move);
            }
        }

        // Benutzer droppt ein BoardGroupControl
        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.Xaml) is BoardGroupControl draggingBoardGroupControl)
            {
                draggingBoardGroupControl.Opacity = 1.0;
                draggingBoardGroupControl.Taskgroup.State = state;
                sort = Sort.None; // Durch den Drop geht die Sortierung kaputt
                foreach (MenuItem menuItem in SortMenuItem.Items)
                    menuItem.IsChecked = false;
            }
        }

        // Benutzer verschiebt einen Drag über das BoardLaneControl
        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.Xaml) is BoardGroupControl draggingBoardGroupControl)
            {

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
                        foreach (BoardGroupControl boardGroupControl in stackPanel.Children)
                        {
                            double height = boardGroupControl.ActualHeight;
                            if (mousePosition.Y >= heightSum && mousePosition.Y <= heightSum + height)
                                break; // Mauszeiger liegt innerhalb dieses Elements, Abbruch weil Index gefunden
                            heightSum += height;
                            insertIndex++;
                        }
                    }
                }

                // DropEnter auf BoardGroupControl => Der Index entspricht dem des gehoverten Elements, Insert in das Parent des gehoverten Elements
                else if (e.Source is BoardGroupControl hoveringBoardGroupControl)
                {
                    stackPanel = (StackPanel)hoveringBoardGroupControl.Parent;
                    insertIndex = stackPanel.Children.IndexOf(hoveringBoardGroupControl);
                }

                // aus dem alten Panel entfernen
                if (draggingBoardGroupControl.Parent != null)
                {
                    StackPanel oldPanel = (StackPanel)draggingBoardGroupControl.Parent;
                    oldPanel.Children.Remove(draggingBoardGroupControl); // aus dem alten Panel entfernen
                }

                try
                {
                    // versuche Insert an der Stelle vom berechneten Index
                    stackPanel.Children.Insert(insertIndex, draggingBoardGroupControl);
                }
                catch
                {
                    // wenn Index -1 (oder ungültig) ist das Panel leer, also Add statt Insert
                    stackPanel.Children.Add(draggingBoardGroupControl);
                }

            }
        }

        // Benutzer klickt auf MenuItem von Sortierung
        private void OnSortMenuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem clickedMenuItem = (MenuItem)sender;
            switch ((string)clickedMenuItem.Tag)
            {
                case "AA": sort = Sort.AlphanumericAscending; break;
                case "AD": sort = Sort.AlphanumericDescending; break;
                case "PA": sort = Sort.PriorityAscending; break;
                case "PD": sort = Sort.PriorityDescending; break;
                case "DA": sort = Sort.DeadlineAscending; break;
                case "DD": sort = Sort.DeadlineDescending; break;
                default: sort = Sort.None; break;
            }
            foreach (MenuItem menuItem in SortMenuItem.Items)
                menuItem.IsChecked = menuItem.Tag.Equals(clickedMenuItem.Tag);
            SortElements();
        }

        // Benutzer hat auf Hinzufügen geklickt
        private void OnAddGroupClicked(object sender, MouseButtonEventArgs e)
        {
            string title = "Neue Gruppe";
            int i = 1;
            while (Project.Taskgroups.Exists(x => x.Title == title))
                title = title.Split(' ')[0] + " " + title.Split(' ')[1] + " " + (++i);
            Taskgroup taskgroup = new Taskgroup(title) { State = state };
            project.Taskgroups.Add(taskgroup);
            BoardGroupControl boardGroupControl = new BoardGroupControl(boardPage, taskgroup);
            TaskgroupStack.Children.Add(boardGroupControl);
            boardGroupControl.NameLabel.BeginEditing();
            TaskgroupStackScroll.ScrollToBottom();
        }

    }
}