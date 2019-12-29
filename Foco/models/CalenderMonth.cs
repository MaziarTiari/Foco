using System;
using System.Collections.Generic;
using System.Windows;



namespace Foco.models
{
    public enum Months { Jan, Feb, März, Apr, Mai, Jun, Jul, Aug, Sep, Okt, Nov, Dez };

    public class CalenderMonth
    {
        private int month;
        private int year;
        private CalenderDay[] days = new CalenderDay[42];
        private List<Deadline> deadlines = new List<Deadline>();
        private Months[] months = { models.Months.Jan, models.Months.Feb, models.Months.März, models.Months.Apr, models.Months.Mai, models.Months.Jun, models.Months.Jul, models.Months.Aug, models.Months.Sep, models.Months.Okt, models.Months.Nov, models.Months.Dez };
        public CalenderMonth(int year, int month)
        {
            this.month = month;
            this.year = year;
            Update();
        }

        public int LastMonth()
        {
            if (Month == 1)
                return 12;
            else
                return Month - 1;
        }

        public int NextMonth()
        {
            if (Month == 12)
                return 1;
            else
                return Month + 1;
        }

        private void Update()
        {
            DateTime date = new DateTime(year, month, 1);
            int firstDayOfMonthIndex = date.DayOfWeek.GetHashCode();
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int daysInLastMonth = DateTime.DaysInMonth( year, LastMonth() );
            int daysFromMonthBefore = daysInLastMonth;

            for (int i = 0; i < Days.Length; i++)
            {
                if (i == 0 && firstDayOfMonthIndex > i)
                    daysFromMonthBefore = daysInLastMonth - firstDayOfMonthIndex;
                if (daysFromMonthBefore < daysInLastMonth)
                {
                    daysFromMonthBefore++;
                    if(Month == 1)
                        SetDay(i, new DateTime(Year -1, LastMonth() ,daysFromMonthBefore));
                    else
                        SetDay(i, new DateTime(Year - 1, LastMonth(), daysFromMonthBefore));
                    Days[i].FromSelectedMonth = false;
                }
                else
                {
                    if (i < daysInMonth + firstDayOfMonthIndex) // days for selected month
                        SetDay( i, new DateTime(Year, Month, (i + 1 - firstDayOfMonthIndex)) );
                    else // days are for the month after
                    {
                        if(Month == 12)
                            SetDay( i, new DateTime(Year + 1, NextMonth(), ((i + 1 - firstDayOfMonthIndex) % daysInMonth) ) );
                        else
                            SetDay(i, new DateTime(Year, NextMonth(), ((i + 1 - firstDayOfMonthIndex) % daysInMonth)));
                        Days[i].FromSelectedMonth = false;
                    }
                }
            }
        }

        private void SetDay(int index, DateTime date)
        {
            //MessageBox.Show(Convert.ToString(Deadlines.Count));
            Days[index] = new CalenderDay(date);
            if (Deadlines.Count < 1)
                return;
            foreach(Deadline deadline in Deadlines)
            {
                if ( deadline.Date == Days[index].Date && !(Days[index].Appointments.Contains(deadline.Title)) )
                {
                    Days[index].Appointments.Add(deadline.Title);
                    //MessageBox.Show(deadline.Title);
                }
            }
        }

        public void setNextMonth()
        {
            if (Month == 12)
                Year += 1;
            Month = NextMonth();
            Update();
        }

        public void setLastMonth()
        {
            if (Month == 1)
                Year -= 1;
            Month = LastMonth();
            Update();
        }

        public int Month
        {
            get => month; set
            {
                month = value;
                Update();
            }
        }
        public int Year
        {
            get => year;
            set
            {
                year = value;
                Update();
            }
        }
        internal CalenderDay[] Days { get => days; set => days = value; }
        public List<Deadline> Deadlines { get => deadlines; set { deadlines = value; Update(); } }

        public Months[] Months { get => months; set => months = value; }
    }
}
