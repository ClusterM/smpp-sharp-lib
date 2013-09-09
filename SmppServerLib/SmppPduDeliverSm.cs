using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduDeliverSm : SmppPduSmBase
    {
        override protected SmppCommandType GetCommandType()
        {
            return SmppCommandType.deliver_sm;
        }

        public SmppPduDeliverSm(string serviceType, byte sourceAddrTon, byte sourceAddrNpi, string sourceAddr, byte destAddrTon, byte destAddrNpi, string destAddr,
            byte esmClass, byte protocolId, byte priorityFlag, byte registeredDelivery, byte dataCoding, /*byte smLength,*/ string shortMessage, SmppOptionalParameter[] optionalParameters = null)
            : base(SmppCommandType.deliver_sm, serviceType, sourceAddrTon, sourceAddrNpi, sourceAddr, destAddrTon, destAddrNpi, destAddr, esmClass, protocolId,
            priorityFlag, "", "", registeredDelivery, 0, dataCoding, 0, /*smLength,*/ shortMessage, optionalParameters)
        {
        }

        public SmppPduDeliverSm(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduDeliverSm(byte[] bytes, uint length)
            : base(bytes, length)
        {
        }
    }
}
