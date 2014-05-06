using System;

namespace SaasOvation.Common.Port.Adapter.Messaging {
    public class MessageException: ApplicationException {
        public bool Retry { get; private set; }

        public MessageException(string message): base(message) {}

        public MessageException(string message, bool retry): base(message) {
            this.Retry = retry;
        }

        public MessageException(string message, Exception innerException, bool retry): base(message, innerException) {
            this.Retry = retry;
        }

        public MessageException(string message, Exception innerException): base(message, innerException) {}
    }
}