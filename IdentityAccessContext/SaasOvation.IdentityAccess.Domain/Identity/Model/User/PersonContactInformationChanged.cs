using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class PersonContactInformationChanged:IDomainEvent {
        public string TenantId { get; private set; }
        public string UserName { get; private set; }
        public ContactInformation ContactInformation { get; private set; }

        public PersonContactInformationChanged(TenantId tenantId, string userName, ContactInformation contactInformation) {
            this.TenantId = tenantId.Id;
            this.UserName = userName;
            this.ContactInformation = contactInformation;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}