using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlProcessSerialize
{
    public class XmlSokect : IComparable<XmlSokect>
    {
        /*
         * <field>ID</field>
			<field>Can1IP</field>
			<field>Can1Port</field>
			<field>Can2IP</field>
			<field>Can2Port</field>
			<field>StartChar</field>
			<field>CommSingle</field>
			<field>CheckSingleSingle</field>
			<field>CheckDataLength</field>
			<field>EndSingle</field>
			<field>SplitChar</field>
			<field>InforPerData</field>
			<field>AddrIndex</field>
			<field>ValueIndex</field>
			<field>DataDefineRule</field> 
         */
        public string ID { get; set; }
        public string Can1IP { get; set; }
        public string Can1Port { get; set; }
        public string Can2IP { get; set; }
        public string Can2Port { get; set; }
        public string StartChar { get; set; }
        public string CommSingle { get; set; }
        public string CheckSingleSingle { get; set; }
        public string CheckDataLength { get; set; }
        public string EndSingle { get; set; }
        public string SplitChar { get; set; }
        public string InforPerData { get; set; }
        public string AddrIndex { get; set; }
        public string ValueIndex { get; set; }
        public string DataDefineRule { get; set; }

        public int CompareTo(XmlSokect other)
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
            var sokect = obj as XmlSokect;
            return sokect != null &&
                   ID == sokect.ID &&
                   Can1IP == sokect.Can1IP &&
                   Can1Port == sokect.Can1Port &&
                   Can2IP == sokect.Can2IP &&
                   Can2Port == sokect.Can2Port &&
                   StartChar == sokect.StartChar &&
                   CommSingle == sokect.CommSingle &&
                   CheckSingleSingle == sokect.CheckSingleSingle &&
                   CheckDataLength == sokect.CheckDataLength &&
                   EndSingle == sokect.EndSingle &&
                   SplitChar == sokect.SplitChar &&
                   InforPerData == sokect.InforPerData &&
                   AddrIndex == sokect.AddrIndex &&
                   ValueIndex == sokect.ValueIndex &&
                   DataDefineRule == sokect.DataDefineRule;
        }

        public override int GetHashCode()
        {
            var hashCode = -532147463;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can1IP);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can1Port);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can2IP);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can2Port);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StartChar);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CommSingle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckSingleSingle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckDataLength);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EndSingle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SplitChar);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InforPerData);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AddrIndex);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ValueIndex);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DataDefineRule);
            return hashCode;
        }
    }
}
