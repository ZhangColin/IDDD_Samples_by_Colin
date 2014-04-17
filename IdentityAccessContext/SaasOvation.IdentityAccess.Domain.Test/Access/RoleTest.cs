using Moq;
using NUnit.Framework;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Test.Access {
    [TestFixture]
    public class RoleTest: DomainTest {
        [Test]
        public void TestUserIsInRole() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);
            Role managerRole = tenant.ProvisionRole("Manager", "A manager role.", true);
            Group group = new Group(user.TenantId, "Managers", "A group of managers.");

            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(
                s => s.IsMemberGroup(group, new GroupMember(group.TenantId, group.Name, GroupMemberType.Group))).Returns(false);
            groupMemberService.Setup(s => s.ConfirmUser(group, user)).Returns(true);
            groupMemberService.Setup(s => s.IsUserInNestedGroup(managerRole.Group, user)).Returns(true);

            managerRole.AssignGroup(group, groupMemberService.Object);
            group.AddUser(user);

            Assert.IsTrue(group.IsMember(user, groupMemberService.Object));
            Assert.IsTrue(managerRole.IsInRole(user, groupMemberService.Object));
        }

        [Test]
        public void TestUserIsNotInRole() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);
            Role managerRole = tenant.ProvisionRole("Manager", "A manager role.", true);
            Group group = new Group(user.TenantId, "Managers", "A group of managers.");

            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(
                s => s.IsMemberGroup(group, new GroupMember(group.TenantId, group.Name, GroupMemberType.Group))).Returns(false);
            groupMemberService.Setup(s => s.IsUserInNestedGroup(managerRole.Group, user)).Returns(false);

            managerRole.AssignGroup(group, groupMemberService.Object);
            Role accountantRole = new Role(user.TenantId, "Accountant", "An accountant role.", false);

            Assert.IsFalse(managerRole.IsInRole(user, groupMemberService.Object));
            Assert.IsFalse(accountantRole.IsInRole(user, groupMemberService.Object));
        }

        [Test]
        public void TestInternalGroupAddedEventsNotPublished() {
            int roleSomethingAssignedCount = 0;
            int groupSomethingAddedCount = 0;
            DomainEventPublisher.Instance.Subscribe<GroupAssignedToRole>(e => roleSomethingAssignedCount++);
            DomainEventPublisher.Instance.Subscribe<GroupGroupAdded>(e => groupSomethingAddedCount++);
            DomainEventPublisher.Instance.Subscribe<UserAssignedToRole>(e => roleSomethingAssignedCount++);
            DomainEventPublisher.Instance.Subscribe<GroupUserAdded>(e => groupSomethingAddedCount++);

            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);
            Role managerRole = tenant.ProvisionRole("Manager", "A manager role.", true);
            Group group = new Group(user.TenantId, "Managers", "A group of managers.");

            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(
                s => s.IsMemberGroup(group, new GroupMember(group.TenantId, group.Name, GroupMemberType.Group))).Returns(false);

            managerRole.AssignGroup(group, groupMemberService.Object);
            managerRole.AssignUser(user);
            group.AddUser(user);

            Assert.AreEqual(2, roleSomethingAssignedCount);
            Assert.AreEqual(1, groupSomethingAddedCount);
        }

        [Test]
        public void TestInternalGroupRemovedeventsNotPublished() {
            int roleSomethingUnassignedCount = 0;
            int groupSomethingRemovedCount = 0;
            DomainEventPublisher.Instance.Subscribe<GroupUnassignedFromRole>(e => roleSomethingUnassignedCount++);
            DomainEventPublisher.Instance.Subscribe<GroupGroupRemoved>(e => groupSomethingRemovedCount++);
            DomainEventPublisher.Instance.Subscribe<UserUnassignedFromRole>(e => roleSomethingUnassignedCount++);
            DomainEventPublisher.Instance.Subscribe<GroupUserRemoved>(e => groupSomethingRemovedCount++);

            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);
            Role managerRole = tenant.ProvisionRole("Manager", "A manager role.", true);
            Group group = new Group(user.TenantId, "Managers", "A group of managers.");

            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(
                s => s.IsMemberGroup(group, new GroupMember(group.TenantId, group.Name, GroupMemberType.Group))).Returns(false);

            managerRole.AssignGroup(group, groupMemberService.Object);
            managerRole.AssignUser(user);

            managerRole.UnassignUser(user);
            managerRole.UnassignGroup(group);

            Assert.AreEqual(2, roleSomethingUnassignedCount);
            Assert.AreEqual(0, groupSomethingRemovedCount);
        }
    }
}