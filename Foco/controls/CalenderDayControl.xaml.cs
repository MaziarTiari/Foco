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

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für CalenderDayControl.xaml
    /// </summary>
    public partial class CalenderDayControl : UserControl
    {
        public CalenderDayControl(int date)
        {
            InitializeComponent();

            Date.Text = Convert.ToString(date);
            
        }
    }
}
