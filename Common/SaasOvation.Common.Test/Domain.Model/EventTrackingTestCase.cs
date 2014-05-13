using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;
using SaasOvation.Common.Domain.Model;
using SaasOvation.Common.Notifications;
using SaasOvation.Common.Port.Adapter.Messaging;
using SaasOvation.Common.Port.Adapter.Messaging.RabbitMq;
using SaasOvation.Common.Port.Adapter.Messaging.SlothMq;
using ExchangeListener = SaasOvation.Common.Port.Adapter.Messaging.SlothMq.ExchangeListener;

namespace SaasOvation.Common.Test.Domain.Model {
    public abstract class EventTrackingTestCase {
        protected TestAgilePmSlothMqExchangeListener AgilePmSlothMqExchangeListener;
        protected TestCollaborationSlothMqExchangeListener CollaborationSlothMqExchangeListener;
        protected TestIdentityAccessSlothMqExchangeListener IdentityAccessSlothMqExchangeListener;

        private static List<IDomainEvent> _handledEvents;
        private static IDictionary<string, string> _handledNotifications;

        protected void ExpectedEvent<T>() {
            this.ExpectedEvent<T>(1);
        }

        protected void ExpectedEvent<T>(int total) {
            int count = 0;
            foreach(IDomainEvent domainEvent in _handledEvents) {
                if(domainEvent is T) {
                    count++;
                }
            }

            
            Assert.AreEqual(total, count, "Expected {0} {1} events, but handled {2} events: {3}", total, typeof(T).Name,
                _handledEvents.Count, string.Join(",", _handledEvents));
        }

        protected void ExpectedEvents(int eventCount) {
            Assert.AreEqual(eventCount, _handledEvents.Count, "Expected {0} events, but handled {1} events: {2}",
                eventCount, _handledEvents.Count, string.Join(",", _handledEvents));
        }

        protected void ExpectedNotification<T>() {
            this.ExpectedNotification<T>(1);
        }

        protected void ExpectedNotification<T>(int total) {
            Thread.Sleep(200);

            int count = 0;
            foreach(string type in _handledNotifications.Values) {
                if(type==typeof(T).Name) {
                    count++;
                }
            }

            Assert.AreEqual(total, count, "Expected {0} {1} notifications, but handled {2} notifications: {3}",
                total, typeof(T).Name, _handledNotifications.Count, string.Join(",", _handledNotifications.Values));
        }

        protected void ExpectedNotifications(int notificationCount) {
            Thread.Sleep(200);

            Assert.AreEqual(notificationCount, _handledNotifications.Count, 
                "Expected {0} notifications, but handled {1} notifications: {2}",
                notificationCount, _handledNotifications.Count, string.Join(",", _handledNotifications.Values));
        }

        [SetUp]
        public void SetUp() {
            Thread.Sleep(100);
            SlothServer.ExecuteInProcessDetachedServer();

            Thread.Sleep(100);
            DomainEventPublisher.Instance.Reset();

            DomainEventPublisher.Instance.Subscribe<IDomainEvent>(e=>_handledEvents.Add(e));

            _handledEvents = new List<IDomainEvent>();
            _handledNotifications = new Dictionary<string, string>();

            this.AgilePmSlothMqExchangeListener = new TestAgilePmSlothMqExchangeListener();
            this.CollaborationSlothMqExchangeListener = new TestCollaborationSlothMqExchangeListener();
            this.IdentityAccessSlothMqExchangeListener = new TestIdentityAccessSlothMqExchangeListener();

            Thread.Sleep(200);

        }

        [TearDown]
        protected void TearDown() {
//            this.AgilePmSlothMqExchangeListener.Close();
//            this.CollaborationSlothMqExchangeListener.Close();
//            this.IdentityAccessSlothMqExchangeListener.Close();

//            SlothClient.Instance.CloseAll();
        }

        protected abstract class TestSlothMqExchangeListener: ExchangeListener {
            public override void FilteredDispatch(string type, string textMessage) {
                Notification notification = JsonConvert.DeserializeObject<Notification>(textMessage);
                _handledNotifications.Add(notification.NotificationId.ToString(), type);
            }

            public override string Name {
                get { return this.GetType().Name; }
            }

            protected override string[] ListensTo() {
                return new string[0];
            }
        }

        protected class TestAgilePmSlothMqExchangeListener: TestSlothMqExchangeListener {
            public override string ExchangeName {
                get { return Exchanges.AgilePmExchangeName; }
            }
        }
        
        protected class TestIdentityAccessSlothMqExchangeListener: TestSlothMqExchangeListener {
            public override string ExchangeName {
                get { return Exchanges.IdentityAccessExchangeName; }
            }
        }
        
        protected class TestCollaborationSlothMqExchangeListener: TestSlothMqExchangeListener {
            public override string ExchangeName {
                get { return Exchanges.CollaborationExchangeName; }
            }
        }
    }
}