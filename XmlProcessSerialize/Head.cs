using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlProcessSerialize
{
    /// <summary>
    /// <creator>mark</creator>
    /// </summary>
    public class Head
    {

        private string _datagramId;
        [XmlElement("datagramId")]
        public string DatagramId
        {
            set
            {
                _datagramId = value;
            }
            get
            {
                return _datagramId;
            }
        }

        private string _priority;
        [XmlElement("priority")]
        public string Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private string _an;
        [XmlElement("an")]
        public string An
        {
            get { return _an; }
            set { _an = value; }
        }

        private string _fi;
        [XmlElement("fi")]
        public string Fi
        {
            get { return _fi; }
            set { _fi = value; }
        }

        private string _rcvAddress;
        [XmlElement("rcvAddress")]
        public string RcvAddress
        {
            get { return _rcvAddress; }
            set { _rcvAddress = value; }
        }

        private string _sndAddress;
        [XmlElement("sndAddress")]
        public string SndAddress
        {
            get { return _sndAddress; }
            set { _sndAddress = value; }
        }

        private string _bepTime;
        [XmlElement("bepTime")]
        public string BepTime
        {
            get { return _bepTime; }
            set { _bepTime = value; }
        }

        private string _smi;
        [XmlElement("smi")]
        public string Smi
        {
            get { return _smi; }
            set { _smi = value; }
        }

        private string _dsp;
        [XmlElement("dsp")]
        public string Dsp
        {
            get { return _dsp; }
            set { _dsp = value; }
        }

        private string _rgs;
        [XmlElement("rgs")]
        public string Rgs
        {
            get { return _rgs; }
            set { _rgs = value; }
        }

        private string _msn;
        [XmlElement("msn")]
        public string Msn
        {
            get { return _msn; }
            set { _msn = value; }
        }

        private string _datagramType;
        [XmlElement("datagramType")]
        public string DatagramType
        {
            get { return _datagramType; }
            set { _datagramType = value; }
        }

        private string _icao24;
        [XmlElement("icao24")]
        public string Icao24
        {
            get { return _icao24; }
            set { _icao24 = value; }
        }

        private string _callcode;
        [XmlElement("callcode")]
        public string Callcode
        {
            get { return _callcode; }
            set { _callcode = value; }
        }



    }
}
