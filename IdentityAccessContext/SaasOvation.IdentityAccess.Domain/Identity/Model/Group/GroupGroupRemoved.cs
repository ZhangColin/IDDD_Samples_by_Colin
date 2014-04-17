using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Group {
    public class GroupGroupRemoved : IDomainEvent {
        public string TenantId { get; private set; }
        public string GroupName { get; private set; }
        public string NestedGroupName { get; private set; }

        public GroupGroupRemoved(TenantId tenantId, string groupName, string nestedGroupName) {
            this.TenantId = tenantId.Id;
            this.GroupName = groupName;
            this.NestedGroupName = nestedGroupName;

            this.OccurredOn = DateTime.Now;
            this.EventVersion = 1;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}