using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Events.Sourcing {
    public interface IEventStream {
        IList<IDomainEvent> Events { get; }
        int Version { get; }
    }
}