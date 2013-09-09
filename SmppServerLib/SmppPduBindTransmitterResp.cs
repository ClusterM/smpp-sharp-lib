using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduBindTransmitterResp : SmppPduBindRespBase
    {
        protected override SmppPdu.SmppCommandType GetCommandType()
        {
            return SmppCommandType.bind_transmitter_resp;
        }

        public SmppPduBindTransmitterResp(uint sequenceId, SmppCommandStatus status, string systemId, SmppOptionalParameter[] optionalParameters = null)
            : base(SmppCommandType.bind_transmitter_resp, sequenceId, status, systemId, optionalParameters)
        {
        }

        public SmppPduBindTransmitterResp(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduBindTransmitterResp(byte[] bytes, uint length)
            : base(bytes, length)
        {
        }
    }
}
