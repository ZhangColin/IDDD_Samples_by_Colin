using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Events.Sourcing {
    public interface IEventStore {
        void AppendWith(EventStreamId startingEventStreamId, IList<IDomainEvent> events);
        void Close();
        IList<DispatchableDomainEvent> EventsSince(long lastReceivedEvent);
        IEventStream EventStreamSince(EventStreamId eventStreamId);
        IEventStream FullEventStreamFor(EventStreamId eventStreamId);
        // 主要用于测试
        void Purge();
        void RegisterEventNotifiable(IEventNotifiable eventNotifiable);
    }
}