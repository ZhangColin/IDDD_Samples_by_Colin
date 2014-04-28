using System;

namespace SaasOvation.Common.Domain.Model.LongRunningProcess {
    public class ProcessTimedOut: IDomainEvent {
        public string TenantId { get; set; }
        public ProcessId ProcessId { get; set; }
        public int TotalRetriesPermitted { get; set; }
        public int RetryCount { get; set; }

        public ProcessTimedOut(string tenantId, ProcessId processId, int totalRetriesPermitted, int retryCount) {
            this.TenantId = tenantId;
            this.ProcessId = processId;
            this.TotalRetriesPermitted = totalRetriesPermitted;
            this.RetryCount = retryCount;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public bool AllowsRetries {
            get { return this.TotalRetriesPermitted > 0; }
        }

        public bool HasFullyTimedOut() {
            return !this.AllowsRetries || this.TotalRetriesReached();
        }

        public bool TotalRetriesReached() {
            return this.RetryCount >= this.TotalRetriesPermitted;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}