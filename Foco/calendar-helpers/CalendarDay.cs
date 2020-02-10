using Foco.models;
using System;
using System.Collections.Generic;

namespace Foco.calendar
{
    public class CalendarDay
    {
        private DateTime date;
        private List<Taskgroup> taskgroups = new List<Taskgroup>();
        private bool monthSelected = true;

        public CalendarDay(DateTime date)
        {
            this.Date = date;
        }

        public DateTime Date { get => date; set => date = value;}
        public List<Taskgroup> Taskgroups 
        { 
            get => taskgroups; set => taskgroups = value;
        }
        public bool FromSelectedMonth 
        { 
            get => monthSelected; set => monthSelected = value;
        }
    }
}
