namespace SaasOvation.Common.Port.Adapter.Messaging.RabbitMq {
    public class Exchange: BrokerChannel {
        public string Type { get; private set; }

        private Exchange(ConnectionSettings connectionSettings, string name, string type, bool durable)
            : base(connectionSettings, name) {
            this.Type = type;
            this.Durable = durable;

            this.Channel.ExchangeDeclare(this.Name, this.Type, durable);
        }

        public static Exchange DirectInstance(ConnectionSettings connectionSettings, string name, bool durable) {
            return new Exchange(connectionSettings, name, "direct", durable);
        }

        public static Exchange FanOutInstance(ConnectionSettings connectionSettings, string name, bool durable) {
            return new Exchange(connectionSettings, name, "fanout", durable);
        }
        
        public static Exchange HeadersInstance(ConnectionSettings connectionSettings, string name, bool durable) {
            return new Exchange(connectionSettings, name, "headers", durable);
        }
        
        public static Exchange TopicInstance(ConnectionSettings connectionSettings, string name, bool durable) {
            return new Exchange(connectionSettings, name, "topic", durable);
        }

        protected override bool IsExchange() {
            return true;
        }
    }
}   