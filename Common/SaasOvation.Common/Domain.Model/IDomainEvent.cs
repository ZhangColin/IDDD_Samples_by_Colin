using System;

namespace SaasOvation.Common.Domain.Model {
    public interface IDomainEvent {
        int EventVersion { get; set; }
        DateTime OccurredOn { get; set; }
    }
}