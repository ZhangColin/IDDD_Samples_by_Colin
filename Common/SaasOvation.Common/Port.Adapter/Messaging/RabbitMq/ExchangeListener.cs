
namespace SaasOvation.Common.Port.Adapter.Messaging.RabbitMq {
    public abstract class ExchangeListener {
        

        protected ExchangeListener() {}

        protected abstract string ExchangeName { get; }

        protected abstract void FilteredDispatch(string type, string textMessage);

        protected abstract string[] ListensTo();

        public void Close() {
            
        }

        protected string QueueName {
            get { return this.GetType().Name; }
        }

        private void AttachToQueue() { }
    }
}