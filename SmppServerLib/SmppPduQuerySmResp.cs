using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPduQuerySmResp : SmppPdu
    {
        uint offsetMessageId, offsetFinalDate, offsetMessageState, offsetErrorCode;

        public string MessageId
        {
            get { return ReadCString(offsetMessageId); }
        }
        public string FinalDate
        {
            get { return ReadCString(offsetFinalDate); }
        }
        public byte MessageState
        {
            get { return ReadByte(offsetMessageState); }
        }
        public byte ErrorCode
        {
            get { return ReadByte(offsetErrorCode); }
        }
        
        public SmppPduQuerySmResp(uint sequenceId, SmppCommandStatus status, string messageId, string finalDate, byte messageState, byte errorCode)
            : base(SmppCommandType.query_sm_resp, status, sequenceId)
        {
            if (messageId.Length > 64) throw new ArgumentOutOfRangeException("messageId");
            offsetMessageId = CurrentOffset;
            WriteCString(messageId);

            if (finalDate.Length != 0 && finalDate.Length != 16) throw new ArgumentOutOfRangeException("finalDate");
            offsetFinalDate = CurrentOffset;
            WriteCString(finalDate);

            offsetMessageState = CurrentOffset;
            WriteByte(messageState);

            offsetErrorCode = CurrentOffset;
            WriteByte(errorCode);
        }

        public SmppPduQuerySmResp(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPduQuerySmResp(byte[] bytes, uint length)
            : base(bytes, length)
        {
            if (CommandId != SmppCommandType.query_sm_resp) throw new Exception("Invaid command ID");
            offsetMessageId = 12;
            offsetFinalDate = FindCStringEnd(offsetMessageId);
            offsetMessageState = FindCStringEnd(offsetFinalDate);
            offsetErrorCode = offsetMessageState+1;
        }
    }
}
