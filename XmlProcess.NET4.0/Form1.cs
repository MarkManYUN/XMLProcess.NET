using DevExpress.LookAndFeel;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XmlProcess.NET4._0
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
            navigationPage1.Tag = navBarGroup1.Caption = navigationPage1.Caption = "Employees";
            navigationPage2.Tag = navBarGroup2.Caption = navigationPage2.Caption = "Customers";
        }

        private void navBarControl1_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            navigationFrame1.SelectedPage = (NavigationPage)navigationFrame1.Pages.FindFirst
                (x => (string)x.Tag == e.Group.Caption);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            navBarControl1.ActiveGroup = navBarControl1.Groups.First(x => x.Caption == e.Link.Caption);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            navBarControl1.ActiveGroup = navBarControl1.Groups.First(x => x.Caption == e.Link.Caption);

        }

        private void gridControl1_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = new List<int> { 1, 2, 3 }; 
        }
        void PreviewPrintableComponent(IPrintable component, UserLookAndFeel lookAndFeel)
        {
            //创建一个链接，将打印控制。
            PrintableComponentLink link = new PrintableComponentLink()
            {
                PrintingSystemBase = new PrintingSystemBase(),
                Component = component,
                Landscape = true,
                PaperKind = PaperKind.A4,
                Margins = new Margins(20, 20, 20, 20)
            };
            link.CreateReportHeaderArea += link_CreateReportHeaderArea;
            //直接通过目录保存PDF，预览 
            //link.ExportToPdf(@"E:\CShapWorkSpace\XMLProcess.NET\1.pdf");
            //link.ExportToImage(@"E:\CShapWorkSpace\XMLProcess.NET\1.jpg");
             
            //显示报告 
            link.ShowRibbonPreview(lookAndFeel);
        }
        private void link_CreateReportHeaderArea(object sender,CreateAreaEventArgs e)
        {
            string reportHeader = "Categories Report";
            e.Graph.StringFormat = new BrickStringFormat(StringAlignment.Center);
            e.Graph.Font = new Font("Tahoma", 14, FontStyle.Bold);
            RectangleF rec = new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 50);
            e.Graph.DrawString(reportHeader, Color.Black, rec, BorderSide.None);
        }

        private void print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PreviewPrintableComponent(this.gridControl1, gridControl1.LookAndFeel);
        }
    }
}
