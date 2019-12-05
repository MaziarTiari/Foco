using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Foco.models;
using Foco.pages;

namespace Foco.ui
{
    /// <summary>
    /// Interaktionslogik für TaskgroupControl.xaml
    /// </summary>
    public partial class TaskgroupControl : UserControl
    {
        private readonly List<TaskControl> taskControls = new List<TaskControl>();
        private readonly ListPage listPage;

        public List<TaskControl> TaskControls => taskControls;

        //private Taskgroup taskgroup = new Taskgroup();
        public TaskgroupControl(string taskgroupTitle, ListPage listPage)
        {
            InitializeComponent();
            this.listPage = listPage;
            TaskgroupHeader.Content = taskgroupTitle;
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void SetSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public void CreateTaskGuiByMouse(object sender, RoutedEventArgs e)
        {
            CreateTaskGui();
        }
        public void CreateTaskGui()
        {
            TaskControl taskControl = new TaskControl(this);
            TaskContainer.Children.Add(taskControl);
            this.TaskControls.Add(taskControl);
            this.Height =+ taskControl.Height;
        }

        public void setCheckBoxes(object sender, MouseEventArgs e)
        {
            if (MainCheckBox.IsChecked == true)
            {

            }
        }
    }
}
