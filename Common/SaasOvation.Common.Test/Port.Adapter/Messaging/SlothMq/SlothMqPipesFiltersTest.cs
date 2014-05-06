using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SaasOvation.Common.Domain.Model;
using SaasOvation.Common.Notifications;
using SaasOvation.Common.Port.Adapter.Messaging.SlothMq;

namespace SaasOvation.Common.Test.Port.Adapter.Messaging.SlothMq {
    [TestFixture]
    public class SlothMqPipesFiltersTest {
        private static ExchangePublisher _publisher;

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

        [Test]
        public void TestPhoneNumbersCounter() {
            string processId = this._phoneNumberExecutive.Start(_phoneNumbers);

            Thread.Sleep(1000);

            PhoneNumberProcess process = this._phoneNumberExecutive.ProcessofId(processId);

            Assert.NotNull(process);
            Assert.AreEqual(2, process.MatchedPhoneNumbers);
            Assert.AreEqual(6, process.TotalPhoneNumbers);
        }

        [SetUp]
        private void SetUp() {
            DomainEventPublisher.Instance.Reset();

            SlothServer.ExecuteInProcessDetachedServer();

            Thread.Sleep(500);

            _phoneNumberExecutive = new PhoneNumberExecutive();
            _phoneNumberFinder = new PhoneNumberFinder();
            _matchtedPhoneNumberCounter = new MatchtedPhoneNumberCounter();
            _totalPhoneNumbersCounter = new TotalPhoneNumbersCounter();

            SlothClient.Instance.Register(this._phoneNumberExecutive);
            SlothClient.Instance.Register(this._phoneNumberFinder);
            SlothClient.Instance.Register(this._matchtedPhoneNumberCounter);
            SlothClient.Instance.Register(this._totalPhoneNumbersCounter);

            _publisher = new ExchangePublisher("PhoneNumberExchange");
        }

        [TearDown]
        private void TearDown() {
            this._phoneNumberExecutive.Close();
            this._phoneNumberFinder.Close();
            this._matchtedPhoneNumberCounter.Close();
            this._totalPhoneNumbersCounter.Close();

//            SlothClient.Instance.CloseAll();
        }

        private static void Send(Notification notification) {
            string serializedNotification = JsonConvert.SerializeObject(notification);
            _publisher.Publish(notification.TypeName, serializedNotification);
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


        private class PhoneNumberExecutive: ExchangeListener {
            private IDictionary<string, PhoneNumberProcess> _processes;
            private object _lock = new object();

            public PhoneNumberExecutive() {
                this._processes = new Dictionary<string, PhoneNumberProcess>();
            }

            public PhoneNumberProcess ProcessofId(string processId) {
                return this._processes[processId];
            }

            public string Start(IEnumerable<string> phoneNumbers) {
                PhoneNumberProcess phoneNumberProcess = new PhoneNumberProcess();
                lock(_lock) {
                    _processes.Add(phoneNumberProcess.Id, phoneNumberProcess);
                }

                string allPhoneNumbers = "";

                foreach(string phoneNumber in phoneNumbers) {
                    if(!string.IsNullOrEmpty(allPhoneNumbers)) {
                        allPhoneNumbers = allPhoneNumbers + "\n";
                    }
                    allPhoneNumbers = allPhoneNumbers + phoneNumber;
                }

                Notification notification = new Notification(1,
                    new AllPhoneNumbersListed(phoneNumberProcess.Id, allPhoneNumbers));

                Send(notification);

                Console.WriteLine("STARTED: " + phoneNumberProcess.Id);
                return phoneNumberProcess.Id;
            }

            public override string ExchangeName {
                get { return "PhoneNumberExchange"; }
            }

            public override void FilteredDispatch(string type, string textMessage) {
                //Notification notification = JsonConvert.DeserializeObject<Notification>(textMessage);

                PhoneNumberProcess process = null;

                JObject notification = JsonConvert.DeserializeObject<JObject>(textMessage);
                if(textMessage.Contains(typeof(AllPhoneNumbersCounted).FullName)) {
                    AllPhoneNumbersCounted allPhoneNumbersCounted =
                        JsonConvert.DeserializeObject<AllPhoneNumbersCounted>(notification.GetValue("DomainEvent").ToString());
                    string processId = allPhoneNumbersCounted.ProcessId;
                    process = this._processes[processId];
                    process.TotalPhoneNumbers = allPhoneNumbersCounted.TotalPhoneNumbersCount;
                    Console.WriteLine("AllPhoneNumbersCounted...");
                }
                else if(textMessage.Contains(typeof(MatchedPhoneNumbersCounted).FullName)) {
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

                if(process.IsCompleted()) {
                    Console.WriteLine("Process: " + process.Id + ":" + process.MatchedPhoneNumbers + " of "
                        + process.TotalPhoneNumbers + " phone numbers fount.");
                }
            }

            public override string Name {
                get { return this.GetType().Name; }
            }

            protected override string[] ListensTo() {
                return new string[]
                {typeof(AllPhoneNumbersCounted).FullName, typeof(MatchedPhoneNumbersCounted).FullName};
            }
        }

        private class PhoneNumberFinder: ExchangeListener {
            public override string ExchangeName {
                get { return "PhoneNumberExchange"; }
            }

            public override void FilteredDispatch(string type, string textMessage) {
                Console.WriteLine("AllPhoneNumbersListed (to match)...");

                //Notification notification = JsonConvert.DeserializeObject<Notification>(textMessage);
                //AllPhoneNumbersListed allPhoneNumbersListed = (notification.DomainEvent as AllPhoneNumbersListed);
                
                JObject notification = JsonConvert.DeserializeObject<JObject>(textMessage);
                AllPhoneNumbersListed allPhoneNumbersListed =
                    JsonConvert.DeserializeObject<AllPhoneNumbersListed>(notification.GetValue("DomainEvent").ToString());

                
                string allPhoneNumbers = allPhoneNumbersListed.PhoneNumbersArray;

                string[] allPhoneNumbersToSearch = allPhoneNumbers.Split('\n');

                string foundPhoneNumbers = "";

                foreach(string phoneNumber in allPhoneNumbersToSearch) {
                    if(phoneNumber.Contains("303-")) {
                        if(!string.IsNullOrEmpty(foundPhoneNumbers)) {
                            foundPhoneNumbers = foundPhoneNumbers + "\n";
                        }
                        foundPhoneNumbers = foundPhoneNumbers + phoneNumber;
                    }
                }

                Notification thisNotification = new Notification(1,
                    new PhoneNumbersMatched(allPhoneNumbersListed.ProcessId, foundPhoneNumbers));

                Send(thisNotification);
            }

            public override string Name {
                get { return this.GetType().Name; }
            }

            protected override string[] ListensTo() {
                return new string[] { typeof(AllPhoneNumbersListed).FullName };
            }
        }

        private class MatchtedPhoneNumberCounter: ExchangeListener {
            public override string ExchangeName {
                get { return "PhoneNumberExchange"; }
            }

            public override void FilteredDispatch(string type, string textMessage) {
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

                Send(thisNotification);
            }

            public override string Name {
                get { return this.GetType().Name; }
            }

            protected override string[] ListensTo() {
                return new []{typeof(PhoneNumbersMatched).FullName};
            }
        }

        private class TotalPhoneNumbersCounter: ExchangeListener {
            public override string ExchangeName {
                get { return "PhoneNumberExchange"; }
            }

            public override void FilteredDispatch(string type, string textMessage) {
                Console.WriteLine("AllPhoneNumbersListed(to total)...");

                JObject notification = JsonConvert.DeserializeObject<JObject>(textMessage);
                AllPhoneNumbersListed allPhoneNumbersListed =
                    JsonConvert.DeserializeObject<AllPhoneNumbersListed>(notification.GetValue("DomainEvent").ToString());

                Notification thisNotification = new Notification(1,
                    new AllPhoneNumbersCounted(allPhoneNumbersListed.ProcessId,
                        allPhoneNumbersListed.PhoneNumbersArray.Split('\n').Length));

                Send(thisNotification);
            }

            public override string Name {
                get { return this.GetType().Name; }
            }

            protected override string[] ListensTo() {
                return new[] {
                    typeof(AllPhoneNumbersListed).FullName
                };
            }
        }
    }
}