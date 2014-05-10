using RabbitMQ.Client;

namespace SaasOvation.Common.Port.Adapter.Messaging.RabbitMq {
    

        public abstract class BrokerChannel {
        private IConnection _connection;
        private string _host;

        public string Name { get; protected set; }
        public bool Durable { get; protected set; }

        public IModel Channel { get; private set; }

        protected BrokerChannel(ConnectionSettings connectionSettings): this(connectionSettings, null) {}

        protected BrokerChannel(ConnectionSettings connectionSettings, string name) {
            this.Name = name;
            ConnectionFactory factory = this.ConfigureConnectionFactoryUsing(connectionSettings);

            this._connection = factory.CreateConnection();
            this.Channel = this._connection.CreateModel();
        }

        protected BrokerChannel(BrokerChannel brokerChannel): this(brokerChannel, null) {}

        protected BrokerChannel(BrokerChannel brokerChannel, string name) {
            this._host = brokerChannel._host;
            this.Name = name;
            this._connection = brokerChannel._connection;
            this.Channel = brokerChannel.Channel;
        }

        protected ConnectionFactory ConfigureConnectionFactoryUsing(ConnectionSettings connectionSettings) {
            ConnectionFactory factory = new ConnectionFactory();

            factory.HostName = connectionSettings.HostName;

            if(connectionSettings.HasPort()) {
                factory.Port = connectionSettings.Port;
            }

            factory.VirtualHost = connectionSettings.VirtualHost;

            if(connectionSettings.HasUserCredentials()) {
                factory.UserName = connectionSettings.UserName;
                factory.Password = connectionSettings.Password;
            }

            return factory;
        }

        public void Close() {
            if(this.Channel!=null && this.Channel.IsOpen) {
                this.Channel.Close();
            }

            if(this._connection!=null && this._connection.IsOpen) {
                this._connection.Close();
            }

            this.Channel= null;
            this._connection= null;
        }

        protected virtual bool IsExchange() {
            return false;
        }

        public string ExchangeName {
            get { return this.IsExchange() ? this.Name : ""; }
        }

        protected virtual bool IsQueue() {
            return false;
        }

        public string QueueName {
            get { return this.IsQueue() ? this.Name : ""; }
        }


    }
}