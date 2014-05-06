using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NHibernate.Linq;

namespace SaasOvation.Common.Port.Adapter.Messaging.SlothMq {
    public class SlothServer {
        private const int _port = 55555;

        private readonly IDictionary<string, ClientRegistration> _clientRegistrations;
        private Socket _serverSocket;

        private readonly IDictionary<string, Socket> _clientSockets;

        private ManualResetEvent _newClientSocketSignal;
        private SocketService _socketService;

        public SlothServer() {
            this._clientRegistrations = new Dictionary<string, ClientRegistration>();
            this._clientSockets = new Dictionary<string, Socket>();
            this._newClientSocketSignal = new ManualResetEvent(false);
            this._socketService = new SocketService();

            this.Open();
        }

        private void Open() {
            try {
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                this._serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this._serverSocket.Bind(new IPEndPoint(ip, _port));
                this._serverSocket.Listen(1000);
                Console.WriteLine("SLOTH SERVER: Opened on port: " + _port);
            }
            catch (Exception e) {
                Console.WriteLine("SLOTH SERVER: Cannot connect because: " + e.Message);
            }

        }

        protected bool IsClosed {
            get { return this._serverSocket == null; }
        }

        public virtual void Close() {
            try {
                this._serverSocket.Shutdown(SocketShutdown.Both);
                this._serverSocket.Close();
            }
            catch (Exception) {
                Console.WriteLine(this.GetType().Name + ": problems closing socket.");
            }
            this._serverSocket = null;
        }

        public static void ExecuteInProcessDetachedServer() {
            Thread thread = new Thread(ExecuteNewServer);
            thread.Start();
        }

        public static void ExecuteNewServer() {
            SlothServer slothServer = new SlothServer();
            slothServer.Execute();
        }

        public void Execute() {
            while(!this.IsClosed) {
                _newClientSocketSignal.Reset();
                try {
                    _serverSocket.BeginAccept(asyncResult => {
                        Socket clientSocket = _serverSocket.EndAccept(asyncResult);
                        _newClientSocketSignal.Set();
                        _socketService.ReceiveMessage(clientSocket, result => {
                            if (!string.IsNullOrEmpty(result)) {
                                this.HandleMessage(result, clientSocket);
                            }
                        });
                    }, _serverSocket);
                }
                catch (SocketException socketException) {
                    Console.WriteLine("SLOTH SERVER: Socket send exception, ErrorCode: {0}", socketException.ErrorCode);
                }
                catch (Exception ex) {
                    Console.WriteLine("SLOTH SERVER: Unknown socket send exception: {0}", ex);
                }

                _newClientSocketSignal.WaitOne();
            }
        }

        public void SendToClient(string clientId, string encodedMessage) {
            Console.WriteLine("SLOTH SERVER: Send to client: " + encodedMessage);
            _socketService.SendMessage(this._clientSockets[clientId], encodedMessage, reply => {});
        }

        private void HandleMessage(string receivedData, Socket clientSocket) {
            Console.WriteLine("SLOTH SERVER: Handling: " + receivedData);

            if(receivedData.StartsWith("ATTACH:")) {
                string clientId = receivedData.Substring(7);
                this.Attach(clientId, clientSocket);
            }
            else if(receivedData.StartsWith("CLOSE:")) {
                this.Close();
                //this.CloseClient(receivedData, clientSocket);
            }
            else if(receivedData.StartsWith("PUBLISH:")) {
                this.PublishToClients(receivedData);
            }
            else if(receivedData.StartsWith("SUBSCRIBE:")) {
                this.SubscribeClientTo(receivedData.Substring(10), clientSocket);
            }
            else if(receivedData.StartsWith("UNSUBSCRIBE:")) {
                this.UnsubscribeClientFrom(receivedData.Substring(12));
            }
            else {
                Console.WriteLine("SLOTH SERVER: Does not understand: " + receivedData);
            }
        }

        private ClientRegistration Attach(string clientId, Socket clientSocket) {
            if(!this._clientRegistrations.ContainsKey(clientId)) {
                ClientRegistration clientRegistration = new ClientRegistration(clientId);
                this._clientRegistrations.Add(clientId, clientRegistration);
                this._clientSockets.Add(clientId, clientSocket);
            }

            return this._clientRegistrations[clientId];
        }

//        private void CloseClient(string receivedData, Socket clientSocket) {
//            string clientId = receivedData.Substring(7);
//            if(this._clientRegistrations.ContainsKey(clientId)) {
//                this._clientRegistrations.Remove(clientId);
//                this._clientSockets.Remove(clientId);
//            }
//            clientSocket.Close();
//
//            this.Close();
//        }

        private void PublishToClients(string exchangeMessage) {
            int exchangeDivider = exchangeMessage.IndexOf("PUBLISH:");
            int typeDivider = exchangeMessage.IndexOf("TYPE:", exchangeDivider + 8);

            if(exchangeDivider == -1) {
                Console.WriteLine("SLOTH SERVER: PUBLISH: No exchange name: ignoring: "+exchangeMessage);
            }
            else if(typeDivider==-1) {
                Console.WriteLine("SLOTH SERVER: PUBLISH: No TYPE: ignoring: "+exchangeMessage);
            }
            else {
                string exchangeName =
                    exchangeMessage.Substring(exchangeDivider + 8, typeDivider - exchangeDivider - 8).Trim();
                this._clientRegistrations.Values.Where(
                    clientRegistration => clientRegistration.IsSubscribedTo(exchangeName)).ForEach(
                        clientRegistration => this.SendToClient(clientRegistration.ClientId, exchangeMessage));
            }
        }

        private void SubscribeClientTo(string clientIdWithExchangeName, Socket clientSocket) {
            string[] parts = clientIdWithExchangeName.Split(':');
            string clientId = parts[0];
            string exchangeName = parts[1];

            ClientRegistration clientRegistration = this._clientRegistrations[clientId] ?? this.Attach(clientId, clientSocket);

            clientRegistration.AddSubscription(exchangeName);

            Console.WriteLine("SLOTH SERVER: Subscribed: " + clientRegistration + " TO: " + exchangeName);
        }

        private void UnsubscribeClientFrom(string clientIdWithExchangeName) {
            string[] parts = clientIdWithExchangeName.Split(':');
            string clientId = parts[0];
            string exchangeName = parts[1];
            ClientRegistration clientRegistration = this._clientRegistrations[clientId];
            if(clientRegistration!=null) {
                clientRegistration.RemoveSubscription(exchangeName);
                Console.WriteLine("SLOTH SERVER: Unsubscribed: " + clientRegistration + " FROM: " + exchangeName);
            }
        }
    }
}