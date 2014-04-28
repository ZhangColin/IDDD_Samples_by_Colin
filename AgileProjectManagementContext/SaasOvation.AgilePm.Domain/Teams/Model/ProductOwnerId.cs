using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Teams.Model {
    public class ProductOwnerId: Identity {
        public ProductOwnerId(TenantId tenantId, string id)
            : base(tenantId + ":" + id) {}

        public TenantId TenantId {
            get { return new TenantId(this.Id.Split(':')[0]);}
        }

        public string Identity {
            get { return this.Id.Split(':')[1]; }
        }
    }
}