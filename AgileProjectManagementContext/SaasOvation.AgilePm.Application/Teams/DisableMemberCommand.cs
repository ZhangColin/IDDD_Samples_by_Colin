using System;

namespace SaasOvation.AgilePm.Application.Teams {
    public class DisableMemberCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public DateTime OccurredOn { get; set; }

        public DisableMemberCommand(string tenantId, string userName, DateTime occurredOn) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.OccurredOn = occurredOn;
        }
    }
}