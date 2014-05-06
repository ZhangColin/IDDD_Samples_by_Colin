using System;
using System.Text;
using RabbitMQ.Client;

namespace SaasOvation.Common.Port.Adapter.Messaging.RabbitMq {
    public class MessageProducer {
        private readonly BrokerChannel _brokerChannel;

        public MessageProducer(BrokerChannel brokerChannel) {
            this._brokerChannel = brokerChannel;
        }

        public static MessageProducer CreateProducer(BrokerChannel brokerChannel) {
            return new MessageProducer(brokerChannel);
        }

        public void Close() {
            this._brokerChannel.Close();
        }

        public MessageProducer Send(string textMessage) {
            this._brokerChannel.Channel.BasicPublish(this._brokerChannel.ExchangeName, this._brokerChannel.QueueName,
                this.TextDurability(), Encoding.UTF8.GetBytes(textMessage));
            return this;
        }

        public MessageProducer Send(string textMessage, MessageParameters messageParameters) {
            this._brokerChannel.Channel.BasicPublish(this._brokerChannel.ExchangeName, this._brokerChannel.QueueName,
                messageParameters.Properties, Encoding.UTF8.GetBytes(textMessage));
            return this;
        }

        public MessageProducer Send(string exchange, string routingKey, string textMessage,
            MessageParameters messageParameters) {
            this._brokerChannel.Channel.BasicPublish(exchange, routingKey, messageParameters.Properties,
                Encoding.UTF8.GetBytes(textMessage));
            return this;
        }

        public MessageProducer Send(string routingKey, string textMessage, MessageParameters messageParameters) {
            this._brokerChannel.Channel.BasicPublish(this._brokerChannel.ExchangeName, routingKey,
                messageParameters.Properties, Encoding.UTF8.GetBytes(textMessage));
            return this;
        }
        
        public MessageProducer Send(byte[] binaryMessage) {
            this._brokerChannel.Channel.BasicPublish(this._brokerChannel.ExchangeName, this._brokerChannel.QueueName,
                this.TextDurability(), binaryMessage);
            return this;
        }

        public MessageProducer Send(byte[] binaryMessage, MessageParameters messageParameters) {
            this._brokerChannel.Channel.BasicPublish(this._brokerChannel.ExchangeName, this._brokerChannel.QueueName,
                messageParameters.Properties, binaryMessage);
            return this;
        }

        public MessageProducer Send(string exchange, string routingKey, byte[] binaryMessage,
            MessageParameters messageParameters) {
            this._brokerChannel.Channel.BasicPublish(exchange, routingKey, messageParameters.Properties,
                binaryMessage);
            return this;
        }

        public MessageProducer Send(string routingKey, byte[] binaryMessage, MessageParameters messageParameters) {
            this._brokerChannel.Channel.BasicPublish(this._brokerChannel.ExchangeName, routingKey,
                messageParameters.Properties, binaryMessage);
            return this;
        }

        private void Check(MessageParameters messageParameters) {
            if(this._brokerChannel.Durable) {
                if(!messageParameters.Durable) {
                    throw new ArgumentException("MessageParameters must be durable.");
                }
            }
            else {
                if(messageParameters.Durable) {
                    throw new ArgumentException("MessageParameters must not be durable.");
                }
            }
        }

        private IBasicProperties BinaryDurability() {
            IBasicProperties durability = null;
            if(this._brokerChannel.Durable) {
                //TODO: PERSISTENT_BASIC
            }
            return durability;
        }

        private IBasicProperties TextDurability() {
            IBasicProperties durability = null;
            if (this._brokerChannel.Durable) {
                //TODO: PERSISTENT_TEXT_PLAIN
            }
            return durability;
        }
    }
}