using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SaasOvation.Common.Notifications;
using SaasOvation.Common.Port.Adapter.Messaging.RabbitMq;

namespace SaasOvation.Common.Test.Port.Adapter.Messaging.RabbitMq {
    [TestFixture]
    public class RabbitMQPipesFiltersTest {
        private ExchangeListener _matchtedPhoneNumberCounter;
        private ExchangeListener _phoneNumberFinder;
        private ExchangeListener _totalPhoneNumbersCounter;
        private PhoneNumberExecutive _phoneNumberExecutive;

        private static string[] _phoneNumbers = {
            "303-555-1212   John",
            "212-555-1212   Joe",
            "718-555-1212   Zoe",
            "720-555-1212   Manny",
            "312-555-1212   Jerry",
            "303-555-9999   Sally"
        };

        private static ConnectionSettings _connectionSettings;

        [Test]
        public void TestPhoneNumbersCounter() {
            string processId = this._phoneNumberExecutive.Start(_phoneNumbers);
            Thread.Sleep(10000);

            PhoneNumberProcess process = this._phoneNumberExecutive.ProcessofId(processId);

            Assert.NotNull(process);
            Assert.AreEqual(2, process.MatchedPhoneNumbers);
            Assert.AreEqual(6, process.TotalPhoneNumbers);
        }

        [SetUp]
        private void SetUp() {
            _connectionSettings = ConnectionSettings.Instance("127.0.0.1", 5672, "rabbitMQSamples", "colin", "123456");
            _phoneNumberExecutive = new PhoneNumberExecutive(_connectionSettings);
            _phoneNumberFinder = new PhoneNumberFinder(_connectionSettings);
            _matchtedPhoneNumberCounter = new MatchtedPhoneNumberCounter(_connectionSettings);
            _totalPhoneNumbersCounter = new TotalPhoneNumbersCounter(_connectionSettings);
        }

        [TearDown]
        private void TearDown() {
            this._phoneNumberExecutive.Close();
            this._phoneNumberFinder.Close();
            this._matchtedPhoneNumberCounter.Close();
            this._totalPhoneNumbersCounter.Close();
        }

        private static void Send(Notification notification) {
            
            Exchange exchange =
                Exchange.FanOutInstance(_connectionSettings,
                    "PhoneNumberExchange", true);

            MessageParameters messageParameters = MessageParameters.DurableTextParameters(exchange.Channel,
                notification.TypeName, notification.NotificationId.ToString(), notification.OccurredOn);

            MessageProducer messageProducer = MessageProducer.CreateProducer(exchange);

            messageProducer.Send(JsonConvert.SerializeObject(notification), messageParameters);
        }

        private class PhoneNumberProcess {
            public string Id { get; private set; }
            public int MatchedPhoneNumbers { get; set; }
            public int TotalPhoneNumbers { get; set; }

            public PhoneNumberProcess() {
                this.Id = Guid.NewGuid().ToString().ToUpper();
                this.MatchedPhoneNumbers = -1;
                this.TotalPhoneNumbers = -1;
            }

            public bool IsCompleted() {
                return this.MatchedPhoneNumbers >= 0 && this.TotalPhoneNumbers >= 0;
            }
        }


        private class PhoneNumberExecutive : ExchangeListener {
            private IDictionary<string, PhoneNumberProcess> _processes;
            private object _lock = new object();

            public PhoneNumberExecutive(ConnectionSettings connectionSettings)
                : base(connectionSettings) {
                this._processes = new Dictionary<string, PhoneNumberProcess>();
            }

            public PhoneNumberProcess ProcessofId(string processId) {
                return this._processes[processId];
            }

            public string Start(IEnumerable<string> phoneNumbers) {
                PhoneNumberProcess phoneNumberProcess = new PhoneNumberProcess();
                lock (_lock) {
                    _processes.Add(phoneNumberProcess.Id, phoneNumberProcess);
                }

                string allPhoneNumbers = "";

                foreach (string phoneNumber in phoneNumbers) {
                    if (!string.IsNullOrEmpty(allPhoneNumbers)) {
                        allPhoneNumbers = allPhoneNumbers + "\n";
                    }
                    allPhoneNumbers = allPhoneNumbers + phoneNumber;
                }

                Notification notification = new Notification(1,
                    new AllPhoneNumbersListed(phoneNumberProcess.Id, allPhoneNumbers));

                Console.WriteLine("Send: {0}; AllPhoneNumbersListed: {1}", phoneNumberProcess.Id, allPhoneNumbers);
                Send(notification);

                Console.WriteLine("STARTED: " + phoneNumberProcess.Id);
                return phoneNumberProcess.Id;
            }

            protected override string ExchangeName {
                get { return "PhoneNumberExchange"; }
            }

            protected override void FilteredDispatch(string type, string textMessage) {
                //Notification notification = JsonConvert.DeserializeObject<Notification>(textMessage);

                PhoneNumberProcess process = null;

                JObject notification = JsonConvert.DeserializeObject<JObject>(textMessage);
                if (textMessage.Contains(typeof(AllPhoneNumbersCounted).FullName)) {
                    Console.WriteLine("Dispatch AllPhoneNumberCounted");
                }
                else if (textMessage.Contains(typeof(MatchedPhoneNumbersCounted).FullName)) {
                    Console.WriteLine("Dispatch MatchedPhoneNumbersCounted");
                }
//                Console.WriteLine(textMessage);
//                Console.WriteLine(typeof(AllPhoneNumbersCounted).FullName);
//                Console.WriteLine(textMessage.Contains(typeof(AllPhoneNumbersCounted).FullName));
                if (textMessage.Contains(typeof(AllPhoneNumbersCounted).FullName)) {
                    AllPhoneNumbersCounted allPhoneNumbersCounted =
                        JsonConvert.DeserializeObject<AllPhoneNumbersCounted>(notification.GetValue("DomainEvent").ToString());
                    string processId = allPhoneNumbersCounted.ProcessId;
                    process = this._processes[processId];
                    process.TotalPhoneNumbers = allPhoneNumbersCounted.TotalPhoneNumbersCount;
                    Console.WriteLine("AllPhoneNumbersCounted...");
                }
                else if (textMessage.Contains(typeof(MatchedPhoneNumbersCounted).FullName)) {
                    MatchedPhoneNumbersCounted matchedPhoneNumbersCounted =
                        JsonConvert.DeserializeObject<MatchedPhoneNumbersCounted>(notification.GetValue("DomainEvent").ToString());
                    string processId = matchedPhoneNumbersCounted.ProcessId;
                    process = this._processes[processId];
                    process.MatchedPhoneNumbers = matchedPhoneNumbersCounted.MatchedPhoneNumbersCount;
                    Console.WriteLine("MatchedPhoneNumbersCounted...");
                }


                //                if(notification.DomainEvent is AllPhoneNumbersCounted) {
                //                    AllPhoneNumbersCounted allPhoneNumbersCounted = notification.DomainEvent as AllPhoneNumbersCounted;
                //                    string processId = allPhoneNumbersCounted.ProcessId;
                //                    process = this._processes[processId];
                //                    process.TotalPhoneNumbers = allPhoneNumbersCounted.TotalPhoneNumbersCount;
                //                    Console.WriteLine("AllPhoneNumbersCounted...");
                //                }
                //                else if(notification.DomainEvent is MatchedPhoneNumbersCounted) {
                //                    MatchedPhoneNumbersCounted matchedPhoneNumbersCounted = notification.DomainEvent as MatchedPhoneNumbersCounted;
                //                    string processId = matchedPhoneNumbersCounted.ProcessId;
                //                    process = this._processes[processId];
                //                    process.MatchedPhoneNumbers = matchedPhoneNumbersCounted.MatchedPhoneNumbersCount;
                //                    Console.WriteLine("MatchedPhoneNumbersCounted...");
                //                }

                if (process.IsCompleted()) {
                    Console.WriteLine("Process: " + process.Id + ":" + process.MatchedPhoneNumbers + " of "
                        + process.TotalPhoneNumbers + " phone numbers fount.");
                }
            }

            protected override string[] ListensTo() {
                return new string[] { typeof(AllPhoneNumbersCounted).FullName, typeof(MatchedPhoneNumbersCounted).FullName };
            }
        }

        private class PhoneNumberFinder : ExchangeListener {
            public PhoneNumberFinder(ConnectionSettings connectionSettings)
                : base(connectionSettings) {
            }

            protected override string ExchangeName {
                get { return "PhoneNumberExchange"; }
            }

            protected override void FilteredDispatch(string type, string textMessage) {
                Console.WriteLine("AllPhoneNumbersListed (to match)...");

                //Notification notification = JsonConvert.DeserializeObject<Notification>(textMessage);
                //AllPhoneNumbersListed allPhoneNumbersListed = (notification.DomainEvent as AllPhoneNumbersListed);

                JObject notification = JsonConvert.DeserializeObject<JObject>(textMessage);
                AllPhoneNumbersListed allPhoneNumbersListed =
                    JsonConvert.DeserializeObject<AllPhoneNumbersListed>(notification.GetValue("DomainEvent").ToString());


                string allPhoneNumbers = allPhoneNumbersListed.PhoneNumbersArray;

                string[] allPhoneNumbersToSearch = allPhoneNumbers.Split('\n');

                string foundPhoneNumbers = "";

                foreach (string phoneNumber in allPhoneNumbersToSearch) {
                    if (phoneNumber.Contains("303-")) {
                        if (!string.IsNullOrEmpty(foundPhoneNumbers)) {
                            foundPhoneNumbers = foundPhoneNumbers + "\n";
                        }
                        foundPhoneNumbers = foundPhoneNumbers + phoneNumber;
                    }
                }

                Notification thisNotification = new Notification(1,
                    new PhoneNumbersMatched(allPhoneNumbersListed.ProcessId, foundPhoneNumbers));

                Console.WriteLine("Send: {0}; PhoneNumbersMatched: {1}", allPhoneNumbersListed.ProcessId, foundPhoneNumbers);
                Send(thisNotification);
            }

            protected override string[] ListensTo() {
                return new string[] { typeof(AllPhoneNumbersListed).FullName };
            }
        }

        private class MatchtedPhoneNumberCounter : ExchangeListener {
            public MatchtedPhoneNumberCounter(ConnectionSettings connectionSettings)
                : base(connectionSettings) {
            }

            protected override string ExchangeName {
                get { return "PhoneNumberExchange"; }
            }

            protected override void FilteredDispatch(string type, string textMessage) {
                Console.WriteLine("PhoneNumbersMatched (to count)...");

                //                Notification notification = JsonConvert.DeserializeObject<Notification>(textMessage);
                //
                //                PhoneNumbersMatched phoneNumbersMatched = (notification.DomainEvent as PhoneNumbersMatched);

                JObject notification = JsonConvert.DeserializeObject<JObject>(textMessage);
                PhoneNumbersMatched phoneNumbersMatched =
                    JsonConvert.DeserializeObject<PhoneNumbersMatched>(notification.GetValue("DomainEvent").ToString());
                string allPhoneNumbers = phoneNumbersMatched.MatchedPhoneNumbers;

                string[] allPhoneNumbersToSearch = allPhoneNumbers.Split('\n');

                Notification thisNotification = new Notification(1,
                    new MatchedPhoneNumbersCounted(phoneNumbersMatched.ProcessId, allPhoneNumbersToSearch.Length));

                Console.WriteLine("Send: {0}; MatchedPhoneNumbersCounted: {1}", phoneNumbersMatched.ProcessId, allPhoneNumbersToSearch.Length);
                Send(thisNotification);
            }

            protected override string[] ListensTo() {
                return new[] { typeof(PhoneNumbersMatched).FullName };
            }
        }

        private class TotalPhoneNumbersCounter : ExchangeListener {
            public TotalPhoneNumbersCounter(ConnectionSettings connectionSettings)
                : base(connectionSettings) {
            }

            protected override string ExchangeName {
                get { return "PhoneNumberExchange"; }
            }

            protected override void FilteredDispatch(string type, string textMessage) {
                Console.WriteLine("AllPhoneNumbersListed (to total)...");

                JObject notification = JsonConvert.DeserializeObject<JObject>(textMessage);
                AllPhoneNumbersListed allPhoneNumbersListed =
                    JsonConvert.DeserializeObject<AllPhoneNumbersListed>(notification.GetValue("DomainEvent").ToString());

                Notification thisNotification = new Notification(1,
                    new AllPhoneNumbersCounted(allPhoneNumbersListed.ProcessId,
                        allPhoneNumbersListed.PhoneNumbersArray.Split('\n').Length));

                Console.WriteLine("Send: {0}; AllPhoneNumbersCounted: {1}", allPhoneNumbersListed.ProcessId, allPhoneNumbersListed.PhoneNumbersArray.Split('\n').Length);
                Send(thisNotification);
            }

            protected override string[] ListensTo() {
                return new[] {
                    typeof(AllPhoneNumbersListed).FullName
                };
            }
        }
    }
}