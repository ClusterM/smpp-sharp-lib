using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduBindTransceiverResp : SmppPduBindRespBase
    {
        protected override SmppPdu.SmppCommandType GetCommandType()
        {
            return SmppCommandType.bind_transceiver_resp;
        }

        public SmppPduBindTransceiverResp(uint sequenceId, SmppCommandStatus status, string systemId, SmppOptionalParameter[] optionalParameters = null)
            : base(SmppCommandType.bind_transceiver_resp, sequenceId, status, systemId, optionalParameters)
        {
        }

        public SmppPduBindTransceiverResp(byte[] bytes) : this(bytes, (uint)bytes.Length) {}
        public SmppPduBindTransceiverResp(byte[] bytes, uint length)
            : base(bytes, length)
        {
        }
    }
}
