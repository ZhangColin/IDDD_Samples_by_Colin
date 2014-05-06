using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using SaasOvation.Common.Domain.Model;
using SaasOvation.Common.Port.Adapter.Messaging.SlothMq;

namespace SaasOvation.Common.Test.Port.Adapter.Messaging.SlothMq {
    [TestFixture]
    public class SlothTest {
        private ExchangePublisher _publisher;
        private TestExchangeListener _testExchangeListener;

        [Test]
        public void TestPublishsubscribe() {
            this._publisher.Publish("my.test.type", "A tiny little message.");
            this._publisher.Publish("my.test.type1", "A slightly bigger message.");
            this._publisher.Publish("my.test.type2", "An even bigger message, still.");
            this._publisher.Publish("my.test.type3", "An even bigger (bigger!) message, still.");

            Thread.Sleep(1000);

            Assert.AreEqual("my.test.type", _testExchangeListener.ReceivedType);
            Assert.AreEqual("A tiny little message.", _testExchangeListener.ReceivedMessage);
            Assert.AreEqual(4, TestExchangeListenerAgain.UniqueMessages.Count);
        }

        [SetUp]
        protected void Setup() {
            DomainEventPublisher.Instance.Reset();

            SlothServer.ExecuteInProcessDetachedServer();

            this._testExchangeListener = new TestExchangeListener();

            SlothClient.Instance.Register(this._testExchangeListener);
//            new TestExchangeListenerAgain();
//            new TestExchangeListenerAgain();
//            new TestExchangeListenerAgain();
            SlothClient.Instance.Register(new TestExchangeListenerAgain());
            SlothClient.Instance.Register(new TestExchangeListenerAgain());
            SlothClient.Instance.Register(new TestExchangeListenerAgain());

            this._publisher = new ExchangePublisher("TestExchange");
        }

        [TearDown]
        private void TearDown() {
//            SlothClient.Instance.CloseAll();
//            SlothServer.Close();
        }

        private class TestExchangeListener: ExchangeListener {
            public string ReceivedType { get; set; }
            public string ReceivedMessage { get; set; }

            public override string ExchangeName {
                get { return "TestExchange"; }
            }

            public override void FilteredDispatch(string type, string textMessage) {
                this.ReceivedType = type;
                this.ReceivedMessage = textMessage;
            }

            public override string Name {
                get { return this.GetType().Name; }
            }

            protected override string[] ListensTo() {
                return new[] { "my.test.type" };
            }
        }

        private class TestExchangeListenerAgain: ExchangeListener {
            private static int _idCount = 0;
            private static HashSet<string> _uniqueMessages = new HashSet<string>();
            public static ISet<string> UniqueMessages {
                get { return _uniqueMessages; }
            }

            private int _id;

            public TestExchangeListenerAgain() {
                this._id = ++_idCount;
            }

            public override string ExchangeName {
                get { return "TestExchange"; }
            }

            public override void FilteredDispatch(string type, string textMessage) {
                _uniqueMessages.Add(type + ":" + textMessage);
            }

            public override string Name {
                get { return this.GetType().Name + "#" + this._id; }
            }

            protected override string[] ListensTo() {
                return null;
            }
        }
    }
}