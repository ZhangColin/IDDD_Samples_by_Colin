using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Group {
    public class GroupProvisioned : IDomainEvent {
        public string Name { get; private set; }
        public string TenantId { get; private set; }

        public GroupProvisioned(TenantId tenantId, string name) {
            this.Name = name;
            this.TenantId = tenantId.Id;

            this.OccurredOn = DateTime.Now;
            this.EventVersion = 1;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}