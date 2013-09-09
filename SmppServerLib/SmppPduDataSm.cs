using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduDataSm : SmppPdu
    {
        uint offsetServiceType, offsetSourceAddrTon, offsetSourceAddrNpi, offsetSourceAddr, offsetDestAddrTon, offsetDestAddrNpi, offsetDestAddr,
            offsetEsmClass, offsetRegisteredDelivery, offsetDataCoding, offsetOptionalParameters;

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
        public byte RegisteredDelivery
        {
            get { return ReadByte(offsetRegisteredDelivery); }
        }
        public byte DataCoding
        {
            get { return ReadByte(offsetDataCoding); }
        }
        public SmppOptionalParameter[] OptionalParameters
        {
            get { return ReadOptionalParameters(offsetOptionalParameters); }
        }

        public SmppPduDataSm(string serviceType, byte sourceAddrTon, byte sourceAddrNpi, string sourceAddr, byte destAddrTon, byte destAddrNpi, string destAddr,
            byte esmClass, byte registeredDelivery, byte dataCoding, SmppOptionalParameter[] optionalParameters = null)
            : base(SmppCommandType.data_sm)
        {
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

            offsetRegisteredDelivery = CurrentOffset;
            WriteByte(registeredDelivery);

            offsetDataCoding = CurrentOffset;
            WriteByte(dataCoding);

            offsetOptionalParameters = CurrentOffset;
            if (optionalParameters != null)
                WriteOptionalParameters(optionalParameters);
        }

        public SmppPduDataSm(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduDataSm(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.data_sm) throw new Exception("Invaid command ID");
            offsetServiceType = 12;
            offsetSourceAddrTon = FindCStringEnd(offsetServiceType);
            offsetSourceAddrNpi = offsetSourceAddrTon + 1;
            offsetSourceAddr = offsetSourceAddrNpi + 1;
            offsetDestAddrTon = FindCStringEnd(offsetSourceAddr);
            offsetDestAddrNpi = offsetDestAddrTon + 1;
            offsetDestAddr = offsetDestAddrNpi + 1;
            offsetEsmClass = FindCStringEnd(offsetDestAddr);
            offsetRegisteredDelivery = offsetEsmClass + 1;
            offsetDataCoding = offsetRegisteredDelivery + 1;
            offsetOptionalParameters = offsetDataCoding + 1;
        }
    }
}
