using System;
using System.Collections.Generic;

namespace SaasOvation.Common.Domain.Model.LongRunningProcess {
    public abstract class AbstractProcess: ConcurrencySafeEntity, IProcess {
        public string TenantId { get; set; }

        public long AllowableDuration { get; private set; }
        public ProcessCompletionType ProcessCompletionType { get; private set; }
        public ProcessId ProcessId { get; private set; }
        public string Description { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime? TimedOutDate { get; private set; }
        public int TotalRetriesPermitted { get; private set; }

        protected AbstractProcess(string tenantId, ProcessId processId, string description) {
            AssertionConcern.NotNull(processId, "Process id must be provided.");
            AssertionConcern.NotEmpty(tenantId, "Tenant id must be provided.");

            this.TenantId = tenantId;
            this.ProcessId = processId;
            this.Description = description;

            this.StartTime = DateTime.Now;
            this.ProcessCompletionType = ProcessCompletionType.NotCompleted;
        }

        protected AbstractProcess(string tenantId, ProcessId processId, string description, long allowableDuration)
            : this(tenantId, processId, description) {
            AssertionConcern.True(allowableDuration > 0, "The allowable duration must be greater than zero.");
            this.AllowableDuration = allowableDuration;
        }

        protected AbstractProcess(string tenantId, ProcessId processId, string description, long allowableDuration, int totalRetriesPermitted)
            : this(tenantId, processId, description, allowableDuration) {
            this.TotalRetriesPermitted = totalRetriesPermitted;
        }

        public bool CanTimeout {
            get { return this.AllowableDuration > 0; }
        }

        public long CurrentDuration {
            get { return this.CalculateTotalCurrentDuration(DateTime.Now); }
        }

        public bool DidProcessingComplete {
            get { return this.IsCompleted && !this.IsTimedOut; }
        }

        public void InformTimeout(DateTime timedOutDate) {
            AssertionConcern.True(this.HasProcessTimedOut(timedOutDate),
                "The date " + timedOutDate + " does not indicate a valid timeout");
            this.ProcessCompletionType = ProcessCompletionType.TimeOut;
            this.TimedOutDate = timedOutDate;
        }

        public bool IsTimedOut {
            get { return this.TimedOutDate != null; }
        }

        public bool IsCompleted {
            get { return !this.NotCompleted; }
        }

        public bool NotCompleted {
            get { return this.ProcessCompletionType == ProcessCompletionType.NotCompleted; }
        }

        public TimeConstrainedProcessTracker TimeConstrainedProcessTracker {
            get {
                AssertionConcern.True(this.CanTimeout, "Process does not timeout.");

                TimeConstrainedProcessTracker tracker = new TimeConstrainedProcessTracker(this.TenantId, this.ProcessId,
                    this.Description, this.StartTime, this.AllowableDuration, this.TotalRetriesPermitted,
                    this.ProcessTimedOutEventType().FullName);

                return tracker;
            }
        }

        public long TotalAllowableDuration {
            get {
                long totalAllowableDuration = this.AllowableDuration;
                if (this.TotalRetriesPermitted > 0) {
                    totalAllowableDuration *= this.TotalRetriesPermitted;
                }

                return totalAllowableDuration;
            }
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.ProcessId;
        }

        protected void CompleteProcess(ProcessCompletionType processCompletionType) {
            if (!this.IsCompleted && this.CompletenessVerified()) {
                this.ProcessCompletionType = processCompletionType;
            }
        }

        protected abstract bool CompletenessVerified();
        protected abstract Type ProcessTimedOutEventType();

        private long CalculateTotalCurrentDuration(DateTime dateFollowingStartTime) {
            return dateFollowingStartTime.Ticks - this.StartTime.Ticks;
        }

        private bool HasProcessTimedOut(DateTime timedOutDate) {
            return this.CalculateTotalCurrentDuration(timedOutDate) >= this.TotalAllowableDuration;
        }


    }
}