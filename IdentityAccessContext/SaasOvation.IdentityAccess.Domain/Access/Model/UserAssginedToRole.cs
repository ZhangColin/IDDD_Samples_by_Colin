using System;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Access.Model {
    public class UserAssignedToRole: IDomainEvent {
        public string TenantId { get; private set; }
        public string RoleName { get; private set; }
        public string UserName { get; private set; }
        public FullName FullName { get; private set; }
        public EmailAddress EmailAddress { get; private set; }

        public UserAssignedToRole(TenantId tenantId, string roleName, string userName, FullName fullName,
            EmailAddress emailAddress) {
            this.TenantId = tenantId.Id;
            this.RoleName = roleName;
            this.UserName = userName;
            this.FullName = fullName;
            this.EmailAddress = emailAddress;

            this.OccurredOn = DateTime.Now;
            this.EventVersion = 1;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}