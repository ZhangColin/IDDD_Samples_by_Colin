using System;

namespace SaasOvation.IdentityAccess.Application.Commands {
    public class RegisterUserCommand {
        public string TenantId { get; set; }
        public string InvitationIdentifier { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Enabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EmailAddress { get; set; }
        public string PrimaryTelephone { get; set; }
        public string SecondaryTelephone { get; set; }
        public string AddressStreetAddress { get; set; }
        public string AddressCity { get; set; }
        public string AddressStateProvince { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCountryCode { get; set; }

        public RegisterUserCommand(string tenantId, string invitationIdentifier, string userName, string password,
            string firstName, string lastName, bool enabled, DateTime startDate, DateTime endDate, string emailAddress,
            string primaryTelephone, string secondaryTelephone, string addressStreetAddress, string addressCity,
            string addressStateProvince, string addressPostalCode, string addressCountryCode) {
            this.TenantId = tenantId;
            this.InvitationIdentifier = invitationIdentifier;
            this.UserName = userName;
            this.Password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Enabled = enabled;
            this.StartDate = startDate;
            this.EndDate = endDate;
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