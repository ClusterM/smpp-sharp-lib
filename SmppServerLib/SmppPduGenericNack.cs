using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduGenerickNack : SmppPdu
    {
        public SmppPduGenerickNack(uint sequenceId, SmppCommandStatus status)
            : base(SmppCommandType.generic_nack, status, sequenceId) { }
        public SmppPduGenerickNack(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduGenerickNack(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.generic_nack) throw new Exception("Invaid command ID");
        }
    }
}
