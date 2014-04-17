using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Access.Model {
    public class UserUnassignedFromRole: IDomainEvent {
        public string TenantId { get; private set; }
        public string RoleName { get; private set; }
        public string UserName { get; private set; }

        public UserUnassignedFromRole(TenantId tenantId, string roleName, string userName) {
            this.TenantId = tenantId.Id;
            this.RoleName = roleName;
            this.UserName = userName;

            this.OccurredOn = DateTime.Now;
            this.EventVersion = 1;
        }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}