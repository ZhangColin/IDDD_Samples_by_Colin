namespace SaasOvation.IdentityAccess.Application.Commands {
    public class ChangeContactInfoCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PrimaryTelephone { get; set; }
        public string SecondaryTelephone { get; set; }
        public string AddressStreetAddress { get; set; }
        public string AddressCity { get; set; }
        public string AddressStateProvince { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCountryCode { get; set; }

        public ChangeContactInfoCommand(string tenantId, string userName, string emailAddress, string primaryTelephone,
            string secondaryTelephone, string addressStreetAddress, string addressCity, string addressStateProvince,
            string addressPostalCode, string addressCountryCode) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.EmailAddress = emailAddress;
            this.PrimaryTelephone = primaryTelephone;
            this.SecondaryTelephone = secondaryTelephone;
            this.AddressStreetAddress = addressStreetAddress;
            this.AddressCity = addressCity;
            this.AddressStateProvince = addressStateProvince;
            this.AddressPostalCode = addressPostalCode;
            this.AddressCountryCode = addressCountryCode;
        }
    }
}