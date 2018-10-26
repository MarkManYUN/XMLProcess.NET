using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlProcessSerialize
{
    /// <summary>  
    /// <creator>zhangdapeng</creator>  
    /// </summary>  
    [XmlRootAttribute("unit")]
    public class Unit
    {
        private string _name;

        [XmlAttribute("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _name_value;

        [XmlTextAttribute]
        public string Name_value
        {
            get { return _name_value; }
            set { _name_value = value; }
        }
    }
}
