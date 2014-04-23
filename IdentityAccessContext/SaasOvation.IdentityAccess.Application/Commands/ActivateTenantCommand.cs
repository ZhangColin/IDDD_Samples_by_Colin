namespace SaasOvation.IdentityAccess.Application.Commands {
    public class ActivateTenantCommand {
        public string TenantId { get; set; }

        public ActivateTenantCommand(string tenantId) {
            this.TenantId = tenantId;
        }
    }
}