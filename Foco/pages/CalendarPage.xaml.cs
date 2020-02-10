using System;
using System.Windows.Controls;
using Foco.models;
using Foco.controls;
using System.Windows;
using System.Windows.Media;
using Foco.calendar;

namespace Foco.pages
{
    // interaction logic for CalendarPage.xaml
    public partial class CalendarPage : Page
    {
        private readonly MainWindow mainWindow;
        private readonly CalendarMonth calendarMonth;
        private Project project;

        public CalendarPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.calendarMonth = new CalendarMonth( DateTime.Today.Year,
                                                    DateTime.Today.Month );
        }
        
        public MainWindow MainWindow => mainWindow;
        public Project Project
        {
            get => project;
            set { project = value; Update();}
        }
        public CalendarMonth CalendarMonth => calendarMonth;

        public void Update()
        {
            DayControlContainer.Children.Clear();
            CalendarMonth.Taskgroups = this.Project.Taskgroups;
            SetCalendarInfo();
            CalendarDay[] days = CalendarMonth.Days;
            int i = 0;
            for (int r = 0; r < 6; r++)
            {
                for (int c = 0; c < 7; c++)
                {
                    CalendarDayControl dayControl = new CalendarDayControl(
                            days[i], this
                        );
                    if( DateTime.Compare(days[i].Date, DateTime.Today) < 0 )
                    {
                        dayControl.DayInfoContainer.Background = Brushes.WhiteSmoke;
                        dayControl.DayOfDate.Foreground = Brushes.Black;
                    }
                    if (!days[i].FromSelectedMonth) {
                        dayControl.DayOfDate.Foreground = Brushes.DarkGray;
                        // set its monthname after days number
                        dayControl.DayOfDate
                            .Text += (
                                    " " + CalendarMonth.MonthNames[days[i].Date.Month - 1]
                                );
                    }
                    if (days[i].Date == DateTime.Today)
                        dayControl.DayInfoContainer.Background = new SolidColorBrush(
                                (Color) ColorConverter.ConvertFromString("#FE9766")
                            );
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

        private void SetCalendarInfo()
        {
            MonthTag.Text = CalendarMonth.MonthNames[calendarMonth.Month - 1];
            YearTag.Text = Convert.ToString(CalendarMonth.Year);
        }

        private void YearChangedHandler(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            
            if(s.Name == "PreviousYear")
                CalendarMonth.Year = CalendarMonth.Year - 1;

            if(s.Name == "NextYear")
                CalendarMonth.Year = CalendarMonth.Year + 1;
            
            Update();
        }

        private void ChangedMonthHandler(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            if(s.Name == "PreviousMonth")
                CalendarMonth.ChangeToPreviousMonth();

            if (s.Name == "NextMonth")
                CalendarMonth.ChangeToNextMonth();

            Update();
        }
    }
}
