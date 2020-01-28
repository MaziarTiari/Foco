﻿using System;
using System.Windows.Controls;
using Foco.models;
using Foco.controls;
using System.Windows;
using System.Windows.Media;
using Foco.calendar;

namespace Foco.pages
{
    /// <summary>
    /// Interaktionslogik für CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : Page
    {
        private readonly MainWindow mainWindow;
        private readonly CalendarMonth calendarMonth;
        private Project project;

        public CalendarPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.calendarMonth = new CalendarMonth(DateTime.Today.Year, DateTime.Today.Month);
        }
        
        public MainWindow MainWindow => mainWindow;
        public Project Project { get => project; set { project = value; Update();} }
        public CalendarMonth CalendarMonth => calendarMonth;

        public void Update()
        {
            DayControlContainer.Children.Clear();
            CalendarMonth.Taskgroups = this.Project.Taskgroups;
            InitialCalendar();
            CalendarDay[] days = CalendarMonth.Days;
            int i = 0;
            for(int r = 0; r < 6; r++)
            {
                for (int c = 0; c < 7; c++)
                {
                    CalendarDayControl dayControl = new CalendarDayControl(days[i], this);
                    if( DateTime.Compare(days[i].Date, DateTime.Today) < 0 )
                    {
                        dayControl.DayInfoContainer.Background = Brushes.WhiteSmoke;
                        dayControl.DayOfDate.Foreground = Brushes.Black;
                    }
                    if (!days[i].FromSelectedMonth) {
                        dayControl.DayOfDate.Foreground = Brushes.DarkGray;
                        dayControl.DayOfDate.Text += (" " + CalendarMonth.MonthNames[days[i].Date.Month - 1]);
                    }
                    if (days[i].Date == DateTime.Today)
                        dayControl.DayInfoContainer.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FE9766"));
                    // Add dayControl to the CalendarPage
                    dayControl.SetValue(Grid.RowProperty, r);
                    dayControl.SetValue(Grid.ColumnProperty, c);
                    DayControlContainer.Children.Add(dayControl);
                    i++;
                }
            }
        }

        public void SetCurrentDate()
        {
            CalendarMonth.Year = DateTime.Today.Year;
            CalendarMonth.Month = DateTime.Today.Month;
        }

        // Show the month and year the Calendar is showing
        private void InitialCalendar()
        {
            MonthTag.Text = CalendarMonth.MonthNames[calendarMonth.Month - 1];
            YearTag.Text = Convert.ToString(CalendarMonth.Year);
        }

        private void LastYearHandler(object sender, RoutedEventArgs e)
        {
            CalendarMonth.Year = CalendarMonth.Year - 1;
            Update();
        }

        private void NextYearHandler(object sender, RoutedEventArgs e)
        {
            CalendarMonth.Year = CalendarMonth.Year + 1;
            Update();
        }

        private void LastMonthHandler(object sender, RoutedEventArgs e)
        {
            CalendarMonth.SetLastMonth();
            Update();
        }

        private void NextMonthHandler(object sender, RoutedEventArgs e)
        {
            CalendarMonth.SetNextMonth();
            Update();
        }
    }
}
