using Foco.models;
using Foco.windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für AppointmentControl.xaml
    /// </summary>
    public partial class AppointmentControl : UserControl
    {
        private readonly CalendarDayControl calendarDayControl;
        private readonly Taskgroup taskgroup;

        public AppointmentControl(CalendarDayControl calendarDayControl, Taskgroup taskgroup)
        {
            InitializeComponent();
            this.calendarDayControl = calendarDayControl;
            this.taskgroup = taskgroup;
            this.TitleLabel.Text = taskgroup.Title;
            
        }

        public Taskgroup Taskgroup  => taskgroup;
        public CalendarDayControl CalendarDayControl => calendarDayControl;

        private void OnOptionClickHandler(object sender, RoutedEventArgs e)
        {
            var menuItem =  sender as MenuItem;
            string itemTag = Convert.ToString(menuItem.Tag);

            if (itemTag  == "edit" )
            {
                CalendarDayControl.CalendarPage.MainWindow.ShowTaskgroup(taskgroup);
            }
            if (itemTag == "move")
            {
                DatepickerWindow datepicker = new DatepickerWindow(Taskgroup.Title, "Verschieben:", RescheduleDeadline);
                datepicker.DateTimePicker.SelectedDate = Taskgroup.Deadline;
                datepicker.ShowDialog();
            }
            if (itemTag == "delete")
            {
                ConfirmWindow confirmDeleteWindow = new ConfirmWindow(Taskgroup.Title, "möchtest diese Aufgabengruppe endgültig löschen?", ConfirmDeleteHandler);
                confirmDeleteWindow.ShowDialog();
            }
        }

        private void ConfirmDeleteHandler(ConfirmState confirmState)
        {
            if (confirmState == ConfirmState.YES)
            {
                CalendarDayControl.CalendarPage.Project.Taskgroups.Remove(Taskgroup);
                this.CalendarDayControl.CalendarPage.Update();
            }
        }

        private void RescheduleDeadline(InputState inputState, DateTime selectedDate)
        {
            if (inputState == InputState.Save)
            {
                Taskgroup.Deadline = selectedDate;
                this.CalendarDayControl.CalendarPage.Update();
            }
            else
                return;
        }
    }
}
