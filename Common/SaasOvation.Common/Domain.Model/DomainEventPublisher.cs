using System;
using System.Collections.Generic;

namespace SaasOvation.Common.Domain.Model {
    public class DomainEventPublisher {
        [ThreadStatic]
        private static DomainEventPublisher _instance;

        public static DomainEventPublisher Instance {
            get {
                if(_instance==null) {
                    _instance = new DomainEventPublisher();
                }

                return _instance;
            }
        }

        private bool _publishing;

        private DomainEventPublisher() {
            this._publishing = false;
        }

        private List<IDomainEventSubscriber<IDomainEvent>> _subscribers;

        public List<IDomainEventSubscriber<IDomainEvent>> Subscribers {
            get { return this._subscribers ?? (this._subscribers = new List<IDomainEventSubscriber<IDomainEvent>>()); }
            set { this._subscribers = value; }
        }

        public void Publish<T>(T domainEvent) where T: IDomainEvent {
            if(!this._publishing && this.HasSubscribers()) {
                try {
                    this._publishing = true;

                    Type eventType = domainEvent.GetType();
                    foreach(IDomainEventSubscriber<IDomainEvent> subscriber in Subscribers) {
                        Type subscribedToType = subscriber.SubscribedToEventType();
                        if(eventType==subscribedToType || subscribedToType==typeof(IDomainEvent)) {
                            subscriber.HandleEvent(domainEvent);
                        }
                    }
                }
                finally {
                    this._publishing = false;
                }
            }
        }

        private bool HasSubscribers() {
            return this._subscribers != null && this.Subscribers.Count != 0;
        }

        public void PublishAll(ICollection<IDomainEvent> domainEvents) {
            foreach(IDomainEvent domainEvent in domainEvents) {
                this.Publish(domainEvent);
            }
        }

        public void Reset() {
            if(!this._publishing) {
                this.Subscribers = null;
            }
        }

        public void Subscribe(IDomainEventSubscriber<IDomainEvent> subscriber)  {
            if(!this._publishing) {
                this.Subscribers.Add(subscriber);
            }
        }

        public void Subscribe<T>(Action<T> handle) where T: IDomainEvent {
            Subscribe(new DomainEventSubscriber<T>(handle));
        }

        class DomainEventSubscriber<TEvent>: IDomainEventSubscriber<IDomainEvent> where TEvent: IDomainEvent {
            private readonly Action<TEvent> _handle;

            public DomainEventSubscriber(Action<TEvent> handle) {
                this._handle = handle;
            }

            public void HandleEvent(IDomainEvent domainEvent) {
                this._handle((TEvent)domainEvent);
            }

            public Type SubscribedToEventType() {
                return typeof(TEvent);
            }
        }
    }
}