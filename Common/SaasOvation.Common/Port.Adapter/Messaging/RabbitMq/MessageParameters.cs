using System;
using RabbitMQ.Client;

namespace SaasOvation.Common.Port.Adapter.Messaging.RabbitMq {
    public class MessageParameters {
        public IBasicProperties Properties { get; private set; }

        private MessageParameters(IBasicProperties properties) {
            this.Properties = properties;
        }

        public bool Durable {
            get { return this.Properties.DeliveryMode == 2; }
        }

        public static MessageParameters DurableTextParameters(IModel model, string type, string messageId,
            AmqpTimestamp timestamp) {
            IBasicProperties properties = model.CreateBasicProperties();
            properties.ContentType = "text/plain";
            properties.DeliveryMode = 2;
            properties.MessageId = messageId;
            properties.Timestamp = timestamp;
            properties.Type = type;
            return new MessageParameters(properties);
        }
        
        public static MessageParameters TextParameters(IModel model, string type, string messageId,
            AmqpTimestamp timestamp) {
            IBasicProperties properties = model.CreateBasicProperties();
            properties.ContentType = "text/plain";
            properties.DeliveryMode = 1;
            properties.MessageId = messageId;
            properties.Timestamp = timestamp;
            properties.Type = type;
            return new MessageParameters(properties);
        }
    }
}