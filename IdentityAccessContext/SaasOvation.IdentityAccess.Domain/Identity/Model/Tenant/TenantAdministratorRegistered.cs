using System;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant {
    public class TenantAdministratorRegistered: IDomainEvent {
        public string TenantId { get; private set; }
        public string Name { get; private set; }
        public FullName AdministorName { get; private set; }
        public string TemporaryPassword { get; private set; }

        public TenantAdministratorRegistered(
            TenantId tenantId, string name, FullName administorName, 
            EmailAddress emailAddress, string userName, string temporaryPassword) {
            this.TenantId = tenantId.Id;
            this.Name = name;
            this.AdministorName = administorName;
            this.TemporaryPassword = temporaryPassword;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}