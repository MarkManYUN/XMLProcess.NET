using DevExpress.XtraBars.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XmlProcess.Net3._2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            navigationPage1.Tag = navBarGroup1.Caption = navigationPage1.Caption = "Employees";
            navigationPage2.Tag = navBarGroup2.Caption = navigationPage2.Caption = "Customers";
        }

        private void navigationPage2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void navigationPage1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void navBarControl1_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            navigationFrame1.SelectedPage =  (NavigationPage)navigationFrame1.Pages.FindFirst
                (x => (string)x.Tag == e.Group.Caption);
        }
    }
}
