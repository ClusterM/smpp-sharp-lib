using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppPdu
    {
        static uint currentSeqNumber = 1;
        const uint offsetCommandId = 0;
        const uint offsetStatus = 4;
        const uint offsetSequenceNumber = 8;

        public uint Length
        {
            get { return (uint)(data.Length + 4); }
        }
        protected uint CurrentOffset
        {
            get { return (uint)(data.Length); }
        }
        public SmppCommandType CommandId
        {
            get { return (SmppCommandType)ReadInteger(offsetCommandId); }
        }
        public SmppCommandStatus Status
        {
            get { return (SmppCommandStatus)ReadInteger(offsetStatus); }
        }
        public uint SequenceNumber
        {
            get { return ReadInteger(offsetSequenceNumber); }
        }
        MemoryStream data;

        public enum SmppCommandType : uint
        {
            generic_nack = 0x80000000,
            bind_receiver = 0x00000001,
            bind_receiver_resp = 0x80000001,
            bind_transmitter = 0x00000002,
            bind_transmitter_resp = 0x80000002,
            query_sm = 0x00000003,
            query_sm_resp = 0x80000003,
            submit_sm = 0x00000004,
            submit_sm_resp = 0x80000004,
            deliver_sm = 0x00000005,
            deliver_sm_resp = 0x80000005,
            unbind = 0x00000006,
            unbind_resp = 0x80000006,
            replace_sm = 0x00000007,
            replace_sm_resp = 0x80000007,
            cancel_sm = 0x00000008,
            cancel_sm_resp = 0x80000008,
            bind_transceiver = 0x00000009,
            bind_transceiver_resp = 0x80000009,
            outbind = 0x0000000B,
            enquire_link = 0x00000015,
            enquire_link_resp = 0x80000015,
            submit_multi = 0x00000021,
            submit_multi_resp = 0x80000021,
            alert_notification = 0x00000102,
            data_sm = 0x00000103,
            data_sm_resp = 0x80000103
        };
        public enum SmppCommandStatus : uint
        {
            ESME_ROK = 0x00000000, // No Error
            ESME_RINVMSGLEN = 0x00000001, // Message Length is invalid
            ESME_RINVCMDLEN = 0x00000002, // Command Length is invalid
            ESME_RINVCMDID = 0x00000003, //  Invalid Command ID
            ESME_RINVBNDSTS = 0x00000004, // Incorrect BIND Status for given command
            ESME_RALYBND = 0x00000005, // ESME Already in Bound State
            ESME_RINVPRTFLG = 0x00000006, // Invalid Priority Flag
            ESME_RINVREGDLVFLG = 0x00000007, // Invalid Registered Delivery Flag
            ESME_RSYSERR = 0x00000008, // System Error
            ESME_RINVSRCADR = 0x0000000A, // Invalid Source Address
            ESME_RINVDSTADR = 0x0000000B, // Invalid Dest Addr
            ESME_RINVMSGID = 0x0000000C, // Message ID is invalid
            ESME_RBINDFAIL = 0x0000000D, // Bind Failed
            ESME_RINVPASWD = 0x0000000E, // Invalid Password
            ESME_RINVSYSID = 0x0000000F, // Invalid System ID
            ESME_RCANCELFAIL = 0x00000011, // 0x00000011 Cancel SM Failed
            ESME_RREPLACEFAIL = 0x00000013, // Replace SM Failed
            ESME_RMSGQFUL = 0x00000014, // Message Queue Full
            ESME_RINVSERTYP = 0x00000015, // Invalid Service Type
            ESME_RINVNUMDESTS = 0x00000033, // Invalid number of destinations
            ESME_RINVDLNAME = 0x00000034, // Invalid Distribution List name
            ESME_RINVDESTFLAG = 0x00000040, // Destination flag is invalid
            ESME_RINVSUBREP = 0x00000042, // Invalid ‘submit with replace’ request
            ESME_RINVESMCLASS = 0x00000043, // Invalid esm_classfield data
            ESME_RCNTSUBDL = 0x00000044, // Cannot Submit to Distribution List 
            ESME_RSUBMITFAIL = 0x00000045, // submit_smor submit_multi failed
            ESME_RINVSRCTON = 0x00000048, // Invalid Source address TON 
            ESME_RINVSRCNPI = 0x00000049, // Invalid Source address NPI 
            ESME_RINVDSTTON = 0x00000050, // Invalid Destination address TON
            ESME_RINVDSTNPI = 0x00000051, // Invalid Destination address NPI
            ESME_RINVSYSTYP = 0x00000053, // Invalid system_typefield
            ESME_RINVREPFLAG = 0x00000054, // Invalid replace_if_present flag
            ESME_RINVNUMMSGS = 0x00000055, // Invalid number of messages
            ESME_RTHROTTLED = 0x00000058, // Throttling error (ESME has exceeded allowed message limits)
            ESME_RINVSCHED = 0x00000061, // Invalid Scheduled Delivery Time
            ESME_RINVEXPIRY = 0x00000062, // Invalid message validity period (Expiry time)
            ESME_RINVDFTMSGID = 0x00000063, // Predefined Message Invalid or Not Found
            ESME_RX_T_APPN = 0x00000064, // ESME Receiver Temporary App Error Code
            ESME_RX_P_APPN = 0x00000065, // ESME Receiver Permanent App Error Code
            ESME_RX_R_APPN = 0x00000066, // ESME Receiver Reject Message Error Code
            ESME_RQUERYFAIL = 0x00000067, // query_smrequest failed
            ESME_RINVOPTPARSTREAM = 0x000000C0, // Error in the optional part of the PDU Body.
            ESME_ROPTPARNOTALLWD = 0x000000C1, // Optional Parameter not allowed
            ESME_RINVPARLEN = 0x000000C2, // Invalid Parameter Length.
            ESME_RMISSINGOPTPARAM = 0x000000C3, // Expected Optional Parameter missing
            ESME_RINVOPTPARAMVAL = 0x000000C4, // Invalid Optional Parameter Value
            ESME_RDELIVERYFAILURE = 0x000000FE, // Delivery Failure (used for data_sm_resp)
            ESME_RUNKNOWNERR = 0x000000FF // Unknown Error
        }

        public byte[] GetData()
        {
            var result = new byte[Length];
            result[0] = (byte)((Length >> 24) & 0xFF);
            result[1] = (byte)((Length >> 16) & 0xFF);
            result[2] = (byte)((Length >> 8) & 0xFF);
            result[3] = (byte)(Length & 0xFF);
            var buffer = data.GetBuffer();
            Array.Copy(buffer, 0, result, 4, Length - 4);
            return result;
        }

        protected void WriteInteger(uint v)
        {
            data.Seek(0, SeekOrigin.End);
            data.WriteByte((byte)((v >> 24) & 0xFF));
            data.WriteByte((byte)((v >> 16) & 0xFF));
            data.WriteByte((byte)((v >> 8) & 0xFF));
            data.WriteByte((byte)(v & 0xFF));
        }
        protected uint ReadInteger(uint offset)
        {
            uint result = 0;
            var buffer = data.GetBuffer();
            for (int c = 0; c < 4; c++)
            {
                var b = buffer[offset + c];
                result <<= 8;
                result |= (byte)b;
            }
            return result;
        }
        protected void WriteShort(ushort v)
        {
            data.Seek(0, SeekOrigin.End);
            data.WriteByte((byte)((v >> 8) & 0xFF));
            data.WriteByte((byte)(v & 0xFF));
        }
        protected ushort ReadShort(uint offset)
        {
            ushort result = 0;
            var buffer = data.GetBuffer();
            for (int c = 0; c < 2; c++)
            {
                var b = buffer[offset + c];
                result <<= 8;
                result |= (byte)b;
            }
            return result;
        }
        protected void WriteByte(byte v)
        {
            data.Seek(0, SeekOrigin.End);
            data.WriteByte(v);
        }
        protected byte ReadByte(uint offset)
        {
            var buffer = data.GetBuffer();
            return buffer[offset];
        }
        protected byte[] ReadBytes(uint offset, int len)
        {
            var buffer = data.GetBuffer();
            var result = new byte[len];
            Array.Copy(buffer, offset, result, 0, len);
            return result;
        }
        protected void WriteCString(string v, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.ASCII;
            var bytes = encoding.GetBytes(v);
            data.Seek(0, SeekOrigin.End);
            data.Write(bytes, 0, bytes.Length);
            data.WriteByte(0);
        }
        protected string ReadCString(uint offset, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.ASCII;
            var buffer = data.GetBuffer();
            int len = 0;
            while (buffer[offset + len] != 0) len++;
            return encoding.GetString(buffer, (int)offset, len);
        }
        protected uint FindCStringEnd(uint offset)
        {
            var buffer = data.GetBuffer();
            uint result = offset;
            while (buffer[result] != 0) result++;
            return result + 1;
        }
        protected void WriteString(string v, uint length, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.ASCII;
            var bytes = encoding.GetBytes(v);
            data.Seek(0, SeekOrigin.End);
            data.Write(bytes, 0, (int)length);
        }
        protected string ReadString(uint offset, uint length, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.ASCII;
            var buffer = data.GetBuffer();
            return encoding.GetString(buffer, (int)offset, (int)length);
        }
        protected void WriteOptionalParameters(SmppOptionalParameter[] parameters)
        {
            foreach (var parameter in parameters)
            {
                WriteShort((ushort)parameter.Tag);
                WriteShort((ushort)parameter.Length);
                data.Write(parameter.Value, 0, parameter.Length);
            }
        }
        protected SmppOptionalParameter[] ReadOptionalParameters(uint offset)
        {
            var parameters = new List<SmppOptionalParameter>();
            uint pos = offset;
            var buffer = data.GetBuffer();
            while (pos + 3 < data.Length)
            {
                ushort tag = ReadShort(pos);
                pos += 2;
                ushort valueLength = ReadShort(pos);
                pos += 2;
                var value = new byte[valueLength];
                Array.Copy(buffer, pos, value, 0, valueLength);
                pos += valueLength;
                parameters.Add(new SmppOptionalParameter((SmppOptionalParameter.ParameterTag)tag, valueLength, value));
            }
            return parameters.ToArray();
        }

        protected SmppPdu(SmppCommandType commandType, SmppCommandStatus commandStatus = 0, uint seqNumber = 0)
        {
            data = new MemoryStream();
            WriteInteger((uint)commandType);
            WriteInteger((uint)commandStatus);
            WriteInteger(seqNumber == 0 ? currentSeqNumber++ : seqNumber);
            if (currentSeqNumber > int.MaxValue) currentSeqNumber = 1;
        }
        public SmppPdu(byte[] bytes) : this(bytes, (uint)bytes.Length) { }
        public SmppPdu(byte[] bytes, uint length)
        {
            data = new MemoryStream();
            data.Write(bytes, 4, (int)length - 4);
        }

        public static SmppPdu CreateFromBytes(byte[] bytes)
        {
            return CreateFromBytes(bytes, (uint)bytes.Length);
        }
        public static SmppPdu CreateFromBytes(byte[] bytes, uint length)
        {
            var pdu = new SmppPdu(bytes, length);
            switch (pdu.CommandId)
            {
                case SmppCommandType.bind_receiver:
                    return new SmppPduBindReceiver(bytes, length);
                case SmppCommandType.bind_receiver_resp:
                    return new SmppPduBindReceiverResp(bytes, length);
                case SmppCommandType.bind_transceiver:
                    return new SmppPduBindTransceiver(bytes, length);
                case SmppCommandType.bind_transceiver_resp:
                    return new SmppPduBindTransceiverResp(bytes, length);
                case SmppCommandType.bind_transmitter:
                    return new SmppPduBindTransmitter(bytes, length);
                case SmppCommandType.bind_transmitter_resp:
                    return new SmppPduBindTransmitterResp(bytes, length);
                case SmppCommandType.outbind:
                    return new SmppPduOutbind(bytes, length);
                case SmppCommandType.unbind:
                    return new SmppPduUnbind(bytes, length);
                case SmppCommandType.unbind_resp:
                    return new SmppPduUnbindResp(bytes, length);
                case SmppCommandType.enquire_link:
                    return new SmppPduEnquireLink(bytes, length);
                case SmppCommandType.enquire_link_resp:
                    return new SmppPduEnquireLinkResp(bytes, length);
                case SmppCommandType.generic_nack:
                    return new SmppPduGenerickNack(bytes, length);
                case SmppCommandType.deliver_sm:
                    return new SmppPduDeliverSm(bytes, length);
                case SmppCommandType.submit_sm:
                    return new SmppPduSubmitSm(bytes, length);
                case SmppCommandType.deliver_sm_resp:
                    return new SmppPduDeliverSmResp(bytes, length);
                case SmppCommandType.submit_sm_resp:
                    return new SmppPduSubmitSmResp(bytes, length);
                case SmppCommandType.data_sm:
                    return new SmppPduDataSm(bytes, length);
                case SmppCommandType.data_sm_resp:
                    return new SmppPduDataSmResp(bytes, length);
                case SmppCommandType.query_sm:
                    return new SmppPduQuerySm(bytes, length);
                case SmppCommandType.query_sm_resp:
                    return new SmppPduQuerySmResp(bytes, length);
                case SmppCommandType.cancel_sm:
                    return new SmppPduCancelSm(bytes, length);
                case SmppCommandType.cancel_sm_resp:
                    return new SmppPduCancelSmResp(bytes, length);
                case SmppCommandType.replace_sm:
                    return new SmppPduReplaceSm(bytes, length);
                case SmppCommandType.replace_sm_resp:
                    return new SmppPduReplaceSmResp(bytes, length);
                case SmppCommandType.alert_notification:
                    return new SmppPduAlertNotification(bytes, length);
            }
            throw new NotSupportedException(string.Format("Command {0} is not supported", pdu.CommandId));
        }

        public override string ToString()
        {
            var d = GetData();
            var dStr = new StringBuilder();
            foreach (var b in d)
                dStr.AppendFormat("{0:X2} ", b);
            return string.Format("Command: {0}, status: {1}, seq: {2}, length: {3}, data: {4}", CommandId, Status, SequenceNumber, Length, dStr.ToString());
        }
    }
}
