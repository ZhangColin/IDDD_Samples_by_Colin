using SaasOvation.Common.Domain.Model.LongRunningProcess;

namespace SaasOvation.Common.Test.Domain.Model.LongRunningProcess {
    public class TestableTimeConstrainedProcessTimedOut: ProcessTimedOut {
        public TestableTimeConstrainedProcessTimedOut(string tenantId, ProcessId processId, int totalRetriesPermitted,
            int retryCount): base(tenantId, processId, totalRetriesPermitted, retryCount) {}

        public TestableTimeConstrainedProcessTimedOut(string tenantId, ProcessId processId): 
            base(tenantId, processId, 0, 0) {}
    }
}