using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduBindReceiver : SmppPduBindBase
    {
        protected override SmppPdu.SmppCommandType GetCommandType()
        {
            return SmppCommandType.bind_receiver;
        }

        public SmppPduBindReceiver(string systemId, string password, string systemType, byte interfaceVersion, byte addrTon, byte addrNpi, string addrRange)
            : base(SmppCommandType.bind_receiver, systemId, password, systemType, interfaceVersion, addrTon, addrNpi, addrRange)
        {
        }

        public SmppPduBindReceiver(byte[] bytes)
            : this(bytes, (uint)bytes.Length)
        {
        }
        public SmppPduBindReceiver(byte[] bytes, uint length)
            : base(bytes, length)
        {
        }
    }
}
