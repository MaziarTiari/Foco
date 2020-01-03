using System;
using System.Windows.Controls;
using Foco.models;
using Foco.controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Foco.pages
{
    /// <summary>
    /// Interaktionslogik für CalendarPage.xaml
    /// </summary>
    public partial class CalenderPage : Page
    {
        private readonly MainWindow mainWindow;
        private readonly CalenderMonth calenderMonth;
        private Project project;
        private readonly DateTime today = DateTime.Today;

        public CalenderPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.calenderMonth = new CalenderMonth(Today.Year, Today.Month);
        }
        
        public MainWindow MainWindow => mainWindow;
        public Project Project { get => project; set { project = value; Update();} }
        public CalenderMonth CalenderMonth => calenderMonth;
        public DateTime Today  => today;

        private void UpdateDeadlines()
        {
            if (this.Project.Taskgroups.Count < 1)
                return;
            CalenderMonth.Taskgroups = this.Project.Taskgroups;
        }

        public void Update()
        {
            DayControlContainer.Children.Clear();
            UpdateDeadlines();
            InitialCalender();
            CalenderDay[] days = CalenderMonth.Days;
            int i = 0;
            for(int r = 1; r < 7; r++)
            {
                for (int c = 0; c < 7; c++)
                {
                    CalenderDayControl dayControl = new CalenderDayControl(days[i], this);
                    if( DateTime.Compare(days[i].Date, Today) < 0 )
                    {
                        SetViewForPastDays(dayControl);
                    }
                    if (!days[i].FromSelectedMonth) {
                        dayControl.DayOfDate.Foreground = Brushes.DarkGray;
                        dayControl.DayOfDate.Text += (" " + CalenderMonth.Months[days[i].Date.Month - 1]);
                    }
                    if (IsToday(days[i].Date))
                        dayControl.DayInfoContainer.Background = Brushes.LightSkyBlue;
                    // Add dayControl to the CalenderPage
                    dayControl.SetValue(Grid.RowProperty, r);
                    dayControl.SetValue(Grid.ColumnProperty, c);
                    DayControlContainer.Children.Add(dayControl);
                    i++;
                }
            }
        }

        public void setCurrentDate()
        {
            CalenderMonth.Year = Today.Year;
            CalenderMonth.Month = Today.Month;
        }

        // Show the month and year the Calender is showing
        private void InitialCalender()
        {
            MonthTag.Text = Convert.ToString(CalenderMonth.Month);
            YearTag.Text = Convert.ToString(CalenderMonth.Year);
        }

        // Set diffrent style for days in the past
        private void SetViewForPastDays(CalenderDayControl dayControl)
        {
            dayControl.DayInfoContainer.Background = Brushes.WhiteSmoke;
            dayControl.DayInfoContainer.BorderBrush = Brushes.White;
            dayControl.DayOfDate.Foreground = Brushes.DarkSlateGray;
        }

        private bool IsToday(DateTime day)
        {
            if (day == Today)
                return true;
            else
                return false;
        }

        private void LastYearHandler(object sender, RoutedEventArgs e)
        {
            CalenderMonth.Year = CalenderMonth.Year - 1;
            Update();
        }

        private void NextYearHandler(object sender, RoutedEventArgs e)
        {
            CalenderMonth.Year = CalenderMonth.Year + 1;
            Update();
        }

        private void LastMonthHandler(object sender, RoutedEventArgs e)
        {
            CalenderMonth.setLastMonth();
            Update();
        }

        private void NextMonthHandler(object sender, RoutedEventArgs e)
        {
            CalenderMonth.setNextMonth();
            Update();
        }
    }
}
