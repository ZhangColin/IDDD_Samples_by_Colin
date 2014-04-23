namespace SaasOvation.IdentityAccess.Application.Commands {
    public class AuthenticateUserCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public AuthenticateUserCommand(string tenantId, string userName, string password) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.Password = password;
        }
    }
}