using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlProcessSerialize
{
    /// <summary>  
    /// <creator>Mark</creator>  
    /// </summary>  
    [XmlRoot("skycenter")]
    public class AdsbEntity
    {
        private string _type;

        [XmlAttribute("type")]
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _source;
        [XmlAttribute("source")]
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }
        private string _reality;
        [XmlAttribute("reality")]
        public string Reality
        {
            get { return _reality; }
            set { _reality = value; }
        }
        private string _rcvTime;
        [XmlAttribute("rcvTime")]
        public string RcvTime
        {
            get { return _rcvTime; }
            set { _rcvTime = value; }
        }
        private Head _head;
        //属性的定义  
        [XmlElement("head")]
        public Head Head
        {
            set   //设定属性  
            {
                _head = value;
            }
            get    //从属性获取值  
            {
                return _head;
            }
        }


        private List<Unit> data = new List<Unit>();

        [XmlArray("data")]
        [XmlArrayItem("unit")]
        public List<Unit> Unit
        {
            get { return data; }
        }
        public void addUnit(Unit e)
        {
            data.Add(e);
        }

        private string _msg;

        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

    }
}
