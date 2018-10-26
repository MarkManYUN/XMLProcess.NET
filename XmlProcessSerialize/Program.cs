using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using XmlProcessSerialize.SerializeClass.Address;
using XmlProcessSerialize.SerializeClass.Test;

namespace XmlProcessSerialize
{
    class Program
    {
        static void Main(string[] args)
        {


            //  WriteAddress();
            // msdntest();
            CommunicationConfig();
            Console.ReadKey();
        }
        public static void Test1()
        {
            //声明一个猫咪对象
            var cWhite = new Cat { Color = "White", Speed = 10, Saying = "White or black,  so long as the cat can catch mice,  it is a good cat" };
            var cBlack = new Cat { Color = "Black", Speed = 10, Saying = "White or black,  so long as the cat can catch mice,  it is a good cat" };

            CatCollection cc = new CatCollection { Cats = new Cat[] { cWhite, cBlack } };

            //序列化这个对象
            XmlSerializer serializer = new XmlSerializer(typeof(CatCollection));

            //将对象序列化输出到控制台
            serializer.Serialize(Console.Out, cc);

        }
        public static void msdntest() {

            // 读写订单。
            MsdnTest t = new MsdnTest();
            t.CreatePO("po.xml");
            t.ReadPO("po.xml");
        }
        public static void WriteAddress()
        {
            Point point = new Point
            {
                BitIndex = "bitindex",
                DataBit = "databit",
                Factor = "factor",
                LocalAddress = "localaddress",
                ProtocolAddress = "protocoladdress",
                TextContent = "text"
            };
            Point point2 = new Point
            {
                BitIndex = "bitindex2",
                DataBit = "databit2",
                Factor = "factor2",
                LocalAddress = "localaddress2",
                ProtocolAddress = "protocoladdress2",
                TextContent = "text2"
            };
            Point point3 = new Point
            {
                BitIndex = "bitindex",
                DataBit = "databit",
                Factor = "factor",
                LocalAddress = "localaddress",
                ProtocolAddress = "protocoladdress",
                TextContent = "text"
            };
            Point point4 = new Point
            {
                BitIndex = "bitindex2",
                DataBit = "databit2",
                Factor = "factor2",
                LocalAddress = "localaddress2",
                ProtocolAddress = "protocoladdress2",
                TextContent = "text2"
            };  
            PointsConnection points = new PointsConnection
            {
                ID = "1",
                FunctionCode = "03",
                SlaveId = "slave",
                
            }; 
            points.ID = "11245";
            Point[] pointArr = new Point[] { point, point2 };
            points.PointArray = pointArr;
            PointsConnection points2 = new PointsConnection
            {
                ID = "12",
                FunctionCode = "032",
                SlaveId = "slave2",
            };
            Point[] pointArray = new Point[] { point3, point4 };
            points2.PointArray = pointArray;
            PointsTypeConnection pointsType = new PointsTypeConnection();
            PointsConnection[] pointsConnections = new PointsConnection[] { points, points2 };
            pointsType.Add(points);pointsType.Add(points2);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PointsTypeConnection));
            //将对象序列化输出到控制台
            xmlSerializer.Serialize(Console.Out, pointsType, namespaces);

            /*
            string xml = XmlSerializeUtil.Serializer(typeof(Points), points);

            byte[] byteToStr = System.Text.Encoding.Default.GetBytes(xml);
            string str64 = Convert.ToBase64String(byteToStr);
            Console.WriteLine(Environment.NewLine + str64);

            byte[] accessory = Convert.FromBase64String(str64);
            string fromStr = System.Text.Encoding.Default.GetString(accessory);
            Console.WriteLine("Base64转字符串" + fromStr);
            FileStream fileStream = File.Create("test2.xml");
            fileStream.Write(byteToStr, 0, byteToStr.Length);
            fileStream.Flush();
            fileStream.Close();
            */
        }


        public static void Address()
        {
            //获取当前根路径
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(@path + "\\xmlSource\\Config\\Protocol\\MSPAddress.xml");
            Console.WriteLine(xmlDoc.OuterXml);//输出XML文档
            PointsTypeConnection cc = XmlSerializeUtil.Deserialize(typeof(PointsTypeConnection), xmlDoc.OuterXml) as PointsTypeConnection;
            Console.WriteLine();
            int i = 1;
            /*   foreach (var item in cc.GetPoints)
               {
                   i++;
                   Console.WriteLine("----------------------" + i + "--------------------------------");
                   Console.WriteLine("PointsType--NameCH:" + item.FunctionCode);
                   Console.WriteLine("PointsType--ID:" + item.ID);
                   Console.WriteLine("PointsType--CommType:" + item.SlaveId);

                   foreach (var point in item.GetPoint)
                   {
                       Console.WriteLine("point--BitIndex:" + point.BitIndex);
                       Console.WriteLine("point--DataBit:" + point.DataBit);
                       Console.WriteLine("point--Factor:" + point.Factor);
                       Console.WriteLine("point--LocalAddress:" + point.LocalAddress);
                       Console.WriteLine("point--ProtocolAddress:" + point.ProtocolAddress);
                       Console.WriteLine("point--TextContent:" + point.TextContent);


                   }
               }
               */
        }
        public static void CommunicationConfig()
        {

            Communication communication1 = new Communication
            {
                CommType = "1",
                ConfigFilePath = "d/d/d/",
                ID = "2",
                NameCH = "中文",
                NameEN = "English"
            };
            Communication communication2 = new Communication
            {
                CommType = "2",
                ConfigFilePath = "opathpath",
                ID = "3",
                NameCH = "中文2",
                NameEN = "英文"
            };
            Communication[] communications = new Communication[] { communication1, communication2 };
            CommunicationConfig communicationConfig = new CommunicationConfig();
            communicationConfig.Communications = communications;
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CommunicationConfig));
            xmlSerializer.Serialize(Console.Out, communicationConfig,namespaces);
            //\\xmlSource\\CommunicationConfig.xml

            /*
            //获取当前根路径
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(@path + "\\xmlSource\\CommunicationConfig.xml");
            Console.WriteLine(xmlDoc.OuterXml);//输出XML文档
            CommunicationConfig cc = XmlSerializeUtil.Deserialize(typeof(CommunicationConfig), xmlDoc.OuterXml) as CommunicationConfig;
            Console.WriteLine();
            int i = 1;
            foreach (var item in cc.Communications)
            {
                i++;
                Console.WriteLine("----------------------" + i + "--------------------------------");
                Console.WriteLine("Communication--NameCH:" + item.NameCH);
                Console.WriteLine("Communication--NameEN:" + item.NameEN);
                Console.WriteLine("Communication--CommType:" + item.CommType);
                Console.WriteLine("Communication--ConfigFilePath:" + item.ConfigFilePath);
                Console.WriteLine("Communication--ID:" + item.ID);
            }
            string xmlstr = XmlSerializeUtil.Serializer(typeof(CommunicationConfig), cc);
            byte[] byteTostr = System.Text.Encoding.Default.GetBytes(xmlstr);
            FileStream fileStream = File.Create("communicationconfig.xml");
            fileStream.Write(byteTostr, 0, byteTostr.Length);
            fileStream.Flush();
            fileStream.Close();
            */
        }
        public static void Test()
        {
            /*
         //获取当前根路径
         string path = System.AppDomain.CurrentDomain.BaseDirectory;
         XmlDocument xmlDoc = new XmlDocument();

         xmlDoc.Load(@path + "../../test.xml");
         Console.WriteLine(xmlDoc.OuterXml);//输出XML文档

         AdsbEntity ad = XmlSerializeUtil.Deserialize(typeof(AdsbEntity), xmlDoc.OuterXml) as AdsbEntity;
         Console.WriteLine("datagramId: " + ad.Head.DatagramId);
         Console.WriteLine("fi:" + ad.Head.Fi);
         ad.RcvTime = DateTime.Now.ToLocalTime().ToString();
         string xml = XmlSerializeUtil.Serializer(typeof(AdsbEntity), ad);

         byte[] byteToStr = System.Text.Encoding.Default.GetBytes(xml);
         string str64 = Convert.ToBase64String(byteToStr);
         Console.WriteLine(Environment.NewLine + str64);

         byte[] accessory = Convert.FromBase64String(str64);
         string fromStr = System.Text.Encoding.Default.GetString(accessory);
         Console.WriteLine("Base64转字符串"+fromStr);
         FileStream fileStream = File.Create("test2.xml");
         fileStream.Write(byteToStr, 0, byteToStr.Length);
         fileStream.Flush();
         fileStream.Close();
         //xmlDoc.Save("test2.xml");
         Console.WriteLine(xml);
         */
        }
    }
}
