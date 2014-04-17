using System;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;

namespace SaasOvation.IdentityAccess.Domain.Test {
    public class IdentityAccessTest {
        protected Tenant CreateTenant() {
            TenantId tenantId = new TenantId(Guid.NewGuid().ToString());

            return new Tenant(tenantId, "Test Tenant", "This is a test tenant.", true);
        }

        protected User CreateUser(Tenant tenant) {
            DateTime now = DateTime.Now;
            RegistrationInvitation registrationInvitation =
                tenant.OfferRegistrationInvitation("Today-and-Tomorrow").WillStartOn(now).LastingUntil(now.AddDays(1));
            return tenant.RegisterUser(registrationInvitation.InvitationId,
                "colin", "secretPassword!", Enablement.IndefiniteEnablement(), PersonEntity(tenant));
        }

        protected static Person PersonEntity(Tenant tenant) {
            return new Person(tenant.TenantId, new FullName("Colin", "Zhang"),
                new ContactInformation(new EmailAddress("colin@saasovation.com"),
                    new PostalAddress("123 Pearl Street", "Boulder", "CO", "80301", "US"),
                    new Telephone("303-555-1210"), new Telephone("303-555-1212")));
        }
    }
}