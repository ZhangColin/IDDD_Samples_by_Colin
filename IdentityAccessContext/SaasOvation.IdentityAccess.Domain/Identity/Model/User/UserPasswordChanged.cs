using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class UserPasswordChanged: IDomainEvent {
        public string TenantId { get; private set; }
        public string UserName { get; set; }

        public UserPasswordChanged(TenantId tenantId, string userName) {
            this.TenantId = tenantId.Id;
            this.UserName = userName;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}