using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public abstract class SmppPduBindBase : SmppPdu
    {
        abstract protected SmppCommandType GetCommandType();

        uint offsetSystemId, offsetPassword, offsetSystemType, offsetInterfaceVersion, offsetAddrTon, offsetAddrNpi, offsetAddrRange;

        public string SystemId
        {
            get { return ReadCString(offsetSystemId); }
        }
        public string Password
        {
            get { return ReadCString(offsetPassword); }
        }
        public string SystemType
        {
            get { return ReadCString(offsetSystemType); }
        }
        public byte InterfaceVersion
        {
            get { return ReadByte(offsetInterfaceVersion);}
        }
        public byte AddrTon
        {
            get { return ReadByte(offsetAddrTon); }
        }
        public byte AddrNpi
        {
            get { return ReadByte(offsetAddrNpi); }
        }
        public string AddrRange
        {
            get { return ReadCString(offsetAddrRange); }
        }

        public SmppPduBindBase(SmppCommandType commandType, string systemId, string password, string systemType, byte interfaceVersion, byte addrTon, byte addrNpi, string addrRange)
            : base(commandType)
        {
            if (commandType != GetCommandType()) throw new Exception("Invaid command ID");

            if (systemId.Length > 15) throw new ArgumentOutOfRangeException("systemId");
            offsetSystemId = CurrentOffset;
            WriteCString(systemId);

            if (password.Length > 8) throw new ArgumentOutOfRangeException("password");
            offsetPassword = CurrentOffset;
            WriteCString(password);

            if (systemType.Length > 12) throw new ArgumentOutOfRangeException("systemType");
            offsetSystemType = CurrentOffset;
            WriteCString(systemType);

            offsetInterfaceVersion = CurrentOffset;
            WriteByte(interfaceVersion);

            offsetAddrTon = CurrentOffset;
            WriteByte(addrTon);

            offsetAddrNpi = CurrentOffset;
            WriteByte(addrNpi);

            if (addrRange.Length > 40) throw new ArgumentOutOfRangeException("addressRange");
            offsetAddrRange = CurrentOffset;
            WriteCString(addrRange);
        }

        public SmppPduBindBase(byte[] bytes) : this(bytes, (uint)bytes.Length)
        {
        }
        public SmppPduBindBase(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != GetCommandType()) throw new Exception("Invaid command ID");
            offsetSystemId = 12;
            offsetPassword = FindCStringEnd(offsetSystemId);
            offsetSystemType = FindCStringEnd(offsetPassword);
            offsetInterfaceVersion = FindCStringEnd(offsetSystemType);
            offsetAddrTon = offsetInterfaceVersion + 1;
            offsetAddrNpi = offsetAddrTon + 1;
            offsetAddrRange = offsetAddrNpi + 1;
        }
    }
}
