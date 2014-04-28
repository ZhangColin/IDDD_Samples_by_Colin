using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Tenants {
    public class TenantId: Identity {
        public TenantId(string id): base(id) {}
    }
}