using Foco.models;
using System;
using System.Collections.Generic;

namespace Foco.calendar
{
    public class CalendarMonth
    {
        private const string PREVIOUS_MONTH = "previousMonth";
        private const string SELECTED_MONTH = "selectedMonth";

        public static readonly string[] MonthNames = new string[] {
            "Jan", "Feb", "März", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez"
        };

        private int month;
        private int year;

        private int weekdayIndexAtMonthBegins;
        private CalendarDay[] days = new CalendarDay[42];
        private List<Taskgroup> taskgroups = new List<Taskgroup>();
        Dictionary<string, int> numberOfDays = new Dictionary<string, int>();
        public CalendarMonth(int year, int month)
        {
            this.month = month;
            this.year = year;
            Update();
        }

        public int Month
        {
            get => month;
            set
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

        public List<Taskgroup> Taskgroups { get => taskgroups; set { taskgroups = value; Update(); } }

        public CalendarDay[] Days { get => days; set => days = value; }

        public int PreviousMonth()
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
            DateTime date = new DateTime(this.year, this.Month, 1);
            weekdayIndexAtMonthBegins = date.DayOfWeek.GetHashCode();
            numberOfDays[SELECTED_MONTH] = DateTime.DaysInMonth(this.year, this.Month);
            numberOfDays[PREVIOUS_MONTH] = DateTime.DaysInMonth(YearOfPreviousMonth(), PreviousMonth());
            AddDaysOfPreviousMonth();
            AddDaysOfSelectedMonth();
            AddDaysOfNextMonth();
        }

        private void AddDaysOfPreviousMonth()
        {
            int d = numberOfDays[PREVIOUS_MONTH] - weekdayIndexAtMonthBegins + 1;
            AddDaysOfMonth(0, weekdayIndexAtMonthBegins, YearOfPreviousMonth(), PreviousMonth(), d, false);
        }

        private void AddDaysOfSelectedMonth()
        {
            int max = numberOfDays[SELECTED_MONTH] + weekdayIndexAtMonthBegins;
            AddDaysOfMonth(weekdayIndexAtMonthBegins, max, Year, Month, 1, true);
        }

        private void AddDaysOfNextMonth()
        {
            int index = numberOfDays[SELECTED_MONTH] + weekdayIndexAtMonthBegins;
            AddDaysOfMonth(index, Days.Length, YearOfNextMonth(), NextMonth(), 1, false);
        }

        private void AddDaysOfMonth(int index, int max , int y, int m, int startingDay, bool daysAreFromSelectedMonth)
        {
            for (int i = index; i < max; i++)
            {
                AddDay(i, new DateTime(y, m, startingDay));
                startingDay++;
                Days[i].FromSelectedMonth = daysAreFromSelectedMonth;
            }
        }

        private int YearOfPreviousMonth()
        {
            if (Month == 1)
                return year - 1;
            else
                return year;
        }

        private int YearOfNextMonth()
        {
            if (Month == 12)
                return year + 1;
            else
                return year;
        }

        private void AddDay(int index, DateTime date)
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
            Month = PreviousMonth();
            Update();
        }
    }
}
