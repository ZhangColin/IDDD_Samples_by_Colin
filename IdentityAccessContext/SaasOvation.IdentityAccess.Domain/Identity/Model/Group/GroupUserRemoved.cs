using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Group {
    public class GroupUserRemoved : IDomainEvent {
        public string TenantId { get; private set; }
        public string GroupName { get; private set; }
        public string UserName { get; private set; }

        public GroupUserRemoved(TenantId tenantId, string groupName, string userName) {
            this.TenantId = tenantId.Id;
            this.GroupName = groupName;
            this.UserName = userName;

            this.OccurredOn = DateTime.Now;
            this.EventVersion = 1;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}