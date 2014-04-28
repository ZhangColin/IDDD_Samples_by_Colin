using System;

namespace SaasOvation.AgilePm.Application.Teams {
    public class EnableProductOwnerCommand: EnableMemberCommand {
        public EnableProductOwnerCommand(string tenantId, string userName, string firstName, string lastName,
            string emailAdddress, DateTime occurredOn)
            : base(tenantId, userName, firstName, lastName, emailAdddress, occurredOn) {
            
        }
    }
}