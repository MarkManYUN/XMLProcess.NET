using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlProcessSerialize.SerializeClass.Address
{
    [XmlRoot("PointsType", IsNullable = true)]
    public class PointsTypeConnection  :List<PointsConnection>
    {
        
        // public List<Points> PointsType = new List<Points>();

        /* 
         * 
         * 
         *   
         *   private List<Points> PointsList = new List<Points>();

        [XmlArray("PointsType")]
        [XmlArrayItem("Points")]
        public List<Points> GetPoints { get { return PointsList; } }
        public void AddPoints(Points points)
        {
            PointsList.Add(points);
        }
         * 
        <PointsType>
             <Points id="1" SlaveId = "2" FunctionCode="2"> 
                <Point LocalAddress = "25" ProtocolAddress="125" Factor="1" BitIndex="0" DataBit="1">主机控制阀空气阀关闭</Point>
                <Point LocalAddress = "26" ProtocolAddress="126" Factor="1" BitIndex="0" DataBit="1">定距桨控制失败</Point>
            </Points>
            <Points id="1" SlaveId = "2" FunctionCode="4">
                <Point LocalAddress = "28" ProtocolAddress="12" Factor="0.1" BitIndex="0" DataBit="16">主机起动空气压力</Point>
                <Point LocalAddress = "29" ProtocolAddress="14" Factor="0.1" BitIndex="0" DataBit="16">主机燃油压力压力</Point>
                <Point LocalAddress = "30" ProtocolAddress="16" Factor="0.1" BitIndex="0" DataBit="16">主机控制空气压力</Point>
            </Points> 
        </PointsType>
        */

    }
    [XmlType("Points")]
    public class PointsConnection  
    {
         
        [XmlElement("Point")]
        public Point[] PointArray { get; set; }
        [XmlAttribute("ID")]
        public string ID { get; set; }
        [XmlAttribute("SlaveId")]
        public string SlaveId { get; set; }
        [XmlAttribute("FunctionCode")]
        public string FunctionCode { get; set; }
         
        /*
        private List<Point> PointList = new List<Point>();
        [XmlArray("Points")] 
        [XmlArrayItem("Point")]
        public List<Point> GetPoint { get { return PointList; } }

        public void AddPoint(Point point)
        {
            PointList.Add(point);
        }
        */
    } 
    
    public class Point
    {
        [XmlAttribute("LocalAddress")]
        public string LocalAddress { get; set; }
        [XmlAttribute("ProtocolAddress")]
        public string ProtocolAddress { get; set; }
        [XmlAttribute("Factor")]
        public string Factor { get; set; }
        [XmlAttribute("BitIndex")]
        public string BitIndex { get; set; }
        [XmlAttribute("DataBit")]
        public string DataBit { get; set; }
        [XmlText]
        public string TextContent { get; set; }
    }
}
