using System;
using Moq;
using NUnit.Framework;
using SaasOvation.Common;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Test.Identity.Service {
    [TestFixture]
    public class TenantProvisioningServiceTest: DomainTest {
        [Test]
        public void TestProvisionTenant() {
            bool handled1 = false;
            bool handled2 = false;
            DomainEventPublisher.Instance.Subscribe<TenantProvisioned>(e => handled1 = true);
            DomainEventPublisher.Instance.Subscribe<TenantAdministratorRegistered>(e => handled2 = true);

            Mock<ITenantRepository> tenantRepository = new Mock<ITenantRepository>();
            tenantRepository.Setup(r => r.GetNextIdentity()).Returns(new TenantId(Guid.NewGuid().ToString()));
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            Mock<IRoleRepository> roleRepository = new Mock<IRoleRepository>();

            TenantProvisioningService service = new TenantProvisioningService(tenantRepository.Object,
                userRepository.Object, roleRepository.Object);
            Tenant tenant = service.ProvisionTenant("Test Tenant", "This is a test tenant.", new FullName("colin", "zhang"),
                new EmailAddress("colin@saasovation.com"),
                new PostalAddress("123 Pearl Street", "Boulder", "CO", "80301", "US"),
                new Telephone("303-555-1210"), new Telephone("303-555-1212"));

            Assert.IsTrue(handled1);
            Assert.IsTrue(handled2);

            Assert.NotNull(tenant.TenantId);
            Assert.NotNull(tenant.TenantId.Id);
            Assert.AreEqual(36, tenant.TenantId.Id.Length);
            Assert.AreEqual("Test Tenant", tenant.Name);
            Assert.AreEqual("This is a test tenant.", tenant.Description);
        }
    }
}