using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using XmlProcessSerialize.SerializeClass.Config;

namespace XmlProcessSerialize
{
    [XmlRoot ("Communications")]
   public class CommunicationConfig
    {
         
        [XmlArray ("CommunicaitonData")]
        [XmlArrayItem ("Communication")]
        public Communication[] Communications { get; set; }
    }
    [XmlRootAttribute("Communication")]
    public class Communication
    {
        // <Communication ID="1" CommType="SokectVDR" NameCH="VDR0" NameEN="VDR0" ConfigFilePath="Config\Protocol\SokectModbus.xml" />
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("CommType")]
        public string CommType { get; set; }

        [XmlAttribute("NameCH")]
        public string NameCH { get; set; }

        [XmlAttribute("NameEN")]
        public string NameEN { get; set; }

        [XmlAttribute("ConfigFilePath")]
        public string ConfigFilePath { get; set; }
    }
}
