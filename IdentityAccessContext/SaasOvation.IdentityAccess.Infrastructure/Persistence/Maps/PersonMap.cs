using FluentNHibernate.Mapping;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence.Maps {
    public class PersonMap: ClassMap<Person> {
        public PersonMap() {
            this.Table("Persons");
            this.Id<int>("Id").GeneratedBy.Foreign("User");
            this.Version(p => p.ConcurrencyVersion);

            this.Component(p => p.TenantId, m => m.Map(tId => tId.Id, "TenantId"));
            this.Component(p => p.Name, m => {
                m.Map(n => n.FirstName);
                m.Map(n => n.LastName);
            });
            this.Component(p => p.ContactInformation, m => {
                m.Component(ci => ci.EmailAddress, subMap => subMap.Map(e => e.Address, "EmailAddress"));
                m.Component(ci => ci.PrimaryTelephone, subMap => subMap.Map(t => t.Number, "PrimaryTelephone"));
                m.Component(ci => ci.SecondaryTelephone, subMap => subMap.Map(t => t.Number, "SecondTelephone"));
                m.Component(ci => ci.PostalAddress, subMap => {
                    subMap.Map(pa => pa.City);
                    subMap.Map(pa => pa.CountryCode);
                    subMap.Map(pa => pa.PostalCode);
                    subMap.Map(pa => pa.StateProvince, "Province");
                    subMap.Map(pa => pa.StreetAddress, "Street");
                });
            });

            this.HasOne(p => p.User).Cascade.All().Not.LazyLoad().Constrained();
        }
    }
}