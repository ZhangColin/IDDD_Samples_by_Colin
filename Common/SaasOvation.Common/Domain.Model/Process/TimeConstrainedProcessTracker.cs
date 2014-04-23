using System;
using System.Collections.Generic;

namespace SaasOvation.Common.Domain.Model.Process {
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

            this.TimeoutOccursOn = originalStartTime.Ticks + allowableDuration;
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

        private long _timeoutOccursOn;
        public long TimeoutOccursOn {
            get { return this._timeoutOccursOn; }
            private set {
                AssertionConcern.True(value > 0, "Timeout must be greater than zero.");
                this._timeoutOccursOn = value;
            }
        }

        private bool _completed;
        public void Completed() {
            this._completed = true;
        }

        public bool IsCompleted {
            get { return _completed; }
        }

        private bool TotalRetriesReached() {
            return this.RetryCount >= this.TotalRetriesPermitted;
        }

        public bool HasTimedOut() {
            DateTime timeout = new DateTime(this.TimeoutOccursOn);
            return timeout <= DateTime.Now;
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
                        this.TimeoutOccursOn = this.TimeoutOccursOn + this.AllowableDuration;
                    }
                }

                DomainEventPublisher.Instance.Publish(processTimedOut);
            }
        }
        
        private void IncrementRetryCount() {
            this.RetryCount++;
        }

        private ProcessTimedOut ProcessTimedOutEvent() {
            return (ProcessTimedOut)Activator.CreateInstance(Type.GetType(ProcessTimedOutEventType), ProcessId);
        }

        private ProcessTimedOut ProcessTimedOutEventWithRetries() {
            return (ProcessTimedOut)Activator.CreateInstance(
                Type.GetType(ProcessTimedOutEventType), ProcessId, TotalRetriesPermitted, RetryCount);
        }

        public override string ToString() {
            return "TimeConstrainedProcessTracker [" +
                "AllowableDuration=" + AllowableDuration + ", " +
                "Completed=" + _completed + ", " +
                "Description=" + Description + ", " +
                "ProcessId=" + ProcessId + ", " +
                "ProcessInformedOfTimeout=" + ProcessInformedOfTimeout + ", " +
                "ProcessTimedOutEventType=" + ProcessTimedOutEventType + ", " +
                "RetryCount=" + RetryCount + ", " +
                "TenantId=" + TenantId + ", " +
                "TimeConstrainedProcessTrackerId=" + TimeConstrainedProcessTrackerId + ", " +
                "TimeoutOccursOn=" + TimeoutOccursOn + "," +
                "TotalRetriesPermitted=" + TotalRetriesPermitted + "]";
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.ProcessId;
        }
    }
}