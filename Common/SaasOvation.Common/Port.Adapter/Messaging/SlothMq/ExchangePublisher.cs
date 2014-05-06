namespace SaasOvation.Common.Port.Adapter.Messaging.SlothMq {
    public class ExchangePublisher {
        private readonly string _exchangeName;

        public ExchangePublisher(string exchangeName) {
            this._exchangeName = exchangeName;
        }

        public void Publish(string type, string message) {
            SlothClient.Instance.Publish(this._exchangeName, type, message);
        }
    }
}