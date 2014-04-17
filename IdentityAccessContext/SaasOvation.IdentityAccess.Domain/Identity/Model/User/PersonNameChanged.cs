using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class PersonNameChanged: IDomainEvent {
        public string TenantId { get; private set; }
        public string UserName { get; private set; }
        public FullName Name { get; private set; }

        public PersonNameChanged(TenantId tenantId, string userName, FullName name) {
            this.TenantId = tenantId.Id;
            this.UserName = userName;
            this.Name = name;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}