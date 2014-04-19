using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Infrastructure.Persistence;

namespace SaasOvation.IdentityAccess.Infrastructure.Test.Repository {
    [TestFixture]
    public class TenantRepositoryTest: RepositoryTest {
        [Test]
        public void TestAddTenant() {
            Tenant tenant = this.CreateTenant();

            TenantRepository.Add(tenant);

            Tenant reTenant = TenantRepository.Get(tenant.TenantId);
            Assert.IsNotNull(reTenant);
            Assert.AreEqual(tenant.TenantId, reTenant.TenantId);
        }
    }
}