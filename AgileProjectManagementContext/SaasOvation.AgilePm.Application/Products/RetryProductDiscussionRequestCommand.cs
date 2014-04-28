namespace SaasOvation.AgilePm.Application.Products {
    public class RetryProductDiscussionRequestCommand {
        public string TenantId { get; set; }
        public string ProcessId { get; set; }

        public RetryProductDiscussionRequestCommand(string tenantId, string processId) {
            this.TenantId = tenantId;
            this.ProcessId = processId;
        }
    }
}