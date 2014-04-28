using System;

namespace SaasOvation.AgilePm.Application.Products {
    public class TimeOutProductDiscussionRequestCommand {
        public string TenantId { get; set; }
        public string ProcessId { get; set; }
        public DateTime TimeOutDate { get; set; }

        public TimeOutProductDiscussionRequestCommand(string tenantId, string processId, DateTime timeOutDate) {
            this.TenantId = tenantId;
            this.ProcessId = processId;
            this.TimeOutDate = timeOutDate;
        }
    }
}