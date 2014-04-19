using FluentNHibernate.Mapping;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence.Maps {
    public class TenantMap: ClassMap<Tenant> {
        public TenantMap() {
            this.Table("Tenants");
            this.Id<int>("Id").GeneratedBy.Native();
            this.Map(t => t.Active).Not.Nullable();
            this.Map(t => t.Name).Not.Nullable();
            this.Map(t => t.Description).Nullable();
            this.Component(t => t.TenantId, m => m.Map(tId => tId.Id, "TenantId"));
            this.HasMany(t => t.RegistrationInvitations).Cascade.AllDeleteOrphan().Not.Inverse().LazyLoad().KeyColumn("TenantTableId");
            this.Version(t => t.ConcurrencyVersion);
        }
    }
}