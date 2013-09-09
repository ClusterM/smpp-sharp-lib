using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public abstract class SmppPduBindRespBase : SmppPdu
    {
        abstract protected SmppCommandType GetCommandType();

        uint offsetSystemId, offsetOptionalParameters;

        public string SystemId
        {
            get { return ReadCString(offsetSystemId); }
        }
        public SmppOptionalParameter[] OptionalParameters
        {
            get { return ReadOptionalParameters(offsetOptionalParameters); }
        }

        public SmppPduBindRespBase(SmppCommandType commandType, uint sequenceId, SmppCommandStatus status, string systemId, SmppOptionalParameter[] optionalParameters = null)
            : base(commandType, status, sequenceId)
        {
            if (commandType != GetCommandType()) throw new Exception("Invaid command ID");

            if (systemId.Length > 15) throw new ArgumentOutOfRangeException("systemId");
            offsetSystemId = CurrentOffset;
            WriteCString(systemId);

            offsetOptionalParameters = CurrentOffset;
            if (optionalParameters != null)
                WriteOptionalParameters(optionalParameters);
        }

        public SmppPduBindRespBase(byte[] bytes) : this(bytes, (uint)bytes.Length) {}
        public SmppPduBindRespBase(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != GetCommandType()) throw new Exception("Invaid command ID");
            offsetSystemId = 12;
            offsetOptionalParameters = FindCStringEnd(offsetSystemId);
        }
    }
}
