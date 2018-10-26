using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlProcessSerialize.SerializeClass.Protocol
{
    [XmlRoot("Communications")]
    public class ProtocolBase : ProtocolCommunicaitonData
    {

    }
    [XmlType("Communications")]
    public class ProtocolCommunicaitonData
    {

    }
    [XmlType("Communication")]
    public class ProtocolCommunication
    {
        [XmlAttribute]
        public string ID { get; set; }
        [XmlAttribute]
        public string ChineseName { get; set; }
        [XmlAttribute]
        public string EnglishiName { get; set; }
        [XmlAttribute]
        public string CommReConnectTime { get; set; }
        [XmlAttribute]
        public string CommErroTime { get; set; }
        [XmlAttribute]
        public string WriteTimeOut { get; set; }
        [XmlAttribute]
        public string ReadTimeOut { get; set; }
        [XmlAttribute]
        public string CommSpaceTime { get; set; }
        [XmlAttribute]
        public string ConfigFilePath { get; set; } 
    }
    [XmlType("TCP")]
    public class TCP
    {
        [XmlAttribute]
        public string Can2IP { get; set; }
        [XmlAttribute]
        public string Can2Port { get; set; }
        [XmlAttribute]
        public string Can1IP { get; set; }
        [XmlAttribute]
        public string Can1Port { get; set; }

        [XmlElement]
        public CommCongfig[] commCongfigs { get; set; }
    }
    public class CommCongfig
    {
        //ID="2" SlaveID="1" FunctionCode="1" FirstAddress="41" RequestCount="11" DataLenght="1"
        [XmlAttribute]
        public string ID { get; set; }
        [XmlAttribute]
        public string SlaveID { get; set; }
        [XmlAttribute]
        public string FunctionCode { get; set; }
        [XmlAttribute]
        public string FirstAddress { get; set; }
        [XmlAttribute]
        public string RequestCount { get; set; }

        [XmlAttribute]
        public string DataLenght { get; set; }

    }
    [XmlType("VDR")]
    public class VDR
    {
        //<VDR PortName="COM10" STOPBITS="0" DATABIT="8" PARITY="0" BAUREATE="9600" />
        [XmlAttribute]
        public string PortName { get; set; }
        [XmlAttribute]
        public string STOPBITS { get; set; }
        [XmlAttribute]
        public string DATABIT { get; set; }
        [XmlAttribute]
        public string PARITY { get; set; }
        [XmlAttribute]
        public string BAUREATE { get; set; }
         
    }
    [XmlType("NMEA")]
    public class NMEA
    {
        // <NMEA PortName="COM7" STOPBITS="1" DATABIT="8" PARITY="0" BAUREATE="9600" RLCSingle="*" ]
        //FrameSingle="KD2XDR" StartSingle="$" IntRLCLenght="0" EndSingle="0D,0A" FrameSplit="S"
        //StrDataSplit="," CountPerData="4" ValueIndex="1" AddrIndex="3" DataDefineRule="" />
        [XmlAttribute]
        public string PortName { get; set; }
        [XmlAttribute]
        public string STOPBITS { get; set; }
        [XmlAttribute]
        public string DATABIT { get; set; }
        [XmlAttribute]
        public string PARITY { get; set; }
        [XmlAttribute]
        public string BAUREATE { get; set; }
        [XmlAttribute]
        public string RLCSingle { get; set; }
        [XmlAttribute]
        public string FrameSingle { get; set; }
        [XmlAttribute]
        public string StartSingle { get; set; }
        [XmlAttribute]
        public string IntRLCLenght { get; set; }
        [XmlAttribute]
        public string EndSingle { get; set; }
        [XmlAttribute]
        public string FrameSplit { get; set; }
        [XmlAttribute]
        public string StrDataSplit { get; set; }
        [XmlAttribute]
        public string CountPerData { get; set; }
        [XmlAttribute]
        public string ValueIndex { get; set; }
        [XmlAttribute]
        public string AddrIndex { get; set; }
        [XmlAttribute]
        public string DataDefineRule { get; set; }

    }
    [XmlType("MOUDBUS")]
    public class MOUDBUS
    {
        // <MOUDBUS PortName="COM8" STOPBITS="1" DATABIT="8" PARITY="0" BAUREATE="19200">
        [XmlAttribute]
        public string PortName { get; set; }
        [XmlAttribute]
        public string STOPBITS { get; set; }
        [XmlAttribute]
        public string DATABIT { get; set; }
        [XmlAttribute]
        public string PARITY { get; set; }
        [XmlAttribute]
        public string BAUREATE { get; set; } 
        [XmlElement]
        public CommCongfig[] CommCongfigs { get; set; }
    }
}
