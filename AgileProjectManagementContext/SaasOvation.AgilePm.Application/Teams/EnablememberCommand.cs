using System;

namespace SaasOvation.AgilePm.Application.Teams {
    public class EnableMemberCommand {
        public string TenantId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAdddress { get; set; }
        public DateTime OccurredOn { get; set; }

        public EnableMemberCommand(string tenantId, string userName, string firstName, string lastName, string emailAdddress, DateTime occurredOn) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAdddress = emailAdddress;
            this.OccurredOn = occurredOn;
        }
    }
}