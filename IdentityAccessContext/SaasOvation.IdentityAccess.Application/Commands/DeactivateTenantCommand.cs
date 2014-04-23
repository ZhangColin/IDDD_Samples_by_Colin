namespace SaasOvation.IdentityAccess.Application.Commands {
    public class DeactivateTenantCommand {
        public string TenantId { get; set; }

        public DeactivateTenantCommand(string tenantId) {
            this.TenantId = tenantId;
        }
    }
}