using System;
using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Domain.Teams.Model {
    public class TeamMember: Member {
        public TeamMember(TenantId tenantId, string userName, string firstName, string lastName, string emailAddress,
            DateTime initializedOn): base(tenantId, userName, firstName, lastName, emailAddress, initializedOn) {}

        public TeamMemberId TeamMemberId {
            get {
                return new TeamMemberId(this.TenantId, this.UserName);
            }
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.UserName;
        }
    }
}