using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduReplaceSmResp : SmppPdu
    {
        public SmppPduReplaceSmResp(uint sequenceId, SmppCommandStatus status)
            : base(SmppCommandType.replace_sm_resp, status, sequenceId)
        {            
        }

        public SmppPduReplaceSmResp(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduReplaceSmResp(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.replace_sm_resp) throw new Exception("Invaid command ID");
        }
    }
}
