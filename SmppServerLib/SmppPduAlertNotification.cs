using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduAlertNotification : SmppPdu
    {
        uint offsetSourceAddrTon, offsetSourceAddrNpi, offsetSourceAddr, offsetEsmeAddrTon, offsetEsmeAddrNpi, offsetEsmeAddr, offsetOptionalParameters;

        public byte SourceAddrTon
        {
            get { return ReadByte(offsetSourceAddrTon); }
        }
        public byte SourceAddrNpi
        {
            get { return ReadByte(offsetSourceAddrNpi); }
        }
        public string SourceAddr
        {
            get { return ReadCString(offsetSourceAddr); }
        }
        public byte EsmeAddrTon
        {
            get { return ReadByte(offsetEsmeAddrTon); }
        }
        public byte EsmeAddrNpi
        {
            get { return ReadByte(offsetEsmeAddrNpi); }
        }
        public string EsmeAddr
        {
            get { return ReadCString(offsetEsmeAddr); }
        }
        public SmppOptionalParameter[] OptionalParameters
        {
            get { return ReadOptionalParameters(offsetOptionalParameters); }
        }

        public SmppPduAlertNotification(byte sourceAddrTon, byte sourceAddrNpi, string sourceAddr, byte esmeAddrTon, byte esmeAddrNpi, string esmeAddr, SmppOptionalParameter[] optionalParameters = null)
            : base(SmppCommandType.alert_notification)
        {
            offsetSourceAddrTon = CurrentOffset;
            WriteByte(sourceAddrTon);

            offsetSourceAddrNpi = CurrentOffset;
            WriteByte(sourceAddrNpi);

            if (sourceAddr.Length > 64) throw new ArgumentOutOfRangeException("sourceAddr");
            offsetSourceAddr = CurrentOffset;
            WriteCString(sourceAddr);

            offsetEsmeAddrTon = CurrentOffset;
            WriteByte(sourceAddrTon);

            offsetEsmeAddrNpi = CurrentOffset;
            WriteByte(sourceAddrNpi);

            if (esmeAddr.Length > 64) throw new ArgumentOutOfRangeException("esmeAddr");
            offsetEsmeAddr = CurrentOffset;
            WriteCString(esmeAddr);

            offsetOptionalParameters = CurrentOffset;
            if (optionalParameters != null)
                WriteOptionalParameters(optionalParameters);
        }

        public SmppPduAlertNotification(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduAlertNotification(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.alert_notification) throw new Exception("Invaid command ID");
            offsetSourceAddrTon = 12;
            offsetSourceAddrNpi = offsetSourceAddrTon + 1;
            offsetSourceAddr = offsetSourceAddrNpi + 1;
            offsetEsmeAddrTon = FindCStringEnd(offsetSourceAddr);
            offsetEsmeAddrNpi = offsetEsmeAddrTon + 1;
            offsetEsmeAddr = offsetEsmeAddrNpi + 1;
            offsetOptionalParameters = FindCStringEnd(offsetEsmeAddr);
        }
    }
}
