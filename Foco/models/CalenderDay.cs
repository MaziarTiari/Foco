using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foco.models
{
    class CalenderDay
    {
        private DateTime date;
        private List<string> appointments = new List<string>();
        private bool fromSelectedMonth = true;
        public CalenderDay(DateTime date)
        {
            this.Date = date;
        }

        public DateTime Date { get => date; set => date = value; }
        public List<string> Appointments { get => appointments; set => appointments = value; }
        public bool FromSelectedMonth { get => fromSelectedMonth; set => fromSelectedMonth = value; }
    }
}
