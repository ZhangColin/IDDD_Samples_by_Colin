namespace SaasOvation.AgilePm.Application.Products {
    public class RequestProductDiscussionCommand {
        public string TenantId { get; set; }
        public string ProductId { get; set; }

        public RequestProductDiscussionCommand(string tenantId, string productId) {
            this.TenantId = tenantId;
            this.ProductId = productId;
        }
    }
}