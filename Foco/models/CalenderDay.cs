using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foco.models
{
    public class CalenderDay
    {
        private DateTime date;
        private List<Taskgroup> taskgroups = new List<Taskgroup>();
        private bool fromSelectedMonth = true;
        public CalenderDay(DateTime date)
        {
            this.Date = date;
        }

        public DateTime Date { get => date; set => date = value; }
        public List<Taskgroup> Taskgroups { get => taskgroups; set => taskgroups = value; }
        public bool FromSelectedMonth { get => fromSelectedMonth; set => fromSelectedMonth = value; }
    }
}
