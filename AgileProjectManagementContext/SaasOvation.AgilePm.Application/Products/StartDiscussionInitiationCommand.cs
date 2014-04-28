namespace SaasOvation.AgilePm.Application.Products {
    public class StartDiscussionInitiationCommand {
        public string TenantId { get; set; }
        public string ProductId { get; set; }

        public StartDiscussionInitiationCommand(string tenantId, string productId) {
            this.TenantId = tenantId;
            this.ProductId = productId;
        }
    }
}