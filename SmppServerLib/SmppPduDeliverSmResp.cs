using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduDeliverSmResp : SmppPduSmRespBase
    {
        override protected SmppCommandType GetCommandType()
        {
            return SmppCommandType.deliver_sm_resp;
        }

        public SmppPduDeliverSmResp(uint sequenceId, SmppCommandStatus status = SmppCommandStatus.ESME_ROK)
            : base(SmppCommandType.deliver_sm_resp, sequenceId, status, "")
        {
        }

        public SmppPduDeliverSmResp(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduDeliverSmResp(byte[] bytes, uint length)
            : base(bytes, length)
        {
        }
    }
}
