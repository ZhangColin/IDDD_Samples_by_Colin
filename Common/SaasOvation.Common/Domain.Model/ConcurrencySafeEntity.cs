using System;

namespace SaasOvation.Common.Domain.Model {
    public abstract class ConcurrencySafeEntity: Entity {
        public virtual int ConcurrencyVersion { get; protected set; }

        public virtual void FailWhenConcurrencyVersion(int version) {
            if(version!=ConcurrencyVersion) {
                throw new InvalidOperationException("Concurrency Violation: Stale data detected. Entity was already modified.");
            }
        }
    }
}