using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlProcess5._0_Potocol
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\Administrator\OneDrive\公司\文档资料\HAMI协议资料\XML源码\Protocol\Fj4000dSokectModbus.xml";
            ReadXml(path);
            Console.ReadKey();
        }
        private static void ReadXml(String fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            StringBuilder sb = new StringBuilder();
            try
            {
                //判断当前打开的页数，判断加载的是什么程序
                // 1. 通讯配置
                // 2. 协议配置
                // 3. 通道配置

                //通讯配置
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                XmlReader reader = XmlReader.Create(fileName, settings);
                xmlDoc.Load(reader);
                // 得到根节点bookstore
                XmlNode xn = xmlDoc.SelectSingleNode("Communications");


                // 得到根节点的所有子节点CommunicaitonData
                XmlNodeList xnl = xn.ChildNodes;
                string separator = "   ";
                sb.Append("ID");sb.Append(separator);
                sb.Append("ChineseName"); sb.Append(separator);
                sb.Append("EnglishiName"); sb.Append(separator);
                sb.Append("CommReConnectTime"); sb.Append(separator);
                sb.Append("CommErroTime"); sb.Append(separator);
                sb.Append("WriteTimeOut"); sb.Append(separator);
                sb.Append("ReadTimeOut"); sb.Append(separator);
                sb.Append("CommSpaceTime"); sb.Append(separator);
                sb.Append("ConfigFilePath"); sb.Append(separator);
                sb.Append(Environment.NewLine);
                foreach (XmlNode xn1 in xnl)
                {
                    // 得到根节点的所有子节点Communication
                    XmlNodeList xnn = xn1.ChildNodes;
                    foreach (XmlNode item in xnn)
                    {
                        // 将节点转换为元素，便于得到节点的属性值
                        XmlElement ele = (XmlElement)item;
                        sb.Append(ele.GetAttribute("ID").ToString()); sb.Append(separator);
                        sb.Append(ele.GetAttribute("ChineseName").ToString()); sb.Append(separator);
                        sb.Append(ele.GetAttribute("EnglishiName").ToString()); sb.Append(separator);
                        sb.Append(ele.GetAttribute("CommReConnectTime").ToString()); sb.Append(separator);
                        sb.Append(ele.GetAttribute("CommErroTime").ToString()); sb.Append(separator);
                        sb.Append(ele.GetAttribute("WriteTimeOut").ToString()); sb.Append(separator);
                        sb.Append(ele.GetAttribute("ReadTimeOut").ToString()); sb.Append(separator);
                        sb.Append(ele.GetAttribute("CommSpaceTime").ToString()); sb.Append(separator);
                        sb.Append(ele.GetAttribute("ConfigFilePath").ToString()); 
                        
                        sb.Append(Environment.NewLine); 
                        XmlNodeList eles = item.ChildNodes;
                        foreach (XmlNode tcp in eles) 
                        {
                            // 将节点转换为元素，便于得到节点的属性值
                            XmlElement xml = (XmlElement)tcp;
                            sb.Append(xml.GetAttribute("Can2IP").ToString()); sb.Append(separator);
                            sb.Append(xml.GetAttribute("Can2Port").ToString()); sb.Append(separator);
                            sb.Append(xml.GetAttribute("Can1Port").ToString()); sb.Append(separator);
                            sb.Append(xml.GetAttribute("Can1IP").ToString()); sb.Append(separator);
                            sb.Append(xml.GetAttribute("ID").ToString()); sb.Append(separator);
                            sb.Append(xml.GetAttribute("StartSingle").ToString()); sb.Append(separator);
                            sb.Append(xml.GetAttribute("EndSingle").ToString()); sb.Append(separator);
                            sb.Append(xml.GetAttribute("PackageLength").ToString()); sb.Append(separator);
                            sb.Append(xml.GetAttribute("FirstAddr").ToString());
                            sb.Append(xml.GetAttribute("CheckSumIndex").ToString());
                            sb.Append(xml.GetAttribute("CheckSumType").ToString());
                            sb.Append(xml.GetAttribute("CheckStartIndex").ToString());
                            sb.Append(xml.GetAttribute("CheckEndIndex").ToString());
                            sb.Append(xml.GetAttribute("CheckSumLength").ToString()); 

                            sb.Append(Environment.NewLine);
                            XmlNodeList list = tcp.ChildNodes;
                            foreach (XmlNode node in list)
                            {
                                // 将节点转换为元素，便于得到节点的属性值
                                XmlElement n = (XmlElement)node;
                                sb.Append(n.GetAttribute("ID").ToString()); sb.Append(separator);
                                sb.Append(n.GetAttribute("SlaveID").ToString()); sb.Append(separator);
                                sb.Append(n.GetAttribute("FunctionCode").ToString()); sb.Append(separator);
                                sb.Append(n.GetAttribute("FirstAddress").ToString()); sb.Append(separator);
                                sb.Append(n.GetAttribute("RequestCount").ToString()); sb.Append(separator);
                                sb.Append(n.GetAttribute("DataLenght").ToString()); sb.Append(separator); 
                                sb.Append(Environment.NewLine);

                            }
                        }
                              
                    }
                }
                reader.Close();
                Console.WriteLine(sb);
            }
            catch (Exception ex)
            {
                Console.WriteLine("文件格式错误" + Environment.NewLine + ex.Message);
            }
        }
    }
}
