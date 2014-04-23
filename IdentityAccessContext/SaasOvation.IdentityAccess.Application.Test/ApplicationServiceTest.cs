using System;
using Autofac;
using NUnit.Framework;
using SaasOvation.Common;
using SaasOvation.Common.Domain.Model;
using SaasOvation.Common.Domain.Model.Process;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Service;
using SaasOvation.IdentityAccess.Infrastructure;

namespace SaasOvation.IdentityAccess.Application.Test {
    public class ApplicationServiceTest {
        protected virtual void SetUp() {

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<MD5EncryptionService>().As<IEncryptionService>();
            IContainer container = builder.Build();
            ServiceLocator.Resolver = new AutofacResolver(container);

            DomainEventPublisher.Instance.Reset();

        }

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

        protected Role CreateRole(Tenant tenant) {
            return tenant.ProvisionRole("Test role", "A test role.", true);
        }

        protected Group CreateGroup1(Tenant tenant) {
            return tenant.ProvisionGroup("Test group 1", "A test group 1.");
        }
        
        protected Group CreateGroup2(Tenant tenant) {
            return tenant.ProvisionGroup("Test group 2", "A test group 2.");
        }
    }
}