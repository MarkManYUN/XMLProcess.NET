using System; 

namespace XmlProcess5_0
{
    public class XmlAddress : IComparable<XmlAddress>
    {
        public string SlaveId { get; set; }
        public string FunctionCode { get; set; }
        public string LocalAddress { get; set; }
        public string ProtocolAddress { get; set; }
        public string Factor { get; set; }
        public string BitIndex { get; set; }
        public string DataBit { get; set; }
        public string Text { get; set; }
        public string EnglishName { get; set; }

        public int CompareTo(XmlAddress other)
        {
            if (other == null) return 1;
            if (this.LocalAddress == "") return -1;
            int id = int.Parse(this.LocalAddress);
            int idO = int.Parse(other.LocalAddress);

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
