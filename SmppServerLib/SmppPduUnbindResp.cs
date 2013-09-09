using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduUnbindResp : SmppPdu
    {
        public SmppPduUnbindResp(uint sequenceId, SmppCommandStatus status) 
            : base(SmppCommandType.unbind_resp, status, sequenceId) { }
        public SmppPduUnbindResp(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduUnbindResp(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.unbind_resp) throw new Exception("Invaid command ID");
        }
    }
}
