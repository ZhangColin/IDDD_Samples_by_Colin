using FluentNHibernate.Mapping;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence.Maps {
    public class RegistrationInvitationMap: ClassMap<RegistrationInvitation> {
        public RegistrationInvitationMap() {
            this.Table("RegistrationInvitations");
            this.Id<int>("Id").GeneratedBy.Native();
            this.Version(ri => ri.ConcurrencyVersion);

            Map(ri => ri.Description);
            Map(ri => ri.InvitationId);
            Map(ri => ri.StartingOn);
            Map(ri => ri.Until);

            Component(ri => ri.TenantId, m => m.Map(tId => tId.Id).Not.Nullable());
        }
    }
}