using System;

namespace SaasOvation.AgilePm.Application.Teams {
    public class EnableTeamMemberCommand: EnableMemberCommand {
        public EnableTeamMemberCommand(string tenantId, string userName, string firstName, string lastName,
            string emailAdddress, DateTime occurredOn)
            : base(tenantId, userName, firstName, lastName, emailAdddress, occurredOn) {
            
        }
    }
}