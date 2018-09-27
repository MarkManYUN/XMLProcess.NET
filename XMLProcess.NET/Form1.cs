using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace XMLProcess.NET
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        List<Communication> modelList = new List<Communication>();

        XmlDocument xmlDoc = new XmlDocument();
        public Form1()
        {
            InitializeComponent();
        }

        private void file_open_Click(object sender, EventArgs e)
        {

            //打开指定文件目录
            openFileDialog.InitialDirectory = @".\";

            openFileDialog.Title = "选中你的配置文件";
            //过滤文件类型
            openFileDialog.Filter = "(*.xml)|*.xml";
            //不支持多选
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ReadXml();
            }

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
            dataGridView1.DataSource = modelList.ToArray();

        }
        private void ReadXml()
        {
            try
            {
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
                dataGridView1.DataSource = modelList;
                dataGridView1.Columns["ID"].HeaderText = "编号";
                dataGridView1.Columns["CommType"].HeaderText = "协议类型";
                dataGridView1.Columns["NameCH"].HeaderText = "中文名";
                dataGridView1.Columns["NameEN"].HeaderText = "英文名";
                dataGridView1.Columns["ConfigFilePath"].HeaderText = "文件路径";

            }
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

        private void showInfoByAddressElements(IEnumerable<XElement> elements)
        {
            List<Address> modelList = new List<Address>();
            foreach (var ele in elements)
            {
                Address model = new Address();
                model.SlaveId = ele.Attribute("SlaveId").Value;
                model.FunctionCode = ele.Attribute("FunctionCode").Value;

                model.Factor = ele.Attribute("Factor").Value;
                model.BitIndex = ele.Attribute("BitIndex").Value;
                model.DataBit = ele.Attribute("DataBit").Value;
                model.ProtocolAddress = ele.Attribute("ProtocolAddress").Value;
                model.LocalAddress = ele.Attribute("LocalAddress").Value;

                modelList.Add(model);
            }
            dataGridView1.DataSource = modelList;

        }
        private void showInfoByElements(IEnumerable<XElement> elements)
        {
            foreach (var ele in elements)
            {
                IEnumerable<XElement> els = from el in ele.Elements("Communication")
                                            select el;
                 
                foreach (var item in els)
                {
                    Communication model = new Communication();

                    model.ID = item.Attribute("ID").Value;
                    model.CommType = item.Attribute("CommType").Value;
                    model.NameCH = item.Attribute("NameCH").Value;
                    model.NameEN = item.Attribute("NameEN").Value;
                    model.ConfigFilePath = item.Attribute("ConfigFilePath").Value;
 
                    modelList.Add(model);

                    
                }

            }
            modelList.Sort();
            dataGridView1.DataSource = modelList;
        }
        private void showInfoByBookElements(IEnumerable<XElement> elements)
        {
            List<BookModel> modelList = new List<BookModel>();
            foreach (var ele in elements)
            {
                BookModel model = new BookModel();
                model.BookAuthor = ele.Element("author").Value;
                model.BookName = ele.Element("title").Value;
                model.BookPrice = Convert.ToDouble(ele.Element("price").Value);
                model.BookISBN = ele.Attribute("ISBN").Value;
                model.BookType = ele.Attribute("Type").Value;

                modelList.Add(model);
            }
            dataGridView1.DataSource = modelList;
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            txt_data1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txt_data2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txt_data3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txt_data4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txt_path.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_insert_Click(object sender, EventArgs e)
        {
            if (openFileDialog.FileName == "")
            {
                MessageBox.Show("请打开配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Communication communication = new Communication();
            communication.ID = (int.Parse(modelList[modelList.Count - 1].ID) + 1).ToString();
            communication.NameCH = txt_insetCH.Text;
            communication.NameEN = txt_insertEN.Text;
            communication.CommType = txt_insertType.Text;
            communication.ConfigFilePath = txt_insertPath.Text;

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
        /// <summary>
        /// 更加ID删除记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DeleteCheck_Click(object sender, EventArgs e)
        {
            XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
            string strPath = string.Format("/Communications/CommunicaitonData/Communication[@ID=\"{0}\"]",
                dataGridView1.CurrentRow.Cells[0].Value.ToString());
            //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            selectXe.ParentNode.RemoveChild(selectXe);
            xmlDoc.Save(openFileDialog.FileName);
            Communication communication = new Communication();
            communication.ID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            communication.CommType = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            communication.NameCH = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            communication.NameEN = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            communication.ConfigFilePath = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            modelList.Remove(communication);
            Reflash();
        }
        /// <summary>
        /// 更新内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_update_Click(object sender, EventArgs e)
        {
            XmlElement xe = xmlDoc.DocumentElement; // DocumentElement 获取xml文档对象的根XmlElement.
            string strPath = string.Format("/Communications/CommunicaitonData/Communication[@ID=\"{0}\"]",
                dataGridView1.CurrentRow.Cells[0].Value.ToString());
            //selectSingleNode 根据XPath表达式,获得符合条件的第一个节点.
            XmlElement selectXe = (XmlElement)xe.SelectSingleNode(strPath);
            // selectXe.SetAttribute("CommType", txt_data2.Text);//也可以通过SetAttribute来增加一个属性
            selectXe.SetAttribute("CommType", txt_data2.Text);
            selectXe.SetAttribute("NameCH", txt_data3.Text);
            selectXe.SetAttribute("NameEN", txt_data4.Text);
            selectXe.SetAttribute("ConfigFilePath", txt_path.Text);


            Communication communication = new Communication();
            communication.ID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            communication.CommType = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            communication.NameCH = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            communication.NameEN = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            communication.ConfigFilePath = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            modelList.Remove(communication);
            communication.ID = txt_data1.Text;
            communication.CommType = txt_data2.Text;
            communication.NameCH = txt_data3.Text;
            communication.NameEN = txt_data4.Text;
            communication.ConfigFilePath = txt_path.Text;
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

        private void btn_newXmlFile_Click(object sender, EventArgs e)
        {
            if (txt_fileName.Text.Trim().Equals(""))
            {
                MessageBox.Show("请填写文件名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                this.Activate(); //当前窗体是this
                txt_fileName.Focus();
                return;
            }
            string path;
            if (openFileDialog.FileName.Equals(""))
            {
                path = "./xmlSource/";
            }
            else { path = Path.GetDirectoryName(openFileDialog.FileName); }
            string filePath = path + "/" + txt_fileName.Text + ".xml";
            if (File.Exists(filePath))
            {
                MessageBox.Show("你的文件已经存在是否替换", "文件已经存在", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            else
            {
                XmlTextWriter myXmlTextWriter = new XmlTextWriter(filePath, null);
                myXmlTextWriter.WriteStartDocument(false);
                myXmlTextWriter.WriteStartElement("Communications");
                myXmlTextWriter.Close();
                XElement xe = XElement.Load(filePath);
                            XElement record = new XElement(
                             new XElement("book",
                             new XAttribute("Type", "选修课"),
                            new XAttribute("ISBN", "7-111-19149-1"),
                             new XElement("title", "计算机操作系统"),
                              new XElement("author", "7-111-19149-1"),
                           new XElement("price", 28.00)));
                             xe.Add(record);
                             xe.Save(filePath);
                            MessageBox.Show("插入成功！"); 
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
            }
        }

        private void btn_DeleteAll_Click(object sender, EventArgs e)
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
    }


}
