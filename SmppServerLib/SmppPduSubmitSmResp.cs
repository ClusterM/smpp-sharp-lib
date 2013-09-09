using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduSubmitSmResp : SmppPduSmRespBase
    {
        override protected SmppCommandType GetCommandType()
        {
            return SmppCommandType.submit_sm_resp;
        }

        public SmppPduSubmitSmResp(uint sequenceId, SmppCommandStatus status, string messageId)
            : base(SmppCommandType.submit_sm_resp, sequenceId, status, messageId)
        {
        }

        public SmppPduSubmitSmResp(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduSubmitSmResp(byte[] bytes, uint length)
            : base(bytes, length)
        {
        }
    }
}
