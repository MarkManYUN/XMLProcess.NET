using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XmlProcess.NET2._0
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            // Handling the QueryControl event that will populate all automatically generated Documents
          //  this.tabbedView1.QueryControl += tabbedView1_QueryControl;
            // Handling the QueryControl event that will populate all automatically generated Documents
            this.windowsUIView1.QueryControl += windowsUIView1_QueryControl;
        }

        // Assigning a required content for each auto generated Document
        void tabbedView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {

            if (e.Document == xtraUserControl1Document)
                e.Control = new XmlProcess.NET2._0.XtraUserControl1();
            if (e.Document == xtraUserControl3Document)
                e.Control = new XmlProcess.NET2._0.XtraUserControl3();
            if (e.Document == xtraUserControl2Document)
                e.Control = new XmlProcess.NET2._0.XtraUserControl2();
            if (e.Control == null)
                e.Control = new System.Windows.Forms.Control();

        }

        // Assigning a required content for each auto generated Document
        void windowsUIView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        {

            if (e.Document == xtraUserControl1Document)
                e.Control = new XmlProcess.NET2._0.XtraUserControl1();
            if (e.Document == xtraUserControl3Document)
                e.Control = new XmlProcess.NET2._0.XtraUserControl3();
            if (e.Document == xtraUserControl2Document)
                e.Control = new XmlProcess.NET2._0.XtraUserControl2();
            if (e.Control == null)
                e.Control = new System.Windows.Forms.Control();
            if(e.Document == document1)
        e.Control = CreateSampleContent("Manually Created Doc 1",Color.FromArgb(0,99,177 ));
            if(e.Document == document2)
        e.Control = CreateSampleContent("Manually Created Doc 2",Color.FromArgb(231,72,86 ));
        }
        public LabelControl CreateSampleContent(string text, Color backcolor)
        {
            Font sampleFont = new Font(new FontFamily("Segoe UI Semibold"), 42f);

            LabelControl lc = new LabelControl()
            {
                AutoSizeMode = LabelAutoSizeMode.None,
                Dock = DockStyle.Fill,
                BackColor = backcolor,
                ForeColor = Color.FromArgb(40, 40, 40),
                Text = text,
                Font = sampleFont,
            };
            lc.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            lc.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            return lc;
        }
    }
}
