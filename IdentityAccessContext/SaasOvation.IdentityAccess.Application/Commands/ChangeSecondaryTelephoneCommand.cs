namespace SaasOvation.IdentityAccess.Application.Commands {
    public class ChangeSecondaryTelephoneCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string Telephone { get; set; }

        public ChangeSecondaryTelephoneCommand(string tenantId, string userName, string telephone) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.Telephone = telephone;
        }
    }
}