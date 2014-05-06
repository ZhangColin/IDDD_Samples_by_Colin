using RabbitMQ.Client;

namespace SaasOvation.Common.Port.Adapter.Messaging.RabbitMq {
    public abstract class MessageListener {
        private readonly Type _type;

        public enum Type {
            Binary,
            Text
        }

        protected MessageListener(Type type) {
            this._type = type;
        }

        public bool IsBinaryListener() {
            return _type == Type.Binary;
        }

        public bool IsTextListener() {
            return _type == Type.Text;
        }

        public abstract void HandleMessage(string type, string messageId, AmqpTimestamp timestamp, byte[] binaryMessage,
            long deliveryTag, bool isRedelivery);

        public abstract void HandleMessage(string type, string messageId, AmqpTimestamp timestamp, string textMessage,
            long deliveryTag, bool isRedelivery);
    }
}