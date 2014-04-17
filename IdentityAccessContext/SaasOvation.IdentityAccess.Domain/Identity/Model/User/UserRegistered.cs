using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class UserRegistered: IDomainEvent {
        public string TenantId { get; private set; }
        public string UserName { get; private set; }
        public FullName Name { get; private set; }
        public EmailAddress EmailAddress { get; private set; }

        public UserRegistered(TenantId tenantId, string userName, FullName name, EmailAddress emailAddress) {
            this.TenantId = tenantId.Id;
            this.UserName = userName;
            this.Name = name;
            this.EmailAddress = emailAddress;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}