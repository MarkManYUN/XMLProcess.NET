using DevExpress.LookAndFeel;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using DevExpress.XtraLayout;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace XmlProcess5._0
{
    public partial class FormXmlPress5 : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        OpenFileDialog openFileDialog = new OpenFileDialog();
        List<Communication> modelList = new List<Communication>();
        String protocal_Path = null;
        XmlDocument xmlDoc = new XmlDocument();

        static Dictionary<string, string> CommFieldList = new Dictionary<string, string>();//公共字段
        static Dictionary<string, string> SokectCustomProtocolList = new Dictionary<string, string>();//socket协议字段
        static Dictionary<string, string> SokectModbusProtocolList = new Dictionary<string, string>();//Modbus协议字段
        static Dictionary<string, string> VDRProtocolList = new Dictionary<string, string>();//VDR协议字段
        static Dictionary<string, string> ProtocolOtherList = new Dictionary<string, string>();//其他协议字段

        List<XmlModbus> XmlModbusList = new List<XmlModbus>();//modbus协议专属
        List<BaseEdit> Page2TXTBaseEdit = new List<BaseEdit>();
        public FormXmlPress5()
        {
            InitializeComponent();
            navigationPage1.Tag = navigationPage1.Caption = "通讯配置";
            navigationPage2.Tag = navigationPage2.Caption = "协议配置";
            navigationPage3.Tag = navigationPage3.Caption = "通道配置";
            navigationPage4.Tag = navigationPage4.Caption = "协议字段配置";

            gridView1.OptionsView.ColumnAutoWidth = true;//自动宽度
            gridView1.OptionsView.ShowGroupPanel = false;//去掉主面板
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;//一次选择一行

            gridView2.OptionsView.ColumnAutoWidth = true;
            gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;

            gridView3.OptionsView.ColumnAutoWidth = true;
            gridView3.OptionsView.ShowGroupPanel = false;
            this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;

            #region
            if (navigationFrame1.SelectedPage.Caption.Trim().Equals("协议字段配置"))
            {
                if (first_Flush_Flag)
                {
                    CreateProtocolLayout();
                }
                else
                {
                    //清空原来的控件信息
                    layoutControl3.BeginUpdate();
                    layoutControl3.Controls.Clear();
                    layoutControl3.Root.Items.Clear();
                    CreateProtocolLayout();
                    dataOpration.EndUpdate();
                }
            }
            #endregion
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            navigationFrame1.SelectedPage = (NavigationPage)navigationFrame1.Pages.FindFirst
                (x => (string)x.Tag == e.Item.Caption);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            navigationFrame1.SelectedPage = (NavigationPage)navigationFrame1.Pages.FindFirst
                (x => (string)x.Tag == e.Item.Caption);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            navigationFrame1.SelectedPage =
                (NavigationPage)navigationFrame1.Pages.FindFirst(x => (string)x.Tag == e.Item.Caption);

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

            //显示报告 
            link.ShowRibbonPreview(lookAndFeel);
        }
        private void link_CreateReportHeaderArea(object sender, CreateAreaEventArgs e)
        {
            string reportHeader = "";
            if (navigationFrame1.SelectedPage.Caption.Trim().Equals("协议配置"))
            {
                reportHeader = "协议配置报告";
            }
            else if (navigationFrame1.SelectedPage.Caption.Trim().Equals("通道配置"))
            {
                reportHeader = "通道配置报告";
            }
            else
            {
                reportHeader = "通讯配置报告";
            }
            e.Graph.StringFormat = new BrickStringFormat(StringAlignment.Center);
            e.Graph.Font = new Font("Tahoma", 14, FontStyle.Bold);
            RectangleF rec = new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 50);
            e.Graph.DrawString(reportHeader, Color.Black, rec, BorderSide.None);

        }
        /// <summary>
        /// 打印配置表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //判断当前打开的页数，判断加载的是什么程序
            // 1. 通讯配置
            // 2. 协议配置
            // 3. 通道配置
            if (navigationFrame1.SelectedPage.Caption.Trim().Equals("协议配置"))
            {
                PreviewPrintableComponent(this.page2_gridview2, Page1_CommunicationConfig.LookAndFeel);//打印
            }
            else if (navigationFrame1.SelectedPage.Caption.Trim().Equals("通道配置"))
            {
                PreviewPrintableComponent(this.Page3_Address, Page1_CommunicationConfig.LookAndFeel);//打印

            }
            else
            {
                PreviewPrintableComponent(this.Page1_CommunicationConfig, Page1_CommunicationConfig.LookAndFeel);
            }
        }
        /// <summary>
        /// 打开xml配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();
            //  backgroundWorker1_DoWork(sender, null);
            // backgroundWorker1.RunWorkerAsync();

        }
        /// <summary>
        　　/// C# Winform打开文件夹（打开窗口）
        　　/// </summary>
        　　/// <param name="path">路径</param>
        public void OpenFolder(string path)
        {
            Process.Start(path);//路径中有中文，需要加双引号
        }
        /// <summary>
        /// 刷新dataGridView数据
        /// </summary>
        public void Reflash()
        {

            if (modelList is null) return;
            //通过modellist获取到新数组解决数据实时刷新问题
            this.Page1_CommunicationConfig.DataSource = modelList.ToArray();

        }

        //协议配置地址路径
        string protocolPath = "";
        /// <summary>
        /// 读XML配置文件
        /// </summary>
        private void ReadXml()
        {


            //判断当前打开的页数，判断加载的是什么程序
            // 1. 通讯配置
            // 2. 协议配置
            // 3. 通道配置
            if (navigationFrame1.SelectedPage.Caption.Trim().Equals("协议配置"))
            {
                this.page2_gridview2.DataSource = null;
                XmlModbusList = new List<XmlModbus>();

                prefixName = "page2_txt_";
                //执行加载XML字段配置文件操作
                //没有加载过为False
                if (!flage_Field)
                {
                    LoadXMLField("./xmlField.xml");
                    flage_Field = true;
                }
                if (protocolPath.Equals(""))
                {
                    protocolPath = openFileDialog.FileName;
                } 
                //通讯配置
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(protocolPath, settings);
                xmlDoc.Load(reader);
                // 得到根节点bookstore
                XmlNode xn = xmlDoc.SelectSingleNode("Communications");


                // 得到根节点的所有子节点
                XmlNodeList xnl = xn.ChildNodes;
                string nameStr =protocolPath.Substring( protocolPath.LastIndexOf("\\")+1);
                #region SocektModbus协议 
                foreach (XmlNode xn1 in xnl)
                {
                    // 得到根节点的所有子节点
                    XmlNodeList xnn = xn1.ChildNodes;
                    foreach (XmlElement item in xnn)
                    { 
                        page2_txt_ID.Text = CommFieldList["ID"] = item.GetAttribute("ID").ToString();
                        page2_txt_CommReConnectTime.Text = CommFieldList["CommReConnectTime"] = item.GetAttribute("CommReConnectTime").ToString();
                        page2_txt_CommErroTime.Text = CommFieldList["CommErroTime"] = item.GetAttribute("CommErroTime").ToString();
                        page2_txt_WriteTimeOut.Text = CommFieldList["WriteTimeOut"] = item.GetAttribute("WriteTimeOut").ToString();
                        page2_txt_ReadTimeOut.Text = CommFieldList["ReadTimeOut"] = item.GetAttribute("ReadTimeOut").ToString();

                        page2_txt_CommSpaceTime.Text = CommFieldList["CommSpaceTime"] = item.GetAttribute("CommSpaceTime").ToString();
                        page2_txt_ConfigFilePath.Text = CommFieldList["ConfigFilePath"] = item.GetAttribute("ConfigFilePath").ToString();

                        page2_txt_EnglishiName.Text = CommFieldList["EnglishiName"] = item.GetAttribute("EnglishiName").ToString();
                        page2_txt_ChineseName.Text = CommFieldList["ChineseName"] = item.GetAttribute("ChineseName").ToString();

                        XmlNodeList nodes = item.ChildNodes;
                        foreach (XmlElement inode in nodes)
                        {
                            string type = inode.Name;
                            if (type.Trim().Equals("TCP"))
                            {
                                SokectModbusProtocolList["Can2IP"] = inode.GetAttribute("Can2IP").ToString();
                                SokectModbusProtocolList["Can2Port"] = inode.GetAttribute("Can2Port").ToString();
                                SokectModbusProtocolList["Can1IP"] = inode.GetAttribute("Can1IP").ToString();
                                SokectModbusProtocolList["Can1Port"] = inode.GetAttribute("Can1Port").ToString();

                                XmlNodeList commconfig = inode.ChildNodes;
                                foreach (XmlElement config in commconfig)
                                {

                                    string protocolType = config.Name;
                                    if (protocolType.Trim().Equals("CommCongfig"))
                                    {
                                        XmlModbus modbus = new XmlModbus();
                                        modbus.Can2IP = SokectModbusProtocolList["Can2IP"];
                                        modbus.Can2Port = SokectModbusProtocolList["Can2Port"];
                                        modbus.Can1IP = SokectModbusProtocolList["Can1IP"];
                                        modbus.Can1Port = SokectModbusProtocolList["Can1Port"];

                                        modbus.ID = SokectModbusProtocolList["ID"] = config.GetAttribute("ID").ToString();
                                        modbus.SlaveID = SokectModbusProtocolList["SlaveID"] = config.GetAttribute("SlaveID").ToString();
                                        modbus.FirstAddress = SokectModbusProtocolList["FirstAddress"] = config.GetAttribute("FirstAddress").ToString();
                                        modbus.FunctionCode = SokectModbusProtocolList["FunctionCode"] = config.GetAttribute("FunctionCode").ToString();
                                        modbus.RequestCount = SokectModbusProtocolList["RequestCount"] = config.GetAttribute("RequestCount").ToString();
                                        modbus.DataLenght = SokectModbusProtocolList["DataLenght"] = config.GetAttribute("DataLenght").ToString();
                                        XmlModbusList.Add(modbus);
                                    }
                                    else if (protocolType.Trim().Equals(""))
                                    {

                                    }
                                }
                            }
                            else if (type.Trim().Equals("VDR"))
                            {
                                VDRProtocolList["PortName"] = inode.GetAttribute("PortName").ToString();
                                VDRProtocolList["STOPBITS"] = inode.GetAttribute("STOPBITS").ToString();
                                VDRProtocolList["DATABIT"] = inode.GetAttribute("DATABIT").ToString();
                                VDRProtocolList["PARITY"] = inode.GetAttribute("PARITY").ToString();
                                VDRProtocolList["BAUREATE"] = inode.GetAttribute("BAUREATE").ToString();
                                VDRProtocolList["FrameLenght"] = inode.GetAttribute("FrameLenght").ToString();
                                VDRProtocolList["FirstAddr"] = inode.GetAttribute("FirstAddr").ToString();
                                VDRProtocolList["StartSingle"] = inode.GetAttribute("StartSingle").ToString();

                            }
                        }
                    }
                    XmlModbusList.Sort();
                    reader.Close();
                    this.page2_gridview2.DataSource = XmlModbusList;
                    //动态加载控件
                    if (first_Flush_Flag)
                    {
                        DynamicCreateControl(SokectModbusProtocolList);
                    }
                    else
                    {
                        //清空原来的控件信息
                        layoutControl3.BeginUpdate();
                        layoutControl3.Controls.Clear();
                        layoutControl3.Root.Items.Clear();
                        DynamicCreateControl(SokectModbusProtocolList);
                        dataOpration.EndUpdate();
                    }

                    try
                    {
                        this.gridView2.Columns[0].Caption = "编号";
                        this.gridView2.Columns[0].Width = 40;
                        this.gridView2.Columns[1].Caption = "Can2 IP";
                        this.gridView2.Columns[1].Width = 100;
                        this.gridView2.Columns[2].Caption = "Can2 端口";
                        this.gridView2.Columns[2].Width = 50;
                        this.gridView2.Columns[3].Caption = "Can1 IP";
                        this.gridView2.Columns[3].Width = 100;
                        this.gridView2.Columns[4].Caption = "Can1 端口";
                        this.gridView2.Columns[4].Width = 50;
                        this.gridView2.Columns[5].Caption = "从机";
                        this.gridView2.Columns[5].Width = 20;
                        this.gridView2.Columns[6].Caption = "功能码";
                        this.gridView2.Columns[6].Width = 50;
                        this.gridView2.Columns[7].Caption = "地址";
                        this.gridView2.Columns[7].Width = 50;
                        this.gridView2.Columns[8].Caption = "请求个数";
                        this.gridView2.Columns[8].Width = 30;
                        this.gridView2.Columns[9].Caption = "数据长度";

                    }

                    catch (Exception ex) { MessageBox.Show("错误信息：" + ex.Message); }
                }
                #endregion
                #region

                #endregion

            }
            else if (navigationFrame1.SelectedPage.Caption.Trim().Equals("通道配置"))
            {
                prefixName = "page3_txt_";
            }
            else
            {
                this.Page1_CommunicationConfig.DataSource = null;
                modelList = new List<Communication>();
                //通讯配置
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(openFileDialog.FileName, settings);
                xmlDoc.Load(reader);
                // 得到根节点bookstore
                XmlNode xn = xmlDoc.SelectSingleNode("Communications");


                // 得到根节点的所有子节点
                XmlNodeList xnl = xn.ChildNodes;


                foreach (XmlNode xn1 in xnl)
                {
                    // 得到根节点的所有子节点
                    XmlNodeList xnn = xn1.ChildNodes;
                    foreach (var item in xnn)
                    {
                        Communication bookModel = new Communication();
                        // 将节点转换为元素，便于得到节点的属性值
                        XmlElement ele = (XmlElement)item;

                        Communication model = new Communication();
                        model.ID = ele.GetAttribute("ID").ToString();
                        model.CommType = ele.GetAttribute("CommType").ToString();
                        model.NameCH = ele.GetAttribute("NameCH").ToString();
                        model.NameEN = ele.GetAttribute("NameEN").ToString();
                        model.ConfigFilePath = ele.GetAttribute("ConfigFilePath").ToString();
                        modelList.Add(model);
                    }
                    modelList.Sort();
                }
                //读取完毕后关闭reader
                reader.Close();
                this.Page1_CommunicationConfig.DataSource = modelList;

                this.gridView1.Columns[0].Caption = "通讯编号";
                this.gridView1.Columns[0].Width = 30;
                this.gridView1.Columns[1].Caption = "协议类型";
                this.gridView1.Columns[1].Width = 100;
                this.gridView1.Columns[2].Caption = "中文名";
                this.gridView1.Columns[2].Width = 70;
                this.gridView1.Columns[3].Caption = "英文名";
                this.gridView1.Columns[3].Width = 70;
                this.gridView1.Columns[4].Caption = "文件路径";

                //Page1_CommunicationConfig.Columns["ID"].HeaderText = "编号";
                //Page1_CommunicationConfig.Columns["CommType"].HeaderText = "协议类型";
                //Page1_CommunicationConfig.Columns["NameCH"].HeaderText = "中文名";
                //Page1_CommunicationConfig.Columns["NameEN"].HeaderText = "英文名";
                //Page1_CommunicationConfig.Columns["ConfigFilePath"].HeaderText = "文件路径";

            }
            try
            { }
            catch (Exception ex)
            {
                MessageBox.Show("文件格式错误" + Environment.NewLine + ex.Message);
            }

            #region linq 方式获取
            /* 
               XmlReaderSettings settings = new XmlReaderSettings();
            //忽略文档中的注释
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(openFileDialog.FileName,settings);
            XElement xe = XElement.Load(reader);

            IEnumerable<XElement> elements = from ele in xe.Elements("Points")
                                             select ele;
            //showInfoByElements(elements);
            showInfoByAddressElements(elements);
            */
            #endregion
        }

        private void page1_btn_delCheck_Click(object sender, EventArgs e)
        {
            XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
            string strPath = string.Format("/Communications/CommunicaitonData/Communication[@ID=\"{0}\"]",
                GetSelectOID(gridView1, "ID"));
            //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            selectXe.ParentNode.RemoveChild(selectXe);
            xmlDoc.Save(openFileDialog.FileName);
            Communication communication = new Communication();
            communication.ID = GetSelectOID(gridView1, "ID");
            communication.CommType = GetSelectOID(gridView1, "CommType");
            communication.NameCH = GetSelectOID(gridView1, "NameCH");
            communication.NameEN = GetSelectOID(gridView1, "NameEN");
            communication.ConfigFilePath = GetSelectOID(gridView1, "ConfigFilePath");

            modelList.Remove(communication);
            Reflash();
        }

        private void page1_btn_delCheck_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void Page1_CommunicationConfig_MouseClick(object sender, MouseEventArgs e)
        {
            page1_txt_ID.Text = GetSelectOID(gridView1, "ID");
            page1_txt_ComType.Text = GetSelectOID(gridView1, "CommType");
            page1_txt_NameCH.Text = GetSelectOID(gridView1, "NameCH");
            page1_txt_NameEN.Text = GetSelectOID(gridView1, "NameEN");
            page1_txt_CommPath.Text = GetSelectOID(gridView1, "ConfigFilePath");
            //  page1_txt_ComType.Text = gridView1.CurrentRow.Cells[1].Value.ToString();
            // page1_txt_NameCH.Text = gridView1.CurrentRow.Cells[2].Value.ToString();
            //page1_txt_NameEN.Text = gridView1.CurrentRow.Cells[3].Value.ToString();
            // page1_txt_CommPath.Text = gridView1.CurrentRow.Cells[4].Value.ToString();
        }

        //mOIDFiledName为要获取列的列名
        private string GetSelectOID(GridView gv, string mOIDFiledName)
        {
            int[] pRows = this.gridView1.GetSelectedRows();//传递实体类过去 获取选中的行
            if (pRows.GetLength(0) > 0)
                return gv.GetRowCellValue(pRows[0], mOIDFiledName).ToString();
            else
                return null;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //打开指定文件目录
            openFileDialog.InitialDirectory = @".\";

            openFileDialog.Title = "选中你的配置文件";
            //过滤文件类型
            openFileDialog.Filter = "(*.xml)|*.xml";
            //不支持多选
            openFileDialog.DefaultExt = ".xml";

            openFileDialog.Multiselect = false;

            for (int i = 0; i <= 100; i = i + 20)
            {
                backgroundWorker1.ReportProgress(i);
            }

            //this.gridView1.Columns["reg_name"].Caption = "区划名称";
            //this.gridView1.Columns["parent_id"].Caption = "父区划编号";
            //this.gridView1.Columns["reg_desc"].Caption = "区划描述";
            // this.gridView1.MoveFirst();

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progress.EditValue = e.ProgressPercentage;
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ReadXml();
            }
            lab_Path.Caption = openFileDialog.FileName;
            progress.Caption = "加载完成";

            //Page1_CommunicationConfig.Columns["ID"].HeaderText = "编号";
            //Page1_CommunicationConfig.Columns["CommType"].HeaderText = "协议类型";
            //Page1_CommunicationConfig.Columns["NameCH"].HeaderText = "中文名";
            //Page1_CommunicationConfig.Columns["NameEN"].HeaderText = "英文名";
            //Page1_CommunicationConfig.Columns["ConfigFilePath"].HeaderText = "文件路径";
        }

        private void page1_btn_addData_Click(object sender, EventArgs e)
        {
            if (openFileDialog.FileName == "")
            {
                MessageBox.Show("请打开配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Communication communication = new Communication();
            communication.ID = (int.Parse(modelList[modelList.Count - 1].ID) + 1).ToString();
            communication.NameCH = page1_txt_NameCH.Text;
            communication.NameEN = page1_txt_NameEN.Text;
            communication.CommType = page1_txt_ComType.Text;
            communication.ConfigFilePath = page1_txt_CommPath.Text;

            xmlDoc.Load(openFileDialog.FileName);
            XmlNode root = xmlDoc.SelectSingleNode("Communications");

            XmlNode node = root.SelectSingleNode("CommunicaitonData");

            XmlElement element = xmlDoc.CreateElement("Communication");
            XmlAttribute attribute = xmlDoc.CreateAttribute("ID");
            XmlAttribute attributeNameCH = xmlDoc.CreateAttribute("NameCH");
            XmlAttribute attributeNameEN = xmlDoc.CreateAttribute("NameEN");
            XmlAttribute attributecCommType = xmlDoc.CreateAttribute("CommType");
            XmlAttribute attributePath = xmlDoc.CreateAttribute("ConfigFilePath");
            attribute.InnerText = communication.ID;
            attributeNameCH.InnerText = communication.NameCH;
            attributeNameEN.InnerText = communication.NameEN;
            attributePath.InnerText = communication.ConfigFilePath;
            attributecCommType.InnerText = communication.CommType;

            element.SetAttributeNode(attribute);
            element.SetAttributeNode(attributeNameCH);
            element.SetAttributeNode(attributecCommType);
            element.SetAttributeNode(attributeNameEN);
            element.SetAttributeNode(attributePath);


            node.AppendChild(element);
            modelList.Add(communication);
            xmlDoc.Save(openFileDialog.FileName);
            Reflash();
        }

        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            ColumnView View = sender as ColumnView;
            View.SetRowCellValue(e.RowHandle, View.Columns[0],
                gridView1.GetRowCellValue(gridView1.GetRowHandle(gridView1.RowCount - 2), gridView1.Columns[0])); //复制最后一行的数据到新行

            View.SetRowCellValue(e.RowHandle, View.Columns[1],
                gridView1.GetRowCellValue(gridView1.GetRowHandle(gridView1.RowCount - 2), gridView1.Columns[1])); //复制最后一行的数据到新行

        }

        private void btn_clear_Click(object sender, EventArgs e)
        {

            page1_txt_CommPath.Text = page1_txt_NameEN.Text
                = page1_txt_NameCH.Text = page1_txt_ID.Text
                = page1_txt_ComType.Text = "";

        }

        private void btn_clearAll_Click(object sender, EventArgs e)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            XmlReader reader = XmlReader.Create(openFileDialog.FileName, settings);
            xmlDoc.Load(reader);
            XElement xElement = XElement.Load(openFileDialog.FileName);
            IEnumerable<XElement> elements = from ele in xElement.Elements("CommunicaitonData")
                                             select ele;
            if (elements.Count() > 0)
            {
                elements.Remove();
            }
            reader.Close();
            xElement.Save(openFileDialog.FileName);
            MessageBox.Show("完成");
            //showInfoByElements(elements);

            /*
            XmlNode node = xmlDoc.SelectSingleNode("Communications");
            XmlNode no = node.SelectSingleNode("CommunicaitonData");
            XmlNode xmlNode= no.SelectSingleNode("Communication");
            xmlDoc.RemoveChild(xmlNode );
            */
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
            string strPath = string.Format("/Communications/CommunicaitonData/Communication[@ID=\"{0}\"]",
                GetSelectOID(gridView1, "ID"));
            //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            // selectXe.SetAttribute("CommType", txt_data2.Text);//也可以通过SetAttribute来增加一个属性
            selectXe.SetAttribute("CommType", page1_txt_ComType.Text);
            selectXe.SetAttribute("NameCH", page1_txt_NameCH.Text);
            selectXe.SetAttribute("NameEN", page1_txt_NameEN.Text);
            selectXe.SetAttribute("ConfigFilePath", page1_txt_CommPath.Text);


            Communication communication = new Communication();
            communication.ID = GetSelectOID(gridView1, "ID");
            communication.CommType = GetSelectOID(gridView1, "CommType");
            communication.NameCH = GetSelectOID(gridView1, "NameCH");
            communication.NameEN = GetSelectOID(gridView1, "NameEN");
            communication.ConfigFilePath = GetSelectOID(gridView1, "ConfigFilePath");

            modelList.Remove(communication);
            communication.ID = page1_txt_ID.Text;
            communication.CommType = page1_txt_ComType.Text;
            communication.NameCH = page1_txt_NameCH.Text;
            communication.NameEN = page1_txt_NameEN.Text;
            communication.ConfigFilePath = page1_txt_CommPath.Text;
            modelList.Add(communication);
            modelList.Sort();
            /* selectXe.GetElementsByTagName("CommType").Item(0).InnerText = txt_data2.Text;
             selectXe.GetElementsByTagName("NameCH").Item(0).InnerText = txt_data3.Text;
             selectXe.GetElementsByTagName("NameEN").Item(0).InnerText = txt_data4.Text;
             selectXe.GetElementsByTagName("ConfigFilePath").Item(0).InnerText = txt_path.Text;
             */
            xmlDoc.Save(openFileDialog.FileName);
            Reflash();
        }
        bool flage_Field = false;
        private void btn_configProtocol_Click(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage =
                (NavigationPage)navigationFrame1.Pages.FindFirst
                (x => (string)x.Tag == barButtonItem10.Caption);
            protocal_Path = page1_txt_CommPath.Text;
            //执行加载XML字段配置文件操作
            //判断已经加载过则不用再次加载
            if (!flage_Field)
            {
                LoadXMLField("./xmlField.xml");
                flage_Field = true;
            }
            string temp = openFileDialog.FileName;//打开文件的路径
            protocolPath = temp.Substring(0, temp.LastIndexOf("\\") + 1) + page1_txt_CommPath.Text;
            navigationFrame1.SelectedPage =
                (NavigationPage)navigationFrame1.Pages.FindFirst
                (x => (string)x.Tag == "协议配置");
            ReadXml();
        }
        /// <summary>
        /// 新建配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_newFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // 1. 通讯配置
                // 2. 协议配置
                // 3. 通道配置
                if (navigationFrame1.SelectedPage.Caption.Trim().Equals("协议配置"))
                {
                    prefixName = "page2_txt_";
                }
                else if (navigationFrame1.SelectedPage.Caption.Trim().Equals("通道配置"))
                {
                    prefixName = "page3_txt_";
                }
                else
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.InitialDirectory = @".\";
                    dialog.Filter = "配置 (*.xml)|*.xml";
                    dialog.DefaultExt = ".xml";
                    dialog.Title = "保存配置文件"; string filename0 = "";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        filename0 = dialog.FileName;
                    }

                    if (dialog.FileName.Equals(""))
                    {
                        filename0 = "./xmlSource/" + Guid.NewGuid() + ".xml";
                        MessageBox.Show("文件已经存储在当前软件目录中：文件名为：" + filename0);
                    }
                    if (File.Exists(filename0))
                    {
                        MessageBox.Show("你的文件已经存在是否替换", "文件已经存在", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        XmlTextWriter myXmlTextWriter = new XmlTextWriter(filename0, null);
                        myXmlTextWriter.WriteStartDocument(false);
                        myXmlTextWriter.WriteStartElement("Communications");

                        myXmlTextWriter.Close();
                        XElement xe = XElement.Load(filename0);
                        XElement record = new XElement(
                        new XElement("CommunicaitonData",
                            new XElement("Communication",
                                new XAttribute("ID", "1"),
                                new XAttribute("CommType", "SocketCustom"),
                                new XAttribute("NameCH", "中文名"),
                                new XAttribute("NameEN", "英文名"),
                                new XAttribute("ConfigFilePath", filename0))));
                        xe.Add(record);
                        xe.Save(filename0);
                        MessageBox.Show("新建完成！");
                        #region
                        /*
                        //使用 Formatting 属性指定希望将 XML 设定为何种格式。 
                        //这样，子元素就可以通过使用 Indentation 和 IndentChar 属性来缩进。
                        XmlTextWriter myXmlTextWriter = new XmlTextWriter(filePath, null);
                        myXmlTextWriter.WriteStartDocument(false);
                        myXmlTextWriter.WriteStartElement("Communications");
                        myXmlTextWriter.WriteComment("记录书本的信息");
                        myXmlTextWriter.WriteStartElement("CommunicaitonData");
                        myXmlTextWriter.WriteStartElement("Communicaiton");

                        myXmlTextWriter.WriteAttributeString("Type", "选修课");
                        myXmlTextWriter.WriteAttributeString("ISBN", "111111111");

                        myXmlTextWriter.WriteElementString("author", "张三");
                        myXmlTextWriter.WriteElementString("title", "职业生涯规划");
                        myXmlTextWriter.WriteElementString("price", "16.00");

                        myXmlTextWriter.WriteEndElement();
                        myXmlTextWriter.WriteEndElement();

                        myXmlTextWriter.Flush();
                        myXmlTextWriter.Close();
                        */
                        #endregion
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("错误信息：" + ex.Message); }

        }

        string prefixName = "";

        /// <summary>
        /// 动态添加控件
        /// </summary>
        /// <param name="editor">控件对象</param>
        /// <param name="dataSource">数据集对象</param>
        /// <param name="dataMember">数据项</param>
        /// <param name="parentGroup">容器所在的组，layoutControl.root表示（未分组）主容器</param>
        /// <returns></returns>
        private LayoutControlItem CreateItemWithBoundEditor(BaseEdit editor, object dataSource,
         string dataMember, LayoutControlGroup parentGroup)
        {
            editor.Name = prefixName + dataMember;
            editor.DataBindings.Add("EditValue", dataSource, dataMember);
            Page2TXTBaseEdit.Add(editor);
            LayoutControlItem item = parentGroup.AddItem(GetNameToLable(dataMember), editor) as LayoutControlItem;

            return item;

        }
        /// <summary>
        /// 显示的字段
        /// </summary>
        /// <param name="nameEn">字段名</param>
        /// <returns></returns>
        private string GetNameToLable(string nameEn)
        {
            string name = nameEn.Trim();
            switch (name)
            {
                case "ID": return "编号";
                case "Can2Port": return "Can2端口";
                case "Can1Port": return "Can1端口";
                case "SlaveID": return "从机号";
                case "FunctionCode": return "功能码";
                case "FirstAddress": return "开始地址";
                case "RequestCount": return "请求个数";
                case "DataLenght": return "数据长度";
                default: return name;
            }
        }

        LayoutControlGroup dataOpration = null;
        bool first_Flush_Flag = true;

        /// <summary>
        /// 穿件分组并添加控件到分组里面
        /// </summary>
        private void CreateProtocolLayout()
        {

            first_Flush_Flag = false;
            object employeesSource = page2_gridview2.DataSource;

            dataOpration = layoutControl3.AddGroup("数据区");

            LayoutControlItem itemFirstName =
                CreateItemWithBoundEditor(new TextEdit(), employeesSource, "ID", dataOpration);
            LayoutControlItem itemLastName =
                CreateItemWithBoundEditor(new TextEdit(), employeesSource, "SlaveID", dataOpration);
            LayoutControlItem itemFirstName1 =
                CreateItemWithBoundEditor(new TextEdit(), employeesSource, "FunctionCode", dataOpration);
            LayoutControlItem itemLastName2 =
                CreateItemWithBoundEditor(new TextEdit(), employeesSource, "FirstAddress", dataOpration);
            LayoutControlItem itemFirstName3 =
                CreateItemWithBoundEditor(new TextEdit(), employeesSource, "RequestCount", dataOpration);
            LayoutControlItem itemFirstName4 =
                CreateItemWithBoundEditor(new TextEdit(), employeesSource, "DataLenght", dataOpration);

            try
            { }
            #region
            /*
            //将姓氏移动到名字的右侧
            itemLastName.Move(itemFirstName, InsertType.Right);
            // 添加带有生日编辑器的Birthday组
            LayoutControlGroup birthdayGroup = layoutControl3.AddGroup("Birthday Information");
            CreateItemWithBoundEditor(new DateEdit(), employeesSource, "BirthDate", birthdayGroup);
            // 添加包含三个地址字段的选项卡
            TabbedControlGroup tabbedGroup = layoutControl3.AddTabbedGroup();
            LayoutControlGroup addressGroup = tabbedGroup.AddTabPage("Address Details");
            string[] dataFields = new string[] { "Country", "City", "Address" };
            foreach (string dataField in dataFields)
                CreateItemWithBoundEditor(new TextEdit(), employeesSource, dataField, addressGroup);
            // Add a tab with a photo
            LayoutControlGroup groupPhoto = tabbedGroup.AddTabPage("Photo");
            CreateItemWithBoundEditor(new PictureEdit(), employeesSource, "Photo", groupPhoto);
            } 
             */

            #endregion
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message);
            }
        }
        private void DynamicCreateControl(Dictionary<string, string> keys)
        {

            first_Flush_Flag = false;
            object employeesSource = gridView2.DataSource;

            dataOpration = layoutControl3.AddGroup("数据区");
            foreach (var item in keys)
            {
                LayoutControlItem itemFirstName =
               CreateItemWithBoundEditor(new TextEdit(), employeesSource, item.Key, dataOpration);
            }

            try
            { }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message);
            }
        }
        private void CreateFieldLayout()
        {
            try
            {
                first_Flush_Flag = false;
                object employeesSource = gridView1.DataSource;

                dataOpration = layoutControl3.AddGroup("数据区");
                LayoutControlItem itemFirstName =
                    CreateItemWithBoundEditor(new TextEdit(), employeesSource, "ID", dataOpration);
                LayoutControlItem itemLastName =
                    CreateItemWithBoundEditor(new TextEdit(), employeesSource, "NameCH", dataOpration);
                LayoutControlItem itemFirstName1 =
                    CreateItemWithBoundEditor(new TextEdit(), employeesSource, "NameEN", dataOpration);
                LayoutControlItem itemLastName2 =
                    CreateItemWithBoundEditor(new TextEdit(), employeesSource, "CommType", dataOpration);
                LayoutControlItem itemFirstName3 =
                    CreateItemWithBoundEditor(new TextEdit(), employeesSource, "ConfigFilePath", dataOpration);
            }
            #region
            /*
            //将姓氏移动到名字的右侧
            itemLastName.Move(itemFirstName, InsertType.Right);
            // 添加带有生日编辑器的Birthday组
            LayoutControlGroup birthdayGroup = layoutControl3.AddGroup("Birthday Information");
            CreateItemWithBoundEditor(new DateEdit(), employeesSource, "BirthDate", birthdayGroup);
            // 添加包含三个地址字段的选项卡
            TabbedControlGroup tabbedGroup = layoutControl3.AddTabbedGroup();
            LayoutControlGroup addressGroup = tabbedGroup.AddTabPage("Address Details");
            string[] dataFields = new string[] { "Country", "City", "Address" };
            foreach (string dataField in dataFields)
                CreateItemWithBoundEditor(new TextEdit(), employeesSource, dataField, addressGroup);
            // Add a tab with a photo
            LayoutControlGroup groupPhoto = tabbedGroup.AddTabPage("Photo");
            CreateItemWithBoundEditor(new PictureEdit(), employeesSource, "Photo", groupPhoto);
            } 
             */

            #endregion
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message);
            }
        }
        /// <summary>
        /// 控件动态添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //layoutControl3.BeginUpdate();
            //layoutControl3.Controls.Clear();
            //layoutControl3.Root.Items.Clear();
            if (first_Flush_Flag)
            {
                CreateProtocolLayout();
            }
            else
            {
                //清空原来的控件信息
                layoutControl3.BeginUpdate();
                layoutControl3.Controls.Clear();
                layoutControl3.Root.Items.Clear();
                CreateProtocolLayout();
                dataOpration.EndUpdate();
            }

        }

        private void page2_btn_ConfigAddress_Click(object sender, EventArgs e)
        {

        }

        private void page2_btn_clearContent_Click(object sender, EventArgs e)
        {

        }
        private Control GetTextControl(string name)
        {
            Control control = null;
            foreach (Control c in this.Controls)
            {
                if (c.Name.Equals(name))
                {
                    control = c;
                }
            }
            return control;
        }
        /// <summary>
        /// 协议字段配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            navigationFrame1.SelectedPage = (NavigationPage)navigationFrame1.Pages.FindFirst(x => (string)x.Tag == e.Item.Caption);

        }

        private void page4_cmb_CheckProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 加载XML字段配置文件
        /// </summary>
        /// <param name="fileName"></param>
        private void LoadXMLField(string fileName)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(fileName, settings);
                xmlDoc.Load(reader);
                XmlNode xn = xmlDoc.SelectSingleNode("Fields");
                XmlNodeList xnl = xn.ChildNodes;

                foreach (XmlNode xnnode in xnl)
                {
                    XmlElement protocol = (XmlElement)xnnode;
                    string id = protocol.GetAttribute("Name").ToString();
                    if (id.Trim().Equals("commField"))//commField
                    {
                        XmlNodeList list = protocol.ChildNodes;
                        foreach (XmlNode item in list)
                        {
                            CommFieldList.Add(item.InnerText, "");
                        }

                    }
                    if (id.Trim().Equals("SokectCustomProtocol"))//SokectCustomProtocol
                    {
                        XmlNodeList eles = xnnode.ChildNodes;
                        foreach (XmlNode tcp in eles)
                        {
                            XmlElement xml = (XmlElement)tcp;
                            SokectCustomProtocolList.Add(xml.InnerText, "");
                        }

                    }
                    else if (id.Trim().Equals("VDRProtocol"))//VDRProtocol
                    {
                        XmlNodeList eles = xnnode.ChildNodes;
                        foreach (XmlNode tcp in eles)
                        {
                            XmlElement xml = (XmlElement)tcp;
                            VDRProtocolList.Add(xml.InnerText, "");
                        }
                    }
                    else if (id.Trim().Equals("SocketModbusProtocol"))//SocketModbusProtocol
                    {
                        XmlNodeList eles = xnnode.ChildNodes;
                        foreach (XmlNode tcp in eles)
                        {
                            XmlElement xml = (XmlElement)tcp;
                            SokectModbusProtocolList.Add(xml.InnerText, "");
                        }
                    }
                    else
                    if (id.Trim().Equals("other"))//
                    {
                        XmlNodeList eles = xnnode.ChildNodes;
                        foreach (XmlNode tcp in eles)
                        {
                            XmlElement xml = (XmlElement)tcp;
                            ProtocolOtherList.Add(xml.InnerText.ToString(), "");
                        }
                    }

                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件格式错误" + Environment.NewLine + ex.Message);
            }
        }

        private void page2_gridview2_MouseClick(object sender, MouseEventArgs e)
        {

            SokectModbusProtocolList["ID"] = GetSelectOID(gridView2, "ID");
            SokectModbusProtocolList["SlaveID"] = GetSelectOID(gridView2, "SlaveID");
            SokectModbusProtocolList["FunctionCode"] = GetSelectOID(gridView2, "FunctionCode");
            SokectModbusProtocolList["FirstAddress"] = GetSelectOID(gridView2, "FirstAddress");
            SokectModbusProtocolList["RequestCount"] = GetSelectOID(gridView2, "RequestCount");
            SokectModbusProtocolList["DataLenght"] = GetSelectOID(gridView2, "DataLenght");

            foreach (Control c in this.Controls)
            {
                foreach (var item in SokectModbusProtocolList)
                {
                    if (c is BaseEdit && c.Name == prefixName + item.Key)
                    {
                        BaseEdit temp = c as BaseEdit;
                        temp.Text = item.Value;
                    }
                }
            }
        }
    }
}
