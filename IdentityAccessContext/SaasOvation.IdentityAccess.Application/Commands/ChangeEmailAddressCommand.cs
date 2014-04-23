namespace SaasOvation.IdentityAccess.Application.Commands {
    public class ChangeEmailAddressCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public ChangeEmailAddressCommand(string tenantId, string userName, string emailAddress) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.EmailAddress = emailAddress;
        }
    }
}