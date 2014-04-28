using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Tenants {
    public class TenantId: Identity {
        public TenantId(string id): base(id) {    }
    }
}