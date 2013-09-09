using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduCancelSmResp : SmppPdu
    {
        public SmppPduCancelSmResp(uint sequenceId, SmppCommandStatus status)
            : base(SmppCommandType.cancel_sm_resp, status, sequenceId)
        {            
        }

        public SmppPduCancelSmResp(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduCancelSmResp(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.cancel_sm_resp) throw new Exception("Invaid command ID");
        }
    }
}
