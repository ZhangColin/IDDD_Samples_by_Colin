using System;
using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;

namespace SaasOvation.IdentityAccess.Domain.Test.Identity.Model {
    [TestFixture]
    public class TenantTest: DomainTest {
        [Test]
        public void TestCreateOpenEndedInvitation() {
            Tenant tenant = this.CreateTenant();

            tenant.OfferRegistrationInvitation("Open-Ended").OpenEnded();

            Assert.NotNull(tenant.RedefineRegistrationInvitationAs("Open-Ended"));
        }

        [Test]
        public void TestOpenEndedInvitationAvailable() {
            Tenant tenant = this.CreateTenant();

            tenant.OfferRegistrationInvitation("Open-Ended").OpenEnded();

            Assert.IsTrue(tenant.IsRegistrationAvailableThrough("Open-Ended"));
        }

        [Test]
        public void TestClosedEndedInvitationAvailable() {
            Tenant tenant = this.CreateTenant();

            DateTime now = DateTime.Now;
            tenant.OfferRegistrationInvitation("Today-and-Tomorrow").WillStartOn(now).LastingUntil(now.AddDays(1));

            Assert.IsTrue(tenant.IsRegistrationAvailableThrough("Today-and-Tomorrow"));
        }

        [Test]
        public void TestClosedEndedInvitationNotAvailable() {
            Tenant tenant = this.CreateTenant();

            DateTime now = DateTime.Now;
            tenant.OfferRegistrationInvitation("Tomorrow-and-Day-After-Tomorrow")
                .WillStartOn(now.AddDays(1)).LastingUntil(now.AddDays(2));

            Assert.IsFalse(tenant.IsRegistrationAvailableThrough("Tomorrow-and-Day-After-Tomorrow"));
        }

        [Test]
        public void TestAvailableInvitationDescriptor() {
            Tenant tenant = this.CreateTenant();

            tenant.OfferRegistrationInvitation("Open-Ended").OpenEnded();

            DateTime now = DateTime.Now;
            tenant.OfferRegistrationInvitation("Today-and-Tomorrow").WillStartOn(now).LastingUntil(now.AddDays(1));

            Assert.AreEqual(tenant.AllAvailableRegistrationInvitations().Count, 2);
        }

        [Test]
        public void TestUnavailableInvitationDescriptor() {
            Tenant tenant = this.CreateTenant();

            DateTime now = DateTime.Now;
            tenant.OfferRegistrationInvitation("Tomorrow-and-Day-After-Tomorrow")
                .WillStartOn(now.AddDays(1)).LastingUntil(now.AddDays(2));


            Assert.AreEqual(tenant.AllUnavailableRegistrationInvitations().Count, 1);
        }

        [Test]
        public void TestRegisterUser() {
            Tenant tenant = this.CreateTenant();

            DateTime now = DateTime.Now;
            RegistrationInvitation registrationInvitation =
                tenant.OfferRegistrationInvitation("Today-and-Tomorrow").WillStartOn(now).LastingUntil(now.AddDays(1));
            User user = tenant.RegisterUser(registrationInvitation.InvitationId,
                "colin", "secretPassword!", Enablement.IndefiniteEnablement(), PersonEntity(tenant));

            Assert.NotNull(user);

            Assert.NotNull(user.Enablement);
            Assert.NotNull(user.Person);
            Assert.NotNull(user.UserDescriptor);
        }
    }
}