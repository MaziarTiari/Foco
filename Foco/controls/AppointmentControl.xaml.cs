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
        private readonly CalenderDayControl calenderDayControl;
        private readonly Taskgroup taskgroup;

        public AppointmentControl(CalenderDayControl calenderDayControl, Taskgroup taskgroup)
        {
            InitializeComponent();
            this.calenderDayControl = calenderDayControl;
            this.taskgroup = taskgroup;
            this.TitleLabel.Text = taskgroup.Title;
            
        }

        public Taskgroup Taskgroup  => taskgroup;
        public CalenderDayControl CalenderDayControl => calenderDayControl;

        private void OnOptionClickHandler(object sender, RoutedEventArgs e)
        {
            var menuItem =  sender as MenuItem;
            string itemTag = Convert.ToString(menuItem.Tag);

            if (itemTag  == "edit" )
            {
                CalenderDayControl.CalenderPage.MainWindow.ShowTaskgroup(taskgroup);
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
                CalenderDayControl.CalenderPage.Project.Taskgroups.Remove(Taskgroup);
                this.CalenderDayControl.CalenderPage.Update();
            }
        }

        private void RescheduleDeadline(InputState inputState, DateTime selectedDate)
        {
            if (inputState == InputState.Save)
            {
                Taskgroup.Deadline = selectedDate;
                this.CalenderDayControl.CalenderPage.Update();
            }
            else
                return;
        }
    }
}
