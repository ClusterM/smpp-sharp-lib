using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduEnquireLink : SmppPdu
    {
        public SmppPduEnquireLink()
            : base(SmppCommandType.enquire_link)
        {
        }

        public SmppPduEnquireLink(byte[] bytes) : this(bytes, (uint)bytes.Length) {}
        public SmppPduEnquireLink(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.enquire_link) throw new Exception("Invaid command ID");
        }
    }
}
