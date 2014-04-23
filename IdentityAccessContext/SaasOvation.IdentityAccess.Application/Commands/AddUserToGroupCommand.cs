namespace SaasOvation.IdentityAccess.Application.Commands {
    public class AddUserToGroupCommand {
        public string TenantId { get; set; }
        public string GroupName { get; set; }
        public string UserName { get; set; }

        public AddUserToGroupCommand(string tenantId, string groupName, string userName) {
            this.TenantId = tenantId;
            this.GroupName = groupName;
            this.UserName = userName;
        }
    }
}