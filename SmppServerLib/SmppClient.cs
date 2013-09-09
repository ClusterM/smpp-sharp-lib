using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Flex.Cluster.Smpp
{
    public class SmppClient : IDisposable
    {
        TcpClient tcpClient;
        Thread receiveThread;
        MemoryStream data;
        uint length = 0;

        public bool Connected
        {
            get { return tcpClient != null && tcpClient.Connected; }
        }
        public delegate void OnConnectedDelegate(SmppClient sender);
        public event OnConnectedDelegate OnConnected = delegate { };
        public delegate void OnDisconnectedDelegate(SmppClient sender);
        public event OnDisconnectedDelegate OnDisconnected = delegate { };
        public delegate void OnSmppPduReceivedDelegate(SmppClient sender, SmppPdu pdu);
        public event OnSmppPduReceivedDelegate OnSmppPduReceived = delegate { };
        public delegate void OnBindTransmitterReceivedDelegate(SmppClient sender, SmppPduBindTransmitter pdu);
        public event OnBindTransmitterReceivedDelegate OnBindTransmitterReceived = delegate { };
        public delegate void OnBindTransmitterRespReceivedDelegate(SmppClient sender, SmppPduBindTransmitterResp pdu);
        public event OnBindTransmitterRespReceivedDelegate OnBindTransmitterRespReceived = delegate { };
        public delegate void OnBindReceiverReceivedDelegate(SmppClient sender, SmppPduBindReceiver pdu);
        public event OnBindReceiverReceivedDelegate OnBindReceiverReceived = delegate { };
        public delegate void OnBindReceiverRespReceivedDelegate(SmppClient sender, SmppPduBindReceiverResp pdu);
        public event OnBindReceiverRespReceivedDelegate OnBindReceiverRespReceived = delegate { };
        public delegate void OnBindTransceiverReceivedDelegate(SmppClient sender, SmppPduBindTransceiver pdu);
        public event OnBindTransceiverReceivedDelegate OnBindTransceiverReceived = delegate { };
        public delegate void OnBindTransceiverRespReceivedDelegate(SmppClient sender, SmppPduBindTransceiverResp pdu);
        public event OnBindTransceiverRespReceivedDelegate OnBindTransceiverRespReceived = delegate { };
        public delegate void OnUnbindReceivedDelegate(SmppClient sender, SmppPduUnbind pdu);
        public event OnUnbindReceivedDelegate OnUnbindReceived = delegate { };
        public delegate void OnUnbindRespReceivedDelegate(SmppClient sender, SmppPduUnbindResp pdu);
        public event OnUnbindRespReceivedDelegate OnUnbindRespReceived = delegate { };
        public delegate void OnGenerickNackReceivedDelegate(SmppClient sender, SmppPduGenerickNack pdu);
        public event OnGenerickNackReceivedDelegate OnGenerickNackReceived = delegate { };
        public delegate void OnSubmitSmReceivedDelegate(SmppClient sender, SmppPduSubmitSm pdu);
        public event OnSubmitSmReceivedDelegate OnSubmitSmReceived = delegate { };
        public delegate void OnSubmitSmRespReceivedDelegate(SmppClient sender, SmppPduSubmitSmResp pdu);
        public event OnSubmitSmRespReceivedDelegate OnSubmitSmRespReceived = delegate { };
        //public delegate void OnSubmitMultiReceivedDelegate(SmppClient sender, SmppPduSubmitMulti pdu);
        //public delegate void OnSubmitMultiRespReceivedDelegate(SmppClient sender, SmppPduSubmitMultiResp pdu);
        public delegate void OnDeliverSmReceivedDelegate(SmppClient sender, SmppPduDeliverSm pdu);
        public event OnDeliverSmReceivedDelegate OnDeliverSmReceived = delegate { };
        public delegate void OnDeliverSmRespReceivedDelegate(SmppClient sender, SmppPduDeliverSmResp pdu);
        public event OnDeliverSmRespReceivedDelegate OnDeliverSmRespReceived = delegate { };
        public delegate void OnDataSmReceivedDelegate(SmppClient sender, SmppPduDataSm pdu);
        public event OnDataSmReceivedDelegate OnDataSmReceived = delegate { };
        public delegate void OnDataSmRespReceivedDelegate(SmppClient sender, SmppPduDataSmResp pdu);
        public event OnDataSmRespReceivedDelegate OnDataSmRespReceived = delegate { };
        public delegate void OnQuerySmReceivedDelegate(SmppClient sender, SmppPduQuerySm pdu);
        public event OnQuerySmReceivedDelegate OnQuerySmReceived = delegate { };
        public delegate void OnQuerySmRespReceivedDelegate(SmppClient sender, SmppPduQuerySmResp pdu);
        public event OnQuerySmRespReceivedDelegate OnQuerySmRespReceived = delegate { };
        public delegate void OnCancelSmReceivedDelegate(SmppClient sender, SmppPduCancelSm pdu);
        public event OnCancelSmReceivedDelegate OnCancelSmReceived = delegate { };
        public delegate void OnCancelSmRespReceivedDelegate(SmppClient sender, SmppPduCancelSmResp pdu);
        public event OnCancelSmRespReceivedDelegate OnCancelSmRespReceived = delegate { };
        public delegate void OnReplaceSmReceivedDelegate(SmppClient sender, SmppPduReplaceSm pdu);
        public event OnReplaceSmReceivedDelegate OnReplaceSmReceived = delegate { };
        public delegate void OnReplaceSmRespReceivedDelegate(SmppClient sender, SmppPduReplaceSmResp pdu);
        public event OnReplaceSmRespReceivedDelegate OnReplaceSmRespReceived = delegate { };
        public delegate void OnEnquireLinkReceivedDelegate(SmppClient sender, SmppPduEnquireLink pdu);
        public event OnEnquireLinkReceivedDelegate OnEnquireLinkReceived = delegate { };
        public delegate void OnEnquireLinkRespReceivedDelegate(SmppClient sender, SmppPduEnquireLinkResp pdu);
        public event OnEnquireLinkRespReceivedDelegate OnEnquireLinkRespReceived = delegate { };
        public delegate void OnAlertNotificationReceivedDelegate(SmppClient sender, SmppPduAlertNotification pdu);
        public event OnAlertNotificationReceivedDelegate OnAlertNotificationReceived = delegate { };

        public SmppClient()
        {
            OnSmppPduReceived += SmppConnection_OnSmppPduReceived;
        }

        public void Connect(string host, int port, IPEndPoint localEndPoint = null)
        {
            Close();
            tcpClient = new TcpClient(localEndPoint ?? new IPEndPoint(IPAddress.Any, 0));
            tcpClient.ReceiveTimeout = 180000;
            tcpClient.Connect(host, port);
            data = new MemoryStream();
            length = 0;
            receiveThread = new Thread(ReceivingThread);
            receiveThread.Start();
            //enquireThread = new Thread(EnquireThread);
            //enquireThread.Start();
            OnConnected(this);
        }

        public void Close()
        {
            if (Connected)
                tcpClient.Client.Disconnect(false);
        }

        private void ReceivingThread()
        {
            try
            {
                int b;
                do
                {
                    b = tcpClient.GetStream().ReadByte();
                    if (b >= 0)
                    {
                        data.WriteByte((byte)b);
                        if (data.Length == 4)
                        {
                            var buf = data.GetBuffer();
                            length = (uint)((buf[0] << 24) | (buf[1] << 16) | (buf[2] << 8) | buf[3]);
                        }
                        if (data.Length == length)
                        {
                            OnSmppPduReceived(this, SmppPdu.CreateFromBytes(data.GetBuffer(), length));
                            data = new MemoryStream();
                            length = 0;
                        }
                    }
                } while (tcpClient != null && tcpClient.Connected && b >= 0);
            }
            catch { }
            finally
            {
                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient = null;
                }
                receiveThread = null;
                OnDisconnected(this);
            }
        }

        private void EnquireThread()
        {
            while (tcpClient != null && tcpClient.Connected)
            {
                Thread.Sleep(55000);
                try
                {
                    SendPdu(new SmppPduEnquireLink());
                }
                catch { }
            }
        }

        public void SendPdu(SmppPdu pdu)
        {
            if (tcpClient == null || !tcpClient.Connected) throw new Exception("Not connected");
            tcpClient.GetStream().Write(pdu.GetData(), 0, (int)pdu.Length);
#if DEBUG
            Console.WriteLine("Sending: " + pdu);
            Console.WriteLine();
#endif
        }

        void SmppConnection_OnSmppPduReceived(SmppClient sender, SmppPdu pdu)
        {
            switch (pdu.CommandId)
            {
                case SmppPdu.SmppCommandType.bind_transmitter:
                    OnBindTransmitterReceived(this, (SmppPduBindTransmitter)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.bind_transmitter_resp:
                    OnBindTransmitterRespReceived(this, (SmppPduBindTransmitterResp)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.bind_receiver:
                    OnBindReceiverReceived(this, (SmppPduBindReceiver)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.bind_receiver_resp:
                    OnBindReceiverRespReceived(this, (SmppPduBindReceiverResp)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.bind_transceiver:
                    OnBindTransceiverReceived(this, (SmppPduBindTransceiver)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.bind_transceiver_resp:
                    OnBindTransceiverRespReceived(this, (SmppPduBindTransceiverResp)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.unbind:
                    OnUnbindReceived(this, (SmppPduUnbind)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.unbind_resp:
                    OnUnbindRespReceived(this, (SmppPduUnbindResp)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.generic_nack:
                    OnGenerickNackReceived(this, (SmppPduGenerickNack)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.submit_sm:
                    OnSubmitSmReceived(this, (SmppPduSubmitSm)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.submit_sm_resp:
                    OnSubmitSmRespReceived(this, (SmppPduSubmitSmResp)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.deliver_sm:
                    OnDeliverSmReceived(this, (SmppPduDeliverSm)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.deliver_sm_resp:
                    OnDeliverSmRespReceived(this, (SmppPduDeliverSmResp)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.data_sm:
                    OnDataSmReceived(this, (SmppPduDataSm)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.data_sm_resp:
                    OnDataSmRespReceived(this, (SmppPduDataSmResp)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.query_sm:
                    OnQuerySmReceived(this, (SmppPduQuerySm)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.query_sm_resp:
                    OnQuerySmRespReceived(this, (SmppPduQuerySmResp)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.cancel_sm:
                    OnCancelSmReceived(this, (SmppPduCancelSm)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.cancel_sm_resp:
                    OnCancelSmRespReceived(this, (SmppPduCancelSmResp)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.enquire_link:
                    OnEnquireLinkReceived(this, (SmppPduEnquireLink)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.enquire_link_resp:
                    OnEnquireLinkRespReceived(this, (SmppPduEnquireLinkResp)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
                case SmppPdu.SmppCommandType.alert_notification:
                    OnAlertNotificationReceived(this, (SmppPduAlertNotification)SmppPdu.CreateFromBytes(pdu.GetData()));
                    break;
            }
        }

        public void Dispose()
        {
            if (Connected) Close();
        }
    }
}
