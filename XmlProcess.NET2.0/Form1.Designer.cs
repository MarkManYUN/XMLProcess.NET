namespace XmlProcess.NET2._0
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager();
            this.windowsUIView1 = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.WindowsUIView();
            this.titleContainer = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.TileContainer();
            this.XtraUserControl1Tile = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile();
            this.xtraUserControl1Document = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document();
            this.XtraUserControl3Tile = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile();
            this.xtraUserControl3Document = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document();
            this.XtraUserControl2Tile = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile();
            this.xtraUserControl2Document = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document();
            this.document1 = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document();
            this.document1Tile = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile();
            this.document2 = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document();
            this.document2Tile = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile();
            this.slideGroup1 = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.SlideGroup();
            this.slideGroup2 = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.SlideGroup();
            this.pageGroup1 = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.PageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsUIView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XtraUserControl1Tile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraUserControl1Document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XtraUserControl3Tile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraUserControl3Document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XtraUserControl2Tile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraUserControl2Document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1Tile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.document2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.document2Tile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slideGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slideGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pageGroup1)).BeginInit();
            this.SuspendLayout();
            // 
            // documentManager1
            // 
            this.documentManager1.ContainerControl = this;
            this.documentManager1.View = this.windowsUIView1;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.windowsUIView1});
            // 
            // windowsUIView1
            // 
            this.windowsUIView1.ContentContainers.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.IContentContainer[] {
            this.titleContainer,
            this.slideGroup1,
            this.slideGroup2,
            this.pageGroup1});
            this.windowsUIView1.Documents.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseDocument[] {
            this.xtraUserControl1Document,
            this.xtraUserControl3Document,
            this.xtraUserControl2Document,
            this.document1,
            this.document2});
            this.windowsUIView1.Tiles.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.BaseTile[] {
            this.XtraUserControl1Tile,
            this.XtraUserControl3Tile,
            this.XtraUserControl2Tile,
            this.document1Tile,
            this.document2Tile});
            // 
            // titleContainer
            // 
            this.titleContainer.ActivationTarget = this.pageGroup1;
            this.titleContainer.Caption = "Tile";
            this.titleContainer.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.BaseTile[] {
            this.XtraUserControl1Tile,
            this.XtraUserControl3Tile,
            this.XtraUserControl2Tile,
            this.document1Tile,
            this.document2Tile});
            this.titleContainer.Name = "titleContainer";
            this.titleContainer.Subtitle = "title";
            // 
            // XtraUserControl1Tile
            // 
            this.XtraUserControl1Tile.ActivationTarget = this.pageGroup1;
            this.XtraUserControl1Tile.Document = this.xtraUserControl1Document;
            this.XtraUserControl1Tile.Name = "XtraUserControl1Tile";
            // 
            // xtraUserControl1Document
            // 
            this.xtraUserControl1Document.Caption = "标题1 windows UI";
            this.xtraUserControl1Document.ControlName = "XtraUserControl1";
            this.xtraUserControl1Document.ControlTypeName = "XmlProcess.NET2._0.XtraUserControl1";
            // 
            // XtraUserControl3Tile
            // 
            this.XtraUserControl3Tile.ActivationTarget = this.pageGroup1;
            this.XtraUserControl3Tile.Document = this.xtraUserControl3Document;
            this.titleContainer.SetID(this.XtraUserControl3Tile, 1);
            this.XtraUserControl3Tile.Name = "XtraUserControl3Tile";
            // 
            // xtraUserControl3Document
            // 
            this.xtraUserControl3Document.Caption = "XtraUserControl3";
            this.xtraUserControl3Document.ControlName = "XtraUserControl3";
            this.xtraUserControl3Document.ControlTypeName = "XmlProcess.NET2._0.XtraUserControl3";
            // 
            // XtraUserControl2Tile
            // 
            this.XtraUserControl2Tile.ActivationTarget = this.pageGroup1;
            this.XtraUserControl2Tile.Document = this.xtraUserControl2Document;
            this.titleContainer.SetID(this.XtraUserControl2Tile, 2);
            this.XtraUserControl2Tile.Name = "XtraUserControl2Tile";
            // 
            // xtraUserControl2Document
            // 
            this.xtraUserControl2Document.Caption = "XtraUserControl2";
            this.xtraUserControl2Document.ControlName = "XtraUserControl2";
            this.xtraUserControl2Document.ControlTypeName = "XmlProcess.NET2._0.XtraUserControl2";
            // 
            // document1
            // 
            this.document1.Caption = "document1";
            this.document1.ControlName = "document1";
            // 
            // document1Tile
            // 
            this.document1Tile.ActivationTarget = this.slideGroup2;
            this.document1Tile.Document = this.document1;
            this.titleContainer.SetID(this.document1Tile, 3);
            this.document1Tile.Name = "document1Tile";
            // 
            // document2
            // 
            this.document2.Caption = "document2";
            this.document2.ControlName = "document2";
            // 
            // document2Tile
            // 
            this.document2Tile.ActivationTarget = this.slideGroup1;
            this.document2Tile.Document = this.document2;
            this.titleContainer.SetID(this.document2Tile, 4);
            this.document2Tile.Name = "document2Tile";
            // 
            // slideGroup1
            // 
            this.slideGroup1.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document[] {
            this.document1,
            this.document2});
            this.slideGroup1.Name = "slideGroup1";
            this.slideGroup1.Parent = this.titleContainer;
            // 
            // slideGroup2
            // 
            this.slideGroup2.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document[] {
            this.xtraUserControl2Document,
            this.xtraUserControl1Document,
            this.xtraUserControl3Document,
            this.document1,
            this.document2});
            this.slideGroup2.Name = "slideGroup2";
            this.slideGroup2.Parent = this.titleContainer;
            // 
            // pageGroup1
            // 
            this.pageGroup1.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document[] {
            this.xtraUserControl1Document,
            this.xtraUserControl3Document,
            this.xtraUserControl2Document});
            this.pageGroup1.Name = "pageGroup1";
            this.pageGroup1.Parent = this.titleContainer;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1253, 523);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsUIView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleContainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XtraUserControl1Tile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraUserControl1Document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XtraUserControl3Tile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraUserControl3Document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XtraUserControl2Tile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraUserControl2Document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.document1Tile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.document2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.document2Tile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slideGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slideGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pageGroup1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup documentGroup1;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.WindowsUIView windowsUIView1;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.TileContainer titleContainer;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile XtraUserControl1Tile;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile XtraUserControl3Tile;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile XtraUserControl2Tile;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document xtraUserControl1Document;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document xtraUserControl3Document;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document xtraUserControl2Document;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile document1Tile;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document document1;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile document2Tile;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document document2;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.PageGroup pageGroup1;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.SlideGroup slideGroup1;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.SlideGroup slideGroup2;
    }
}

