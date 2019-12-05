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

namespace Foco.ui
{
    /// <summary>
    /// Interaktionslogik für TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        private readonly TaskgroupControl taskgroupControl;

        public TaskgroupControl TaskgroupControl => taskgroupControl;

        public TaskControl(TaskgroupControl taskgroupControl)
        {
            InitializeComponent();
            this.taskgroupControl = taskgroupControl;
        }

        private void TaskTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void TaskTextBox_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                taskgroupControl.CreateTaskGui();
            }
        }
    }
}
