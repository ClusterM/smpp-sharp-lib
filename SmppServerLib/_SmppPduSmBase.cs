using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public abstract class SmppPduSmBase : SmppPdu
    {
        abstract protected SmppCommandType GetCommandType();

        uint offsetServiceType, offsetSourceAddrTon, offsetSourceAddrNpi, offsetSourceAddr, offsetDestAddrTon, offsetDestAddrNpi, offsetDestAddr,
            offsetEsmClass, offsetProtocolId, offsetPriorityFlag, offsetScheduleDeliveryTime, offsetRegisteredDelivery, offsetValidityPeriod, offsetReplaceIfPresentFlag, offsetDataCoding,
            offsetSmDefaultMsgId, offsetSmLength, offsetShortMessage, offsetOptionalParameters;

        public string ServiceType
        {
            get { return ReadCString(offsetServiceType); }
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
        public byte EsmClass
        {
            get { return ReadByte(offsetEsmClass); }
        }
        public byte ProtocolId
        {
            get { return ReadByte(offsetProtocolId); }
        }
        public byte PriorityFlag
        {
            get { return ReadByte(offsetPriorityFlag); }
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
        public byte ReplaceIfPresentFlag
        {
            get { return ReadByte(offsetReplaceIfPresentFlag); }
        }
        public byte DataCoding
        {
            get { return ReadByte(offsetDataCoding); }
        }
        public Encoding DataEncoder
        {
            get
            {
                switch (DataCoding)
                {
                    case 0:
                        return Encoding.ASCII;
                    case 8:
                        return Encoding.BigEndianUnicode;
                    default:
                        throw new NotSupportedException(string.Format("Codepage {0} is not supported yet", DataCoding));
                }
            }
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
            get
            {
                if ((EsmClass & 64) == 0)
                    return ReadString(offsetShortMessage, SmLength, DataEncoder);
                byte udhiLength = ReadByte(offsetShortMessage);
                return ReadString(offsetShortMessage + udhiLength + 1, (uint)(SmLength - udhiLength - 1), DataEncoder); // UDHI
            }
        }
        public SmppOptionalParameter[] OptionalParameters
        {
            get { return ReadOptionalParameters(offsetOptionalParameters); }
        }
        public SmppUdhiParameter[] UdhiParameters
        {
            get
            {
                if ((EsmClass & 64) == 0) return new SmppUdhiParameter[0];
                byte udhiLength = ReadByte(offsetShortMessage);
                var result = new List<SmppUdhiParameter>();
                byte pos = 0;
                while (pos < udhiLength)
                {
                    byte tag = ReadByte(offsetShortMessage + 1 + pos);
                    byte len = ReadByte(offsetShortMessage + 1 + pos + 1);
                    var data = ReadBytes(offsetShortMessage + 1 + pos + 2, len);
                    result.Add(new SmppUdhiParameter(tag, len, data));
                    pos += (byte)(len + 2);
                }
                return result.ToArray();
            }
        }

        public SmppPduSmBase(SmppCommandType commandType, string serviceType, byte sourceAddrTon, byte sourceAddrNpi, string sourceAddr, byte destAddrTon, byte destAddrNpi, string destAddr,
            byte esmClass, byte protocolId, byte priorityFlag, string scheduleDeliveryTime, string validityPeriod, byte registeredDelivery, byte replaceIfPresentFlag,
            byte dataCoding, byte smDefaultMsgId, /*byte smLength,*/ string shortMessage, SmppOptionalParameter[] optionalParameters = null)
            : base(commandType)
        {
            if (commandType != GetCommandType()) throw new Exception("Invaid command ID");

            if (serviceType.Length > 5) throw new ArgumentOutOfRangeException("systemType");
            offsetServiceType = CurrentOffset;
            WriteCString(serviceType);

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
            WriteByte(destAddrNpi);

            if (destAddr.Length > 20) throw new ArgumentOutOfRangeException("destAddr");
            offsetDestAddr = CurrentOffset;
            WriteCString(destAddr);

            offsetEsmClass = CurrentOffset;
            WriteByte(esmClass);

            offsetProtocolId = CurrentOffset;
            WriteByte(protocolId);

            offsetPriorityFlag = CurrentOffset;
            WriteByte(priorityFlag);

            if (scheduleDeliveryTime.Length != 0 && scheduleDeliveryTime.Length != 16) throw new ArgumentOutOfRangeException("scheduleDeliveryTime");
            offsetScheduleDeliveryTime = CurrentOffset;
            WriteCString(scheduleDeliveryTime);

            if (validityPeriod.Length != 0 && validityPeriod.Length != 16) throw new ArgumentOutOfRangeException("validityPeriod");
            offsetValidityPeriod = CurrentOffset;
            WriteCString(validityPeriod);

            offsetRegisteredDelivery = CurrentOffset;
            WriteByte(registeredDelivery);

            offsetReplaceIfPresentFlag = CurrentOffset;
            WriteByte(replaceIfPresentFlag);

            offsetDataCoding = CurrentOffset;
            WriteByte(dataCoding);

            offsetSmDefaultMsgId = CurrentOffset;
            WriteByte(smDefaultMsgId);

            if (DataEncoder.GetByteCount(shortMessage) > 254) throw new ArgumentOutOfRangeException("smLength");
            offsetSmLength = CurrentOffset;
            WriteByte((byte)DataEncoder.GetByteCount(shortMessage));

            offsetShortMessage = CurrentOffset;
            WriteString(shortMessage, (byte)DataEncoder.GetByteCount(shortMessage), DataEncoder);

            offsetOptionalParameters = CurrentOffset;
            if (optionalParameters != null)
                WriteOptionalParameters(optionalParameters);
        }

        public SmppPduSmBase(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduSmBase(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != GetCommandType()) throw new Exception("Invaid command ID");
            offsetServiceType = 12;
            offsetSourceAddrTon = FindCStringEnd(offsetServiceType);
            offsetSourceAddrNpi = offsetSourceAddrTon + 1;
            offsetSourceAddr = offsetSourceAddrNpi + 1;
            offsetDestAddrTon = FindCStringEnd(offsetSourceAddr);
            offsetDestAddrNpi = offsetDestAddrTon + 1;
            offsetDestAddr = offsetDestAddrNpi + 1;
            offsetEsmClass = FindCStringEnd(offsetDestAddr);
            offsetProtocolId = offsetEsmClass + 1;
            offsetPriorityFlag = offsetProtocolId + 1;
            offsetScheduleDeliveryTime = offsetPriorityFlag + 1;
            offsetValidityPeriod = FindCStringEnd(offsetScheduleDeliveryTime);
            offsetRegisteredDelivery = FindCStringEnd(offsetValidityPeriod);
            offsetReplaceIfPresentFlag = offsetRegisteredDelivery + 1;
            offsetDataCoding = offsetReplaceIfPresentFlag + 1;
            offsetSmDefaultMsgId = offsetDataCoding + 1;
            offsetSmLength = offsetSmDefaultMsgId + 1;
            offsetShortMessage = offsetSmLength + 1;
            offsetOptionalParameters = offsetShortMessage + SmLength;
        }

        public override string ToString()
        {
            var options = new StringBuilder();
            foreach (var option in OptionalParameters)
                options.AppendFormat("\r\n{0}", option.ToString());
            foreach (var udhi in UdhiParameters)
                options.AppendFormat("\r\n{0}", udhi.ToString());
            return base.ToString() + string.Format("\r\nservice type: {0}, source_addr_ton: {1}, source_addr_npi: {2}, sounce_addr: {3}, dest_addr_ton: {4}, dest_addr_npi: {5}, destination_addr: {6}, length: {8}, message: {7}",
                ServiceType, SourceAddrTon, SourceAddrNpi, SourceAddr, DestAddrTon, DestAddrNpi, DestAddr, ShortMessage, SmLength)
                + options.ToString();
        }
    }
}
