using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlProcess5._0
{
    public class XmlSokectCustomProtocol : IComparable<XmlSokectCustomProtocol>
    {
        /*
         * <field>ID</field>
			<field>Can2IP</field>
			<field>Can2Port</field>
			<field>Can1IP</field>
			<field>Can1Port</field> 
			<field>StartSingle</field>
			<field>EndSingle</field>
			<field>PackageLength</field>
			<field>FirstAddr</field>
			<field>CheckSumIndex</field>
			<field>CheckSumType</field>
			<field>CheckStartIndex</field>
			<field>CheckEndIndex</field>
			<field>CheckSumLength</field> 
         * */

        public string ID { get; set; }
        public string Can2IP { get; set; }
        public string Can2Port { get; set; }
        public string Can1IP { get; set; }
        public string Can1Port { get; set; }
        public string StartSingle { get; set; }
        public string EndSingle { get; set; }
        public string PackageLength { get; set; }
        public string FirstAddr { get; set; }
        public string CheckSumIndex { get; set; }
        public string CheckSumType { get; set; }
        public string CheckStartIndex { get; set; }
        public string CheckEndIndex { get; set; }
        public string CheckSumLength { get; set; }

        public int CompareTo(XmlSokectCustomProtocol other)
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
            var protocol = obj as XmlSokectCustomProtocol;
            return protocol != null &&
                   ID == protocol.ID &&
                   Can2IP == protocol.Can2IP &&
                   Can2Port == protocol.Can2Port &&
                   Can1IP == protocol.Can1IP &&
                   Can1Port == protocol.Can1Port &&
                   StartSingle == protocol.StartSingle &&
                   EndSingle == protocol.EndSingle &&
                   PackageLength == protocol.PackageLength &&
                   FirstAddr == protocol.FirstAddr &&
                   CheckSumIndex == protocol.CheckSumIndex &&
                   CheckSumType == protocol.CheckSumType &&
                   CheckStartIndex == protocol.CheckStartIndex &&
                   CheckEndIndex == protocol.CheckEndIndex &&
                   CheckSumLength == protocol.CheckSumLength;
        }

        public override int GetHashCode()
        {
            var hashCode = -871612615;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can2IP);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can2Port);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can1IP);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can1Port);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StartSingle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EndSingle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PackageLength);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstAddr);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckSumIndex);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckSumType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckStartIndex);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckEndIndex);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckSumLength);
            return hashCode;
        }
    }
}
