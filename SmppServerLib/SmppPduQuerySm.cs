using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduQuerySm : SmppPdu
    {
        uint offsetMessageId, offsetSourceAddrTon, offsetSourceAddrNpi, offsetSourceAddr;

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
        
        public SmppPduQuerySm(string messageId, byte sourceAddrTon, byte sourceAddrNpi, string sourceAddr)
            : base(SmppCommandType.query_sm)
        {
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
        }

        public SmppPduQuerySm(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduQuerySm(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.query_sm) throw new Exception("Invaid command ID");
            offsetMessageId = 12;
            offsetSourceAddrTon = FindCStringEnd(offsetMessageId);
            offsetSourceAddrNpi = offsetSourceAddrTon + 1;
            offsetSourceAddr = offsetSourceAddrNpi + 1;
        }
    }
}
