using System;
using Iesi.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.Impl.v0_8;

namespace SaasOvation.Common.Port.Adapter.Messaging.RabbitMq {
    public class MessageConsumer {
        protected MessageConsumer(Queue queue, bool autoAcknowledged) {
            this.Queue = queue;
            this.AutoAcknowledged = autoAcknowledged;

            this.MessageTypes = new HashedSet<string>();
        }

        public bool Closed { get; set; }

        public string Tag { get; private set; }

        public bool AutoAcknowledged { get; private set; }

        protected Queue Queue { get; set; }

        private ISet<string> MessageTypes { get; set; }

        public static MessageConsumer Instance(Queue queue) {
            return new MessageConsumer(queue, false);
        }

        public static MessageConsumer Instance(Queue queue, bool autoAcknowledged) {
            return new MessageConsumer(queue, autoAcknowledged);
        }

        public static MessageConsumer AutoAcknowledgedInstance(Queue queue) {
            return MessageConsumer.Instance(queue);
        }

        public void Close() {
            this.Closed = true;
            this.Queue.Close();
        }

        public void EqualizeMessageDistribution() {
            try {
                this.Queue.Channel.BasicQos(0, 1, false);
            }
            catch(Exception e) {
                throw new MessageException("Cannot equalize distribution.", e);
            }
        }

        public void ReceiveAll(MessageListener messageListener) {
            this.ReceiveFor(messageListener);
        }

        public void ReceiveOnly(string[] messageTypes, MessageListener messageListener) {
            if(messageTypes==null) {
                messageTypes = new string[0];
            }
            this.MessageTypes = new HashedSet<string>(messageTypes);
            this.ReceiveFor(messageListener);
        }

        private void ReceiveFor(MessageListener messageListener) {
            try {
                this.Tag = this.Queue.Channel.BasicConsume(this.Queue.Name, this.AutoAcknowledged,
                    new DispatchingConsumer(this.Queue.Channel, messageListener, this.MessageTypes,
                        this.AutoAcknowledged, this));
            }
            catch(Exception e) {
                throw new MessageException("Failed to initiate consumer.", e);
            }
        }

        private class DispatchingConsumer: DefaultBasicConsumer  {
            private MessageListener MessageListener { get; set; }
            private ISet<string> FilteredMessageTypes { get; set; }
            private bool AutoAcknowledged { get; set; }
            private MessageConsumer MessageConsumer { get; set; }

            public DispatchingConsumer(IModel model, MessageListener messageListener, ISet<string> filteredMessageTypes,
                bool autoAcknowledged, MessageConsumer messageConsumer): base(model) {
                this.MessageListener = messageListener;
                this.FilteredMessageTypes = filteredMessageTypes;
                this.AutoAcknowledged = autoAcknowledged;
                this.MessageConsumer = messageConsumer;
            }

            public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
                IBasicProperties properties, byte[] body) {
                if(!this.MessageConsumer.Closed) {
                    this.Handle(deliveryTag,redelivered,properties,body);
                }

                MessageConsumer.Close();
            }

            public override void HandleModelShutdown(IModel model, ShutdownEventArgs reason) {
                MessageConsumer.Close();
                base.HandleModelShutdown(model, reason);
            }

            private void Handle(ulong deliveryTag, bool redelivered, IBasicProperties basicProperties, byte[] body) {
                try {
                    if(this.FilteredMessagetype(basicProperties)) {}
                    else if(this.MessageListener.IsBinaryListener()) {
                        this.MessageListener.HandleMessage(basicProperties.Type, basicProperties.MessageId,
                            basicProperties.Timestamp, body, (long)deliveryTag, redelivered);
                    }
                    else if(this.MessageListener.IsTextListener()) {
                        this.MessageListener.HandleMessage(basicProperties.Type, basicProperties.MessageId,
                            basicProperties.Timestamp, body, (long)deliveryTag, redelivered);
                    }

                    this.Ack(deliveryTag);
                }
                catch(MessageException e) {
                    this.Nack(deliveryTag, e.Retry);
                }
                catch(Exception e) {
                    this.Nack(deliveryTag, false);
                }

            }

            private void Ack(ulong deliveryTag) {
                try {
                    if (!this.AutoAcknowledged) {
                        this.Model.BasicNack(deliveryTag, false, false);
                    }
                }
                catch (Exception) {

                }
            }

            private void Nack(ulong deliveryTag, bool retry) {
                try {
                    if(!this.AutoAcknowledged) {
                        this.Model.BasicNack(deliveryTag, false, retry);
                    }
                }
                catch(Exception) {
                    
                }
            }

            private bool FilteredMessagetype(IBasicProperties basicProperties) {
                bool filtered = false;
                if(!this.FilteredMessageTypes.IsEmpty) {
                    string messageType = basicProperties.Type;
                    if(string.IsNullOrEmpty(messageType) || !FilteredMessageTypes.Contains(messageType)) {
                        filtered = true;
                    }
                }

                return filtered;
            }
        }
    }
}