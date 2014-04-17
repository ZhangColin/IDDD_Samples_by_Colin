using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Access.Model {
    public class RoleProvisioned: IDomainEvent {
        public string TenantId { get; private set; }
        public string Name { get; private set; }

        public RoleProvisioned(TenantId tenantId, string name) {
            this.TenantId = tenantId.Id;
            this.Name = name;

            this.OccurredOn = DateTime.Now;
            this.EventVersion = 1;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}