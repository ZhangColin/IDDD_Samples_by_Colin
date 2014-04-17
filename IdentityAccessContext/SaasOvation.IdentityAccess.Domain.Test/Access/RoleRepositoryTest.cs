using System;
using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Test.Access {
    [TestFixture]
    public class RoleRepositoryTest: RepositoryTest {
        [Test]
        public void TestProvisionRole() {
            Tenant tenant = this.CreateTenant();
            Role role = tenant.ProvisionRole("Manager", "A manager role.");
            RoleRepository.Add(role);
            Assert.AreEqual(1, RoleRepository.AllRoles(tenant.TenantId).Count);
        }

        [Test]
        [ExpectedException]
        public void TestRoleUniqueness() {
            Tenant tenant = this.CreateTenant();
            Role role = tenant.ProvisionRole("Manager", "A manager role.");
            RoleRepository.Add(role);

            Role role2 = tenant.ProvisionRole("Manager", "A manager role.");
            RoleRepository.Add(role2);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNoRoleInternalGroupsInFindGroupByName() {
            Tenant tenant = this.CreateTenant();
            Role roleA = tenant.ProvisionRole("RoleA", "A role of A.");
            RoleRepository.Add(roleA);

            GroupRepository.GroupNamed(tenant.TenantId, roleA.Group.Name);
        }
    }
}