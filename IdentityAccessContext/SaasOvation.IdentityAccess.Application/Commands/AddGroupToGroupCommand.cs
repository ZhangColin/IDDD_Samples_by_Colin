namespace SaasOvation.IdentityAccess.Application.Commands {
    public class AddGroupToGroupCommand {
        public string TenantId { get; set; }
        public string ChildGroupName { get; set; }
        public string ParentGroupName { get; set; }

        public AddGroupToGroupCommand(string tenantId, string parentGroupName, string childGroupName) {
            this.TenantId = tenantId;
            this.ChildGroupName = childGroupName;
            this.ParentGroupName = parentGroupName;
        }
    }
}