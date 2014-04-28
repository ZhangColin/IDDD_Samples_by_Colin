using System;

namespace SaasOvation.AgilePm.Application.Teams {
    public class DisableTeamMemberCommand: DisableMemberCommand {
        public DisableTeamMemberCommand(string tenantId, string userName, DateTime occurredOn)
            : base(tenantId, userName, occurredOn) {
            
        }
    }
}