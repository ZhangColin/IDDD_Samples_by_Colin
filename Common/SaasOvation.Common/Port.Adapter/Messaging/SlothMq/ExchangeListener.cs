using System.Collections.Generic;

namespace SaasOvation.Common.Port.Adapter.Messaging.SlothMq {
    public abstract class ExchangeListener {
        private ISet<string> _messageTypes; 
        protected ExchangeListener() {
            this.EstablishMessageTypes();
//            SlothClient.Instance.Register(this);
        }

        public void Close() {
            SlothClient.Instance.UnRegister(this);
        }

        public abstract string ExchangeName { get; }
        public abstract void FilteredDispatch(string type, string textMessage);
        public abstract string Name { get; }

        protected abstract string[] ListensTo();

        public bool ListensTo(string type) {
            return this._messageTypes.Count==0 || _messageTypes.Contains(type);
        }

        private void EstablishMessageTypes() {
            string[] filterOutAllBut = this.ListensTo() ?? new string[0];

            this._messageTypes = new HashSet<string>(filterOutAllBut);
        }
    }
}