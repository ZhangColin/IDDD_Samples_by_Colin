namespace SaasOvation.IdentityAccess.Application.Commands {
    public class ProvisionRoleCommand {
        public string TenantId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool SupportsNesting { get; set; }

        public ProvisionRoleCommand(string tenantId, string roleName, string description, bool supportsNesting) {
            this.TenantId = tenantId;
            this.RoleName = roleName;
            this.Description = description;
            this.SupportsNesting = supportsNesting;
        }
    }
}