namespace SaasOvation.IdentityAccess.Application.Commands {
    public class AssignUserToRoleCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }

        public AssignUserToRoleCommand(string tenantId, string userName, string roleName) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.RoleName = roleName;
        }
    }
}