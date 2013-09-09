using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduBindReceiverResp : SmppPduBindRespBase
    {
        protected override SmppPdu.SmppCommandType GetCommandType()
        {
            return SmppCommandType.bind_receiver_resp;
        }

        public SmppPduBindReceiverResp(uint sequenceId, SmppCommandStatus status, string systemId, SmppOptionalParameter[] optionalParameters = null)
            : base(SmppCommandType.bind_receiver_resp, sequenceId, status, systemId, optionalParameters)
        {
        }

        public SmppPduBindReceiverResp(byte[] bytes) : this(bytes, (uint)bytes.Length) {}
        public SmppPduBindReceiverResp(byte[] bytes, uint length)
            : base(bytes, length)
        {
        }
    }
}
