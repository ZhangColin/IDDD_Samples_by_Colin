using System.Collections.Generic;
using NUnit.Framework;
using SaasOvation.Common;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Test.Identity.Repository {
    [TestFixture]
    public class GroupRepositoryTest: RepositoryTest {
        [Test]
        public void TestProvisionGroup() {
            Tenant tenant = this.CreateTenant();
            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA");
            GroupRepository.Add(groupA);

            Assert.AreEqual(1, GroupRepository.AllGroups(tenant.TenantId).Count);
        }

        [Test]
        public void TestRemoveGroupReferencedUser() {
            Tenant tenant = this.CreateTenant();
            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA.");
            User user = this.CreateUser(tenant);
            UserRepository.Add(user);
            groupA.AddUser(user);
            GroupRepository.Add(groupA);

            Assert.AreEqual(1, groupA.GroupMembers.Count);
            Assert.IsTrue(groupA.IsMember(user, ServiceLocator.GetService<GroupMemberService>()));

            UserRepository.Remove(user);

            Group reGroup = GroupRepository.GroupNamed(tenant.TenantId, "GroupA");

            Assert.AreEqual(1, reGroup.GroupMembers.Count);
            Assert.IsFalse(groupA.IsMember(user, ServiceLocator.GetService<GroupMemberService>()));
        }

        [Test]
        public void TestRepositoryRemoveGroup() {
            Tenant tenant = this.CreateTenant();
            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA.");
            GroupRepository.Add(groupA);

            Group notNullGroup = GroupRepository.GroupNamed(tenant.TenantId, "GroupA");
            Assert.IsNotNull(notNullGroup);

            GroupRepository.Remove(groupA);

            Group nullGroup = GroupRepository.GroupNamed(tenant.TenantId, "GroupA");
            Assert.IsNull(nullGroup);
        }

        [Test]
        public void TestNoRoleInternalGroupsInFindAllGroups() {
            Tenant tenant = this.CreateTenant();
            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA.");
            GroupRepository.Add(groupA);

            Role roleA = tenant.ProvisionRole("RoleA", "A role of A.");
            RoleRepository.Add(roleA);

            Role roleB = tenant.ProvisionRole("RoleB", "A role of B.");
            RoleRepository.Add(roleB);

            Role roleC = tenant.ProvisionRole("RoleC", "A role of C.");
            RoleRepository.Add(roleC);

            ICollection<Group> allGroups = GroupRepository.AllGroups(tenant.TenantId);

            Assert.AreEqual(1, allGroups.Count);
        }
    }
}