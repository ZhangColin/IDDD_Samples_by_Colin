namespace SaasOvation.AgilePm.Application.Products {
    public class NewProductCommand {
        public string TenantId { get; set; }
        public string ProductOwnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public NewProductCommand(string tenantId, string productOwnerId, string name, string description) {
            this.TenantId = tenantId;
            this.ProductOwnerId = productOwnerId;
            this.Name = name;
            this.Description = description;
        }
    }
}