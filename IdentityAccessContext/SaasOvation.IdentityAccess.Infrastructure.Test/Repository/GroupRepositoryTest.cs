using System.Collections.Generic;
using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Infrastructure.Test.Repository {
    [TestFixture]
    public class GroupRepositoryTest: RepositoryTest {
        [Test]
        public void TestProvisionGroup() {
            Tenant tenant = this.CreateTenant();
            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA");
            this.GroupRepository.Add(groupA);

            Assert.AreEqual(1, this.GroupRepository.AllGroups(tenant.TenantId).Count);
        }

        [Test]
        public void TestRemoveGroupReferencedUser() {
            Tenant tenant = this.CreateTenant();
            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA.");
            User user = this.CreateUser(tenant);
            this.UserRepository.Add(user);
            groupA.AddUser(user);
            this.GroupRepository.Add(groupA);

            GroupMemberService groupMemberService = new GroupMemberService(this.UserRepository, this.GroupRepository);

            Assert.AreEqual(1, groupA.GroupMembers.Count);
            Assert.IsTrue(groupA.IsMember(user, groupMemberService));

            this.UserRepository.Remove(user);

            Group reGroup = this.GroupRepository.GroupNamed(tenant.TenantId, "GroupA");

            Assert.AreEqual(1, reGroup.GroupMembers.Count);
            Assert.IsFalse(groupA.IsMember(user, groupMemberService));
        }

        [Test]
        public void TestRepositoryRemoveGroup() {
            Tenant tenant = this.CreateTenant();
            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA.");
            this.GroupRepository.Add(groupA);

            Group notNullGroup = this.GroupRepository.GroupNamed(tenant.TenantId, "GroupA");
            Assert.IsNotNull(notNullGroup);

            this.GroupRepository.Remove(groupA);

            Group nullGroup = this.GroupRepository.GroupNamed(tenant.TenantId, "GroupA");
            Assert.IsNull(nullGroup);
        }

        [Test]
        public void TestNoRoleInternalGroupsInFindAllGroups() {
            Tenant tenant = this.CreateTenant();
            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA.");
            this.GroupRepository.Add(groupA);

            Role roleA = tenant.ProvisionRole("RoleA", "A role of A.");
            this.RoleRepository.Add(roleA);

            Role roleB = tenant.ProvisionRole("RoleB", "A role of B.");
            this.RoleRepository.Add(roleB);

            Role roleC = tenant.ProvisionRole("RoleC", "A role of C.");
            this.RoleRepository.Add(roleC);

            ICollection<Group> allGroups = this.GroupRepository.AllGroups(tenant.TenantId);

            Assert.AreEqual(1, allGroups.Count);
        }
    }
}