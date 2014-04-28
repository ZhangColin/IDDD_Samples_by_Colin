using System;

namespace SaasOvation.AgilePm.Application.Teams {
    public class ChangeTeamMemberEmailAddressCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime OccurredOn { get; set; }

        public ChangeTeamMemberEmailAddressCommand(string tenantId, string userName, string emailAddress,
            DateTime occurredOn) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.EmailAddress = emailAddress;
            this.OccurredOn = occurredOn;
        }
    }
}