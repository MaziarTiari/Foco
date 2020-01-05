using Foco.models;
using Foco.pages;
using Foco.windows;
using System;
using System.Windows;
using System.Windows.Controls;


namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für CalendarDayControl.xaml
    /// </summary>
    public partial class CalendarDayControl : UserControl
    {
        private readonly CalendarPage calendarPage;
        private readonly CalendarDay day;
        private int indexOfAlreadyDefinedTaskgroup;

        public CalendarDayControl(CalendarDay day, CalendarPage calendarPage)
        {
            InitializeComponent();
            this.day = day;
            this.calendarPage = calendarPage;
            DayOfDate.Text = Convert.ToString(day.Date.Day);
            Update();
        }

        public CalendarPage CalendarPage => calendarPage;
        public int IndexOfTaskgroupInProject { get => indexOfAlreadyDefinedTaskgroup; set => indexOfAlreadyDefinedTaskgroup = value; }
        public CalendarDay Day => day;

        private void Update()
        {
            if (this.Day.Taskgroups.Count < 1)
                return;
            foreach (Taskgroup taskgroup in Day.Taskgroups)
            {
                AppointmentControl appointmentControl = new AppointmentControl(this, taskgroup);
                AppointmentContainer.Children.Add(appointmentControl);
            }
        }

        private void TaskgroupCreateInputWindow()
        {
            InputWindow inputWindow = new InputWindow("Neue Aufgabengruppe", "Name:", "", CreatedCallback, false);
            inputWindow.ShowDialog();
        }

        private void CreateNewTaskByDoubleClick(object sender, RoutedEventArgs e)
        {
            TaskgroupCreateInputWindow();
        }

        private void CreatedCallback(InputState inputState, string inputText)
        {
            if (inputState == InputState.Save)
            {
                this.IndexOfTaskgroupInProject = this.CalendarPage.Project.Taskgroups.FindIndex(t => t.Title == inputText);
                if(IndexOfTaskgroupInProject < 0) // there is no Taskgroup with the given title
                {
                    Taskgroup taskgroup = new Taskgroup(inputText);
                    taskgroup.Deadline = this.Day.Date;
                    this.CalendarPage.Project.Taskgroups.Add(taskgroup);
                    this.CalendarPage.Update();
                }
                else // there is already a taskgroup with the given title
                {
                    ConfirmWindow confirmWindow = new ConfirmWindow("Aufgabengruppe existiert bereits", "möchtest du dafür eine neue Deadline setzen?", SetDeadlineForExistingTaskgroup);
                    confirmWindow.ShowDialog();
                }
            }
        }

        private void SetDeadlineForExistingTaskgroup(ConfirmState confirmState)
        {
            if(confirmState == ConfirmState.YES)
            {
                this.CalendarPage.Project.Taskgroups[indexOfAlreadyDefinedTaskgroup].Deadline = this.Day.Date;
                this.CalendarPage.Update();
            }
            else
            {
                TaskgroupCreateInputWindow();
            }
        }
    }
}
