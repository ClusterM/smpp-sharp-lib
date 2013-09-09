using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduBindTransmitter : SmppPduBindBase
    {
        protected override SmppPdu.SmppCommandType GetCommandType()
        {
            return SmppCommandType.bind_transmitter;
        }

        public SmppPduBindTransmitter(string systemId, string password, string systemType, byte interfaceVersion, byte addrTon, byte addrNpi, string addrRange)
            : base(SmppCommandType.bind_transmitter, systemId, password, systemType, interfaceVersion, addrTon, addrNpi, addrRange)
        {
        }

        public SmppPduBindTransmitter(byte[] bytes)
            : this(bytes, (uint)bytes.Length)
        {
        }
        public SmppPduBindTransmitter(byte[] bytes, uint length)
            : base(bytes, length)
        {
        }
    }
}
