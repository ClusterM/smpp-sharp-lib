using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduEnquireLinkResp : SmppPdu
    {
        public SmppPduEnquireLinkResp(uint sequenceId, SmppCommandStatus status = SmppCommandStatus.ESME_ROK)
            : base(SmppCommandType.enquire_link_resp, status, sequenceId)
        {
        }

        public SmppPduEnquireLinkResp(byte[] bytes) : this(bytes, (uint)bytes.Length) {}
        public SmppPduEnquireLinkResp(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.enquire_link_resp) throw new Exception("Invaid command ID");
        }
    }
}
