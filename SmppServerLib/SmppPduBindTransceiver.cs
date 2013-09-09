using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduBindTransceiver : SmppPduBindBase
    {
        protected override SmppPdu.SmppCommandType GetCommandType()
        {
            return SmppCommandType.bind_transceiver;
        }

        public SmppPduBindTransceiver(string systemId, string password, string systemType, byte interfaceVersion, byte addrTon, byte addrNpi, string addrRange)
            : base(SmppCommandType.bind_transceiver, systemId, password, systemType, interfaceVersion, addrTon, addrNpi, addrRange)
        {
        }

        public SmppPduBindTransceiver(byte[] bytes)
            : this(bytes, (uint)bytes.Length)
        {
        }
        public SmppPduBindTransceiver(byte[] bytes, uint length)
            : base(bytes, length)
        {
        }
    }
}
