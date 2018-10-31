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
using DevExpress.XtraEditors.Controls;
using System.Text;

namespace XmlProcess5._0
{
    public partial class FormXmlPress5 : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        OpenFileDialog openFileDialog = new OpenFileDialog();
        List<Communication> modelList = new List<Communication>();

        XmlDocument xmlDoc = new XmlDocument();

        static Dictionary<string, string> CommFieldDictionary = new Dictionary<string, string>();//公共字段
        static Dictionary<string, string> SokectCustomProtocolDictionary = new Dictionary<string, string>();//socket协议字段
        static Dictionary<string, string> SokectModbusProtocolDictionary = new Dictionary<string, string>();//Modbus协议字段
        static Dictionary<string, string> VDRProtocolDictionary = new Dictionary<string, string>();//VDR协议字段
        static Dictionary<string, string> SokectProtocolDictionary = new Dictionary<string, string>();//VDR协议字段 
        static Dictionary<string, string> ProtocolOtherDictionary = new Dictionary<string, string>();//其他协议字段

        List<XmlModbus> XmlModbusList = new List<XmlModbus>();//modbus协议专属
        List<XmlSokectCustomProtocol> XmlcustomList = new List<XmlSokectCustomProtocol>();
        List<XmlVDRProtocol> XmlVDRList = new List<XmlVDRProtocol>();
        List<XmlSokect> XmlSokectList = new List<XmlSokect>();
        List<XmlOther> XmlOtherList = new List<XmlOther>();
        List<XmlAddress> XmlAddressList = new List<XmlAddress>();
        List<XmlField> XmlFieldList = new List<XmlField>();

        Dictionary<string, StringBuilder> FieldDirectory = new Dictionary<string, StringBuilder>();


        String configFilePath, protocolFilePath, addressFilePath, fieldFilePath = "";
        string findCommand = "/Communications/CommunicaitonData/Communication/TCP[@ID=\"{0}\"]";

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
            gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;//一次选择一行

            gridView2.OptionsView.ColumnAutoWidth = true;
            gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView2.OptionsBehavior.Editable = false;

            gridView3.OptionsView.ColumnAutoWidth = true;
            gridView3.OptionsView.ShowGroupPanel = false;
            this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView3.OptionsBehavior.Editable = false;

            page2_txt_ID.ReadOnly = true;
            page3_txt_LocalAddress.ReadOnly = true;

            panelControl1.Visible = false;

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
            e.Graph.DrawString(reportHeader, Color.Black, rec, DevExpress.XtraPrinting.BorderSide.None);

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
                PreviewPrintableComponent(this.Page3_Address, Page3_Address.LookAndFeel);//打印 
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

        ProtocolType currentProtocolType = new ProtocolType();
        XmlReader reader;
        public void LoadXml(string path)
        {
            //通讯配置
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            reader = XmlReader.Create(path, settings);

            xmlDoc.Load(reader);
        }
        /// <summary>
        /// 读XML配置文件
        /// </summary>
        private void ReadXml()
        {
            #region 自动对打开的文件进行识别处理以及路径的检查优化
            if (openFileDialog.FileName.LastIndexOf("CommunicationConfig") > 0)
            {
                navigationFrame1.SelectedPage =
        (NavigationPage)navigationFrame1.Pages.FindFirst
        (x => (string)x.Tag == barButtonItem9.Caption);
            }
            else if (openFileDialog.FileName.LastIndexOf("Address") > 0)
            {
                navigationFrame1.SelectedPage =
          (NavigationPage)navigationFrame1.Pages.FindFirst
          (x => (string)x.Tag == barButtonItem11.Caption);
            }
            else if (openFileDialog.FileName.LastIndexOf("xmlField") > 0)
            {
                navigationFrame1.SelectedPage =
         (NavigationPage)navigationFrame1.Pages.FindFirst
         (x => (string)x.Tag == barButtonItem12.Caption);
            }
            else
            {
                navigationFrame1.SelectedPage =
         (NavigationPage)navigationFrame1.Pages.FindFirst
         (x => (string)x.Tag == barButtonItem10.Caption);
            }
            //获取到打开的文件的路径
            if (navigationFrame1.SelectedPage.Caption.Trim().Equals("协议配置"))
            {
                protocolFilePath = openFileDialog.FileName;
                if (protocolFilePath.LastIndexOf("Config\\Config\\") > 0)
                {
                    protocolFilePath = protocolFilePath.Replace("Config\\Config\\", "Config\\");
                }
            }
            else if (navigationFrame1.SelectedPage.Caption.Trim().Equals("通道配置"))
            {

                addressFilePath = openFileDialog.FileName;
                if (addressFilePath.LastIndexOf("Config\\Config\\") > 0)
                {
                    addressFilePath = addressFilePath.Replace("Config\\Config\\", "Config\\");
                }
            }
            else if (navigationFrame1.SelectedPage.Caption.Trim().Equals("协议字段配置"))
            {
                fieldFilePath = openFileDialog.FileName;
                if (fieldFilePath.LastIndexOf("Config\\Config\\") > 0)
                {
                    fieldFilePath = fieldFilePath.Replace("Config\\Config\\", "Config\\");
                }
            }
            else
            {
                configFilePath = openFileDialog.FileName;
            }
            //判断当前打开的页数，判断加载的是什么程序
            // 1. 通讯配置
            // 2. 协议配置
            // 3. 通道配置
            #endregion
            if (navigationFrame1.SelectedPage.Caption.Trim().Equals("协议配置"))
            {
                //清空缓存的数据
                this.page2_gridview2.DataSource = null;
                XmlModbusList = new List<XmlModbus>();
                XmlcustomList = new List<XmlSokectCustomProtocol>();
                XmlVDRList = new List<XmlVDRProtocol>();
                XmlOtherList = new List<XmlOther>();
                XmlSokectList = new List<XmlSokect>();

                prefixName = "page2_txt_";
                //执行加载XML字段配置文件操作
                //没有加载过为False
                if (!flage_Field)
                {
                    LoadXMLField("./xmlField.xml");
                    flage_Field = true;
                }
                if (!File.Exists(protocolFilePath) || protocolFilePath.Equals(""))
                {
                    MessageBox.Show("找不到该文件配置文件 ");
                    return;
                }
                if (Path.GetExtension(protocolFilePath).Contains("json"))
                {
                    MessageBox.Show("目前不支持该文件解析,请选择文件");
                    return;
                }
                LoadXml(protocolFilePath);

                // 得到根节点bookstore
                XmlNode xn = xmlDoc.SelectSingleNode("Communications");


                // 得到根节点的所有子节点
                XmlNodeList xnl = xn.ChildNodes;
                string nameStr = protocolFilePath.Substring(protocolFilePath.LastIndexOf("\\") + 1).ToUpper();

                //gridView2.RefreshData();
                //gridView2.Dispose(); 
                //this.page2_gridview2.DataSource = null;
                gridView2.Columns.Clear();
                foreach (XmlNode xn1 in xnl)
                {
                    // 得到根节点的所有子节点
                    XmlNodeList xnn = xn1.ChildNodes;
                    foreach (XmlElement item in xnn)
                    {
                        #region txt控件赋值
                        page2_txt_ID.Text = CommFieldDictionary["ID"] = item.GetAttribute("ID").ToString();
                        page2_txt_CommReConnectTime.Text = CommFieldDictionary["CommReConnectTime"] = item.GetAttribute("CommReConnectTime").ToString();
                        page2_txt_CommErroTime.Text = CommFieldDictionary["CommErroTime"] = item.GetAttribute("CommErroTime").ToString();
                        page2_txt_WriteTimeOut.Text = CommFieldDictionary["WriteTimeOut"] = item.GetAttribute("WriteTimeOut").ToString();
                        page2_txt_ReadTimeOut.Text = CommFieldDictionary["ReadTimeOut"] = item.GetAttribute("ReadTimeOut").ToString();
                        page2_txt_CommSpaceTime.Text = CommFieldDictionary["CommSpaceTime"] = item.GetAttribute("CommSpaceTime").ToString();
                        page2_txt_ConfigFilePath.Text = CommFieldDictionary["ConfigFilePath"] = item.GetAttribute("ConfigFilePath").ToString();
                        page2_txt_EnglishiName.Text = CommFieldDictionary["EnglishiName"] = item.GetAttribute("EnglishiName").ToString();
                        page2_txt_ChineseName.Text = CommFieldDictionary["ChineseName"] = item.GetAttribute("ChineseName").ToString();
                        #endregion
                        XmlNodeList nodes = item.ChildNodes;

                        if (nameStr.Contains(ProtocolType.custom.ToString().ToUpper()))
                        {
                            findCommand = "/Communications/CommunicaitonData/Communication/TCP[@ID=\"{0}\"]";

                            currentProtocolType = ProtocolType.custom;
                            #region customProtocol
                            foreach (XmlElement inode in nodes)
                            {
                                string type = inode.Name;
                                if (type.Trim().Equals("TCP"))
                                {
                                    XmlSokectCustomProtocol custom = new XmlSokectCustomProtocol
                                    {
                                        ID = SokectCustomProtocolDictionary["ID"] = inode.GetAttribute("ID"),
                                        Can1IP = SokectCustomProtocolDictionary["Can1IP"] = inode.GetAttribute("Can1IP"),
                                        Can1Port = SokectCustomProtocolDictionary["Can1Port"] = inode.GetAttribute("Can1Port"),
                                        Can2IP = SokectCustomProtocolDictionary["Can2IP"] = inode.GetAttribute("Can2IP"),
                                        Can2Port = SokectCustomProtocolDictionary["Can2Port"] = inode.GetAttribute("Can2Port"),
                                        CheckEndIndex = SokectCustomProtocolDictionary["CheckEndIndex"] = inode.GetAttribute("CheckEndIndex"),
                                        CheckStartIndex = SokectCustomProtocolDictionary["CheckStartIndex"] = inode.GetAttribute("CheckStartIndex"),
                                        CheckSumIndex = SokectCustomProtocolDictionary["CheckSumIndex"] = inode.GetAttribute("CheckSumIndex"),
                                        CheckSumLength = SokectCustomProtocolDictionary["CheckSumLength"] = inode.GetAttribute("CheckSumLength"),
                                        CheckSumType = SokectCustomProtocolDictionary["CheckSumType"] = inode.GetAttribute("CheckSumType"),
                                        EndSingle = SokectCustomProtocolDictionary["EndSingle"] = inode.GetAttribute("EndSingle"),
                                        FirstAddr = SokectCustomProtocolDictionary["FirstAddr"] = inode.GetAttribute("FirstAddr"),
                                        PackageLength = SokectCustomProtocolDictionary["PackageLength"] = inode.GetAttribute("PackageLength"),
                                        StartSingle = SokectCustomProtocolDictionary["StartSingle"] = inode.GetAttribute("StartSingle")
                                    };
                                    XmlcustomList.Add(custom);
                                }
                            }

                            reader.Close();
                            this.page2_gridview2.DataSource = XmlcustomList;
                            //动态加载控件
                            if (first_Flush_Flag)
                            {
                                DynamicCreateControl(SokectCustomProtocolDictionary);
                            }
                            else
                            {
                                //清空原来的控件信息
                                layoutControl3.BeginUpdate();
                                layoutControl3.Controls.Clear();
                                layoutControl3.Root.Items.Clear();
                                DynamicCreateControl(SokectCustomProtocolDictionary);
                                dataOpration.EndUpdate();
                            }
                            try
                            {
                                this.gridView2.Columns[0].Caption = "ID";
                                this.gridView2.Columns[0].Width = 20;
                                this.gridView2.Columns[1].Caption = "IP地址1";
                                this.gridView2.Columns[1].Width = 100;
                                this.gridView2.Columns[2].Caption = "Port1";
                                this.gridView2.Columns[2].Width = 40;
                                this.gridView2.Columns[3].Caption = "IP地址2";
                                this.gridView2.Columns[3].Width = 100;
                                this.gridView2.Columns[4].Caption = "Port2";
                                this.gridView2.Columns[4].Width = 40;
                                this.gridView2.Columns[5].Caption = "结束位";
                                this.gridView2.Columns[5].Width = 40;
                                this.gridView2.Columns[6].Caption = "开始位";
                                this.gridView2.Columns[6].Width = 40;
                                this.gridView2.Columns[7].Caption = "检验位";
                                this.gridView2.Columns[7].Width = 40;
                                this.gridView2.Columns[8].Caption = "校验长度";
                                this.gridView2.Columns[8].Width = 40;
                                this.gridView2.Columns[9].Caption = "类型";
                                this.gridView2.Columns[9].Width = 40;
                                this.gridView2.Columns[10].Caption = "结束标志";
                                this.gridView2.Columns[10].Width = 40;
                                this.gridView2.Columns[11].Caption = "开始地址";
                                this.gridView2.Columns[11].Width = 40;
                                this.gridView2.Columns[12].Caption = "包长度";
                                this.gridView2.Columns[12].Width = 40;
                                this.gridView2.Columns[13].Caption = "开始标志";
                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show("错误信息：" + ex.Message);
                            }
                            #endregion
                        }
                        else if (nameStr.Contains(ProtocolType.Modbus.ToString().ToUpper()))
                        {
                            findCommand = "/Communications/CommunicaitonData/Communication/TCP/CommCongfig[@ID=\"{0}\"]";
                            currentProtocolType = ProtocolType.Modbus;
                            #region SocektModbus协议 

                            foreach (XmlElement inode in nodes)
                            {
                                string type = inode.Name;
                                if (type.Trim().Equals("TCP"))
                                {
                                    SokectModbusProtocolDictionary["Can2IP"] = inode.GetAttribute("Can2IP").ToString();
                                    SokectModbusProtocolDictionary["Can2Port"] = inode.GetAttribute("Can2Port").ToString();
                                    SokectModbusProtocolDictionary["Can1IP"] = inode.GetAttribute("Can1IP").ToString();
                                    SokectModbusProtocolDictionary["Can1Port"] = inode.GetAttribute("Can1Port").ToString();

                                    XmlNodeList commconfig = inode.ChildNodes;
                                    foreach (XmlElement config in commconfig)
                                    {

                                        string protocolType = config.Name;
                                        if (protocolType.Trim().Equals("CommCongfig"))
                                        {
                                            XmlModbus modbus = new XmlModbus
                                            {
                                                Can2IP = SokectModbusProtocolDictionary["Can2IP"],
                                                Can2Port = SokectModbusProtocolDictionary["Can2Port"],
                                                Can1IP = SokectModbusProtocolDictionary["Can1IP"],
                                                Can1Port = SokectModbusProtocolDictionary["Can1Port"],
                                                ID = SokectModbusProtocolDictionary["ID"] = config.GetAttribute("ID").ToString(),
                                                SlaveID = SokectModbusProtocolDictionary["SlaveID"] = config.GetAttribute("SlaveID").ToString(),
                                                FirstAddress = SokectModbusProtocolDictionary["FirstAddress"] = config.GetAttribute("FirstAddress").ToString(),
                                                FunctionCode = SokectModbusProtocolDictionary["FunctionCode"] = config.GetAttribute("FunctionCode").ToString(),
                                                RequestCount = SokectModbusProtocolDictionary["RequestCount"] = config.GetAttribute("RequestCount").ToString(),
                                                DataLenght = SokectModbusProtocolDictionary["DataLenght"] = config.GetAttribute("DataLenght").ToString()
                                            };
                                            XmlModbusList.Add(modbus);
                                        }
                                    }
                                }
                            }
                            reader.Close();
                            this.page2_gridview2.DataSource = XmlModbusList;
                            //动态加载控件
                            if (first_Flush_Flag)
                            {
                                DynamicCreateControl(SokectModbusProtocolDictionary);
                            }
                            else
                            {
                                //清空原来的控件信息
                                layoutControl3.BeginUpdate();
                                layoutControl3.Controls.Clear();
                                layoutControl3.Root.Items.Clear();
                                DynamicCreateControl(SokectModbusProtocolDictionary);
                                dataOpration.EndUpdate();
                            }
                            try
                            {
                                this.gridView2.Columns[0].Caption = "ID";
                                this.gridView2.Columns[0].Width = 40;
                                this.gridView2.Columns[1].Caption = "IP地址2";
                                this.gridView2.Columns[1].Width = 100;
                                this.gridView2.Columns[2].Caption = "端口2";
                                this.gridView2.Columns[2].Width = 50;
                                this.gridView2.Columns[3].Caption = "IP地址1";
                                this.gridView2.Columns[3].Width = 100;
                                this.gridView2.Columns[4].Caption = "端口1";
                                this.gridView2.Columns[4].Width = 50;
                                this.gridView2.Columns[5].Caption = "从机号";
                                this.gridView2.Columns[5].Width = 50;
                                this.gridView2.Columns[6].Caption = "功能码";
                                this.gridView2.Columns[6].Width = 50;
                                this.gridView2.Columns[7].Caption = "地址";
                                this.gridView2.Columns[7].Width = 50;
                                this.gridView2.Columns[8].Caption = "请求个数";
                                this.gridView2.Columns[8].Width = 30;
                                this.gridView2.Columns[9].Caption = "数据长度";

                            }

                            catch (Exception ex) { MessageBox.Show("错误信息：" + ex.Message); }

                            #endregion
                        }
                        else
                         if (nameStr.Contains(ProtocolType.vdr.ToString().ToUpper()))
                        {
                            findCommand = "/Communications/CommunicaitonData/Communication/VDR";
                            currentProtocolType = ProtocolType.vdr;
                            #region VDRProtocol
                            foreach (XmlElement inode in nodes)
                            {
                                XmlVDRProtocol vdr = new XmlVDRProtocol
                                {
                                    PortName = VDRProtocolDictionary["PortName"] = inode.GetAttribute("PortName").ToString(),
                                    STOPBITS = VDRProtocolDictionary["STOPBITS"] = inode.GetAttribute("STOPBITS").ToString(),
                                    DATABIT = VDRProtocolDictionary["DATABIT"] = inode.GetAttribute("DATABIT").ToString(),
                                    PARITY = VDRProtocolDictionary["PARITY"] = inode.GetAttribute("PARITY").ToString(),
                                    BAUREATE = VDRProtocolDictionary["BAUREATE"] = inode.GetAttribute("BAUREATE").ToString(),
                                    FrameLenght = VDRProtocolDictionary["FrameLenght"] = inode.GetAttribute("FrameLenght").ToString(),
                                    FirstAddr = VDRProtocolDictionary["FirstAddr"] = inode.GetAttribute("FirstAddr").ToString(),
                                    StartSingle = VDRProtocolDictionary["StartSingle"] = inode.GetAttribute("StartSingle").ToString()
                                };
                                XmlVDRList.Add(vdr);
                            }

                            reader.Close();
                            this.page2_gridview2.DataSource = XmlVDRList;
                            //动态加载控件
                            if (first_Flush_Flag)
                            {
                                DynamicCreateControl(VDRProtocolDictionary);
                            }
                            else
                            {
                                //清空原来的控件信息
                                layoutControl3.BeginUpdate();
                                layoutControl3.Controls.Clear();
                                layoutControl3.Root.Items.Clear();
                                DynamicCreateControl(VDRProtocolDictionary);
                                dataOpration.EndUpdate();
                            }
                            try
                            {
                                this.gridView2.Columns[0].Caption = "端口号";
                                this.gridView2.Columns[0].Width = 70;
                                this.gridView2.Columns[1].Caption = "停止位";
                                this.gridView2.Columns[1].Width = 70;
                                this.gridView2.Columns[2].Caption = "数据位";
                                this.gridView2.Columns[2].Width = 70;
                                this.gridView2.Columns[3].Caption = "校验位";
                                this.gridView2.Columns[3].Width = 70;
                                this.gridView2.Columns[4].Caption = "波特率";
                                this.gridView2.Columns[4].Width = 70;
                                this.gridView2.Columns[5].Caption = "长度";
                                this.gridView2.Columns[5].Width = 70;
                                this.gridView2.Columns[6].Caption = "开始地址";
                                this.gridView2.Columns[6].Width = 70;
                                this.gridView2.Columns[7].Caption = "开始标志";
                                this.gridView2.Columns[7].Width = 70;

                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show("错误信息：" + ex.Message);
                            }

                            #endregion
                        }
                        else
                         if (nameStr.Contains(ProtocolType.other.ToString().ToUpper()))
                        {
                            findCommand = "/Communications/CommunicaitonData/Communication/TCP/CommCongfig[@ID=\"{0}\"]";

                            currentProtocolType = ProtocolType.other;
                            #region other
                            foreach (XmlElement inode in nodes)
                            {
                                XmlOther other = new XmlOther
                                {
                                    ID = ProtocolOtherDictionary["ID"] = inode.GetAttribute("ID").ToString(),
                                    Can2IP = ProtocolOtherDictionary["Can2IP"] = inode.GetAttribute("Can2IP").ToString(),
                                    Can2Port = ProtocolOtherDictionary["Can2Port"] = inode.GetAttribute("Can2Port").ToString(),
                                    Can1IP = ProtocolOtherDictionary["Can1IP"] = inode.GetAttribute("Can1IP").ToString(),
                                    Can1Port = ProtocolOtherDictionary["Can1Port"] = inode.GetAttribute("Can1Port").ToString(),
                                    SlaveID = ProtocolOtherDictionary["SlaveID"] = inode.GetAttribute("SlaveID").ToString(),
                                    FunctionCode = ProtocolOtherDictionary["FunctionCode"] = inode.GetAttribute("FunctionCode").ToString(),
                                    FirstAddress = ProtocolOtherDictionary["FirstAddress"] = inode.GetAttribute("FirstAddress").ToString(),
                                    RequestCount = ProtocolOtherDictionary["RequestCount"] = inode.GetAttribute("RequestCount").ToString(),
                                    DataLenght = ProtocolOtherDictionary["DataLenght"] = inode.GetAttribute("DataLenght").ToString(),
                                    PortName = ProtocolOtherDictionary["PortName"] = inode.GetAttribute("PortName").ToString(),
                                    STOPBITS = ProtocolOtherDictionary["STOPBITS"] = inode.GetAttribute("STOPBITS").ToString(),
                                    DATABIT = ProtocolOtherDictionary["DATABIT"] = inode.GetAttribute("DATABIT").ToString(),
                                    PARITY = ProtocolOtherDictionary["PARITY"] = inode.GetAttribute("PARITY").ToString(),
                                    BAUREATE = ProtocolOtherDictionary["BAUREATE"] = inode.GetAttribute("BAUREATE").ToString(),
                                    FrameLenght = ProtocolOtherDictionary["FrameLenght"] = inode.GetAttribute("FrameLenght").ToString(),
                                    StartSingle = ProtocolOtherDictionary["StartSingle"] = inode.GetAttribute("StartSingle").ToString(),
                                    EndSingle = ProtocolOtherDictionary["EndSingle"] = inode.GetAttribute("EndSingle").ToString(),
                                    PackageLength = ProtocolOtherDictionary["PackageLength"] = inode.GetAttribute("PackageLength").ToString(),
                                    FirstAddr = ProtocolOtherDictionary["FirstAddr"] = inode.GetAttribute("FirstAddr").ToString(),
                                    CheckSumIndex = ProtocolOtherDictionary["CheckSumIndex"] = inode.GetAttribute("CheckSumIndex").ToString(),
                                    CheckSumType = ProtocolOtherDictionary["CheckSumType"] = inode.GetAttribute("CheckSumType").ToString(),
                                    CheckStartIndex = ProtocolOtherDictionary["CheckStartIndex"] = inode.GetAttribute("CheckStartIndex").ToString(),
                                    CheckEndIndex = ProtocolOtherDictionary["CheckEndIndex"] = inode.GetAttribute("CheckEndIndex").ToString(),
                                    Standby1 = ProtocolOtherDictionary["Standby1"] = inode.GetAttribute("Standby1").ToString(),
                                    Standby2 = ProtocolOtherDictionary["Standby2"] = inode.GetAttribute("Standby2").ToString(),
                                    Standby3 = ProtocolOtherDictionary["Standby3"] = inode.GetAttribute("Standby3").ToString(),
                                    Standby4 = ProtocolOtherDictionary["Standby4"] = inode.GetAttribute("Standby4").ToString(),
                                    Standby5 = ProtocolOtherDictionary["Standby5"] = inode.GetAttribute("Standby5").ToString(),
                                    Standby6 = ProtocolOtherDictionary["Standby6"] = inode.GetAttribute("Standby6").ToString(),
                                    Standby7 = ProtocolOtherDictionary["Standby7"] = inode.GetAttribute("Standby7").ToString(),
                                    Standby8 = ProtocolOtherDictionary["Standby8"] = inode.GetAttribute("Standby8").ToString()
                                };
                                XmlOtherList.Add(other);
                            }
                            reader.Close();
                            this.page2_gridview2.DataSource = XmlOtherList;
                            //动态加载控件
                            if (first_Flush_Flag)
                            {
                                DynamicCreateControl(ProtocolOtherDictionary);
                            }
                            else
                            {
                                //清空原来的控件信息
                                layoutControl3.BeginUpdate();
                                layoutControl3.Controls.Clear();
                                layoutControl3.Root.Items.Clear();
                                DynamicCreateControl(ProtocolOtherDictionary);
                                dataOpration.EndUpdate();
                            }

                            #endregion
                        }
                        else
                         if (nameStr.Contains(ProtocolType.Sokect.ToString().ToUpper()))
                        {
                            findCommand = "/Communications/CommunicaitonData/Communication/TCP[@ID=\"{0}\"]";


                            currentProtocolType = ProtocolType.Sokect;
                            #region Sokect
                            foreach (XmlElement inode in nodes)
                            {
                                XmlSokect sokect = new XmlSokect
                                {
                                    ID = SokectProtocolDictionary["ID"] = inode.GetAttribute("ID").ToString(),
                                    Can1IP = SokectProtocolDictionary["Can1IP"] = inode.GetAttribute("Can1IP").ToString(),
                                    Can1Port = SokectProtocolDictionary["Can1Port"] = inode.GetAttribute("Can1Port").ToString(),
                                    Can2IP = SokectProtocolDictionary["Can2IP"] = inode.GetAttribute("PARITY").ToString(),
                                    Can2Port = SokectProtocolDictionary["Can2Port"] = inode.GetAttribute("Can2Port").ToString(),
                                    StartChar = SokectProtocolDictionary["StartChar"] = inode.GetAttribute("StartChar").ToString(),//0183协议通讯数据包包头
                                    CommSingle = SokectProtocolDictionary["CommSingle"] = inode.GetAttribute("CommSingle").ToString(), //设备标识符，应该是一个字符串
                                    CheckSingleSingle = SokectProtocolDictionary["CheckSingleSingle"] = inode.GetAttribute("CheckSingleSingle").ToString(),//校验标志位
                                    CheckDataLength = SokectProtocolDictionary["CheckDataLength"] = inode.GetAttribute("CheckDataLength").ToString(),//校验位长度
                                    EndSingle = SokectProtocolDictionary["EndSingle"] = inode.GetAttribute("EndSingle").ToString(),//结束位对应的16进制，用","隔开 例："0D,0A"
                                    SplitChar = SokectProtocolDictionary["SplitChar"] = inode.GetAttribute("SplitChar").ToString(),//分隔符
                                    InforPerData = SokectProtocolDictionary["InforPerData"] = inode.GetAttribute("InforPerData").ToString(),//每条信息包含的数据量
                                    AddrIndex = SokectProtocolDictionary["AddrIndex"] = inode.GetAttribute("AddrIndex").ToString(),//地址索引
                                    ValueIndex = SokectProtocolDictionary["ValueIndex"] = inode.GetAttribute("ValueIndex").ToString() //值索引
                                };
                                sokect.StartChar = SokectProtocolDictionary["DataDefineRule"] = inode.GetAttribute("DataDefineRule").ToString();//数据转义，待转字符-转义数据，如果有多条数据，中间用逗号隔开  例："V-0,A-1"
                                XmlSokectList.Add(sokect);
                            }
                            reader.Close();
                            this.page2_gridview2.DataSource = XmlSokectList;
                            //动态加载控件
                            if (first_Flush_Flag)
                            {
                                DynamicCreateControl(SokectProtocolDictionary);
                            }
                            else
                            {
                                //清空原来的控件信息
                                layoutControl3.BeginUpdate();
                                layoutControl3.Controls.Clear();
                                layoutControl3.Root.Items.Clear();
                                DynamicCreateControl(SokectProtocolDictionary);
                                dataOpration.EndUpdate();
                            }
                            try
                            {
                                this.gridView2.Columns[0].Caption = "ID";
                                this.gridView2.Columns[0].Width = 40;
                                this.gridView2.Columns[1].Caption = "地址1";
                                this.gridView2.Columns[1].Width = 100;
                                this.gridView2.Columns[0].Caption = "端口1";
                                this.gridView2.Columns[0].Width = 40;
                                this.gridView2.Columns[1].Caption = "地址2";
                                this.gridView2.Columns[1].Width = 100;
                                this.gridView2.Columns[0].Caption = "端口2";
                                this.gridView2.Columns[0].Width = 40;
                                this.gridView2.Columns[1].Caption = "包头";//0183协议通讯数据包包头
                                this.gridView2.Columns[1].Width = 40;
                                this.gridView2.Columns[0].Caption = "设备标";
                                this.gridView2.Columns[0].Width = 40;
                                this.gridView2.Columns[1].Caption = "检验位";
                                this.gridView2.Columns[1].Width = 40;
                                this.gridView2.Columns[0].Caption = "校位长度";
                                this.gridView2.Columns[0].Width = 40;
                                this.gridView2.Columns[1].Caption = "结束位";
                                this.gridView2.Columns[1].Width = 40;
                                this.gridView2.Columns[0].Caption = "分隔符";
                                this.gridView2.Columns[0].Width = 40;
                                this.gridView2.Columns[1].Caption = "数据量";
                                this.gridView2.Columns[1].Width = 40;
                                this.gridView2.Columns[0].Caption = "地址索引";
                                this.gridView2.Columns[0].Width = 40;
                                this.gridView2.Columns[1].Caption = "值索引";
                                this.gridView2.Columns[1].Width = 40;
                                this.gridView2.Columns[0].Caption = "数据转义";
                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show("错误信息：" + ex.Message);
                            }
                            #endregion

                        }
                    }
                }
            }
            else if (navigationFrame1.SelectedPage.Caption.Trim().Equals("通道配置"))
            {
                try
                {
                    XmlAddressList = new List<XmlAddress>();
                    prefixName = "page3_txt_";
                    this.Page3_Address.DataSource = null;

                    LoadXml(addressFilePath);
                    XmlNode xmlNode = xmlDoc.SelectSingleNode("PointsType");
                    XmlNodeList nodeList = xmlNode.ChildNodes;
                    foreach (XmlNode no in nodeList)
                    {
                        XmlNodeList xmlNodeList = no.ChildNodes;
                        XmlElement xmlElement = (XmlElement)no;
                        string slaveid = xmlElement.GetAttribute("SlaveId");
                        string functionCode = xmlElement.GetAttribute("FunctionCode");
                        string englishName = xmlElement.GetAttribute("EnglishName");
                        foreach (var item in xmlNodeList)
                        {
                            XmlElement element = (XmlElement)item;
                            XmlAddress address = new XmlAddress
                            {
                                DataBit = element.GetAttribute("DataBit"),
                                BitIndex = element.GetAttribute("BitIndex"),
                                Factor = element.GetAttribute("Factor"),
                                LocalAddress = element.GetAttribute("LocalAddress"),
                                ProtocolAddress = element.GetAttribute("ProtocolAddress"),
                                EnglishName = element.GetAttribute("EnglishName"),
                                Text = element.InnerText,
                                FunctionCode = functionCode,
                                SlaveId = slaveid,


                            };
                            XmlAddressList.Add(address);
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("文件不正确或者暂时不能识别（报警组）" + Environment.NewLine + ex.Message); }
                page3_txt_opSlaveId.Text = page3_txt_SalveId.Text = XmlAddressList[0].SlaveId;
                page3_txt_Text.Text = XmlAddressList[0].Text;
                page3_txt_ProtocolAddress.Text = XmlAddressList[0].ProtocolAddress;
                page3_txt_LocalAddress.Text = XmlAddressList[0].LocalAddress;
                page3_txt_opFunction.Text = page3_txt_FunctionCode.Text = XmlAddressList[0].FunctionCode;
                page3_txt_Factor.Text = XmlAddressList[0].Factor;
                page3_txt_DataBit.Text = XmlAddressList[0].DataBit;
                page3_txt_BitIndex.Text = XmlAddressList[0].BitIndex;
                reader.Close();

                this.Page3_Address.DataSource = XmlAddressList;
                panelControl1.Visible = true;
                #region 列名
                this.gridView3.Columns[0].Caption = "从机ID";
                this.gridView3.Columns[0].Width = 50;
                this.gridView3.Columns[1].Caption = "功能码";
                this.gridView3.Columns[1].Width = 50;
                this.gridView3.Columns[2].Caption = "本地地址";
                this.gridView3.Columns[2].Width = 60;
                this.gridView3.Columns[3].Caption = "协议地址";
                this.gridView3.Columns[3].Width = 60;
                this.gridView3.Columns[4].Caption = "精度系数";
                this.gridView3.Columns[4].Width = 50;
                this.gridView3.Columns[5].Caption = "位位置";
                this.gridView3.Columns[5].Width = 50;
                this.gridView3.Columns[6].Caption = "数据位";
                this.gridView3.Columns[6].Width = 50;
                this.gridView3.Columns[7].Caption = "中文名称";
                this.gridView3.Columns[7].Width = 70;
                this.gridView3.Columns[8].Caption = "英文名称";

                #endregion
            }
            else if (navigationFrame1.SelectedPage.Caption.Trim().Equals("协议字段配置"))
            {
                prefixName = "page4_txt_";
                LoadXml(fieldFilePath);
                XmlNode xFields = xmlDoc.SelectSingleNode("Fields");
                XmlNodeList xFieldList = xFields.ChildNodes;
                foreach (XmlNode item in xFieldList)
                {
                    XmlElement ele = (XmlElement)item;
                    XmlField field = new XmlField
                    {
                        ID = ele.GetAttribute("ID"),
                        Name = ele.GetAttribute("Name")
                    };

                    XmlNodeList nodeList = item.ChildNodes;
                    StringBuilder sb = new StringBuilder();
                    foreach (XmlElement nodefield in nodeList)
                    {
                        sb.Append(nodefield.InnerText + ",");
                    }
                    FieldDirectory[field.Name] = sb;
                    sb = null;
                    XmlFieldList.Add(field);
                    page4_cmb_CheckProtocol.Properties.Items.Add(field.Name);
                }
                page4_cmb_CheckProtocol.SelectedIndex = 0;
                page4_cmb_CheckField.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                page4_cmb_CheckProtocol.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                page4_cmb_CheckProtocol_SelectedIndexChanged(this.page4_cmb_CheckProtocol, new EventArgs());
            }
            else
            {
                this.Page1_CommunicationConfig.DataSource = null;
                modelList = new List<Communication>();
                LoadXml(configFilePath);
                // 得到根节点Communications
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

                        Communication model = new Communication
                        {
                            ID = ele.GetAttribute("ID").ToString(),
                            CommType = ele.GetAttribute("CommType").ToString(),
                            NameCH = ele.GetAttribute("NameCH").ToString(),
                            NameEN = ele.GetAttribute("NameEN").ToString(),
                            ConfigFilePath = ele.GetAttribute("ConfigFilePath").ToString()
                        };
                        modelList.Add(model);
                    }
                }
                page1_txt_CommPath.Text = modelList[0].ConfigFilePath;
                page1_txt_ComType.Text = modelList[0].CommType;
                page1_txt_ID.Text = modelList[0].ID;
                page1_txt_NameCH.Text = modelList[0].NameCH;
                page1_txt_NameEN.Text = modelList[0].NameEN;
                //读取完毕后关闭reader
                reader.Close();
                this.Page1_CommunicationConfig.DataSource = modelList;

                this.gridView1.Columns[0].Caption = "ID";
                this.gridView1.Columns[0].Width = 30;
                this.gridView1.Columns[1].Caption = "协议类型";
                this.gridView1.Columns[1].Width = 100;
                this.gridView1.Columns[2].Caption = "中文名";
                this.gridView1.Columns[2].Width = 70;
                this.gridView1.Columns[3].Caption = "英文名";
                this.gridView1.Columns[3].Width = 70;
                this.gridView1.Columns[4].Caption = "文件路径";

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
            reader.Close();
            xmlDoc.Save(configFilePath);
            Communication communication = new Communication
            {
                ID = GetSelectOID(gridView1, "ID"),
                CommType = GetSelectOID(gridView1, "CommType"),
                NameCH = GetSelectOID(gridView1, "NameCH"),
                NameEN = GetSelectOID(gridView1, "NameEN"),
                ConfigFilePath = GetSelectOID(gridView1, "ConfigFilePath")
            };

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
            // page1_txt_ComType.Text = gridView1.CurrentRow.Cells[1].Value.ToString();
            // page1_txt_NameCH.Text = gridView1.CurrentRow.Cells[2].Value.ToString();
            // page1_txt_NameEN.Text = gridView1.CurrentRow.Cells[3].Value.ToString();
            // page1_txt_CommPath.Text = gridView1.CurrentRow.Cells[4].Value.ToString();
        }

        //mOIDFiledName为要获取列的列名
        private string GetSelectOID(GridView gv, string mOIDFiledName)
        {
            int[] pRows = gv.GetSelectedRows();//传递实体类过去 获取选中的行
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
            //this.gridView1.Columns["parent_id"].Caption = "父区划ID";
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

        }

        private void page1_btn_addData_Click(object sender, EventArgs e)
        {
            if (openFileDialog.FileName == "")
            {
                MessageBox.Show("请打开配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Communication communication = new Communication
            {
                ID = (int.Parse(modelList[modelList.Count - 1].ID) + 1).ToString(),
                NameCH = page1_txt_NameCH.Text,
                NameEN = page1_txt_NameEN.Text,
                CommType = page1_txt_ComType.Text,
                ConfigFilePath = page1_txt_CommPath.Text
            };

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
            reader.Close();
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
            LoadXml(configFilePath);
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
            LoadXml(configFilePath);
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


            Communication communication = new Communication
            {
                ID = GetSelectOID(gridView1, "ID"),
                CommType = GetSelectOID(gridView1, "CommType"),
                NameCH = GetSelectOID(gridView1, "NameCH"),
                NameEN = GetSelectOID(gridView1, "NameEN"),
                ConfigFilePath = GetSelectOID(gridView1, "ConfigFilePath")
            };

            modelList.Remove(communication);
            communication.ID = page1_txt_ID.Text;
            communication.CommType = page1_txt_ComType.Text;
            communication.NameCH = page1_txt_NameCH.Text;
            communication.NameEN = page1_txt_NameEN.Text;
            communication.ConfigFilePath = page1_txt_CommPath.Text;
            modelList.Add(communication);
            reader.Close();
            xmlDoc.Save(configFilePath);
            Reflash();
        }
        bool flage_Field = false;
        private void btn_configProtocol_Click(object sender, EventArgs e)
        {
            if (page1_txt_CommPath.Text.Equals(""))
            {
                MessageBox.Show("请打开配置文件");
                return;
            }

            navigationFrame1.SelectedPage =
                (NavigationPage)navigationFrame1.Pages.FindFirst
                (x => (string)x.Tag == barButtonItem10.Caption);
            //执行加载XML字段配置文件操作
            //判断已经加载过则不用再次加载
            if (!flage_Field)
            {
                LoadXMLField("./xmlField.xml");
                flage_Field = true;
            }
            openFileDialog.FileName = configFilePath.Substring(0, configFilePath.LastIndexOf("\\") + 1) + page1_txt_CommPath.Text;

            navigationFrame1.SelectedPage = (NavigationPage)
                navigationFrame1.Pages.FindFirst(x => (string)x.Tag == "协议配置");
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
                        reader.Close();
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
            if (dataMember.Equals("ID"))
            {
                editor.ReadOnly = true;
            }
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
                case "ID": return "ID";
                case "Can2Port": return "Can2端口";
                case "Can1Port": return "Can1端口";
                case "SlaveID": return "从机号";
                case "FunctionCode": return "功能码";
                case "FirstAddress": return "开始地址";
                case "RequestCount": return "请求个数";
                case "DataLenght": return "数据长度";
                case "StartSingle": return "开始标志";
                case "EndSingle": return "结束标志";
                case "PackageLength": return "包长度";
                case "FirstAddr": return "开始地址";
                case "CheckSumIndex": return "校验位置";
                case "CheckSumType": return "校验类型";
                case "CheckStartIndex": return "开始位置";
                case "CheckEndIndex": return "结束位置";
                case "CheckSumLength": return "校验长度";

                case "PortName": return "端口号";
                case "STOPBITS": return "停止位";
                case "DATABIT": return "数据位";
                case "PARITY": return "校验位";
                case "BAUREATE": return "波特率";
                case "FrameLenght": return "长度";



                case "StartChar": return "包头";
                case "CommSingle": return "标识符";
                case "CheckSingleSingle": return "检验标志位";
                case "CheckDataLength": return "检验位长度";
                case "SplitChar": return "分隔符";
                case "InforPerData": return "数据量";
                case "AddrIndex": return "地址索引";
                case "ValueIndex": return "值索引";
                case "DataDefineRule": return "数据转义";

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
            try
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
        private void DynamicCreateControl(Dictionary<string, string> keys)
        {
            try
            {
                first_Flush_Flag = false;
                object employeesSource = page2_gridview2.DataSource;

                dataOpration = layoutControl3.AddGroup("数据区");
                foreach (var item in keys)
                {
                    LayoutControlItem itemFirstName = CreateItemWithBoundEditor(new TextEdit(), employeesSource, item.Key, dataOpration);
                }
            }
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
            if (page1_txt_CommPath.Text.Equals(""))
            {
                MessageBox.Show("请打开配置文件");
                return;
            }

            navigationFrame1.SelectedPage =
                (NavigationPage)navigationFrame1.Pages.FindFirst
                (x => (string)x.Tag == barButtonItem10.Caption);
            //执行加载XML字段配置文件操作
            //判断已经加载过则不用再次加载
            if (!flage_Field)
            {
                LoadXMLField("./xmlField.xml");
                flage_Field = true;
            }
            if (protocolFilePath.Equals(""))
            {
                MessageBox.Show("找不到路径");
                return;
            }
            if (!configFilePath.Equals(""))
            {

                openFileDialog.FileName = configFilePath.Substring(0, configFilePath.LastIndexOf("\\") + 1)
                                  + page2_txt_ConfigFilePath.Text;
            }
            else if (!protocolFilePath.Equals(""))
            {
                openFileDialog.FileName = protocolFilePath.Substring(0, protocolFilePath.LastIndexOf("\\") + 1)
                    + page2_txt_ConfigFilePath.Text.Substring(protocolFilePath.LastIndexOf("\\"));
            }

            navigationFrame1.SelectedPage = (NavigationPage)
                navigationFrame1.Pages.FindFirst(x => (string)x.Tag == "通道配置");
            ReadXml();
        }
        /// <summary>
        /// 清空Page2中的文本框的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void page2_btn_clearContent_Click(object sender, EventArgs e)
        {
            foreach (BaseEdit item in Page2TXTBaseEdit)
            {

            }
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
        /// <summary>
        /// 第四页选择框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void page4_cmb_CheckProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            page4_cmb_CheckField.SelectedIndex = 0;
            page4_cmb_CheckField.Properties.Items.Clear();
            string selectStr = page4_cmb_CheckProtocol.EditValue.ToString();

            StringBuilder stringBuilder = FieldDirectory[selectStr];

            string[] fieldArray = stringBuilder.ToString().Trim().Split(',');
            foreach (var item in fieldArray)
            {
                if (item != "")
                {

                    page4_cmb_CheckField.Properties.Items.Add(item);
                }
            }

        }

        /// <summary>
        /// 加载XML字段配置文件
        /// </summary>
        /// <param name="fileName"></param>
        private void LoadXMLField(string fileName)
        {
            try
            {
                LoadXml(fileName);
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
                            CommFieldDictionary.Add(item.InnerText, "");
                        }

                    }
                    if (id.Trim().Equals("SokectCustomProtocol"))//SokectCustomProtocol
                    {
                        XmlNodeList eles = xnnode.ChildNodes;
                        foreach (XmlNode tcp in eles)
                        {
                            XmlElement xml = (XmlElement)tcp;
                            SokectCustomProtocolDictionary.Add(xml.InnerText, "");
                        }

                    }
                    else if (id.Trim().Equals("VDRProtocol"))//VDRProtocol
                    {
                        XmlNodeList eles = xnnode.ChildNodes;
                        foreach (XmlNode tcp in eles)
                        {
                            XmlElement xml = (XmlElement)tcp;
                            VDRProtocolDictionary.Add(xml.InnerText, "");
                        }
                    }
                    else if (id.Trim().Equals("SocketModbusProtocol"))//SocketModbusProtocol
                    {
                        XmlNodeList eles = xnnode.ChildNodes;
                        foreach (XmlNode tcp in eles)
                        {
                            XmlElement xml = (XmlElement)tcp;
                            SokectModbusProtocolDictionary.Add(xml.InnerText, "");
                        }
                    }
                    else
                    if (id.Trim().Equals("Sokect"))//SocketModbusProtocol
                    {
                        XmlNodeList eles = xnnode.ChildNodes;
                        foreach (XmlNode tcp in eles)
                        {
                            XmlElement xml = (XmlElement)tcp;
                            SokectProtocolDictionary.Add(xml.InnerText, "");
                        }
                    }
                    else
                    if (id.Trim().Equals("other"))//
                    {
                        XmlNodeList eles = xnnode.ChildNodes;
                        foreach (XmlNode tcp in eles)
                        {
                            XmlElement xml = (XmlElement)tcp;
                            ProtocolOtherDictionary.Add(xml.InnerText.ToString(), "");
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
            if (currentProtocolType == ProtocolType.Modbus)
            {
                /*
                SokectModbusProtocolDictionary["ID"] = GetSelectOID(gridView2, "ID");
                SokectModbusProtocolDictionary["SlaveID"] = GetSelectOID(gridView2, "SlaveID");
                SokectModbusProtocolDictionary["FunctionCode"] = GetSelectOID(gridView2, "FunctionCode");
                SokectModbusProtocolDictionary["FirstAddress"] = GetSelectOID(gridView2, "FirstAddress");
                SokectModbusProtocolDictionary["RequestCount"] = GetSelectOID(gridView2, "RequestCount");
                SokectModbusProtocolDictionary["DataLenght"] = GetSelectOID(gridView2, "DataLenght");
                */
                for (int i = 0; i < SokectModbusProtocolDictionary.Count; i++)
                {
                    var item = SokectModbusProtocolDictionary.ToArray()[i];
                    SokectCustomProtocolDictionary[item.Key] = GetSelectOID(gridView2, item.Key);

                }

                foreach (Control c in this.Controls)
                {
                    foreach (var item in SokectModbusProtocolDictionary)
                    {
                        if (c is BaseEdit && c.Name == prefixName + item.Key)
                        {
                            BaseEdit temp = c as BaseEdit;
                            temp.Text = item.Value;
                        }
                    }
                }
            }
            if (currentProtocolType == ProtocolType.custom)
            {
                for (int i = 0; i < SokectCustomProtocolDictionary.Count; i++)
                {
                    var item = SokectCustomProtocolDictionary.ToArray()[i];
                    SokectCustomProtocolDictionary[item.Key] = GetSelectOID(gridView2, item.Key);

                }
                foreach (var item in SokectCustomProtocolDictionary)
                {
                    BaseEdit edit = layoutControl3.Controls.Find(prefixName + item.Key, false)[0] as BaseEdit;
                    edit.Text = item.Value;

                }


            }
            if (currentProtocolType == ProtocolType.other)
            {

                for (int i = 0; i < ProtocolOtherDictionary.Count; i++)
                {
                    var item = ProtocolOtherDictionary.ToArray()[i];
                    ProtocolOtherDictionary[item.Key] = GetSelectOID(gridView2, item.Key);

                }
                foreach (var item in ProtocolOtherDictionary)
                {
                    BaseEdit edit = layoutControl3.Controls.Find(prefixName + item.Key, false)[0] as BaseEdit;
                    edit.Text = item.Value;
                }
            }
            if (currentProtocolType == ProtocolType.Sokect)
            {

                for (int i = 0; i < SokectProtocolDictionary.Count; i++)
                {
                    var item = SokectProtocolDictionary.ToArray()[i];
                    SokectProtocolDictionary[item.Key] = GetSelectOID(gridView2, item.Key);

                }

                foreach (var item in SokectProtocolDictionary)
                {
                    BaseEdit edit = layoutControl3.Controls.Find(prefixName + item.Key, false)[0] as BaseEdit;
                    edit.Text = item.Value;
                }
            }
            if (currentProtocolType == ProtocolType.vdr)
            {

                for (int i = 0; i < VDRProtocolDictionary.Count; i++)
                {
                    var item = VDRProtocolDictionary.ToArray()[i];
                    VDRProtocolDictionary[item.Key] = GetSelectOID(gridView2, item.Key);

                }
                foreach (var item in VDRProtocolDictionary)
                {
                    BaseEdit edit = layoutControl3.Controls.Find(prefixName + item.Key, false)[0] as BaseEdit;
                    edit.Text = item.Value;
                }
            }

        }
        private void page2_btn_update_Click(object sender, EventArgs e)
        {
            LoadXml(protocolFilePath);
            XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
            string strPath = string.Format("/Communications/CommunicaitonData/Communication[@ID=\"{0}\"]",
                page2_txt_ID.Text);


            //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点. 
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);

            // selectXe.SetAttribute("CommType", txt_data2.Text);//也可以通过SetAttribute来增加一个属性

            selectXe.SetAttribute("ID", page2_txt_ID.Text);
            selectXe.SetAttribute("CommReConnectTime", page2_txt_CommReConnectTime.Text);
            selectXe.SetAttribute("CommErroTime", page2_txt_CommErroTime.Text);
            selectXe.SetAttribute("WriteTimeOut", page2_txt_WriteTimeOut.Text);

            selectXe.SetAttribute("ReadTimeOut", page2_txt_ReadTimeOut.Text);
            selectXe.SetAttribute("CommSpaceTime", page2_txt_CommSpaceTime.Text);
            selectXe.SetAttribute("ConfigFilePath", page2_txt_ConfigFilePath.Text);
            selectXe.SetAttribute("EnglishiName", page2_txt_EnglishiName.Text);
            selectXe.SetAttribute("ChineseName", page2_txt_ChineseName.Text);

            switch (currentProtocolType)
            {
                case ProtocolType.custom:

                    XmlElement doc = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
                    string Path = string.Format("/Communications/CommunicaitonData/Communication[@ID=\"{0}\"]/TCP[@ID=\"{1}\"]",
                         page2_txt_ID.Text, layoutControl3.Controls.Find(prefixName + "ID", false)[0].Text);
                    //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                    XmlElement xmlElement = (XmlElement)doc.SelectSingleNode(Path);
                    // selectXe.SetAttribute("CommType", txt_data2.Text);//也可以通过SetAttribute来增加一个属性
                    var custom = SokectCustomProtocolDictionary.ToArray();
                    for (int i = 0; i < custom.Length; i++)
                    {
                        BaseEdit baseEdit = layoutControl3.Controls.Find(prefixName + custom[i].Key, false)[0] as BaseEdit;
                        SokectCustomProtocolDictionary[custom[i].Key] = baseEdit.Text;
                        xmlElement.SetAttribute(custom[i].Key, baseEdit.Text);
                    }

                    /* 
                     * foreach (Control c in this.Controls)
                                            {
                                                foreach (var custom in SokectCustomProtocolDictionary)
                                                {
                                                    if (c is BaseEdit && c.Name == prefixName + item.Key)
                                                    {
                                                        BaseEdit temp = c as BaseEdit;
                                                        temp.Text = item.Value;
                                                        xmlElement.SetAttribute(custom.Key, temp.Text); 
                                                    }
                                                }
                                            }
                                            */

                    /*
                    XmlSokectCustomProtocol sokectCustomProtocol = new XmlSokectCustomProtocol
                    {
                        ID = GetSelectOID(gridView2, "ID"),
                        Can1IP = GetSelectOID(gridView2, "Can1IP"),
                        Can1Port = GetSelectOID(gridView2, "Can1Port"),
                        Can2IP = GetSelectOID(gridView2, "Can2IP"),
                        Can2Port = GetSelectOID(gridView2, "Can2Port"),
                        CheckEndIndex = GetSelectOID(gridView2, "CheckEndIndex"),
                        CheckStartIndex = GetSelectOID(gridView2, "CheckStartIndex"),
                        CheckSumIndex = GetSelectOID(gridView2, "CheckSumIndex"),
                        CheckSumLength = GetSelectOID(gridView2, "CheckSumLength"),
                        CheckSumType = GetSelectOID(gridView2, "CheckSumType"),
                        EndSingle = GetSelectOID(gridView2, "EndSingle"),
                        FirstAddr = GetSelectOID(gridView2, "FirstAddr"),
                        PackageLength = GetSelectOID(gridView2, "PackageLength"),
                        StartSingle = GetSelectOID(gridView2, "StartSingle")
                    };
                    XmlcustomList.Remove(sokectCustomProtocol);

                    sokectCustomProtocol.ID = SokectCustomProtocolDictionary["ID"];
                    sokectCustomProtocol.Can1IP = SokectCustomProtocolDictionary["Can1IP"];
                    sokectCustomProtocol.Can1Port = SokectCustomProtocolDictionary["Can1Port"];
                    sokectCustomProtocol.Can2IP = SokectCustomProtocolDictionary["Can2IP"];
                    sokectCustomProtocol.Can2Port = SokectCustomProtocolDictionary["Can2Port"];
                    sokectCustomProtocol.CheckEndIndex = SokectCustomProtocolDictionary["CheckEndIndex"];
                    sokectCustomProtocol.CheckStartIndex = SokectCustomProtocolDictionary["CheckStartIndex"];
                    sokectCustomProtocol.CheckSumIndex = SokectCustomProtocolDictionary["CheckSumIndex"];
                    sokectCustomProtocol.CheckSumLength = SokectCustomProtocolDictionary["CheckSumLength"];
                    sokectCustomProtocol.CheckSumType = SokectCustomProtocolDictionary["CheckSumType"];
                    sokectCustomProtocol.EndSingle = SokectCustomProtocolDictionary["EndSingle"];
                    sokectCustomProtocol.FirstAddr = SokectCustomProtocolDictionary["FirstAddr"];
                    sokectCustomProtocol.PackageLength = SokectCustomProtocolDictionary["PackageLength"];
                    sokectCustomProtocol.StartSingle = SokectCustomProtocolDictionary["StartSingle"];
                    XmlcustomList.Add(sokectCustomProtocol);
                    /* selectXe.GetElementsByTagName("CommType").Item(0).InnerText = txt_data2.Text;
                     selectXe.GetElementsByTagName("NameCH").Item(0).InnerText = txt_data3.Text;
                     selectXe.GetElementsByTagName("NameEN").Item(0).InnerText = txt_data4.Text;
                     selectXe.GetElementsByTagName("ConfigFilePath").Item(0).InnerText = txt_path.Text;
                     */
                    string path = xmlDoc.BaseURI.ToString().Substring(xmlDoc.BaseURI.ToString().LastIndexOf("///") + 3);
                    reader.Close();

                    xmlDoc.Save(path);
                    ReadXml();
                    Page2_Reflash(XmlcustomList);

                    break;
                case ProtocolType.Modbus:

                    doc = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
                    Path = string.Format("/Communications/CommunicaitonData/Communication[@ID=\"{0}\"]/TCP",
                      page2_txt_ID.Text);
                    //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                    xmlElement = (XmlElement)doc.SelectSingleNode(Path);
                    // selectXe.SetAttribute("CommType", txt_data2.Text);//也可以通过SetAttribute来增加一个属性

                    custom = SokectModbusProtocolDictionary.ToArray();
                    for (int i = 0; i < custom.Length; i++)
                    {
                        BaseEdit baseEdit = layoutControl3.Controls.Find(prefixName + custom[i].Key, false)[0] as BaseEdit;
                        SokectModbusProtocolDictionary[custom[i].Key] = baseEdit.Text;
                        xmlElement.SetAttribute(custom[i].Key, baseEdit.Text);
                    }
                    /*
                    XmlModbus modbus = new XmlModbus
                    {
                        ID = GetSelectOID(gridView2, "ID"),
                        Can1IP = GetSelectOID(gridView2, "Can1IP"),
                        Can1Port = GetSelectOID(gridView2, "Can1Port"),
                        Can2IP = GetSelectOID(gridView2, "Can2IP"),
                        Can2Port = GetSelectOID(gridView2, "Can2Port"),
                        DataLenght = GetSelectOID(gridView2, "DataLenght"),
                        FirstAddress = GetSelectOID(gridView2, "FirstAddress"),
                        FunctionCode = GetSelectOID(gridView2, "FunctionCode"),
                        RequestCount = GetSelectOID(gridView2, "RequestCount"),
                        SlaveID = GetSelectOID(gridView2, "SlaveID")
                    };
                    XmlModbusList.Remove(modbus);

                    modbus.ID = SokectModbusProtocolDictionary["ID"];
                    modbus.Can1IP = SokectModbusProtocolDictionary["Can1IP"];
                    modbus.Can1Port = SokectModbusProtocolDictionary["Can1Port"];
                    modbus.Can2IP = SokectModbusProtocolDictionary["Can2IP"];
                    modbus.Can2Port = SokectModbusProtocolDictionary["Can2Port"];
                    modbus.DataLenght = SokectModbusProtocolDictionary["DataLenght"];
                    modbus.FirstAddress = SokectModbusProtocolDictionary["FirstAddress"];
                    modbus.FunctionCode = SokectModbusProtocolDictionary["FunctionCode"];
                    modbus.RequestCount = SokectModbusProtocolDictionary["RequestCount"];
                    modbus.SlaveID = SokectModbusProtocolDictionary["SlaveID"];
                    XmlModbusList.Add(modbus);
                    /* selectXe.GetElementsByTagName("CommType").Item(0).InnerText = txt_data2.Text;
                     selectXe.GetElementsByTagName("NameCH").Item(0).InnerText = txt_data3.Text;
                     selectXe.GetElementsByTagName("NameEN").Item(0).InnerText = txt_data4.Text;
                     selectXe.GetElementsByTagName("ConfigFilePath").Item(0).InnerText = txt_path.Text;
                     */
                    path = xmlDoc.BaseURI.ToString().Substring(xmlDoc.BaseURI.ToString().LastIndexOf("///") + 3);
                    reader.Close();
                    xmlDoc.Save(path);
                    ReadXml();
                    Page2_Reflash(XmlModbusList);

                    break;
                case ProtocolType.Sokect:

                    doc = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
                    Path = string.Format("/Communications/CommunicaitonData/Communication[@ID=\"{0}\"]/TCP",
                      page2_txt_ID.Text);
                    //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                    xmlElement = (XmlElement)doc.SelectSingleNode(Path);
                    // selectXe.SetAttribute("CommType", txt_data2.Text);//也可以通过SetAttribute来增加一个属性



                    custom = SokectProtocolDictionary.ToArray();
                    for (int i = 0; i < custom.Length; i++)
                    {
                        BaseEdit baseEdit = layoutControl3.Controls.Find(prefixName + custom[i].Key, false)[0] as BaseEdit;
                        SokectProtocolDictionary[custom[i].Key] = baseEdit.Text;
                        xmlElement.SetAttribute(custom[i].Key, baseEdit.Text);
                    }

                    /*
                    XmlSokect sokect = new XmlSokect();

                    sokect.ID = GetSelectOID(gridView2, "ID");
                    sokect.Can1IP = GetSelectOID(gridView2, "Can1IP");
                    sokect.Can1Port = GetSelectOID(gridView2, "Can1Port");
                    sokect.Can1Port = GetSelectOID(gridView2, "Can1Port");
                    sokect.Can2IP = GetSelectOID(gridView2, "Can2IP");
                    sokect.Can2Port = GetSelectOID(gridView2, "Can2Port");
                    sokect.AddrIndex = GetSelectOID(gridView2, "AddrIndex");
                    sokect.CheckDataLength = GetSelectOID(gridView2, "CheckDataLength");
                    sokect.CheckSingleSingle = GetSelectOID(gridView2, "CheckSingleSingle");
                    sokect.CommSingle = GetSelectOID(gridView2, "CommSingle");
                    sokect.DataDefineRule = GetSelectOID(gridView2, "DataDefineRule");
                    sokect.EndSingle = GetSelectOID(gridView2, "EndSingle");
                    sokect.InforPerData = GetSelectOID(gridView2, "InforPerData");
                    sokect.SplitChar = GetSelectOID(gridView2, "SplitChar");
                    sokect.StartChar = GetSelectOID(gridView2, "StartChar");
                    sokect.ValueIndex = GetSelectOID(gridView2, "ValueIndex");

                    XmlSokectList.Remove(sokect);

                    sokect.ID = SokectProtocolDictionary["ID"];
                    sokect.Can1IP = SokectProtocolDictionary["Can1IP"];
                    sokect.Can1Port = SokectProtocolDictionary["Can1Port"];
                    sokect.Can2IP = SokectProtocolDictionary["Can2IP"];
                    sokect.Can2Port = SokectProtocolDictionary["Can2Port"];
                    sokect.AddrIndex = SokectProtocolDictionary["CheckEndIndex"];
                    sokect.CheckDataLength = SokectProtocolDictionary["CheckStartIndex"];
                    sokect.CheckSingleSingle = SokectProtocolDictionary["CheckSumIndex"];
                    sokect.CommSingle = SokectProtocolDictionary["CheckSumLength"];
                    sokect.DataDefineRule = SokectProtocolDictionary["CheckSumType"];
                    sokect.EndSingle = SokectProtocolDictionary["EndSingle"];
                    sokect.InforPerData = SokectProtocolDictionary["FirstAddr"];
                    sokect.SplitChar = SokectProtocolDictionary["PackageLength"];
                    sokect.StartChar = SokectProtocolDictionary["StartSingle"];
                    sokect.ValueIndex = SokectProtocolDictionary["ValueIndex"];

                    XmlSokectList.Add(sokect);
                    path = xmlDoc.BaseURI.ToString().Substring(xmlDoc.BaseURI.ToString().LastIndexOf("///") + 3);
                    xmlDoc.Save(path);
                    Page2_Reflash(XmlSokectList);

                    break;
                case ProtocolType.vdr:

                    doc = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
                    Path = string.Format("/Communications/CommunicaitonData/Communication[@ID=\"{0}\"]/TCP",
                     page2_txt_ID.Text);
                    //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                    xmlElement = (XmlElement)doc.SelectSingleNode(Path);
                    // selectXe.SetAttribute("CommType", txt_data2.Text);//也可以通过SetAttribute来增加一个属性


                    custom = VDRProtocolDictionary.ToArray();
                    for (int i = 0; i < custom.Length; i++)
                    {
                        BaseEdit baseEdit = layoutControl3.Controls.Find(prefixName + custom[i].Key, false)[0] as BaseEdit;
                        VDRProtocolDictionary[custom[i].Key] = baseEdit.Text;
                        xmlElement.SetAttribute(custom[i].Key, baseEdit.Text);
                    }



                    XmlVDRProtocol vdr = new XmlVDRProtocol();

                    vdr.ID = GetSelectOID(gridView2, "ID");
                    vdr.BAUREATE = GetSelectOID(gridView2, "BAUREATE");
                    vdr.DATABIT = GetSelectOID(gridView2, "DATABIT");
                    vdr.FirstAddr = GetSelectOID(gridView2, "FirstAddr");
                    vdr.FrameLenght = GetSelectOID(gridView2, "FrameLenght");
                    vdr.PARITY = GetSelectOID(gridView2, "PARITY");
                    vdr.PortName = GetSelectOID(gridView2, "PortName");
                    vdr.StartSingle = GetSelectOID(gridView2, "StartSingle");
                    vdr.STOPBITS = GetSelectOID(gridView2, "STOPBITS");
                    XmlVDRList.Remove(vdr);

                    vdr.ID = VDRProtocolDictionary["ID"];
                    vdr.BAUREATE = VDRProtocolDictionary["BAUREATE"];
                    vdr.DATABIT = VDRProtocolDictionary["DATABIT"];
                    vdr.FirstAddr = VDRProtocolDictionary["FirstAddr"];
                    vdr.FrameLenght = VDRProtocolDictionary["FrameLenght"];
                    vdr.PARITY = VDRProtocolDictionary["PARITY"];
                    vdr.PortName = VDRProtocolDictionary["PortName"];
                    vdr.StartSingle = VDRProtocolDictionary["StartSingle"];
                    vdr.STOPBITS = VDRProtocolDictionary["STOPBITS"];
                    XmlVDRList.Add(vdr);
                    path = xmlDoc.BaseURI.ToString().Substring(xmlDoc.BaseURI.ToString().LastIndexOf("///") + 3);
                    reader.Close();
                    xmlDoc.Save(path);
                    Page2_Reflash(XmlVDRList);

                    break;
                case ProtocolType.other:

                    doc = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
                    Path = string.Format("/Communications/CommunicaitonData/Communication[@ID=\"{0}\"]/TCP",
                     page2_txt_ID.Text);
                    //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
                    xmlElement = (XmlElement)doc.SelectSingleNode(Path);
                    // selectXe.SetAttribute("CommType", txt_data2.Text);//也可以通过SetAttribute来增加一个属性



                    custom = ProtocolOtherDictionary.ToArray();
                    for (int i = 0; i < custom.Length; i++)
                    {
                        BaseEdit baseEdit = layoutControl3.Controls.Find(prefixName + custom[i].Key, false)[0] as BaseEdit;
                        ProtocolOtherDictionary[custom[i].Key] = baseEdit.Text;
                        xmlElement.SetAttribute(custom[i].Key, baseEdit.Text);
                    }




                    XmlOther other = new XmlOther();

                    other.ID = GetSelectOID(gridView2, "ID");
                    other.Can1IP = GetSelectOID(gridView2, "Can1IP");
                    other.Can1Port = GetSelectOID(gridView2, "Can1Port");
                    other.Can2IP = GetSelectOID(gridView2, "Can2IP");
                    other.Can2Port = GetSelectOID(gridView2, "Can2Port");
                    other.CheckEndIndex = GetSelectOID(gridView2, "CheckEndIndex");
                    other.CheckStartIndex = GetSelectOID(gridView2, "CheckStartIndex");
                    other.CheckSumIndex = GetSelectOID(gridView2, "CheckSumIndex");
                    other.CheckSumType = GetSelectOID(gridView2, "CheckSumType");
                    other.EndSingle = GetSelectOID(gridView2, "EndSingle");
                    other.FirstAddr = GetSelectOID(gridView2, "FirstAddr");
                    other.PackageLength = GetSelectOID(gridView2, "PackageLength");
                    other.StartSingle = GetSelectOID(gridView2, "StartSingle");
                    other.DATABIT = GetSelectOID(gridView2, "");
                    other.DataLenght = GetSelectOID(gridView2, "");
                    other.FirstAddress = GetSelectOID(gridView2, "");
                    other.FrameLenght = GetSelectOID(gridView2, "");
                    other.FunctionCode = GetSelectOID(gridView2, "");
                    other.PARITY = GetSelectOID(gridView2, "");
                    other.PortName = GetSelectOID(gridView2, "");
                    other.RequestCount = GetSelectOID(gridView2, "");
                    other.SlaveID = GetSelectOID(gridView2, "");
                    other.Standby1 = GetSelectOID(gridView2, "");
                    other.Standby2 = GetSelectOID(gridView2, "");
                    other.Standby3 = GetSelectOID(gridView2, "");
                    other.Standby4 = GetSelectOID(gridView2, "");
                    other.Standby5 = GetSelectOID(gridView2, "");
                    other.Standby6 = GetSelectOID(gridView2, "");
                    other.Standby7 = GetSelectOID(gridView2, "");
                    other.Standby8 = GetSelectOID(gridView2, "");
                    other.STOPBITS = GetSelectOID(gridView2, "");
                    XmlOtherList.Remove(other);

                    other.ID = ProtocolOtherDictionary["ID"];
                    other.Can1IP = ProtocolOtherDictionary["Can1IP"];
                    other.Can1Port = ProtocolOtherDictionary["Can1Port"];
                    other.Can2IP = ProtocolOtherDictionary["Can2IP"];
                    other.Can2Port = ProtocolOtherDictionary["Can2Port"];
                    other.CheckEndIndex = ProtocolOtherDictionary["CheckEndIndex"];
                    other.CheckStartIndex = ProtocolOtherDictionary["CheckStartIndex"];
                    other.CheckSumIndex = ProtocolOtherDictionary["CheckSumIndex"];
                    other.CheckSumType = ProtocolOtherDictionary["CheckSumType"];
                    other.EndSingle = ProtocolOtherDictionary["EndSingle"];
                    other.FirstAddr = ProtocolOtherDictionary["FirstAddr"];
                    other.PackageLength = ProtocolOtherDictionary["PackageLength"];
                    other.StartSingle = ProtocolOtherDictionary["StartSingle"];
                    other.DATABIT = ProtocolOtherDictionary["DATABIT"];
                    other.DataLenght = ProtocolOtherDictionary["DataLenght"];
                    other.FirstAddress = ProtocolOtherDictionary["FirstAddress"];
                    other.FrameLenght = ProtocolOtherDictionary["FrameLenght"];
                    other.FunctionCode = ProtocolOtherDictionary["FunctionCode"];
                    other.PARITY = ProtocolOtherDictionary["PARITY"];
                    other.PortName = ProtocolOtherDictionary["PortName"];
                    other.RequestCount = ProtocolOtherDictionary["RequestCount"];
                    other.SlaveID = ProtocolOtherDictionary["SlaveID"];
                    other.Standby1 = ProtocolOtherDictionary["Standby1"];
                    other.Standby2 = ProtocolOtherDictionary["Standby2"];
                    other.Standby3 = ProtocolOtherDictionary["Standby3"];
                    other.Standby4 = ProtocolOtherDictionary["Standby4"];
                    other.Standby5 = ProtocolOtherDictionary["Standby5"];
                    other.Standby6 = ProtocolOtherDictionary["Standby6"];
                    other.Standby7 = ProtocolOtherDictionary["Standby7"];
                    other.Standby8 = ProtocolOtherDictionary["Standby8"];
                    other.STOPBITS = ProtocolOtherDictionary["STOPBITS"];

                    XmlOtherList.Add(other);
                    */
                    path = xmlDoc.BaseURI.ToString().Substring(xmlDoc.BaseURI.ToString().LastIndexOf("///") + 3);
                    reader.Close();
                    xmlDoc.Save(path);
                    ReadXml();
                    Page2_Reflash(XmlOtherList);

                    break;
            }

        }

        private void page2_btn_deleteCheck_Click(object sender, EventArgs e)
        {
            LoadXml(protocolFilePath);

            XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
            string strPath = string.Format(findCommand,
                GetSelectOID(gridView2, "ID"));
            //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            selectXe.ParentNode.RemoveChild(selectXe);
            reader.Close();
            xmlDoc.Save(openFileDialog.FileName);

            switch (currentProtocolType)
            {
                case ProtocolType.custom:
                    XmlSokectCustomProtocol custom = new XmlSokectCustomProtocol
                    {
                        ID = GetSelectOID(gridView2, "ID"),
                        Can1IP = GetSelectOID(gridView2, "Can1IP"),
                        Can1Port = GetSelectOID(gridView2, "Can1Port"),
                        Can2IP = GetSelectOID(gridView2, "Can2IP"),
                        Can2Port = GetSelectOID(gridView2, "Can2Port"),
                        CheckEndIndex = GetSelectOID(gridView2, "CheckEndIndex"),
                        CheckStartIndex = GetSelectOID(gridView2, "CheckStartIndex"),
                        CheckSumIndex = GetSelectOID(gridView2, "CheckSumIndex"),
                        CheckSumLength = GetSelectOID(gridView2, "CheckSumLength"),
                        CheckSumType = GetSelectOID(gridView2, "CheckSumType"),
                        EndSingle = GetSelectOID(gridView2, "EndSingle"),
                        FirstAddr = GetSelectOID(gridView2, "FirstAddr"),
                        PackageLength = GetSelectOID(gridView2, "PackageLength"),
                        StartSingle = GetSelectOID(gridView2, "StartSingle")

                    };
                    XmlcustomList.Remove(custom);
                    page2_gridview2.DataSource = XmlcustomList.ToArray();
                    break;
                case ProtocolType.Modbus:
                    XmlModbus modbus = new XmlModbus
                    {
                        Can2IP = GetSelectOID(gridView2, "Can2IP"),
                        Can2Port = GetSelectOID(gridView2, "Can2Port"),
                        Can1IP = GetSelectOID(gridView2, "Can1IP"),
                        Can1Port = GetSelectOID(gridView2, "Can1Port"),
                        ID = GetSelectOID(gridView2, "ID"),
                        SlaveID = GetSelectOID(gridView2, "SlaveID"),
                        FirstAddress = GetSelectOID(gridView2, "FirstAddress"),
                        FunctionCode = GetSelectOID(gridView2, "FunctionCode"),
                        RequestCount = GetSelectOID(gridView2, "RequestCount"),
                        DataLenght = GetSelectOID(gridView2, "DataLenght")
                    };
                    XmlModbusList.Remove(modbus);
                    page2_gridview2.DataSource = XmlModbusList.ToArray();
                    break;
                case ProtocolType.other:
                    XmlOther other = new XmlOther
                    {
                        ID = GetSelectOID(gridView2, "ID"),
                        Can2IP = GetSelectOID(gridView2, "Can2IP"),
                        Can2Port = GetSelectOID(gridView2, "Can2Port"),
                        Can1IP = GetSelectOID(gridView2, "Can1IP"),
                        Can1Port = GetSelectOID(gridView2, "Can1Port"),
                        SlaveID = GetSelectOID(gridView2, "SlaveID"),
                        FunctionCode = GetSelectOID(gridView2, "FunctionCode"),
                        FirstAddress = GetSelectOID(gridView2, "FirstAddress"),
                        RequestCount = GetSelectOID(gridView2, "RequestCount"),
                        DataLenght = GetSelectOID(gridView2, "DataLenght"),
                        PortName = GetSelectOID(gridView2, "PortName"),
                        STOPBITS = GetSelectOID(gridView2, "STOPBITS"),
                        DATABIT = GetSelectOID(gridView2, "DATABIT"),
                        PARITY = GetSelectOID(gridView2, "PARITY"),
                        BAUREATE = GetSelectOID(gridView2, "BAUREATE"),
                        FrameLenght = GetSelectOID(gridView2, "FrameLenght"),
                        StartSingle = GetSelectOID(gridView2, "StartSingle"),
                        EndSingle = GetSelectOID(gridView2, "EndSingle"),
                        PackageLength = GetSelectOID(gridView2, "PackageLength"),
                        FirstAddr = GetSelectOID(gridView2, "FirstAddr"),
                        CheckSumIndex = GetSelectOID(gridView2, "CheckSumIndex"),
                        CheckSumType = GetSelectOID(gridView2, "CheckSumType"),
                        CheckStartIndex = GetSelectOID(gridView2, "CheckStartIndex"),
                        CheckEndIndex = GetSelectOID(gridView2, "CheckEndIndex"),
                        Standby1 = GetSelectOID(gridView2, "Standby1"),
                        Standby2 = GetSelectOID(gridView2, "Standby2"),
                        Standby3 = GetSelectOID(gridView2, "Standby3"),
                        Standby4 = GetSelectOID(gridView2, "Standby4"),
                        Standby5 = GetSelectOID(gridView2, "Standby5"),
                        Standby6 = GetSelectOID(gridView2, "Standby6"),
                        Standby7 = GetSelectOID(gridView2, "Standby7"),
                        Standby8 = GetSelectOID(gridView2, "Standby8")
                    };
                    XmlOtherList.Remove(other);
                    page2_gridview2.DataSource = XmlOtherList.ToArray();
                    break;
                case ProtocolType.Sokect:
                    XmlSokect sokect = new XmlSokect
                    {
                        ID = GetSelectOID(gridView2, "PortName"),
                        Can1IP = GetSelectOID(gridView2, "PortName"),
                        Can1Port = GetSelectOID(gridView2, "PortName"),
                        Can2IP = GetSelectOID(gridView2, "PortName"),
                        Can2Port = GetSelectOID(gridView2, "PortName"),
                        StartChar = GetSelectOID(gridView2, "PortName"),//0183协议通讯数据包包头  //数据转义，待转字符-转义数据，如果有多条数据，中间用逗号隔开  例："V-0,A-1"
                        CommSingle = GetSelectOID(gridView2, "PortName"),//设备标识符，应该是一个字符串
                        CheckSingleSingle = GetSelectOID(gridView2, "PortName"),//校验标志位
                        CheckDataLength = GetSelectOID(gridView2, "PortName"),//校验位长度
                        EndSingle = GetSelectOID(gridView2, "PortName"),//结束位对应的16进制，用","隔开 例："0D,0A"
                        SplitChar = GetSelectOID(gridView2, "PortName"),//分隔符
                        InforPerData = GetSelectOID(gridView2, "PortName"),//每条信息包含的数据量
                        AddrIndex = GetSelectOID(gridView2, "PortName"),//地址索引
                        ValueIndex = GetSelectOID(gridView2, "PortName"),//值索引 
                    };

                    XmlSokectList.Remove(sokect);
                    page2_gridview2.DataSource = XmlSokectList.ToArray();
                    break;
                case ProtocolType.vdr:
                    XmlVDRProtocol vdr = new XmlVDRProtocol
                    {
                        PortName = GetSelectOID(gridView2, "PortName"),
                        STOPBITS = GetSelectOID(gridView2, "STOPBITS"),
                        DATABIT = GetSelectOID(gridView2, "DATABIT"),
                        PARITY = GetSelectOID(gridView2, "PARITY"),
                        BAUREATE = GetSelectOID(gridView2, "BAUREATE"),
                        FrameLenght = GetSelectOID(gridView2, "FrameLenght"),
                        FirstAddr = GetSelectOID(gridView2, "FirstAddr"),
                        StartSingle = GetSelectOID(gridView2, "StartSingle")
                    };
                    XmlVDRList.Remove(vdr);
                    page2_gridview2.DataSource = XmlVDRList.ToArray();
                    break;

            }
        }

        private void Page3_Address_MouseClick(object sender, MouseEventArgs e)
        {
            this.page3_txt_BitIndex.Text = GetSelectOID(gridView3, "BitIndex");
            this.page3_txt_DataBit.Text = GetSelectOID(gridView3, "DataBit");
            this.page3_txt_Factor.Text = GetSelectOID(gridView3, "Factor");
            this.page3_txt_FunctionCode.Text = page3_txt_opFunction.Text = GetSelectOID(gridView3, "FunctionCode");
            this.page3_txt_LocalAddress.Text = GetSelectOID(gridView3, "LocalAddress");
            this.page3_txt_ProtocolAddress.Text = GetSelectOID(gridView3, "ProtocolAddress");
            this.page3_txt_SalveId.Text = page3_txt_opSlaveId.Text = GetSelectOID(gridView3, "SlaveId");
            this.page3_txt_Text.Text = GetSelectOID(gridView3, "Text");
            this.page3_txt_englishName.Text = GetSelectOID(gridView3, "EnglishName");

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            page3_txt_SalveId.Text =
            page3_txt_Text.Text =
            page3_txt_ProtocolAddress.Text =
            page3_txt_LocalAddress.Text =
            page3_txt_FunctionCode.Text =
            page3_txt_Factor.Text =
            page3_txt_DataBit.Text =
            page3_txt_BitIndex.Text = "";
        }

        private void page3_btn_delCheck_Click(object sender, EventArgs e)
        {
            DeleteAddress();
            ReadXml();
        }
        /// <summary>
        /// 删除当前选择行的记录
        /// </summary>
        private void DeleteAddress()
        {
            LoadXml(addressFilePath);
            XmlElement xe = xmlDoc.DocumentElement;
            string strPath = string.Format("/PointsType/Points[@SlaveId=\"{0}\"and @FunctionCode=\"{1}\"]/Point[@LocalAddress=\"{2}\"]",
                GetSelectOID(gridView3, "SlaveId"), GetSelectOID(gridView3, "FunctionCode"), GetSelectOID(gridView3, "LocalAddress"));
            //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            if (selectXe == null)
            {
                reader.Close();
                return;
            }
            selectXe.ParentNode.RemoveChild(selectXe);
            reader.Close();
            xmlDoc.Save(addressFilePath);
            #region
            /*
            XmlAddress address = new XmlAddress()
            {
                BitIndex = GetSelectOID(gridView3, "BitIndex"),
                DataBit = GetSelectOID(gridView3, "DataBit"),
                Factor = GetSelectOID(gridView3, "Factor"),
                FunctionCode = GetSelectOID(gridView3, "FunctionCode"),
                LocalAddress = GetSelectOID(gridView3, "LocalAddress"),
                ProtocolAddress = GetSelectOID(gridView3, "ProtocolAddress"),
                SlaveId = GetSelectOID(gridView3, "SlaveId"),
                Text = GetSelectOID(gridView3, "Text") 
            };

              XmlAddressList.Remove(address);
              */
            #endregion

        }
        /// <summary>
        ///根据从机ID、功能码、地址号来删除当前记录
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="functionCode"></param>
        /// <param name="localAddress"></param>
        private void DeleteAddress(string slaveId, string functionCode, string localAddress)
        {
            LoadXml(addressFilePath);
            XmlElement xe = xmlDoc.DocumentElement;
            string strPath = string.Format("/PointsType/Points[@SlaveId=\"{0}\"and @FunctionCode=\"{1}\"]/Point[@LocalAddress=\"{2}\"]",
                slaveId, functionCode, localAddress);
            //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            if (selectXe == null)
            {
                reader.Close();
                return;
            }
            selectXe.ParentNode.RemoveChild(selectXe);
            reader.Close();
            xmlDoc.Save(addressFilePath);
        }
        private void page3_btn_SaveEdit_Click(object sender, EventArgs e)
        {
            if (openFileDialog.FileName == "")
            {
                MessageBox.Show("请打开配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DeleteAddress(page3_txt_SalveId.Text, page3_txt_FunctionCode.Text, page3_txt_LocalAddress.Text);

            xmlDoc.Load(addressFilePath);
            XmlElement xe = xmlDoc.DocumentElement;
            string strPath = string.Format("/PointsType/Points[@SlaveId=\"{0}\"and @FunctionCode=\"{1}\"]",
                page3_txt_SalveId.Text, page3_txt_FunctionCode.Text);
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            if (selectXe == null)
            {
                XmlNode Type = xmlDoc.SelectSingleNode("PointsType");

                selectXe = xmlDoc.CreateElement("Points");
                XmlAttribute xmlAttributeFunctionCode = xmlDoc.CreateAttribute("FunctionCode");
                XmlAttribute xmlAttributeSlaveId = xmlDoc.CreateAttribute("SlaveId");

                xmlAttributeFunctionCode.InnerText = page3_txt_FunctionCode.Text;
                xmlAttributeSlaveId.InnerText = page3_txt_SalveId.Text;

                selectXe.SetAttributeNode(xmlAttributeFunctionCode);
                selectXe.SetAttributeNode(xmlAttributeSlaveId);
                Type.AppendChild(selectXe);
            }
            strPath = string.Format("/PointsType/Points[@SlaveId=\"{0}\"and @FunctionCode=\"{1}\"]", page3_txt_SalveId.Text, page3_txt_FunctionCode.Text);
            selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            XmlAddress address = new XmlAddress
            {
                BitIndex = page3_txt_BitIndex.Text,
                Text = page3_txt_Text.Text,
                DataBit = page3_txt_DataBit.Text,
                Factor = page3_txt_Factor.Text,
                FunctionCode = page3_txt_FunctionCode.Text,
                LocalAddress = page3_txt_LocalAddress.Text,
                ProtocolAddress = page3_txt_ProtocolAddress.Text,
                SlaveId = page3_txt_SalveId.Text,
                EnglishName = page3_txt_englishName.Text
            };

            XmlElement element = xmlDoc.CreateElement("Point");
            XmlAttribute xmlAttributeBitIndex = xmlDoc.CreateAttribute("BitIndex");
            XmlText xmlAttributeText = xmlDoc.CreateTextNode("Text");
            XmlAttribute xmlAttributeDataBit = xmlDoc.CreateAttribute("DataBit");
            XmlAttribute xmlAttributeFactor = xmlDoc.CreateAttribute("Factor");
            XmlAttribute xmlAttributeLocalAddress = xmlDoc.CreateAttribute("LocalAddress");
            XmlAttribute xmlAttributeProtocolAddress = xmlDoc.CreateAttribute("ProtocolAddress");
            XmlAttribute xmlAttributeProtocolEnglishName = xmlDoc.CreateAttribute("EnglishName");


            xmlAttributeBitIndex.InnerText = address.BitIndex;
            xmlAttributeDataBit.InnerText = address.DataBit;
            xmlAttributeFactor.InnerText = address.Factor;
            xmlAttributeLocalAddress.InnerText = address.LocalAddress;
            xmlAttributeProtocolAddress.InnerText = address.ProtocolAddress;
            xmlAttributeProtocolEnglishName.InnerText = address.EnglishName;

            element.SetAttributeNode(xmlAttributeBitIndex);
            element.SetAttributeNode(xmlAttributeDataBit);
            element.SetAttributeNode(xmlAttributeFactor);
            element.SetAttributeNode(xmlAttributeLocalAddress);
            element.SetAttributeNode(xmlAttributeProtocolAddress);
            element.SetAttributeNode(xmlAttributeProtocolEnglishName);
            element.InnerText = address.Text;


            selectXe.AppendChild(element);
            XmlAddressList.Add(address);
            reader.Close();
            xmlDoc.Save(addressFilePath);
            ReadXml();
        }

        private void page3_btn_add_Click(object sender, EventArgs e)
        {
            if (openFileDialog.FileName == "")
            {
                MessageBox.Show("请打开配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            xmlDoc.Load(addressFilePath);
            XmlElement xe = xmlDoc.DocumentElement;
            string strPath = string.Format("/PointsType/Points[@SlaveId=\"{0}\"and @FunctionCode=\"{1}\"]",
                 page3_txt_SalveId.Text, page3_txt_FunctionCode.Text);
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            if (selectXe == null)
            {
                XmlNode Type = xmlDoc.SelectSingleNode("PointsType");

                selectXe = xmlDoc.CreateElement("Points");
                XmlAttribute xmlAttributeFunctionCode = xmlDoc.CreateAttribute("FunctionCode");
                XmlAttribute xmlAttributeSlaveId = xmlDoc.CreateAttribute("SlaveId");

                xmlAttributeFunctionCode.InnerText = page3_txt_FunctionCode.Text;
                xmlAttributeSlaveId.InnerText = page3_txt_SalveId.Text;

                selectXe.SetAttributeNode(xmlAttributeFunctionCode);
                selectXe.SetAttributeNode(xmlAttributeSlaveId);
                Type.AppendChild(selectXe);
            }
            strPath = string.Format("/PointsType/Points[@SlaveId=\"{0}\"and @FunctionCode=\"{1}\"]", page3_txt_SalveId.Text, page3_txt_FunctionCode.Text);
            selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            XmlAddress address = new XmlAddress
            {
                BitIndex = page3_txt_BitIndex.Text,
                Text = page3_txt_Text.Text,
                DataBit = page3_txt_DataBit.Text,
                Factor = page3_txt_Factor.Text,
                FunctionCode = page3_txt_FunctionCode.Text,
                LocalAddress = GetAddressMaxIndex(XmlAddressList).ToString(),
                ProtocolAddress = page3_txt_ProtocolAddress.Text,
                SlaveId = page3_txt_SalveId.Text,
                EnglishName = page3_txt_englishName.Text

            };

            XmlElement element = xmlDoc.CreateElement("Point");
            XmlAttribute xmlAttributeBitIndex = xmlDoc.CreateAttribute("BitIndex");
            XmlText xmlAttributeText = xmlDoc.CreateTextNode("Text");
            XmlAttribute xmlAttributeDataBit = xmlDoc.CreateAttribute("DataBit");
            XmlAttribute xmlAttributeFactor = xmlDoc.CreateAttribute("Factor");
            XmlAttribute xmlAttributeLocalAddress = xmlDoc.CreateAttribute("LocalAddress");
            XmlAttribute xmlAttributeProtocolAddress = xmlDoc.CreateAttribute("ProtocolAddress");
            XmlAttribute xmlAttributeProtoEnglishName = xmlDoc.CreateAttribute("EnglishName");

            xmlAttributeBitIndex.InnerText = address.BitIndex;
            xmlAttributeDataBit.InnerText = address.DataBit;
            xmlAttributeFactor.InnerText = address.Factor;
            xmlAttributeLocalAddress.InnerText = address.LocalAddress;
            xmlAttributeProtocolAddress.InnerText = address.ProtocolAddress;
            xmlAttributeProtoEnglishName.InnerText = address.EnglishName;

            element.SetAttributeNode(xmlAttributeLocalAddress);
            element.SetAttributeNode(xmlAttributeBitIndex);
            element.SetAttributeNode(xmlAttributeDataBit);
            element.SetAttributeNode(xmlAttributeFactor);
            element.SetAttributeNode(xmlAttributeProtocolAddress);
            element.SetAttributeNode(xmlAttributeProtoEnglishName);
            element.InnerText = address.Text;


            selectXe.AppendChild(element);
            XmlAddressList.Add(address);
            reader.Close();
            xmlDoc.Save(addressFilePath);
            ReadXml();
        }
        public int GetAddressMaxIndex(List<XmlAddress> list)
        {
            list.Sort();
            object temp = null;
            if (list.Count == 0) return 1;
            if (currentProtocolType == ProtocolType.Modbus)
            {

                temp = list[list.Count - 1] as XmlAddress;
                if (temp != null)
                {
                    return 1 + int.Parse(((XmlAddress)temp).LocalAddress);
                }
            }
            return 1;
        }
        private void page3_btn_clear_Click(object sender, EventArgs e)
        {
            LoadXml(configFilePath);
            XElement xElement = XElement.Load(addressFilePath);
            IEnumerable<XElement> elements = from ele in xElement.Elements("CommunicaitonData")
                                             select ele;
            if (elements.Count() > 0)
            {
                elements.Remove();
            }
            reader.Close();
            xElement.Save(openFileDialog.FileName);
            MessageBox.Show("完成");
        }

        private void page3_btn_allDelete_Click(object sender, EventArgs e)
        {
            int indexBegin = int.Parse(page3_txt_IndexBegin.Text);
            int indexEnd = int.Parse(page3_txt_IndexEnd.Text);
            string functionCode = page3_txt_opFunction.Text;
            string slaveId = page3_txt_opSlaveId.Text;
            if (indexBegin == indexEnd)
            {
                MessageBox.Show("请输入正确的索引值");
            }
            for (; indexBegin <= indexEnd; indexBegin++)
            {
                DeleteAddress(slaveId, functionCode, indexBegin.ToString());
            }
            ReadXml();
        }

        private void page3_btn_AllAdd_Click(object sender, EventArgs e)
        {
            string slaveId = page3_txt_AllSlaveId.Text;
            string functionCode = page3_txt_AllFunction.Text;
            if (slaveId == "" || functionCode == "" || page3_txt_BeginAddress.Text == "") { MessageBox.Show("请检查功能和从机ID"); return; }
            string strPath = string.Format("/PointsType/Points[@SlaveId=\"{0}\"and @FunctionCode=\"{1}\"]", slaveId, functionCode);
            int number = int.Parse(page3_txt_DataNumber.Text);
            int number1 = int.Parse(page3_txt_Bit1.Text);
            int i = number - int.Parse(page3_txt_Bit16.Text);
            int BeginAddress = int.Parse(page3_txt_BeginAddress.Text);
            string tempIndex = GetAddressMaxIndex(XmlAddressList).ToString();
            int bit16 = 0, bit1 = 0;
            for (int n = 0; n < i; n++)
            {
                string findStr = "page3_combo_" + n;
                DevExpress.XtraEditors.ComboBoxEdit combo = page3_panel_dataControl.Controls.Find(findStr, false)[0] as DevExpress.XtraEditors.ComboBoxEdit;
                string text = combo.EditValue.ToString();
                switch (text)
                {
                    case "16位":
                        bit16++;
                        break;
                    case "8位":
                        break;
                    case "1位":
                        bit1++;
                        break;
                }
            }
            if (bit16 != int.Parse(page3_txt_Bit16.Text) || bit1 != int.Parse(page3_txt_Bit1.Text))
            {
                if (MessageBox.Show("输入的与数据设置不符，是否继续？", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    return;
                }
            }
            for (int n = 0; n < i; n++)
            {
                string findStr = "page3_combo_" + n;
                try
                {
                    DevExpress.XtraEditors.ComboBoxEdit combo = page3_panel_dataControl.Controls.Find(findStr, false)[0] as DevExpress.XtraEditors.ComboBoxEdit;
                    string text = combo.EditValue.ToString();
                    switch (text)
                    {
                        case "16位":
                            bit16++;
                            AddAddress(slaveId, functionCode, 16, ref BeginAddress);
                            break;
                        case "8位":
                            AddAddress(slaveId, functionCode, 8, ref BeginAddress);
                            break;
                        case "1位":
                            bit1++;
                            AddAddress(slaveId, functionCode, 1, ref BeginAddress);

                            break;
                    }
                }
                catch { MessageBox.Show("请先设置数据类型"); }
            }
            MessageBox.Show("添加完成：从" + tempIndex + "开始到" + GetAddressMaxIndex(XmlAddressList) + "");

        }

        private void AddAddress(string slaveID, string functionCode, int number, ref int BeginAddress)
        {
            if (number == 16)
            {

                AddAddressPrecess(slaveID, "16", functionCode, ref BeginAddress, 0);
                BeginAddress = BeginAddress + 2;
            }
            if (number == 8)
            {

                AddAddressPrecess(slaveID, "8", functionCode, ref BeginAddress, 0);
                BeginAddress++;

            }
            else if (number == 1)
            {
                for (int i = 0; i < 8; i++)
                {
                    AddAddressPrecess(slaveID, "1", functionCode, ref BeginAddress, i);
                }
                BeginAddress++;

            }


        }
        private void AddAddressPrecess(string slaveID, string dataBit, string functionCode, ref int BeginAddress, int bitIndex)
        {
            xmlDoc.Load(addressFilePath);
            XmlElement xe = xmlDoc.DocumentElement;
            string strPath = string.Format("/PointsType/Points[@SlaveId=\"{0}\"and @FunctionCode=\"{1}\"]",
                 slaveID, functionCode);

            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            if (selectXe == null)
            {
                XmlNode Type = xmlDoc.SelectSingleNode("PointsType");

                selectXe = xmlDoc.CreateElement("Points");
                XmlAttribute xmlAttributeFunctionCode = xmlDoc.CreateAttribute("FunctionCode");
                XmlAttribute xmlAttributeSlaveId = xmlDoc.CreateAttribute("SlaveId");

                xmlAttributeFunctionCode.InnerText = functionCode;
                xmlAttributeSlaveId.InnerText = slaveID;

                selectXe.SetAttributeNode(xmlAttributeFunctionCode);
                selectXe.SetAttributeNode(xmlAttributeSlaveId);
                Type.AppendChild(selectXe);
            }
            strPath = string.Format("/PointsType/Points[@SlaveId=\"{0}\"and @FunctionCode=\"{1}\"]", slaveID, functionCode);
            selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            XmlAddress address = new XmlAddress
            {
                BitIndex = bitIndex.ToString(),
                Text = "备用-" + BeginAddress + "-" + bitIndex,
                DataBit = dataBit,
                Factor = "1",
                FunctionCode = functionCode,
                LocalAddress = GetAddressMaxIndex(XmlAddressList).ToString(),
                ProtocolAddress = BeginAddress.ToString(),
                SlaveId = slaveID
            };

            XmlElement element = xmlDoc.CreateElement("Point");
            XmlAttribute xmlAttributeBitIndex = xmlDoc.CreateAttribute("BitIndex");
            XmlText xmlAttributeText = xmlDoc.CreateTextNode("Text");
            XmlAttribute xmlAttributeDataBit = xmlDoc.CreateAttribute("DataBit");
            XmlAttribute xmlAttributeFactor = xmlDoc.CreateAttribute("Factor");
            XmlAttribute xmlAttributeLocalAddress = xmlDoc.CreateAttribute("LocalAddress");
            XmlAttribute xmlAttributeProtocolAddress = xmlDoc.CreateAttribute("ProtocolAddress");
            xmlAttributeBitIndex.InnerText = address.BitIndex;
            xmlAttributeDataBit.InnerText = address.DataBit;
            xmlAttributeFactor.InnerText = address.Factor;
            xmlAttributeLocalAddress.InnerText = address.LocalAddress;
            xmlAttributeProtocolAddress.InnerText = address.ProtocolAddress;

            element.SetAttributeNode(xmlAttributeLocalAddress);
            element.SetAttributeNode(xmlAttributeBitIndex);
            element.SetAttributeNode(xmlAttributeDataBit);
            element.SetAttributeNode(xmlAttributeFactor);
            element.SetAttributeNode(xmlAttributeProtocolAddress);
            element.InnerText = address.Text;


            selectXe.AppendChild(element);
            XmlAddressList.Add(address);
            reader.Close();
            xmlDoc.Save(addressFilePath);
        }
        private void page3_btn_NumberToControl_Click(object sender, EventArgs e)
        {
            string dataNumber = (int.Parse(page3_txt_DataNumber.Text) - int.Parse(page3_txt_Bit16.Text)).ToString();
            if (dataNumber.Trim().Equals("0"))
            {
                MessageBox.Show("请输入要生成的个数");
            }
            page3_CreatControl(dataNumber);
        }

        private void page3_CreatControl(string dataNumber)
        {
            first_Flush_Flag = false;
            page3_panel_dataControl.Clear();
            object employeesSource = page2_gridview2.DataSource;

            LayoutControlGroup group = page3_panel_dataControl.AddGroup("数据设置");
            for (int i = 0; i < int.Parse(dataNumber); i++)
            {
                LayoutControlItem item = new LayoutControlItem();
                ComboBoxEdit combo = new ComboBoxEdit
                {
                    SelectedItem = 2,
                    Name = "page3_combo_" + i,
                };
                ComboBoxItemCollection coll = combo.Properties.Items;
                coll.BeginUpdate();
                coll.Add("16位");
                coll.Add("8位");
                coll.Add("1位");
                coll.EndUpdate();
                combo.SelectedIndex = 1;

                group.AddItem("数据" + (i + 1).ToString(), combo);
            }
        }

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void page4_btn_delete_Click(object sender, EventArgs e)
        {
            XmlElement xe = xmlDoc.DocumentElement;
            string strPath = string.Format("/Fields/Protocol[@Name=\"{0}\"]/field[@name=\"{1}\"]",
                page4_cmb_CheckProtocol.EditValue.ToString(), page4_cmb_CheckField.EditValue.ToString());
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            if (selectXe == null) { return; }
            XmlNode fieldNode = selectXe.SelectSingleNode(page4_cmb_CheckField.EditValue.ToString());
            fieldNode.ParentNode.RemoveChild(fieldNode);
            reader.Close();
            xmlDoc.Save(fieldFilePath);
            ReadXml();
        }

        private void page4_btn_add_Click(object sender, EventArgs e)
        {
            XmlElement xe = xmlDoc.DocumentElement;
            string strPath = string.Format("/Fields/Protocol[@Name=\"{0}\"]",
                page4_cmb_CheckProtocol.EditValue.ToString());
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            if (selectXe == null) { MessageBox.Show("找不到字段"); return; }
            XmlElement xmlElement = xmlDoc.CreateElement("field");
            xmlElement.SetAttribute("name", page4_txt_inputField.Text);
            xmlElement.InnerText = page4_txt_inputField.Text;
            selectXe.AppendChild(xmlElement);
            reader.Close();
            xmlDoc.Save(fieldFilePath);
            ReadXml();
        }

        private void page4_btn_update_Click(object sender, EventArgs e)
        {
            XmlElement xe = xmlDoc.DocumentElement;
            string strPath = string.Format("/Fields/Protocol[@Name=\"{0}\"]/field[@name=\"{1}\"]",
                page4_cmb_CheckProtocol.EditValue.ToString(), page4_cmb_CheckField.EditValue.ToString());
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            if (selectXe == null) { MessageBox.Show("找不到该字段"); return; }

            selectXe.ParentNode.RemoveChild(selectXe);
            strPath = string.Format("/Fields/Protocol[@Name=\"{0}\"]",
           page4_cmb_CheckProtocol.EditValue.ToString());
            selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            XmlElement xmlElement = xmlDoc.CreateElement("field");
            xmlElement.SetAttribute("name", page4_txt_inputField.Text);
            xmlElement.InnerText = page4_txt_inputField.Text;
            selectXe.AppendChild(xmlElement);
            reader.Close();
            xmlDoc.Save(fieldFilePath);
            ReadXml();
        }

        private void page4_btn_clearContent_Click(object sender, EventArgs e)
        {
            page4_txt_inputField.Text = "";
        }

        private void FormXmlPress5_Load(object sender, EventArgs e)
        {
            // asc.controllInitializeSize(this);
        }
        //1.声明自适应类实例
        //AutoSizeFormClass asc = new AutoSizeFormClass(); 
        private void FormXmlPress5_SizeChanged(object sender, EventArgs e)
        {
            // asc.controllInitializeSize(this);
            //   asc.controlAutoSize(this);
        }

        /// <summary>
        /// 协议配置的数据刷新
        /// </summary>
        public void Page2_Reflash<T>(List<T> list)
        {
            if (list is null) return;
            this.page2_gridview2.DataSource = null;
            //通过modellist获取到新数组解决数据实时刷新问题
            this.page2_gridview2.DataSource = list.ToArray();
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private int GetMaxIndex<T>(List<T> list)
        {
            list.Sort();
            object temp = null;
            if (list.Count == 0) return 1;
            if (currentProtocolType == ProtocolType.Modbus)
            {

                temp = list[list.Count - 1] as XmlModbus;
                if (temp != null)
                {
                    return 1 + int.Parse(((XmlModbus)temp).ID);
                }
            }
            else if (currentProtocolType == ProtocolType.other)
            {
                temp = list[list.Count - 1] as XmlOther;
                if (temp != null)
                {
                    return 1 + int.Parse(((XmlOther)temp).ID);

                }
            }
            else if (currentProtocolType == ProtocolType.custom)
            {

                temp = list[list.Count - 1] as XmlSokectCustomProtocol;
                string id = ((XmlSokectCustomProtocol)temp).ID;
                if (id == null)
                {
                    ((XmlSokectCustomProtocol)temp).ID = "1";
                }
                if (temp != null)
                {
                    return 1 + int.Parse(((XmlSokectCustomProtocol)temp).ID);
                }
            }
            else if (currentProtocolType == ProtocolType.Sokect)
            {
                temp = list[list.Count - 1] as XmlSokect;

                if (temp != null)
                {
                    return 1 + int.Parse(((XmlSokect)temp).ID);
                }
            }
            else if (currentProtocolType == ProtocolType.vdr)
            {
                temp = list[list.Count - 1] as XmlVDRProtocol;
                if (temp != null)
                {
                    if (((XmlVDRProtocol)temp).ID == null) { ((XmlVDRProtocol)temp).ID = "1"; }
                    return 1 + int.Parse(((XmlVDRProtocol)temp).ID);
                }
            }
            return 1;
        }
        private void page2_btn_add_Click(object sender, EventArgs e)
        {
            if (openFileDialog.FileName == "")
            {
                MessageBox.Show("请打开配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            XmlNode TCPNode;
            XmlElement TCPElement;
            TCPNode = xmlDoc.SelectSingleNode("Communications").SelectSingleNode("CommunicaitonData").SelectSingleNode("Communication");
            TCPElement = xmlDoc.CreateElement("TCP");
            switch (currentProtocolType)
            {
                case ProtocolType.custom:
                    TCPNode = xmlDoc.SelectSingleNode("Communications").SelectSingleNode("CommunicaitonData").SelectSingleNode("Communication");
                    TCPElement = xmlDoc.CreateElement("TCP");
                    var custom = SokectCustomProtocolDictionary.ToArray();
                    for (int i = 0; i < custom.Length; i++)
                    {
                        BaseEdit baseEdit = layoutControl3.Controls.Find(prefixName + custom[i].Key, false)[0] as BaseEdit;
                        SokectCustomProtocolDictionary[custom[i].Key] = baseEdit.Text;
                    }

                    XmlSokectCustomProtocol sokectCustomProtocol = new XmlSokectCustomProtocol();
                    #region 

                    XmlAttribute attr_ID = xmlDoc.CreateAttribute("ID");
                    XmlAttribute attr_Can1IP = xmlDoc.CreateAttribute("Can1IP");
                    XmlAttribute attr_Can1Port = xmlDoc.CreateAttribute("Can1Port");
                    XmlAttribute attr_Can2IP = xmlDoc.CreateAttribute("Can2IP");
                    XmlAttribute attr_Can2Port = xmlDoc.CreateAttribute("Can2Port");
                    XmlAttribute attr_CheckEndIndex = xmlDoc.CreateAttribute("CheckEndIndex");
                    XmlAttribute attr_CheckStartIndex = xmlDoc.CreateAttribute("CheckStartIndex");
                    XmlAttribute attr_CheckSumIndex = xmlDoc.CreateAttribute("CheckSumIndex");
                    XmlAttribute attr_CheckSumLength = xmlDoc.CreateAttribute("CheckSumLength");
                    XmlAttribute attr_CheckSumType = xmlDoc.CreateAttribute("CheckSumType");
                    XmlAttribute attr_EndSingle = xmlDoc.CreateAttribute("EndSingle");
                    XmlAttribute attr_FirstAddr = xmlDoc.CreateAttribute("FirstAddr");
                    XmlAttribute attr_PackageLength = xmlDoc.CreateAttribute("PackageLength");
                    XmlAttribute attr_StartSingle = xmlDoc.CreateAttribute("StartSingle");

                    attr_ID.InnerText = sokectCustomProtocol.ID = GetMaxIndex(XmlcustomList).ToString();
                    attr_Can1IP.InnerText = sokectCustomProtocol.Can1IP = SokectCustomProtocolDictionary["Can1IP"];
                    attr_Can1Port.InnerText = sokectCustomProtocol.Can1Port = SokectCustomProtocolDictionary["Can1Port"];
                    attr_Can2IP.InnerText = sokectCustomProtocol.Can2IP = SokectCustomProtocolDictionary["Can2IP"];
                    attr_Can2Port.InnerText = sokectCustomProtocol.Can2Port = SokectCustomProtocolDictionary["Can2Port"];
                    attr_CheckEndIndex.InnerText = sokectCustomProtocol.CheckEndIndex = SokectCustomProtocolDictionary["CheckEndIndex"];
                    attr_CheckStartIndex.InnerText = sokectCustomProtocol.CheckStartIndex = SokectCustomProtocolDictionary["CheckStartIndex"];
                    attr_CheckSumIndex.InnerText = sokectCustomProtocol.CheckSumIndex = SokectCustomProtocolDictionary["CheckSumIndex"];
                    attr_CheckSumLength.InnerText = sokectCustomProtocol.CheckSumLength = SokectCustomProtocolDictionary["CheckSumLength"];
                    attr_CheckSumType.InnerText = sokectCustomProtocol.CheckSumType = SokectCustomProtocolDictionary["CheckSumType"];
                    attr_EndSingle.InnerText = sokectCustomProtocol.EndSingle = SokectCustomProtocolDictionary["EndSingle"];
                    attr_FirstAddr.InnerText = sokectCustomProtocol.FirstAddr = SokectCustomProtocolDictionary["FirstAddr"];
                    attr_PackageLength.InnerText = sokectCustomProtocol.PackageLength = SokectCustomProtocolDictionary["PackageLength"];
                    attr_StartSingle.InnerText = sokectCustomProtocol.StartSingle = SokectCustomProtocolDictionary["StartSingle"];
                    XmlcustomList.Add(sokectCustomProtocol);


                    TCPElement.SetAttributeNode(attr_ID);
                    TCPElement.SetAttributeNode(attr_Can1IP);
                    TCPElement.SetAttributeNode(attr_Can1Port);
                    TCPElement.SetAttributeNode(attr_Can2IP);
                    TCPElement.SetAttributeNode(attr_Can2Port);
                    TCPElement.SetAttributeNode(attr_CheckEndIndex);
                    TCPElement.SetAttributeNode(attr_CheckStartIndex);
                    TCPElement.SetAttributeNode(attr_CheckSumIndex);
                    TCPElement.SetAttributeNode(attr_CheckSumLength);
                    TCPElement.SetAttributeNode(attr_CheckSumType);
                    TCPElement.SetAttributeNode(attr_EndSingle);
                    TCPElement.SetAttributeNode(attr_FirstAddr);
                    TCPElement.SetAttributeNode(attr_PackageLength);
                    TCPElement.SetAttributeNode(attr_StartSingle);


                    TCPNode.AppendChild(TCPElement);

                    string path = xmlDoc.BaseURI.ToString().Substring(xmlDoc.BaseURI.ToString().LastIndexOf("///") + 3);
                    reader.Close();
                    xmlDoc.Save(path);
                    #endregion

                    Page2_Reflash(XmlcustomList);

                    break;
                case ProtocolType.Modbus:
                    TCPNode = xmlDoc.SelectSingleNode("Communications").SelectSingleNode("CommunicaitonData").SelectSingleNode("Communication").SelectSingleNode("TCP");
                    TCPElement = xmlDoc.CreateElement("CommCongfig");

                    custom = SokectModbusProtocolDictionary.ToArray();
                    for (int i = 0; i < custom.Length; i++)
                    {
                        BaseEdit baseEdit = layoutControl3.Controls.Find(prefixName + custom[i].Key, false)[0] as BaseEdit;
                        SokectModbusProtocolDictionary[custom[i].Key] = baseEdit.Text;
                    }

                    XmlModbus modbus = new XmlModbus();
                    XmlAttribute modbusattr_ID = xmlDoc.CreateAttribute("ID");
                    XmlAttribute modbusattr_Can1IP = xmlDoc.CreateAttribute("Can1IP");
                    XmlAttribute modbusattr_Can1Port = xmlDoc.CreateAttribute("Can1Port");
                    XmlAttribute modbusattr_Can2IP = xmlDoc.CreateAttribute("Can2IP");
                    XmlAttribute modbusattr_Can2Port = xmlDoc.CreateAttribute("Can2Port");
                    XmlAttribute modbusattr_DataLenght = xmlDoc.CreateAttribute("DataLenght");
                    XmlAttribute modbusattr_FirstAddress = xmlDoc.CreateAttribute("FirstAddress");
                    XmlAttribute modbusattr_FunctionCode = xmlDoc.CreateAttribute("FunctionCode");
                    XmlAttribute modbusattr_RequestCount = xmlDoc.CreateAttribute("RequestCount");
                    XmlAttribute modbusattr_SlaveID = xmlDoc.CreateAttribute("SlaveID");

                    modbusattr_ID.InnerText = modbus.ID = GetMaxIndex(XmlModbusList).ToString();
                    modbusattr_Can1IP.InnerText = modbus.Can1IP = SokectModbusProtocolDictionary["Can1IP"];
                    modbusattr_Can1Port.InnerText = modbus.Can1Port = SokectModbusProtocolDictionary["Can1Port"];
                    modbusattr_Can2Port.InnerText = modbus.Can2IP = SokectModbusProtocolDictionary["Can2IP"];
                    modbusattr_Can2Port.InnerText = modbus.Can2Port = SokectModbusProtocolDictionary["Can2Port"];
                    modbusattr_DataLenght.InnerText = modbus.DataLenght = SokectModbusProtocolDictionary["DataLenght"];
                    modbusattr_FirstAddress.InnerText = modbus.FirstAddress = SokectModbusProtocolDictionary["FirstAddress"];
                    modbusattr_FunctionCode.InnerText = modbus.FunctionCode = SokectModbusProtocolDictionary["FunctionCode"];
                    modbusattr_RequestCount.InnerText = modbus.RequestCount = SokectModbusProtocolDictionary["RequestCount"];
                    modbusattr_SlaveID.InnerText = modbus.SlaveID = SokectModbusProtocolDictionary["SlaveID"];
                    XmlModbusList.Add(modbus);

                    TCPElement.SetAttributeNode(modbusattr_ID);
                    TCPElement.SetAttributeNode(modbusattr_Can1IP);
                    TCPElement.SetAttributeNode(modbusattr_Can1Port);
                    TCPElement.SetAttributeNode(modbusattr_Can2IP);
                    TCPElement.SetAttributeNode(modbusattr_Can2Port);
                    TCPElement.SetAttributeNode(modbusattr_DataLenght);
                    TCPElement.SetAttributeNode(modbusattr_FirstAddress);
                    TCPElement.SetAttributeNode(modbusattr_FunctionCode);
                    TCPElement.SetAttributeNode(modbusattr_RequestCount);
                    TCPElement.SetAttributeNode(modbusattr_SlaveID);

                    TCPNode.AppendChild(TCPElement);
                    path = xmlDoc.BaseURI.ToString().Substring(xmlDoc.BaseURI.ToString().LastIndexOf("///") + 3);
                    reader.Close();
                    xmlDoc.Save(path);
                    Page2_Reflash(XmlModbusList);

                    break;
                case ProtocolType.Sokect:
                    TCPNode = xmlDoc.SelectSingleNode("Communications").SelectSingleNode("CommunicaitonData").SelectSingleNode("Communication");
                    TCPElement = xmlDoc.CreateElement("TCP");
                    custom = SokectProtocolDictionary.ToArray();
                    for (int i = 0; i < custom.Length; i++)
                    {
                        BaseEdit baseEdit = layoutControl3.Controls.Find(prefixName + custom[i].Key, false)[0] as BaseEdit;
                        SokectProtocolDictionary[custom[i].Key] = baseEdit.Text;
                    }
                    XmlSokect sokect = new XmlSokect();
                    XmlAttribute sokect_ID = xmlDoc.CreateAttribute("ID");
                    XmlAttribute sokect_Can1IP = xmlDoc.CreateAttribute("Can1IP");
                    XmlAttribute sokect_Can1Port = xmlDoc.CreateAttribute("Can1Port");
                    XmlAttribute sokect_Can2IP = xmlDoc.CreateAttribute("Can2IP");
                    XmlAttribute sokect_Can2Port = xmlDoc.CreateAttribute("Can2Port");
                    XmlAttribute sokect_CheckEndIndex = xmlDoc.CreateAttribute("CheckEndIndex");
                    XmlAttribute sokect_CheckStartIndex = xmlDoc.CreateAttribute("CheckStartIndex");
                    XmlAttribute sokect_CheckSumIndex = xmlDoc.CreateAttribute("CheckSumIndex");
                    XmlAttribute sokect_CheckSumLength = xmlDoc.CreateAttribute("CheckSumLength");
                    XmlAttribute sokect_CheckSumType = xmlDoc.CreateAttribute("CheckSumType");
                    XmlAttribute sokect_EndSingle = xmlDoc.CreateAttribute("EndSingle");
                    XmlAttribute sokect_FirstAddr = xmlDoc.CreateAttribute("FirstAddr");
                    XmlAttribute sokect_PackageLength = xmlDoc.CreateAttribute("PackageLength");
                    XmlAttribute sokect_StartSingle = xmlDoc.CreateAttribute("StartSingle");
                    XmlAttribute sokect_ValueIndex = xmlDoc.CreateAttribute("ValueIndex");

                    sokect_ID.InnerText = sokect.ID = GetMaxIndex(XmlSokectList).ToString();
                    sokect_Can1IP.InnerText = sokect.Can1IP = SokectProtocolDictionary["Can1IP"];
                    sokect_Can1Port.InnerText = sokect.Can1Port = SokectProtocolDictionary["Can1Port"];
                    sokect_Can2IP.InnerText = sokect.Can2IP = SokectProtocolDictionary["Can2IP"];
                    sokect_Can2Port.InnerText = sokect.Can2Port = SokectProtocolDictionary["Can2Port"];
                    sokect_CheckEndIndex.InnerText = sokect.AddrIndex = SokectProtocolDictionary["CheckEndIndex"];
                    sokect_CheckStartIndex.InnerText = sokect.CheckDataLength = SokectProtocolDictionary["CheckStartIndex"];
                    sokect_CheckSumIndex.InnerText = sokect.CheckSingleSingle = SokectProtocolDictionary["CheckSumIndex"];
                    sokect_CheckSumLength.InnerText = sokect.CommSingle = SokectProtocolDictionary["CheckSumLength"];
                    sokect_CheckSumType.InnerText = sokect.DataDefineRule = SokectProtocolDictionary["CheckSumType"];
                    sokect_EndSingle.InnerText = sokect.EndSingle = SokectProtocolDictionary["EndSingle"];
                    sokect_FirstAddr.InnerText = sokect.InforPerData = SokectProtocolDictionary["FirstAddr"];
                    sokect_PackageLength.InnerText = sokect.SplitChar = SokectProtocolDictionary["PackageLength"];
                    sokect_StartSingle.InnerText = sokect.StartChar = SokectProtocolDictionary["StartSingle"];
                    sokect_ValueIndex.InnerText = sokect.ValueIndex = SokectProtocolDictionary["ValueIndex"];

                    XmlSokectList.Add(sokect);
                    TCPElement.SetAttributeNode(sokect_ID);
                    TCPElement.SetAttributeNode(sokect_Can1IP);
                    TCPElement.SetAttributeNode(sokect_Can1Port);
                    TCPElement.SetAttributeNode(sokect_Can2IP);
                    TCPElement.SetAttributeNode(sokect_Can2Port);
                    TCPElement.SetAttributeNode(sokect_CheckEndIndex);
                    TCPElement.SetAttributeNode(sokect_CheckStartIndex);
                    TCPElement.SetAttributeNode(sokect_CheckSumIndex);
                    TCPElement.SetAttributeNode(sokect_CheckSumLength);
                    TCPElement.SetAttributeNode(sokect_CheckSumType);
                    TCPElement.SetAttributeNode(sokect_EndSingle);
                    TCPElement.SetAttributeNode(sokect_FirstAddr);
                    TCPElement.SetAttributeNode(sokect_PackageLength);
                    TCPElement.SetAttributeNode(sokect_StartSingle);
                    TCPElement.SetAttributeNode(sokect_ValueIndex);

                    TCPNode.AppendChild(TCPElement);
                    path = xmlDoc.BaseURI.ToString().Substring(xmlDoc.BaseURI.ToString().LastIndexOf("///") + 3);
                    reader.Close();
                    xmlDoc.Save(path);
                    Page2_Reflash(XmlSokectList);

                    break;
                case ProtocolType.vdr:
                    TCPElement = xmlDoc.CreateElement("VDR");

                    custom = VDRProtocolDictionary.ToArray();
                    for (int i = 0; i < custom.Length; i++)
                    {
                        BaseEdit baseEdit = layoutControl3.Controls.Find(prefixName + custom[i].Key, false)[0] as BaseEdit;
                        VDRProtocolDictionary[custom[i].Key] = baseEdit.Text;

                    }

                    XmlAttribute vdr_ID = xmlDoc.CreateAttribute("ID");
                    XmlAttribute vdr_BAUREATE = xmlDoc.CreateAttribute("BAUREATE");
                    XmlAttribute vdr_DATABIT = xmlDoc.CreateAttribute("DATABIT");
                    XmlAttribute vdr_FirstAddr = xmlDoc.CreateAttribute("FirstAddr");
                    XmlAttribute vdr_FrameLenght = xmlDoc.CreateAttribute("FrameLenght");
                    XmlAttribute vdr_PARITY = xmlDoc.CreateAttribute("PARITY");
                    XmlAttribute vdr_PortName = xmlDoc.CreateAttribute("PortName");
                    XmlAttribute vdr_StartSingle = xmlDoc.CreateAttribute("StartSingle");
                    XmlAttribute vdr_STOPBITS = xmlDoc.CreateAttribute("STOPBITS");

                    XmlVDRProtocol vdr = new XmlVDRProtocol();
                    XmlElement VDRElement = xmlDoc.CreateElement("VDR");
                    vdr_ID.InnerText = vdr.ID = GetMaxIndex(XmlVDRList).ToString();
                    vdr_BAUREATE.InnerText = vdr.BAUREATE = VDRProtocolDictionary["BAUREATE"];
                    vdr_DATABIT.InnerText = vdr.DATABIT = VDRProtocolDictionary["DATABIT"];
                    vdr_FirstAddr.InnerText = vdr.FirstAddr = VDRProtocolDictionary["FirstAddr"];
                    vdr_FrameLenght.InnerText = vdr.FrameLenght = VDRProtocolDictionary["FrameLenght"];
                    vdr_PARITY.InnerText = vdr.PARITY = VDRProtocolDictionary["PARITY"];
                    vdr_PortName.InnerText = vdr.PortName = VDRProtocolDictionary["PortName"];
                    vdr_StartSingle.InnerText = vdr.StartSingle = VDRProtocolDictionary["StartSingle"];
                    vdr_STOPBITS.InnerText = vdr.STOPBITS = VDRProtocolDictionary["STOPBITS"];

                    VDRElement.SetAttributeNode(vdr_ID);
                    VDRElement.SetAttributeNode(vdr_BAUREATE);
                    VDRElement.SetAttributeNode(vdr_DATABIT);
                    VDRElement.SetAttributeNode(vdr_FirstAddr);
                    VDRElement.SetAttributeNode(vdr_FrameLenght);
                    VDRElement.SetAttributeNode(vdr_PARITY);
                    VDRElement.SetAttributeNode(vdr_PortName);
                    VDRElement.SetAttributeNode(vdr_StartSingle);

                    TCPNode.AppendChild(VDRElement);
                    XmlVDRList.Add(vdr);

                    path = xmlDoc.BaseURI.ToString().Substring(xmlDoc.BaseURI.ToString().LastIndexOf("///") + 3);
                    reader.Close();
                    xmlDoc.Save(path);
                    Page2_Reflash(XmlVDRList);

                    break;
                case ProtocolType.other:



                    custom = ProtocolOtherDictionary.ToArray();
                    for (int i = 0; i < custom.Length; i++)
                    {
                        BaseEdit baseEdit = layoutControl3.Controls.Find(prefixName + custom[i].Key, false)[0] as BaseEdit;
                        ProtocolOtherDictionary[custom[i].Key] = baseEdit.Text;

                    }

                    XmlOther other = new XmlOther();
                    XmlAttribute other_ID = xmlDoc.CreateAttribute("ID");
                    XmlAttribute other_Can1IP = xmlDoc.CreateAttribute("Can1IP");
                    XmlAttribute other_Can1Port = xmlDoc.CreateAttribute("Can1Port");
                    XmlAttribute other_Can2IP = xmlDoc.CreateAttribute("Can2IP");
                    XmlAttribute other_Can2Port = xmlDoc.CreateAttribute("Can2Port");
                    XmlAttribute other_CheckEndIndex = xmlDoc.CreateAttribute("CheckEndIndex");
                    XmlAttribute other_CheckStartIndex = xmlDoc.CreateAttribute("CheckStartIndex");
                    XmlAttribute other_CheckSumIndex = xmlDoc.CreateAttribute("CheckSumIndex");
                    XmlAttribute other_CheckSumType = xmlDoc.CreateAttribute("CheckSumType");
                    XmlAttribute other_EndSingle = xmlDoc.CreateAttribute("EndSingle");
                    XmlAttribute other_FirstAddr = xmlDoc.CreateAttribute("FirstAddr");
                    XmlAttribute other_PackageLength = xmlDoc.CreateAttribute("PackageLength");
                    XmlAttribute other_StartSingle = xmlDoc.CreateAttribute("StartSingle");
                    XmlAttribute other_DATABIT = xmlDoc.CreateAttribute("DATABIT");
                    XmlAttribute other_DataLenght = xmlDoc.CreateAttribute("DataLenght");
                    XmlAttribute other_FirstAddress = xmlDoc.CreateAttribute("FirstAddress");
                    XmlAttribute other_FrameLenght = xmlDoc.CreateAttribute("FrameLenght");
                    XmlAttribute other_FunctionCode = xmlDoc.CreateAttribute("FunctionCode");
                    XmlAttribute other_PARITY = xmlDoc.CreateAttribute("PARITY");
                    XmlAttribute other_PortName = xmlDoc.CreateAttribute("PortName");
                    XmlAttribute other_RequestCount = xmlDoc.CreateAttribute("RequestCount");
                    XmlAttribute other_SlaveID = xmlDoc.CreateAttribute("SlaveID");
                    XmlAttribute other_Standby1 = xmlDoc.CreateAttribute("Standby1");
                    XmlAttribute other_Standby2 = xmlDoc.CreateAttribute("Standby2");
                    XmlAttribute other_Standby3 = xmlDoc.CreateAttribute("Standby3");
                    XmlAttribute other_Standby4 = xmlDoc.CreateAttribute("Standby4");
                    XmlAttribute other_Standby5 = xmlDoc.CreateAttribute("Standby5");
                    XmlAttribute other_Standby6 = xmlDoc.CreateAttribute("Standby6");
                    XmlAttribute other_Standby7 = xmlDoc.CreateAttribute("Standby7");
                    XmlAttribute other_Standby8 = xmlDoc.CreateAttribute("Standby8");
                    XmlAttribute other_STOPBITS = xmlDoc.CreateAttribute("STOPBITS");

                    other_ID.InnerText = other.ID = GetMaxIndex(XmlOtherList).ToString();
                    other_Can1IP.InnerText = other.Can1IP = ProtocolOtherDictionary["Can1IP"];
                    other_Can1Port.InnerText = other.Can1Port = ProtocolOtherDictionary["Can1Port"];
                    other_Can2IP.InnerText = other.Can2IP = ProtocolOtherDictionary["Can2IP"];
                    other_Can2Port.InnerText = other.Can2Port = ProtocolOtherDictionary["Can2Port"];
                    other_CheckEndIndex.InnerText = other.CheckEndIndex = ProtocolOtherDictionary["CheckEndIndex"];
                    other_CheckStartIndex.InnerText = other.CheckStartIndex = ProtocolOtherDictionary["CheckStartIndex"];
                    other_CheckSumIndex.InnerText = other.CheckSumIndex = ProtocolOtherDictionary["CheckSumIndex"];
                    other_CheckSumType.InnerText = other.CheckSumType = ProtocolOtherDictionary["CheckSumType"];
                    other_EndSingle.InnerText = other.EndSingle = ProtocolOtherDictionary["EndSingle"];
                    other_FirstAddr.InnerText = other.FirstAddr = ProtocolOtherDictionary["FirstAddr"];
                    other_PackageLength.InnerText = other.PackageLength = ProtocolOtherDictionary["PackageLength"];
                    other_StartSingle.InnerText = other.StartSingle = ProtocolOtherDictionary["StartSingle"];
                    other_DATABIT.InnerText = other.DATABIT = ProtocolOtherDictionary["DATABIT"];
                    other_DataLenght.InnerText = other.DataLenght = ProtocolOtherDictionary["DataLenght"];
                    other_FirstAddress.InnerText = other.FirstAddress = ProtocolOtherDictionary["FirstAddress"];
                    other_FrameLenght.InnerText = other.FrameLenght = ProtocolOtherDictionary["FrameLenght"];
                    other_FunctionCode.InnerText = other.FunctionCode = ProtocolOtherDictionary["FunctionCode"];
                    other_PARITY.InnerText = other.PARITY = ProtocolOtherDictionary["PARITY"];
                    other_PortName.InnerText = other.PortName = ProtocolOtherDictionary["PortName"];
                    other_RequestCount.InnerText = other.RequestCount = ProtocolOtherDictionary["RequestCount"];
                    other_SlaveID.InnerText = other.SlaveID = ProtocolOtherDictionary["SlaveID"];
                    other_Standby1.InnerText = other.Standby1 = ProtocolOtherDictionary["Standby1"];
                    other_Standby2.InnerText = other.Standby2 = ProtocolOtherDictionary["Standby2"];
                    other_Standby3.InnerText = other.Standby3 = ProtocolOtherDictionary["Standby3"];
                    other_Standby4.InnerText = other.Standby4 = ProtocolOtherDictionary["Standby4"];
                    other_Standby5.InnerText = other.Standby5 = ProtocolOtherDictionary["Standby5"];
                    other_Standby6.InnerText = other.Standby6 = ProtocolOtherDictionary["Standby6"];
                    other_Standby7.InnerText = other.Standby7 = ProtocolOtherDictionary["Standby7"];
                    other_Standby8.InnerText = other.Standby8 = ProtocolOtherDictionary["Standby8"];
                    other_STOPBITS.InnerText = other.STOPBITS = ProtocolOtherDictionary["STOPBITS"];

                    TCPElement.SetAttributeNode(other_ID);
                    TCPElement.SetAttributeNode(other_Can1IP);
                    TCPElement.SetAttributeNode(other_Can2Port);
                    TCPElement.SetAttributeNode(other_Can2IP);
                    TCPElement.SetAttributeNode(other_CheckEndIndex);
                    TCPElement.SetAttributeNode(other_CheckStartIndex);
                    TCPElement.SetAttributeNode(other_CheckSumIndex);
                    TCPElement.SetAttributeNode(other_CheckSumType);
                    TCPElement.SetAttributeNode(other_EndSingle);
                    TCPElement.SetAttributeNode(other_FirstAddr);
                    TCPElement.SetAttributeNode(other_PackageLength);
                    TCPElement.SetAttributeNode(other_PARITY);
                    TCPElement.SetAttributeNode(other_PortName);
                    TCPElement.SetAttributeNode(other_RequestCount);
                    TCPElement.SetAttributeNode(other_SlaveID);
                    TCPElement.SetAttributeNode(other_Standby1);
                    TCPElement.SetAttributeNode(other_Standby2);
                    TCPElement.SetAttributeNode(other_Standby3);
                    TCPElement.SetAttributeNode(other_Standby4);
                    TCPElement.SetAttributeNode(other_Standby5);
                    TCPElement.SetAttributeNode(other_Standby6);
                    TCPElement.SetAttributeNode(other_Standby7);
                    TCPElement.SetAttributeNode(other_Standby8);
                    TCPElement.SetAttributeNode(other_StartSingle);
                    TCPElement.SetAttributeNode(other_STOPBITS);

                    TCPNode.AppendChild(TCPElement);
                    XmlOtherList.Add(other);

                    path = xmlDoc.BaseURI.ToString().Substring(xmlDoc.BaseURI.ToString().LastIndexOf("///") + 3);
                    reader.Close();
                    xmlDoc.Save(path);
                    Page2_Reflash(XmlOtherList);

                    break;
            }






        }
    }
}
