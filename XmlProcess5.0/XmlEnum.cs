using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlProcess5._0
{
    enum ECommunication
    {
        CommReConnectTime,
        EnglishiName,
        ChineseName,
        CommErroTime,
        WriteTimeOut,
        ReadTimeOut,
        CommSpaceTime,
        ConfigFilePath,
        ID,
        WriteConfigPath
    }
    enum ECommunication_TCP
    {
        Can2IP,
        Can2Port,
        Can1IP,
        Can1Port,
        ID,
        StartSingle,
        EndSingle,
        PackageLength,
        FirstAddr,
        CheckSumIndex,
        CheckSumType,
        CheckStartIndex,
        CheckEndIndex,
        CheckSumLength
    }
    enum ECommunication_Modbus
    {
        PortName,
        STOPBITS,
        DATABIT,
        PARITY,
        BAUREATE,
        FrameLenght,
        FirstAddr,
        StartSingle
    }
    class XmlEnum
    {
    }
}
