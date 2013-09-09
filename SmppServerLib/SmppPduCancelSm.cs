using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduCancelSm : SmppPdu
    {
        uint offsetServiceType, offsetMessageId, offsetSourceAddrTon, offsetSourceAddrNpi, offsetSourceAddr,
            offsetDestAddrTon, offsetDestAddrNpi, offsetDestAddr;

        public string ServiceType
        {
            get { return ReadCString(offsetServiceType); }
        }
        public string MessageId
        {
            get { return ReadCString(offsetMessageId); }
        }
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
        public byte DestAddrTon
        {
            get { return ReadByte(offsetDestAddrTon); }
        }
        public byte DestAddrNpi
        {
            get { return ReadByte(offsetDestAddrNpi); }
        }
        public string DestAddr
        {
            get { return ReadCString(offsetDestAddr); }
        }

        public SmppPduCancelSm(string serviceType, string messageId, byte sourceAddrTon, byte sourceAddrNpi, string sourceAddr,
            byte destAddrTon, byte destAddrNpi, string destAddr)
            : base(SmppCommandType.cancel_sm)
        {
            if (serviceType.Length > 5) throw new ArgumentOutOfRangeException("serviceType");
            offsetServiceType = CurrentOffset;
            WriteCString(serviceType);

            if (messageId.Length > 64) throw new ArgumentOutOfRangeException("messageId");
            offsetMessageId = CurrentOffset;
            WriteCString(messageId);

            offsetSourceAddrTon = CurrentOffset;
            WriteByte(sourceAddrTon);

            offsetSourceAddrNpi = CurrentOffset;
            WriteByte(sourceAddrNpi);

            if (sourceAddr.Length > 20) throw new ArgumentOutOfRangeException("sourceAddr");
            offsetSourceAddr = CurrentOffset;
            WriteCString(sourceAddr);

            offsetDestAddrTon = CurrentOffset;
            WriteByte(destAddrTon);

            offsetDestAddrNpi = CurrentOffset;
            WriteByte(destAddrTon);

            if (destAddr.Length > 20) throw new ArgumentOutOfRangeException("destAddr");
            offsetDestAddr = CurrentOffset;
            WriteCString(destAddr);
        }

        public SmppPduCancelSm(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduCancelSm(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.cancel_sm) throw new Exception("Invaid command ID");
            offsetServiceType = 12;
            offsetMessageId = FindCStringEnd(offsetServiceType);
            offsetSourceAddrTon = FindCStringEnd(offsetMessageId);
            offsetSourceAddrNpi = offsetSourceAddrTon + 1;
            offsetSourceAddr = offsetSourceAddrNpi + 1;
            offsetDestAddrTon = FindCStringEnd(offsetSourceAddr);
            offsetDestAddrNpi = offsetDestAddrTon + 1;
            offsetDestAddr = offsetDestAddrNpi + 1;
        }
    }
}
