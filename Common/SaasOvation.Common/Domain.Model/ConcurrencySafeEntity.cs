using System;

namespace SaasOvation.Common.Domain.Model {
    public abstract class ConcurrencySafeEntity: EntityWithCompositeId {
        public int ConcurrencyVersion { get; protected set; }

        public void FailWhenConcurrencyVersion(int version) {
            if(version!=ConcurrencyVersion) {
                throw new InvalidOperationException("Concurrency Violation: Stale data detected. Entity was already modified.");
            }
        }
    }
}