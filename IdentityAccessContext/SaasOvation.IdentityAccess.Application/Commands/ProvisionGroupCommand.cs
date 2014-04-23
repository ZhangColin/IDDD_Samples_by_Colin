namespace SaasOvation.IdentityAccess.Application.Commands {
    public class ProvisionGroupCommand {
        public string TenantId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }

        public ProvisionGroupCommand(string tenantId, string groupName, string description) {
            this.TenantId = tenantId;
            this.GroupName = groupName;
            this.Description = description;
        }
    }
}