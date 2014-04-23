namespace SaasOvation.IdentityAccess.Application.Commands {
    public class RemoveGroupFromGroupCommand {
        public string TenantId { get; set; }
        public string ParentGroupName { get; set; }
        public string ChildGroupName { get; set; }

        public RemoveGroupFromGroupCommand(string tenantId, string parentGroupName, string childGroupName) {
            this.TenantId = tenantId;
            this.ParentGroupName = parentGroupName;
            this.ChildGroupName = childGroupName;
        }
    }
}