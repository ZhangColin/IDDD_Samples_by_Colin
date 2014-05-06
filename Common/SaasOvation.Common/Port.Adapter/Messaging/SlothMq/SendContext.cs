using System;
using System.Net.Sockets;

namespace SaasOvation.Common.Port.Adapter.Messaging.SlothMq {
    public class SendContext {
        public Socket TargetSocket;
        public string MessageContent;
        public Action<string> MessageSentCallback; 
    }
}