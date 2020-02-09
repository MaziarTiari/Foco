using Foco.models;
using System;
using System.Collections.Generic;

namespace Foco.calendar
{

    public class CalendarMonth
    {

        public static readonly string[] MonthNames = new string[] { "Jan", "Feb", "März", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez" };

        private int month;
        private int year;
        private CalendarDay[] days = new CalendarDay[42];
        private List<Taskgroup> taskgroups = new List<Taskgroup>();
        public CalendarMonth(int year, int month)
        {
            this.month = month;
            this.year = year;
            Update();
        }

        public CalendarDay[] Days { get => days; set => days = value; }
        public List<Taskgroup> Taskgroups { get => taskgroups; set { taskgroups = value; Update(); } }

        public int LastMonth()
        {
            if (Month == 1) return 12;
            else return Month - 1;
        }

        public int NextMonth()
        {
            if (Month == 12) return 1;
            else return Month + 1;
        }

        private void Update()
        {
            DateTime date = new DateTime(this.year, this.month, 1);
            int firstDayOfMonthIndex = date.DayOfWeek.GetHashCode(); // Check with which day of the Week selected month starts
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int daysInLastMonth = DateTime.DaysInMonth( year, LastMonth() );
            int daysShowingFromMonthBefore = daysInLastMonth;

            for (int i = 0; i < Days.Length; i++) // Calender has 7 columns and 6 rows which means 42 days to show
            {
                if (i == 0 && firstDayOfMonthIndex > i) // if first day of the month is not the first day of the weak
                    daysShowingFromMonthBefore = daysInLastMonth - firstDayOfMonthIndex; // check how many days to show from last month
                if (daysShowingFromMonthBefore < daysInLastMonth) // continue with days from last month til we reach end of the last month
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
                            SetDay( i, new DateTime(Year + 1, NextMonth(), ((i + 1 - firstDayOfMonthIndex) % daysInMonth)) );
                        else
                            SetDay(i, new DateTime(Year, NextMonth(), ((i + 1 - firstDayOfMonthIndex) % daysInMonth)));
                        Days[i].FromSelectedMonth = false;
                    }
                }
            }
        }

        private void SetDay(int index, DateTime date)
        {
            Days[index] = new CalendarDay(date);
            if (Taskgroups.Count < 1)
                return;
            foreach(Taskgroup taskgroup in Taskgroups)
            {
                if ( taskgroup.Deadline.Date == Days[index].Date )
                {
                    Days[index].Taskgroups.Add(taskgroup);
                }
            }
        }

        public void SetNextMonth()
        {
            if (Month == 12) Year++;
            Month = NextMonth();
            Update();
        }

        public void SetLastMonth()
        {
            if (Month == 1) Year --;
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
    }
}
