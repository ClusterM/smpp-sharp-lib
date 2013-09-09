using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduOutbind : SmppPdu
    {
        uint offsetSystemId, offsetPassword;

        public string SystemId
        {
            get { return ReadCString(offsetSystemId); }
        }
        public string Password
        {
            get { return ReadCString(offsetPassword); }
        }

        public SmppPduOutbind(string systemId, string password)
            : base(SmppCommandType.outbind)
        {
            if (systemId.Length > 15) throw new ArgumentOutOfRangeException("systemId");
            offsetSystemId = CurrentOffset;
            WriteCString(systemId);

            if (password.Length > 8) throw new ArgumentOutOfRangeException("password");
            offsetPassword = CurrentOffset;
            WriteCString(password);
        }

        public SmppPduOutbind(byte[] bytes) : this(bytes, (uint)bytes.Length)
        {
        }
        public SmppPduOutbind(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.outbind) throw new Exception("Invaid command ID");
            offsetSystemId = 12;
            offsetPassword = FindCStringEnd(offsetSystemId);
        }
    }
}
