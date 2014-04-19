using FluentNHibernate.Mapping;
using SaasOvation.IdentityAccess.Domain.Access.Model;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence.Maps {
    public class RoleMap: ClassMap<Role> {
        public RoleMap() {
            this.Table("Roles");
            this.Id<int>("Id").GeneratedBy.Native();
            this.Version(r => r.ConcurrencyVersion);

            Map(r => r.Description);
            Map(r => r.Name);
            Map(r => r.SupportsNesting);

            Component(r => r.TenantId, m => m.Map(tId => tId.Id, "TenantId"));

            References(r => r.Group).Column("GroupId");
        }
    }
}