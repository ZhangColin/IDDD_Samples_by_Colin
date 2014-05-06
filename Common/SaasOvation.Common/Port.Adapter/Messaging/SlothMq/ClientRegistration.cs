using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SaasOvation.Common.Port.Adapter.Messaging.SlothMq {
    public class ClientRegistration {
        public string ClientId { get; private set; }
        private ISet<string> Exchanges { get; set; }
        public IPAddress IpAddress { get; private set; }

        private ClientRegistration(IPAddress ipAddress, string clientId) {
            this.Exchanges = new HashSet<string>();
            this.IpAddress = ipAddress ?? IPAddress.Parse("127.0.0.1");
            this.ClientId = clientId;
        }

        public ClientRegistration(string clientId): this(null, clientId) {
        }

        public void AddSubscription(string exchangeName) {
            Console.WriteLine("ADDING EXCHANGE: " + exchangeName);
            this.Exchanges.Add(exchangeName);
        }

        public bool IsSubscribedTo(string exchangeName) {
            return this.Exchanges.Contains(exchangeName);
        }

        public void RemoveSubscription(string exchangeName) {
            this.Exchanges.Remove(exchangeName);
        }

        public bool Matches(IPAddress ipAddress, string clientId) {
            return this.IpAddress.ToString().Equals(ipAddress.ToString()) && this.ClientId == clientId;
        }

        public override string ToString() {
            return "ClientRegistration [IpAddress=" + this.IpAddress + ", ClientId=" + this.ClientId + ", Exchanges="
                + string.Join(", ", this.Exchanges.ToArray()) + "]";
        }
    }
}