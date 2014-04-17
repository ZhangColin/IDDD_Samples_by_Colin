using System;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant {
    public class TenantActivated: IDomainEvent {
        public string TenantId { get; private set; }

        public TenantActivated(TenantId tenantId) {
            this.TenantId = tenantId.Id;

            this.OccurredOn = DateTime.Now;
            this.EventVersion = 1;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}