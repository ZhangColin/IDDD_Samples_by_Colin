using System;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant {
    public class InvitationDescriptor {
        public TenantId TenantId { get; private set; }
        public string InvitationId { get; private set; }
        public string Description { get; private set; }
        public DateTime StartingOn { get; private set; }
        public DateTime Until { get; private set; }

        public InvitationDescriptor(TenantId tenantId, string invitationId,
            string description, DateTime startingOn, DateTime until) {
            this.TenantId = tenantId;
            this.InvitationId = invitationId;
            this.Description = description;
            this.StartingOn = startingOn;
            this.Until = until;
        } 
    }
}