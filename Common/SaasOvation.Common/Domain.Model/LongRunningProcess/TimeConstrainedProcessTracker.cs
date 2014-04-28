using System;
using System.Collections.Generic;

namespace SaasOvation.Common.Domain.Model.LongRunningProcess {
    public class TimeConstrainedProcessTracker: ConcurrencySafeEntity {
        

        public TimeConstrainedProcessTracker(string tenantId, ProcessId processId, string description,
            DateTime originalStartTime, long allowableDuration, int totalRetriesPermitted,
            string processTimedOutEventType) {
                AssertionConcern.NotEmpty(tenantId, "TenantId is required.");
                AssertionConcern.NotNull(processId, "ProcessId is required.");
                AssertionConcern.NotEmpty(description, "Description is required.");
                AssertionConcern.Length(description, 1, 100, "Description must be 1 to 100 characters in length.");
                AssertionConcern.True(allowableDuration > 0, "The allowable duration must be greater than zero.");
                AssertionConcern.True(totalRetriesPermitted > 0, "Total retries must be greater than or equal to zero.");
                AssertionConcern.NotEmpty(processTimedOutEventType, "ProcessTimedOutEventType is required.");

            this.TenantId = tenantId;
            this.ProcessId = processId;
            this.Description = description;
            this.AllowableDuration = allowableDuration;
            this.TotalRetriesPermitted = totalRetriesPermitted;
            this.ProcessTimedOutEventType = processTimedOutEventType;

            this.ProcessInformedOfTimeout = false;
            this.TimeConstrainedProcessTrackerId = -1L;

            this.SetTimeoutOccursOn(originalStartTime.Ticks + allowableDuration);
        }

        public string TenantId { get; private set; }
        public ProcessId ProcessId { get; private set; }
        public string Description { get; private set; }
        public long AllowableDuration { get; private set; }
        private int TotalRetriesPermitted { get; set; }
        public string ProcessTimedOutEventType { get; set; }

        public long TimeConstrainedProcessTrackerId { get; private set; }
        public bool ProcessInformedOfTimeout { get; private set; }
        public int RetryCount { get; private set; }

        public long TimeoutOccursOn { get; private set; } 

        public bool Completed { get; private set; }

        public void MarkProcessCompleted() {
            this.Completed = true;
        }

        private bool TotalRetriesReached() {
            return this.RetryCount >= this.TotalRetriesPermitted;
        }

        public bool HasTimedOut() {
            return this.TimeoutOccursOn <= DateTime.Now.Ticks;
        }

        public void InformProcessTimedOut() {
            if(!this.ProcessInformedOfTimeout && this.HasTimedOut()) {
                ProcessTimedOut processTimedOut = null;

                if(this.TotalRetriesPermitted==0) {
                    processTimedOut = this.ProcessTimedOutEvent();
                    this.ProcessInformedOfTimeout = true;
                }
                else {
                    this.IncrementRetryCount();
                    processTimedOut = this.ProcessTimedOutEventWithRetries();

                    if(this.TotalRetriesReached()) {
                        this.ProcessInformedOfTimeout = true;
                    }
                    else {
                        this.SetTimeoutOccursOn(this.TimeoutOccursOn + this.AllowableDuration);
                    }
                }

                DomainEventPublisher.Instance.Publish(processTimedOut);
            }
        }

        private void SetTimeoutOccursOn(long timeoutOccursOn) {
            AssertionConcern.True(timeoutOccursOn > 0, "Timeout must be greater than zero.");
            this.TimeoutOccursOn = timeoutOccursOn;
        }

        private void IncrementRetryCount() {
            this.RetryCount++;
        }

        private ProcessTimedOut ProcessTimedOutEvent() {
            return (ProcessTimedOut)Activator.CreateInstance(Type.GetType(this.ProcessTimedOutEventType), this.ProcessId);
        }

        private ProcessTimedOut ProcessTimedOutEventWithRetries() {
            return (ProcessTimedOut)Activator.CreateInstance(
                Type.GetType(this.ProcessTimedOutEventType), this.ProcessId, this.TotalRetriesPermitted, this.RetryCount);
        }

        public override string ToString() {
            return "TimeConstrainedProcessTracker [" +
                "AllowableDuration=" + this.AllowableDuration + ", " +
                "Completed=" + this.Completed + ", " +
                "Description=" + this.Description + ", " +
                "ProcessId=" + this.ProcessId + ", " +
                "ProcessInformedOfTimeout=" + this.ProcessInformedOfTimeout + ", " +
                "ProcessTimedOutEventType=" + this.ProcessTimedOutEventType + ", " +
                "RetryCount=" + this.RetryCount + ", " +
                "TenantId=" + this.TenantId + ", " +
                "TimeConstrainedProcessTrackerId=" + this.TimeConstrainedProcessTrackerId + ", " +
                "TimeoutOccursOn=" + this.TimeoutOccursOn + "," +
                "TotalRetriesPermitted=" + this.TotalRetriesPermitted + "]";
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.ProcessId;
        }
    }
}