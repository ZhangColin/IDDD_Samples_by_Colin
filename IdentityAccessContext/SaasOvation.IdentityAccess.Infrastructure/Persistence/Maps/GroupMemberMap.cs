using FluentNHibernate.Mapping;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence.Maps {
    public class GroupMemberMap: ClassMap<GroupMember> {
        public GroupMemberMap() {
            this.Table("GroupMembers");
            this.Id<int>("Id").GeneratedBy.Native();

            Map(gm => gm.Name);
            Map(gm => gm.Type);
            Component(gm => gm.TenantId, m => m.Map(tId => tId.Id, "TenantId"));

            References(gm => gm.Group).Column("GroupId");
        }
    }
}