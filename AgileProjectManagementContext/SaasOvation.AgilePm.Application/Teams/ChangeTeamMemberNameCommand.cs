using System;

namespace SaasOvation.AgilePm.Application.Teams {
    public class ChangeTeamMemberNameCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OccurredOn { get; set; }

        public ChangeTeamMemberNameCommand(string tenantId, string userName, string firstName, string lastName,
            DateTime occurredOn) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.OccurredOn = occurredOn;
        }
    }
}