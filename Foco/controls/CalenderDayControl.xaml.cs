using Foco.models;
using Foco.pages;
using Foco.windows;
using System;
using System.Windows;
using System.Windows.Controls;


namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für CalenderDayControl.xaml
    /// </summary>
    public partial class CalenderDayControl : UserControl
    {
        private readonly CalenderPage calenderPage;
        private readonly CalenderDay day;
        private int indexOfAlreadyDefinedTaskgroup;

        public CalenderDayControl(CalenderDay day, CalenderPage calenderPage)
        {
            InitializeComponent();
            this.day = day;
            this.calenderPage = calenderPage;
            DayOfDate.Text = Convert.ToString(day.Date.Day);
            Update();
        }

        public CalenderPage CalenderPage => calenderPage;
        public int IndexOfTaskgroupInProject { get => indexOfAlreadyDefinedTaskgroup; set => indexOfAlreadyDefinedTaskgroup = value; }
        public CalenderDay Day => day;

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
                this.IndexOfTaskgroupInProject = this.CalenderPage.Project.Taskgroups.FindIndex(t => t.Title == inputText);
                if(IndexOfTaskgroupInProject < 0) // there is no Taskgroup with the given title
                {
                    Taskgroup taskgroup = new Taskgroup(inputText);
                    taskgroup.Deadline = this.Day.Date;
                    this.CalenderPage.Project.Taskgroups.Add(taskgroup);
                    this.CalenderPage.Update();
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
                this.CalenderPage.Project.Taskgroups[indexOfAlreadyDefinedTaskgroup].Deadline = this.Day.Date;
                this.CalenderPage.Update();
            }
            else
            {
                TaskgroupCreateInputWindow();
            }
        }
    }
}
