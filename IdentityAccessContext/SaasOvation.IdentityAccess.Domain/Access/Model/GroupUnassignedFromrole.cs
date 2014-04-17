using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Access.Model {
    public class GroupUnassignedFromRole : IDomainEvent {
        public string TenantId { get; private set; }
        public string RoleName { get; private set; }
        public string GroupName { get; private set; }

        public GroupUnassignedFromRole(TenantId tenantId, string roleName, string groupName) {
            this.TenantId = tenantId.Id;
            this.RoleName = roleName;
            this.GroupName = groupName;

            this.OccurredOn = DateTime.Now;
            this.EventVersion = 1;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}