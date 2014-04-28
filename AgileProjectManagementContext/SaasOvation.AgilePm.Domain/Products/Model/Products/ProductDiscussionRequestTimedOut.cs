using SaasOvation.Common.Domain.Model.LongRunningProcess;

namespace SaasOvation.AgilePm.Domain.Products.Model.Products {
    public class ProductDiscussionRequestTimedOut: ProcessTimedOut {
        public ProductDiscussionRequestTimedOut(string tenantId, ProcessId processId, int totalRetriesPermitted,
            int retryCount): base(tenantId, processId, totalRetriesPermitted, retryCount) {
            
        }
    }
}