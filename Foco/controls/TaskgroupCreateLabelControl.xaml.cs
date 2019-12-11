using Foco.pages;
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
    /// Interaktionslogik für TaskgroupCreateLabelControl.xaml
    /// </summary>
    public partial class TaskgroupCreateLabelControl : UserControl
    {
        private readonly ListPage listPage;
        public TaskgroupCreateLabelControl(ListPage listPage)
        {
            InitializeComponent();
            this.listPage = listPage;
        }

        public ListPage ListPage => listPage;

        public void CreateTaskgroupGui(object sender, MouseButtonEventArgs e)
        {
            listPage.RequestNewTaskgroupByListPage();
        }
    }
}
