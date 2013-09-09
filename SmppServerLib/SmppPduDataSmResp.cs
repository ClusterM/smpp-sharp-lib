using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduDataSmResp : SmppPduSmRespBase
    {
        uint offsetOptionalParameters;

        override protected SmppCommandType GetCommandType()
        {
            return SmppCommandType.data_sm_resp;
        }
        public SmppOptionalParameter[] OptionalParameters
        {
            get { return ReadOptionalParameters(offsetOptionalParameters); }
        }

        public SmppPduDataSmResp(uint sequenceId, SmppCommandStatus status, string messageId, SmppOptionalParameter[] optionalParameters = null)
            : base(SmppCommandType.data_sm_resp, sequenceId, status, messageId)
        {
            offsetOptionalParameters = CurrentOffset;
            if (optionalParameters != null)
                WriteOptionalParameters(optionalParameters);
        }

        public SmppPduDataSmResp(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduDataSmResp(byte[] bytes, uint length)
            : base(bytes, length)
        {
            offsetOptionalParameters = FindCStringEnd(offsetMessageId);
        }
    }
}
