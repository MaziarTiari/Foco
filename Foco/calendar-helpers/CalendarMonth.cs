using Foco.models;
using System;
using System.Collections.Generic;

namespace Foco.calendar
{
    public class CalendarMonth
    {
        public static readonly string[] MonthNames = new string[] {
            "Jan", "Feb", "März", "Apr", "Mai", "Jun",
            "Jul", "Aug", "Sep", "Okt", "Nov", "Dez"
        };

        private int month;
        private int year;
        private int monthsFirstWeekdayIndex;
        private CalendarDay[] days = new CalendarDay[42];
        private List<Taskgroup> taskgroups = new List<Taskgroup>();
        public CalendarMonth(int year, int month)
        {
            this.month = month;
            this.year = year;
            Update();
        }

        public int Month
        {
            get => month;
            set { month = value; Update(); }
        }
        public int Year
        {
            get => year;
            set { year = value; Update(); }
        }
        public List<Taskgroup> Taskgroups
        { 
            get => taskgroups;
            set { taskgroups = value; Update(); }
        }
        public CalendarDay[] Days { get => days; set => days = value; }

        private int PreviousMonth()
        {
            if (Month == 1) return 12;
            else return Month - 1;
        }

        private int NextMonth()
        {
            if (Month == 12) return 1;
            else return Month + 1;
        }

        private void Update()
        {
            DateTime startDateOfMonth = new DateTime(Year, Month, 1);
            monthsFirstWeekdayIndex = startDateOfMonth.DayOfWeek.GetHashCode();
            int numberOfDaysInSelectedMonth = DateTime.DaysInMonth(Year, Month);
            int numberOfDaysInPreviousMonth =
                DateTime.DaysInMonth(YearOfPreviousMonth(), PreviousMonth());

            int startingIndex, endingIndex, startingDay;
            // Add days from previous month
            startingIndex = 0;
            endingIndex = monthsFirstWeekdayIndex;
            startingDay = numberOfDaysInPreviousMonth - monthsFirstWeekdayIndex + 1;
            AddDaysOfMonth(
                startingIndex, endingIndex, YearOfPreviousMonth(), PreviousMonth(),
                startingDay, false);

            // Add days from selected month
            startingIndex = endingIndex;
            endingIndex = numberOfDaysInSelectedMonth + endingIndex;
            AddDaysOfMonth(startingIndex, endingIndex, Year, Month, 1, true);

            // Add days from next month
            startingIndex = endingIndex;
            endingIndex = Days.Length;
            AddDaysOfMonth(
                startingIndex, endingIndex, YearOfNextMonth(), NextMonth(), 1, false);
        }

        private void AddDaysOfMonth(int index, int max , int y, int m,
                                    int startingDay, bool fromSelectedMonth)
        {
            for (int i = index; i < max; i++)
            {
                AddDay(i, new DateTime(y, m, startingDay));
                startingDay++;
                Days[i].FromSelectedMonth = fromSelectedMonth;
            }
        }

        private int YearOfPreviousMonth()
        {
            if (Month == 1)
                return Year - 1;
            else
                return Year;
        }

        private int YearOfNextMonth()
        {
            if (Month == 12)
                return Year + 1;
            else
                return Year;
        }

        private void AddDay(int index, DateTime date)
        {
            Days[index] = new CalendarDay(date);
            var taskgroups = Taskgroups.FindAll(t => t.Deadline.Date == date);

            if (taskgroups.Count > 0)
                foreach (Taskgroup taskgroup in taskgroups)
                    Days[index].Taskgroups.Add(taskgroup);
        }

        public void ChangeToNextMonth()
        {
            Year = YearOfNextMonth();
            Month = NextMonth();
            Update();
        }

        public void ChangeToPreviousMonth()
        {
            Year = YearOfPreviousMonth();
            Month = PreviousMonth();
            Update();
        }
    }
}