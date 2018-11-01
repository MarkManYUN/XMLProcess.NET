using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlProcess5_0
{
    public class XmlOther:IComparable<XmlOther>
    {
        /*
         *<field>ID</field>
			<field>Can2IP</field>
			<field>Can2Port</field>
			<field>Can1IP</field>
			<field>Can1Port</field> 
			<field>SlaveID</field>
			<field>FunctionCode</field>
			<field>FirstAddress</field>
			<field>RequestCount</field> 
			<field>DataLenght</field>  
			<field>PortName</field>
			<field>STOPBITS</field>
			<field>DATABIT</field>
			<field>PARITY</field>
			<field>BAUREATE</field>

			<field>FrameLenght</field>   
			<field>StartSingle</field>
			<field>EndSingle</field>
			<field>PackageLength</field>
			<field>FirstAddr</field>
			<field>CheckSumIndex</field>
			<field>CheckSumType</field>
			<field>CheckStartIndex</field>
			<field>CheckEndIndex</field>

			<field>Standby1</field> 
			<field>Standby2</field> 
			<field>Standby3</field> 
			<field>Standby4</field> 
			<field>Standby5</field> 
			<field>Standby6</field> 
			<field>Standby7</field> 
			<field>Standby8</field>  
         */
        public string ID { get; set; }
        public string Can2IP { get; set; }
        public string Can2Port { get; set; }
        public string Can1IP { get; set; }
        public string Can1Port { get; set; }
        public string SlaveID { get; set; }
        public string FunctionCode { get; set; }
        public string FirstAddress { get; set; }
        public string RequestCount { get; set; }
        public string DataLenght { get; set; }
        public string PortName { get; set; }
        public string STOPBITS { get; set; }
        public string DATABIT { get; set; }
        public string PARITY { get; set; }
        public string BAUREATE { get; set; }
        public string FrameLenght { get; set; }
        public string StartSingle { get; set; }


        public string EndSingle { get; set; }
        public string PackageLength { get; set; }
        public string FirstAddr { get; set; }
        public string CheckSumIndex { get; set; }
        public string CheckSumType { get; set; }
        public string CheckStartIndex { get; set; }
        public string CheckEndIndex { get; set; }

        public string Standby1 { get; set; }
        public string Standby2 { get; set; }
        public string Standby3 { get; set; }
        public string Standby4 { get; set; }
        public string Standby5 { get; set; }
        public string Standby6 { get; set; }
        public string Standby7 { get; set; }
        public string Standby8 { get; set; }

        public int CompareTo(XmlOther other)
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
            var other = obj as XmlOther;
            return other != null &&
                   ID == other.ID &&
                   Can2IP == other.Can2IP &&
                   Can2Port == other.Can2Port &&
                   Can1IP == other.Can1IP &&
                   Can1Port == other.Can1Port &&
                   SlaveID == other.SlaveID &&
                   FunctionCode == other.FunctionCode &&
                   FirstAddress == other.FirstAddress &&
                   RequestCount == other.RequestCount &&
                   DataLenght == other.DataLenght &&
                   PortName == other.PortName &&
                   STOPBITS == other.STOPBITS &&
                   DATABIT == other.DATABIT &&
                   PARITY == other.PARITY &&
                   BAUREATE == other.BAUREATE &&
                   FrameLenght == other.FrameLenght &&
                   StartSingle == other.StartSingle &&
                   EndSingle == other.EndSingle &&
                   PackageLength == other.PackageLength &&
                   FirstAddr == other.FirstAddr &&
                   CheckSumIndex == other.CheckSumIndex &&
                   CheckSumType == other.CheckSumType &&
                   CheckStartIndex == other.CheckStartIndex &&
                   CheckEndIndex == other.CheckEndIndex &&
                   Standby1 == other.Standby1 &&
                   Standby2 == other.Standby2 &&
                   Standby3 == other.Standby3 &&
                   Standby4 == other.Standby4 &&
                   Standby5 == other.Standby5 &&
                   Standby6 == other.Standby6 &&
                   Standby7 == other.Standby7 &&
                   Standby8 == other.Standby8;
        }

        public override int GetHashCode()
        {
            var hashCode = -1141916821;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can2IP);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can2Port);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can1IP);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Can1Port);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SlaveID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FunctionCode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstAddress);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RequestCount);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DataLenght);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PortName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(STOPBITS);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DATABIT);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PARITY);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BAUREATE);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FrameLenght);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StartSingle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EndSingle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PackageLength);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstAddr);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckSumIndex);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckSumType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckStartIndex);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CheckEndIndex);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Standby1);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Standby2);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Standby3);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Standby4);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Standby5);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Standby6);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Standby7);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Standby8);
            return hashCode;
        }
    }
}
