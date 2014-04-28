using System;

namespace SaasOvation.AgilePm.Application.Teams {
    public class DisableProductOwnerCommand: DisableMemberCommand {
        public DisableProductOwnerCommand(string tenantId, string userName, DateTime occurredOn)
            : base(tenantId, userName, occurredOn) {
            
        }
    }
}