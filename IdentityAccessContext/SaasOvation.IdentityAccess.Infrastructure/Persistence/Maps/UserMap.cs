using FluentNHibernate.Mapping;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence.Maps {
    public class UserMap: ClassMap<User> {
        public UserMap() {
            this.Table("Users");
            this.Id<int>("Id").GeneratedBy.Native();
            this.Version(u => u.ConcurrencyVersion);

            this.Map(u => u.UserName).Not.Nullable();
            this.Map(u => u.Password).Not.Nullable();

            this.Component(u => u.Enablement, m => {
                m.Map(e => e.Enabled);
                m.Map(e => e.StartDate, "EnablementStartDate");
                m.Map(e => e.EndDate, "EnablementEndDate");
            });
            this.Component(u => u.TenantId, m => m.Map(tId => tId.Id, "TenantId"));

            this.HasOne(u => u.Person).Cascade.All().Not.LazyLoad();
        }
    }
}