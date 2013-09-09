using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduSubmitSm : SmppPduSmBase
    {
        override protected SmppCommandType GetCommandType()
        {
            return SmppCommandType.submit_sm;
        }

        public SmppPduSubmitSm(string serviceType, byte sourceAddrTon, byte sourceAddrNpi, string sourceAddr, byte destAddrTon, byte destAddrNpi, string destAddr,
            byte esmClass, byte protocolId, byte priorityFlag, string scheduleDeliveryTime, string validityPeriod, byte registeredDelivery, byte replaceIfPresentFlag,
            byte dataCoding, byte smDefaultMsgId, /*byte smLength,*/ string shortMessage, SmppOptionalParameter[] optionalParameters = null)
            : base(SmppCommandType.submit_sm, serviceType, sourceAddrTon, sourceAddrNpi, sourceAddr, destAddrTon, destAddrNpi, destAddr, esmClass, protocolId,
            priorityFlag, scheduleDeliveryTime, validityPeriod, registeredDelivery, replaceIfPresentFlag, dataCoding, smDefaultMsgId, /*smLength,*/ shortMessage, 
            optionalParameters)
        {
        }

        public SmppPduSubmitSm(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduSubmitSm(byte[] bytes, uint length)
            : base(bytes, length)
        {
        }
    }
}
