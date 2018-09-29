using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlProcess5._0_XmlField
{
    class Program
    {
        static Dictionary<string, string> CommFieldList = new Dictionary<string, string>();//公共字段
        static Dictionary<string, string> SokectCustomProtocolList = new Dictionary<string, string>();//socket协议字段
        static Dictionary<string, string> ModbusProtocolList = new Dictionary<string, string>();
        static Dictionary<string, string> VDRProtocolList = new Dictionary<string, string>();
        static Dictionary<string, string> ProtocolOtherList = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            LoadXMLField("./xmlField.xml");
            foreach (var item in CommFieldList)
            {
                Console.WriteLine(item.Key);
            }
            foreach (var item in SokectCustomProtocolList)
            {
                Console.WriteLine(item.Key);

            }
            foreach (var item in ModbusProtocolList)
            {
                Console.WriteLine(item.Key);

            }
            foreach (var item in VDRProtocolList)
            {
                Console.WriteLine(item.Key);

            }
            Console.ReadKey();
        }


        /// <summary>
        /// 加载XML字段配置文件
        /// </summary>
        /// <param name="fileName"></param>
        private static void LoadXMLField(string fileName)
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
                            ModbusProtocolList.Add(xml.InnerText, "");
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
                // Console.WriteLine("文件格式错误" + Environment.NewLine + ex.Message);
            }
        }
    }
}
