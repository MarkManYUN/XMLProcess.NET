using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlProcess5_0
{
    public class XmlModbus : IComparable<XmlModbus>
    {
        public string ID { get; set; }
        public string Can2IP { get; set; }
        public string Can2Port { get; set; }
        public string Can1IP { get; set; }
        public string Can1Port { get; set; }
        public string SlaveID { get; set; }
        public string FunctionCode { set; get; }
        public string FirstAddress { set; get; }
        public string RequestCount { set; get; }
        public string DataLenght { set; get; }

        public override bool Equals(object obj)
        {
            var modbus = obj as XmlModbus;
            return modbus != null &&
                   ID == modbus.ID &&
                   SlaveID == modbus.SlaveID;
        }

        public override int GetHashCode()
        {
            var hashCode = -1455060291;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SlaveID);
            return hashCode;
        }
        public int CompareTo(XmlModbus other)
        {
            if (other == null) return 1;
            if (this.ID == "") return -1;
            int id = int.Parse(this.ID);
            int idO = int.Parse(other.ID);

            if (id > idO)
            {
                return 1;
            }
            else if (id < idO)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

    }
}
