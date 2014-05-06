using System;
using System.Net.Sockets;
using System.Text;

namespace SaasOvation.Common.Port.Adapter.Messaging.SlothMq {
    public class SocketService {
        public void SendMessage(Socket targetSocket, string messageContent, Action<string> messageSentCallBack) {
            byte[] message = BuildMessage(messageContent, Encoding.ASCII);
            if (message.Length > 0) {
                var context = new SendContext() {
                    TargetSocket = targetSocket,
                    MessageContent = messageContent,
                    MessageSentCallback = messageSentCallBack
                };
                targetSocket.BeginSend(message, 0, message.Length, 0, new AsyncCallback(this.SendCallback), context);
            }
        }

        private void SendCallback(IAsyncResult asyncResult) {
            try {
                SendContext context = (SendContext)asyncResult.AsyncState;
                context.TargetSocket.EndSend(asyncResult);
                context.MessageSentCallback(context.MessageContent);
            }
            catch (SocketException socketException) {
                Console.WriteLine("Socket send exception, ErrorCode: {0}", socketException.ErrorCode);
            }
            catch (Exception ex) {
                Console.WriteLine("Unknown socket send exception: {0}", ex);
            }
        }

        private static byte[] BuildMessage(string content, Encoding encoding) {
            byte[] data = encoding.GetBytes(content);
            byte[] header = BitConverter.GetBytes(data.Length);
            byte[] message = new byte[data.Length + header.Length];
            header.CopyTo(message, 0);
            data.CopyTo(message, header.Length);

            return message;
        }

        public void ReceiveMessage(Socket sourceSocket, Action<string> messageReceivedCallback) {
            ReceiveState receiveState = new ReceiveState() {
                SourceSocket = sourceSocket,
                MessageReceivedCallback = messageReceivedCallback
            };
            this.ReceiveInternal(receiveState, 4);
        }

        private void ReceiveInternal(ReceiveState receiveState, int size) {
            receiveState.SourceSocket.BeginReceive(receiveState.Buffer, 0, size, 0, this.ReceiveCallback, receiveState);
        }

        private void ReceiveCallback(IAsyncResult asyncResult) {
            ReceiveState receiveState = (ReceiveState)asyncResult.AsyncState;
            var sourceSocket = receiveState.SourceSocket;
            var data = receiveState.Data;
            var bytesRead = 0;
            try {
                bytesRead = sourceSocket.EndReceive(asyncResult);
            }
            catch (SocketException socketException) {
                Console.WriteLine("Socket send exception, ErrorCode: {0}", socketException.ErrorCode);
            }
            catch (Exception ex) {
                Console.WriteLine("Unknown socket send exception: {0}", ex);
            }

            if (bytesRead > 0) {
                if (receiveState.MessageSize == null) {
                    receiveState.MessageSize = GetMessageLength(receiveState.Buffer);
                    int size = receiveState.MessageSize <= ReceiveState.BufferSize
                        ? receiveState.MessageSize.Value : ReceiveState.BufferSize;
                    this.ReceiveInternal(receiveState, size);
                }
                else {
                    for (int index = 0; index < bytesRead; index++) {
                        data.Add(receiveState.Buffer[index]);
                    }
                    if (receiveState.Data.Count < receiveState.MessageSize.Value) {
                        int remainSize = receiveState.MessageSize.Value - receiveState.Data.Count;
                        int size = remainSize <= ReceiveState.BufferSize ? remainSize : ReceiveState.BufferSize;
                        this.ReceiveInternal(receiveState, size);
                    }
                    else {
                        receiveState.MessageReceivedCallback(Encoding.ASCII.GetString(data.ToArray()));
                        receiveState.MessageSize = null;
                        receiveState.Data.Clear();
                        this.ReceiveInternal(receiveState, 4);
                    }
                }
            }
        }

        private int GetMessageLength(byte[] buffer) {
            byte[] data = new byte[4];
            for (int i = 0; i < 4; i++) {
                data[i] = buffer[i];
            }
            return BitConverter.ToInt32(data, 0);
        } 
    }
}