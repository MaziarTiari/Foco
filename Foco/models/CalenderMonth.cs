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

        public void AddOrRaplaceDeadline(Deadline deadline)
        {
            int index = Deadlines.FindIndex(d => d.Title == deadline.Title);
            if (index <= -1)
                Deadlines.Add(deadline);
            else
                Deadlines[index] = deadline;
            Update();
        }

        private void Update()
        {
            DateTime date = new DateTime(year, month, 1);
            int firstDayOfMonthIndex = date.DayOfWeek.GetHashCode(); // Check with which day of the Week selected month starts
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int daysInLastMonth = DateTime.DaysInMonth( year, LastMonth() );
            int daysShowingFromMonthBefore = daysInLastMonth;

            for (int i = 0; i < Days.Length; i++) // Calender has 7 columns and 6 rows which means 42 days to show
            {
                if (i == 0 && firstDayOfMonthIndex > i) // if first day of the month is not a sunday
                    daysShowingFromMonthBefore = daysInLastMonth - firstDayOfMonthIndex; // check how many days to show from last month
                if (daysShowingFromMonthBefore < daysInLastMonth) // continue with days from last month till we reach end of the last month
                {
                    daysShowingFromMonthBefore++;
                    if(Month == 1)
                        SetDay(i, new DateTime(Year -1, LastMonth() ,daysShowingFromMonthBefore));
                    else
                        SetDay(i, new DateTime(Year, LastMonth(), daysShowingFromMonthBefore));
                    Days[i].FromSelectedMonth = false;
                }
                else
                {
                    if (i < daysInMonth + firstDayOfMonthIndex) // set days for selected month
                        SetDay( i, new DateTime(Year, Month, (i + 1 - firstDayOfMonthIndex)) );
                    else // set days for the month after
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
            Days[index] = new CalenderDay(date);
            if (Deadlines.Count < 1)
                return;
            foreach(Deadline deadline in Deadlines)
            {
                if ( deadline.Date == Days[index].Date && !(Days[index].Appointments.Contains(deadline.Title)) )
                {
                    Days[index].Appointments.Add(deadline.Title);
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
