using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduUnbind : SmppPdu
    {
        public SmppPduUnbind() : base(SmppCommandType.unbind) { }
        public SmppPduUnbind(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduUnbind(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.unbind) throw new Exception("Invaid command ID");
        }
    }
}
