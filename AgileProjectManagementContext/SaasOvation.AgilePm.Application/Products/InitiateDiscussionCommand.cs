namespace SaasOvation.AgilePm.Application.Products {
    public class InitiateDiscussionCommand {
        public string TenantId { get; set; }
        public string DiscussionId { get; set; }
        public string ProductId { get; set; }

        public InitiateDiscussionCommand(string tenantId, string discussionId, string productId) {
            this.TenantId = tenantId;
            this.DiscussionId = discussionId;
            this.ProductId = productId;
        }
    }
}