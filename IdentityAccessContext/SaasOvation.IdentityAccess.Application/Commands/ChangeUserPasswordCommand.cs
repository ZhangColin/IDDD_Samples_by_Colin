namespace SaasOvation.IdentityAccess.Application.Commands {
    public class ChangeUserPasswordCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string ChangedPassword { get; set; }

        public ChangeUserPasswordCommand(string tenantId, string userName, string currentPassword, string changedPassword) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.CurrentPassword = currentPassword;
            this.ChangedPassword = changedPassword;
        }
    }
}