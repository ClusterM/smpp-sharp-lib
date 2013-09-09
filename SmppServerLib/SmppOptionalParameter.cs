using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flex.Cluster.Smpp
{
    public class SmppOptionalParameter
    {
        public enum ParameterTag : ushort
        {
            dest_addr_subunit = 0x0005, // GSM
            dest_network_type = 0x0006, // Generic
            dest_bearer_type = 0x0007, // Generic
            dest_telematics_id = 0x0008, // GSM
            source_addr_subunit = 0x000D, // GSM
            source_network_type = 0x000E, // Generic
            source_bearer_type = 0x000F, // Generic
            source_telematics_id = 0x0010, // GSM
            qos_time_to_live = 0x0017, // Generic
            payload_type = 0x0019, // Generic
            additional_status_info_text = 0x001D, // Generic
            receipted_message_id = 0x001E, // Generic
            ms_msg_wait_facilities = 0x0030, // GSM
            privacy_indicator = 0x0201, // CDMA, TDMA
            source_subaddress = 0x0202, // CDMA, TDMA
            dest_subaddress = 0x0203, // CDMA, TDMA
            user_message_reference = 0x0204, // 0x0204 Generic
            user_response_code = 0x0205, // CDMA, TDMA
            source_port = 0x020A, // Generic
            destination_port = 0x020B, // Generic
            sar_msg_ref_num = 0x020C, // Generic
            language_indicator = 0x020D, // CDMA, TDMA
            sar_total_segments = 0x020E, // Generic
            sar_segment_seqnum = 0x020F, // Generic
            SC_interface_version = 0x0210, // Generic
            callback_num_pres_ind = 0x0302, // TDMA
            callback_num_atag = 0x0303, // TDMA
            number_of_messages = 0x0304, // CDMA
            callback_num = 0x0381, // CDMA, TDMA, GSM, iDEN
            dpf_result = 0x0420, // Generic
            set_dpf = 0x0421, // Generic
            ms_availability_status = 0x0422, // Generic
            network_error_code = 0x0423, // Generic
            message_payload = 0x0424, // Generic
            delivery_failure_reason = 0x0425, // Generic
            more_messages_to_send = 0x0426, // GSM
            message_state = 0x0427, // Generic
            ussd_service_op = 0x0501, // GSM (USSD)
            display_time = 0x1201, // CDMA, TDMA
            sms_signal = 0x1203, // TDMA
            ms_validity = 0x1204, // CDMA, TDMA
            alert_on_message_delivery = 0x130C, // CDMA
            its_reply_type = 0x1380, // CDMA
            its_session_info = 0x1383 // CDMA
        }

        public readonly ParameterTag Tag;
        public readonly ushort Length;
        public readonly byte[] Value;

        public SmppOptionalParameter(ParameterTag tag, ushort length, byte[] value)
        {
            Tag = tag;
            Length = length;
            Value = value;
        }

        public override string ToString()
        {
            var data = new StringBuilder();
            foreach(var b in Value)
                data.AppendFormat("{0:X2} ",b);
            return string.Format("Option: {0} = {1}", Tag, data.ToString());
        }
    }
}
