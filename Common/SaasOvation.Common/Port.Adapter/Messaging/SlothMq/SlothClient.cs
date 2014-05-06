using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SaasOvation.Common.Port.Adapter.Messaging.SlothMq {
    public class SlothClient {
        private const int _port = 55555;
        private static SlothClient _instance;
        private readonly object _lock;
        private readonly IDictionary<string, ExchangeListener> _exchangeListeners;
        private readonly string _clientId;

        private Socket _clientSocket;
        private SocketService _socketService;
        private ManualResetEvent _signal = new ManualResetEvent(false);

        public static SlothClient Instance {
            get { return _instance ?? (_instance = new SlothClient()); }
        }

        private SlothClient() {
            this._exchangeListeners = new Dictionary<string, ExchangeListener>();
            this._lock = new object();

            this._clientId = Guid.NewGuid().ToString();
            _socketService = new SocketService();

            this.Open();

            this.Attach();
            this.ReceiveAll();
        }

        private void Open() {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            this._clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try {
                this._clientSocket.Connect(new IPEndPoint(ip, _port));
                Console.WriteLine("SLOTH CLIENT: Opened on clientId: " + this._clientId);
            }
            catch (Exception e) {
                Console.WriteLine("SLOTH CLIENT: Cannot connect because: " + e.Message);
            }
        }

        protected bool IsClosed {
            get { return this._clientSocket == null; }
        }

        protected void Close() {
            Console.WriteLine("SLOTH CLIENT: Closing...");
            try {
                _signal.Set();
                this._clientSocket.Shutdown(SocketShutdown.Both);
                this._clientSocket.Close();
            }
            catch (Exception) {
                Console.WriteLine(this.GetType().Name + ": problems closing socket.");
            }
            this._clientSocket = null;

            foreach(ExchangeListener exchangeListener in this._exchangeListeners.Values) {
                this.UnRegister(exchangeListener);
            }
            Console.WriteLine("SLOTH CLIENT: Closed.");
        }

        public void CloseAll() {
            _instance = null;
            this.Close();
            this.SendToServer("CLOSE:" + this._clientId);
        }

        public void Publish(string exchangeName, string type, string message) {
            string encodedMessage = "PUBLISH: " + exchangeName + " TYPE:" + type + " MSG:" + message;
            this.SendToServer(encodedMessage);
        }

        public void Register(ExchangeListener exchangeListener) {
            lock (_lock) {
                this._exchangeListeners.Add(exchangeListener.Name, exchangeListener);
            }
            this.SendToServer("SUBSCRIBE:" + this._clientId + ":" + exchangeListener.ExchangeName);
        }

        public void UnRegister(ExchangeListener exchangeListener) {
            lock (_lock) {
                this._exchangeListeners.Remove(exchangeListener.Name);
            }
            this.SendToServer("UNSUBSCRIBE:" + this._clientId + ":" + exchangeListener.ExchangeName);
        }

        private void Attach() {
            this.SendToServer("ATTACH:" + this._clientId);
        }

        private void ReceiveAll() {
            Task.Factory.StartNew(() => {
                _socketService.ReceiveMessage(_clientSocket, replyMessage => {
                    Console.WriteLine("SLOTH CLIENT: Reply: "+replyMessage);
                    this.DispatchMessage(replyMessage.Trim());
                });
                _signal.WaitOne();
            });
        }

        private void DispatchMessage(string encodedMessage) {
            int exchangeDivider = encodedMessage.IndexOf("PUBLISH:");
            int typeDivider = encodedMessage.IndexOf("TYPE:", exchangeDivider + 8, StringComparison.Ordinal);
            int msgDivider = encodedMessage.IndexOf("MSG:", typeDivider + 5, StringComparison.Ordinal);

            string exchangeName =
                encodedMessage.Substring(exchangeDivider + 8, (typeDivider - exchangeDivider - 8)).Trim();
            string type = encodedMessage.Substring(typeDivider + 5, (msgDivider - typeDivider - 5)).Trim();
            string message = encodedMessage.Substring(msgDivider + 4).Trim();

            List<ExchangeListener> listeners = null;
            lock (_lock) {
                listeners = new List<ExchangeListener>(this._exchangeListeners.Values);
            }

            listeners.ForEach(listener => {
                if (listener.ExchangeName.Equals(exchangeName) && listener.ListensTo(type)) {
                    try {
                        Console.WriteLine("SLOTH CLIENT: Dispatching: Exchange: " + exchangeName + " Type: " + type
                            + "Msg; " + message);
                        listener.FilteredDispatch(type, message);
                    }
                    catch (Exception e) {
                        Console.WriteLine("SLOTH CLIENT: Exception while dispatching message: " + e.Message + ": "
                            + encodedMessage);
                    }
                }
            });
        }

        public void SendToServer(string encodedMessage) {
            _socketService.SendMessage(_clientSocket, encodedMessage, message=>{});

        }
    }
}