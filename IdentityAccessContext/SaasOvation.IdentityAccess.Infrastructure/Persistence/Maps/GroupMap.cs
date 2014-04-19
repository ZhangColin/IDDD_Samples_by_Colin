using FluentNHibernate.Mapping;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence.Maps {
    public class GroupMap: ClassMap<Group> {
        public GroupMap() {
            this.Table("Groups");
            this.Id<int>("Id").GeneratedBy.Native();
            this.Version(g => g.ConcurrencyVersion);

            Map(g => g.Name).Not.Nullable();
            Map(g => g.Description).Not.Nullable();
            Component(g => g.TenantId, m => m.Map(tId => tId.Id, "TenantId").Not.Nullable());
            HasMany(g => g.GroupMembers).Cascade.AllDeleteOrphan().KeyColumn("GroupId");
            //LazyLoad().Not.Inverse().
        }
    }
}