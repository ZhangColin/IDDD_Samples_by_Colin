using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Port.Adapter.Messaging.RabbitMq {
    public class ConnectionSettings {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        protected ConnectionSettings(string hostName, int port, string virtualHost, string userName, string password) {
            AssertionConcern.NotEmpty(hostName, "Host name must be provided.");
            AssertionConcern.NotEmpty(virtualHost, "Virtual host must be provided.");

            this.HostName = hostName;
            this.Port = port;
            this.VirtualHost = virtualHost;
            this.UserName = userName;
            this.Password = password;
        }

        public bool HasPort() {
            return this.Port > 0;
        }

        public bool HasUserCredentials() {
            return this.UserName != null && this.Password != null;
        }

        public static ConnectionSettings Instance() {
            return new ConnectionSettings("localhost", -1, "/", null, null);
        }

        public static ConnectionSettings Instance(string hostName, string virtualHost) {
            return new ConnectionSettings(hostName, -1, virtualHost, null, null);
        }

        public static ConnectionSettings Instance(string hostName, int port, string virtualHost, string userName,
            string password) {
            return new ConnectionSettings(hostName, port, virtualHost, userName, password);
        }
    }
}