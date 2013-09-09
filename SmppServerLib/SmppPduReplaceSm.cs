using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduReplaceSm : SmppPdu
    {
        uint offsetMessageId, offsetSourceAddrTon, offsetSourceAddrNpi, offsetSourceAddr, offsetScheduleDeliveryTime, 
            offsetRegisteredDelivery, offsetValidityPeriod, offsetSmDefaultMsgId, offsetSmLength, offsetShortMessage;

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
        public string ScheduleDeliveryTime
        {
            get { return ReadCString(offsetScheduleDeliveryTime); }
        }
        public string ValidityPeriod
        {
            get { return ReadCString(offsetValidityPeriod); }
        }
        public byte RegisteredDelivery
        {
            get { return ReadByte(offsetRegisteredDelivery); }
        }
        public byte SmDefaultMsgId
        {
            get { return ReadByte(offsetSmDefaultMsgId); }
        }
        public byte SmLength
        {
            get { return ReadByte(offsetSmLength); }
        }
        public string ShortMessage
        {
            get { return ReadString(offsetShortMessage, SmLength ); }
        }

        public SmppPduReplaceSm(string messageId, byte sourceAddrTon, byte sourceAddrNpi, string sourceAddr, string scheduleDeliveryTime, string validityPeriod, byte registeredDelivery, byte smDefaultMsgId, byte smLength, string shortMessage)
            : base(SmppCommandType.replace_sm)
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

            if (scheduleDeliveryTime.Length != 0 && scheduleDeliveryTime.Length != 16) throw new ArgumentOutOfRangeException("scheduleDeliveryTime");
            offsetScheduleDeliveryTime = CurrentOffset;
            WriteCString(scheduleDeliveryTime);

            if (validityPeriod.Length != 0 && validityPeriod.Length != 16) throw new ArgumentOutOfRangeException("validityPeriod");
            offsetValidityPeriod = CurrentOffset;
            WriteCString(validityPeriod);

            offsetRegisteredDelivery = CurrentOffset;
            WriteByte(registeredDelivery);

            offsetSmDefaultMsgId = CurrentOffset;
            WriteByte(smDefaultMsgId);

            // TODO: Codepage?
            offsetSmLength = CurrentOffset;
            WriteByte(smLength);

            offsetShortMessage = CurrentOffset;
            WriteString(shortMessage, smLength);
        }

        public SmppPduReplaceSm(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduReplaceSm(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.replace_sm) throw new Exception("Invaid command ID");
            offsetMessageId = 12;
            offsetSourceAddrTon = FindCStringEnd(offsetMessageId);
            offsetSourceAddrNpi = offsetSourceAddrTon + 1;
            offsetSourceAddr = offsetSourceAddrNpi + 1;
            offsetScheduleDeliveryTime = FindCStringEnd(offsetSourceAddr);
            offsetValidityPeriod = FindCStringEnd(offsetScheduleDeliveryTime);
            offsetRegisteredDelivery = FindCStringEnd(offsetValidityPeriod);
            offsetSmDefaultMsgId = offsetRegisteredDelivery + 1;
            offsetSmLength = offsetSmDefaultMsgId + 1;
            offsetShortMessage = offsetSmLength + 1;
        }
    }
}
