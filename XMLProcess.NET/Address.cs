using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLProcess.NET
{
    enum AddressEnum
    {
        SlaveId, FunctionCode, LocalAddress, ProtocolAddress, Factor, BitIndex, DataBit
    }
    public class Communication : IComparable<Communication>
    {
        public string ID { get; set; }
        public string CommType { get; set; }
        public string NameCH { get; set; } 

        public string NameEN { get; set; }
        public string ConfigFilePath { get; set; }
         
        public int CompareTo(Communication other)
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
            var communication = obj as Communication;
            return communication != null &&
                   ID == communication.ID &&
                   CommType == communication.CommType &&
                   NameCH == communication.NameCH &&
                   NameEN == communication.NameEN &&
                   ConfigFilePath == communication.ConfigFilePath;
        }

        public override int GetHashCode()
        {
            var hashCode = -1348519651;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            return hashCode;
        }
    }
    public class BookModel
    {
        public string BookType { get; set; }
        public string BookISBN { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public double BookPrice { get; set; }

    }
    public  class Address
    {
        public string SlaveId  { get; set; }

        public string FunctionCode { get; set; }

        public string LocalAddress { get; set; }

        public string ProtocolAddress { get; set; }

        public string Factor { get; set; }

        public string BitIndex { get; set; }

        public string DataBit { get; set; }

        public override bool Equals(object obj)
        {
            var address = obj as Address;
            return address != null &&
                   SlaveId == address.SlaveId &&
                   FunctionCode == address.FunctionCode &&
                   LocalAddress == address.LocalAddress &&
                   ProtocolAddress == address.ProtocolAddress &&
                   Factor == address.Factor &&
                   BitIndex == address.BitIndex &&
                   DataBit == address.DataBit;
        }

        public override int GetHashCode()
        {
            var hashCode = -822694286;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SlaveId); 
            return hashCode;
        }
    }
}
