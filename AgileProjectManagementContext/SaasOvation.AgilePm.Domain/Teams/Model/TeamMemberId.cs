using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Teams.Model {
    public class TeamMemberId: Identity {
        public TeamMemberId(TenantId tenantId, string id)
            : base(tenantId + ":" + id) {
        }

        protected override void ValidateId(string id) {
            AssertionConcern.NotNull(TenantId, "The tenantId must be provided.");
            AssertionConcern.NotEmpty(Identity, "The id must be provided.");
            AssertionConcern.Length(Identity, 36, "The id must be 36 characters or less.");
        }

        public TenantId TenantId {
            get { return new TenantId(this.Id.Split(':')[0]); }
        }

        public string Identity {
            get { return this.Id.Split(':')[1]; }
        }
    }
}