namespace SaasOvation.IdentityAccess.Application.Commands {
    public class ChangeUserPersonalNameCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ChangeUserPersonalNameCommand(string tenantId, string userName, string firstName, string lastName) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}