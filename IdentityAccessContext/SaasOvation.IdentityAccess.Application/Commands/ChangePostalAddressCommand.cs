namespace SaasOvation.IdentityAccess.Application.Commands {
    public class ChangePostalAddressCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string AddressStreetAddress { get; set; }
        public string AddressCity { get; set; }
        public string AddressStateProvince { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCountryCode { get; set; }

        public ChangePostalAddressCommand(string tenantId, string userName, string addressStreetAddress,
            string addressCity, string addressStateProvince, string addressPostalCode, string addressCountryCode) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.AddressStreetAddress = addressStreetAddress;
            this.AddressCity = addressCity;
            this.AddressStateProvince = addressStateProvince;
            this.AddressPostalCode = addressPostalCode;
            this.AddressCountryCode = addressCountryCode;
        }
    }
}