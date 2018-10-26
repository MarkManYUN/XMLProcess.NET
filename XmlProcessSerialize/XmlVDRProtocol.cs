using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlProcessSerialize
{
  public  class XmlVDRProtocol :IComparable<XmlVDRProtocol>
    {
        /*
         * <field>PortName</field>
			<field>STOPBITS</field>
			<field>DATABIT</field>
			<field>PARITY</field>
			<field>BAUREATE</field>
			<field>FrameLenght</field>
			<field>FirstAddr</field>
			<field>StartSingle</field>  
         */
        public string ID { get; set; }
        public string PortName { get; set; }
        public string STOPBITS { get; set; }
        public string DATABIT { get; set; }
        public string PARITY { get; set; }
        public string BAUREATE { get; set; }
        public string FrameLenght { get; set; }
        public string FirstAddr { get; set; }
        public string StartSingle { get; set; }

        public int CompareTo(XmlVDRProtocol other)
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

        public override bool Equals(object obj)
        {
            var protocol = obj as XmlVDRProtocol;
            return protocol != null &&
                   ID == protocol.ID &&
                   PortName == protocol.PortName &&
                   STOPBITS == protocol.STOPBITS &&
                   DATABIT == protocol.DATABIT &&
                   PARITY == protocol.PARITY &&
                   BAUREATE == protocol.BAUREATE &&
                   FrameLenght == protocol.FrameLenght &&
                   FirstAddr == protocol.FirstAddr &&
                   StartSingle == protocol.StartSingle;
        }

        public override int GetHashCode()
        {
            var hashCode = 1812231103;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PortName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(STOPBITS);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DATABIT);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PARITY);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BAUREATE);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FrameLenght);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstAddr);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StartSingle);
            return hashCode;
        }

    }
}
