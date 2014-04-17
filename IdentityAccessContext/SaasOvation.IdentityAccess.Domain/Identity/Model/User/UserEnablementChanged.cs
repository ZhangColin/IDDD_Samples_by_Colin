using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class UserEnablementChanged: IDomainEvent {
        public string TenantId { get; private set; }
        public string UserName { get; private set; }
        public Enablement Enablement { get; private set; }

        public UserEnablementChanged(TenantId tenantId, string userName, Enablement enablement) {
            this.TenantId = tenantId.Id;
            this.UserName = userName;
            this.Enablement = enablement;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}