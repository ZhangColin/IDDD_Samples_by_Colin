
using System.Text;
using RabbitMQ.Client;

namespace SaasOvation.Common.Port.Adapter.Messaging.RabbitMq {
    public abstract class ExchangeListener {
        private readonly ConnectionSettings _connectionSettings;
        private MessageConsumer _messageConsumer;
        private Queue _queue;

        protected ExchangeListener(ConnectionSettings connectionSettings) {
            this._connectionSettings = connectionSettings;
            this.AttachToQueue();
            this.RegisterConsumer();
        }

        protected abstract string ExchangeName { get; }

        protected abstract void FilteredDispatch(string type, string textMessage);

        protected abstract string[] ListensTo();

        public void Close() {
            this._queue.Close();
        }

        protected string QueueName {
            get { return this.GetType().Name; }
        }

        private void AttachToQueue() {
            Exchange exchange = Exchange.FanOutInstance(_connectionSettings, this.ExchangeName, true);

            this._queue = Queue.IndividualExchangeSubscriberInstance(exchange, this.ExchangeName + "." + this.QueueName);
        }

        private void RegisterConsumer() {
            this._messageConsumer = MessageConsumer.Instance(this._queue, false);
            this._messageConsumer.ReceiveOnly(this.ListensTo(), new ExchangeMessageListener(MessageListener.Type.Text, this));
        }

        private class ExchangeMessageListener: MessageListener {
            private readonly ExchangeListener _exchangeListener;

            public ExchangeMessageListener(Type type, ExchangeListener exchangeListener)
                : base(type) {
                this._exchangeListener = exchangeListener;
            }

            public override void HandleMessage(string type, string messageId, AmqpTimestamp timestamp,
                byte[] binaryMessage, long deliveryTag,
                bool isRedelivery) {
                    this._exchangeListener.FilteredDispatch(type, Encoding.UTF8.GetString(binaryMessage));
            }

            public override void HandleMessage(string type, string messageId, AmqpTimestamp timestamp,
                string textMessage, long deliveryTag,
                bool isRedelivery) {
                this._exchangeListener.FilteredDispatch(type, textMessage);
            }
        }
    }


}