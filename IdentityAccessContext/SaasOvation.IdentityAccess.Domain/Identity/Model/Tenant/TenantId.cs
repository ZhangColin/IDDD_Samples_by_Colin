namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant {
    public class TenantId: Common.Domain.Model.Identity {
        protected TenantId() { }
        public TenantId(string id): base(id) {}
    }
}