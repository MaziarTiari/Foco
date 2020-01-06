using Foco.calendar;
using Foco.models;
using Foco.pages;
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
        //private int indexOfAlreadyDefinedTaskgroup;

        public CalendarDayControl(CalendarDay day, CalendarPage calendarPage)
        {
            InitializeComponent();
            this.day = day;
            this.calendarPage = calendarPage;
            DayOfDate.Text = Convert.ToString(day.Date.Day);
            AddButton.Visibility = Visibility.Hidden;
            Update();
        }

        public CalendarPage CalendarPage => calendarPage;
        //public int IndexOfTaskgroupInProject { get => indexOfAlreadyDefinedTaskgroup; set => indexOfAlreadyDefinedTaskgroup = value; }
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

        private void OnAddButtonClicked(object sender, RoutedEventArgs e)
        {
            string title = "Neue Gruppe";
            int i = 1;
            while (CalendarPage.Project.Taskgroups.Exists(x => x.Title == title))
                title = title.Split(' ')[0] + " " + title.Split(' ')[1] + " " + (++i);
            Taskgroup taskgroup = new Taskgroup(title) { Deadline = this.Day.Date };
            this.CalendarPage.Project.Taskgroups.Add(taskgroup);
            AppointmentControl appointmentControl = new AppointmentControl(this, taskgroup);
            AppointmentContainer.Children.Add(appointmentControl);
            appointmentControl.TitleLabel.BeginEditing();
        }

        /*private void SetDeadlineForExistingTaskgroup(ConfirmState confirmState)
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
        }*/

        private void OnMouseOverChange(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AddButton.Visibility = (Day.Date >= DateTime.Today && IsMouseOver) ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
