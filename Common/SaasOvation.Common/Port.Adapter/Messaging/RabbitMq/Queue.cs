using System.Linq;
using NHibernate.Linq;
using RabbitMQ.Client;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Port.Adapter.Messaging.RabbitMq {
    public class Queue: BrokerChannel {
        public Queue(ConnectionSettings connectionSettings, string name, bool durable, bool exclusive, bool autoDeleted)
            : base(connectionSettings) {
            this.Durable = durable;

            QueueDeclareOk result = this.Channel.QueueDeclare(name, durable, exclusive, autoDeleted, null);

            this.Name = result.QueueName;
        }
        
        public Queue(BrokerChannel brokerChannel, string name, bool durable, bool exclusive, bool autoDeleted)
            : base(brokerChannel) {
            this.Durable = durable;

            QueueDeclareOk result = this.Channel.QueueDeclare(name, durable, exclusive, autoDeleted, null);

            this.Name = result.QueueName;
        }

        protected override bool IsQueue() {
            return true;
        }

        public static Queue Create(ConnectionSettings connectionSettings, string name) {
            return new Queue(connectionSettings, name, false, false, false);
        }

        public static Queue Create(ConnectionSettings connectionSettings, string name, bool durable, bool exclusive,
            bool autoDeleted) {
            return new Queue(connectionSettings, name, durable, exclusive, autoDeleted);
        }

        public static Queue DurableInstance(ConnectionSettings connectionSettings, string name) {
            return new Queue(connectionSettings, name, true, false, false);
        }

        public static Queue DurableExclsuiveInstance(ConnectionSettings connectionSettings, string name) {
            return new Queue(connectionSettings, name, true, true, false);
        }

        public static Queue ExchangeSubscriberInstance(Exchange exchange) {
            Queue queue = new Queue(exchange, "", false, true, true);
            queue.Channel.QueueBind(queue.Name, exchange.Name, "");

            return queue;
        }

        public static Queue ExchangeSubscriberInstance(Exchange exchange, string[] routingKeys) {
            Queue queue = new Queue(exchange, "", false, true, true);
            if(!routingKeys.Any()) {
                queue.Channel.QueueBind(queue.Name, exchange.Name, "");
            }
            else {
                routingKeys.ForEach(routingKey => queue.Channel.QueueBind(queue.Name, exchange.Name, routingKey));
            }

            return queue;
        }

        public static Queue ExchangeSubscriberInstance(Exchange exchange, string[] routingKeys, bool durable,
            bool exclusive, bool autoDeleted) {
            Queue queue = new Queue(exchange, "", durable, exclusive, autoDeleted);
            if(!routingKeys.Any()) {
                queue.Channel.QueueBind(queue.Name, exchange.Name, "");
            }
            else {
                routingKeys.ForEach(routingKey => queue.Channel.QueueBind(queue.Name, exchange.Name, routingKey));
            }

            return queue;
        }

        public static Queue IndividualExchangeSubscriberInstance(Exchange exchange, string name) {
            AssertionConcern.NotEmpty(name, "An individual subscriber must be named.");

            Queue queue = new Queue(exchange, name, true, false, false);

            queue.Channel.QueueBind(queue.Name, exchange.Name, "");

            return queue;
        }
        
        public static Queue IndividualExchangeSubscriberInstance(Exchange exchange, string name, string[] routingKeys) {
            AssertionConcern.NotEmpty(name, "An individual subscriber must be named.");

            Queue queue = new Queue(exchange, name, true, false, false);

            if(!routingKeys.Any()) {
                queue.Channel.QueueBind(queue.Name, exchange.Name, "");
            }
            else {
                routingKeys.ForEach(routingKey => queue.Channel.QueueBind(queue.Name, exchange.Name, routingKey));
            }

            return queue;
        }
    }
}