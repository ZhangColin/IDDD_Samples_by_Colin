using System;
using SaasOvation.Common.Domain.Model.LongRunningProcess;

namespace SaasOvation.Common.Test.Domain.Model.LongRunningProcess {
    public class TestableTimeConstrainedProcess: AbstractProcess {
        private bool _confirm1;
        private bool _confirm2;

        public TestableTimeConstrainedProcess(string tenantId, ProcessId processId, string description,
            long allowableDuration): base(tenantId, processId, description, allowableDuration) {}

        public void Confirm1() {
            this._confirm1 = true;
            this.CompleteProcess(ProcessCompletionType.NotCompleted);
        }

        public void Confirm2() {
            this._confirm2 = true;
            this.CompleteProcess(ProcessCompletionType.CompleteNormally);
        }

        protected override bool CompletenessVerified() {
            return this._confirm1 && this._confirm2;
        }

        protected override Type ProcessTimedOutEventType() {
            return typeof(TestableTimeConstrainedProcessTimedOut);
        }
    }
}