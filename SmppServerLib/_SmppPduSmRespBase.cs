using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public abstract class SmppPduSmRespBase : SmppPdu
    {
        abstract protected SmppCommandType GetCommandType();

        protected uint offsetMessageId;

        public string MessageId
        {
            get { return ReadCString(offsetMessageId); }
        }

        public SmppPduSmRespBase(SmppCommandType commandType, uint sequenceId, SmppCommandStatus status, string messageId)
            : base(commandType, status, sequenceId)
        {
            if (commandType != GetCommandType()) throw new Exception("Invaid command ID");

            if (messageId.Length > 64) throw new ArgumentOutOfRangeException("messageId");
            offsetMessageId = CurrentOffset;
            WriteCString(messageId);
        }

        public SmppPduSmRespBase(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduSmRespBase(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != GetCommandType()) throw new Exception("Invaid command ID");
            offsetMessageId = 12;
        }
    }
}
