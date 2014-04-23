using Moq;
using NUnit.Framework;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
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

            Mock<IGroupRepository> groupRepository = new Mock<IGroupRepository>();
            groupRepository.Setup(r => r.GroupNamed(group.TenantId, group.Name)).Returns(group);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            GroupMemberService groupMemberService = new GroupMemberService(userRepository.Object, groupRepository.Object);

            managerRole.AssignGroup(group, groupMemberService);
            group.AddUser(user);

            Assert.IsTrue(group.IsMember(user, groupMemberService));
            Assert.IsTrue(managerRole.IsInRole(user, groupMemberService));
        }

        [Test]
        public void TestUserIsNotInRole() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);
            Role managerRole = tenant.ProvisionRole("Manager", "A manager role.", true);
            Group group = new Group(user.TenantId, "Managers", "A group of managers.");

            Mock<IGroupRepository> groupRepository = new Mock<IGroupRepository>();
            groupRepository.Setup(r => r.GroupNamed(group.TenantId, group.Name)).Returns(group);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            GroupMemberService groupMemberService = new GroupMemberService(userRepository.Object, groupRepository.Object);

            managerRole.AssignGroup(group, groupMemberService);
            Role accountantRole = new Role(user.TenantId, "Accountant", "An accountant role.", false);

            Assert.IsFalse(managerRole.IsInRole(user, groupMemberService));
            Assert.IsFalse(accountantRole.IsInRole(user, groupMemberService));
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

            Mock<IGroupRepository> groupRepository = new Mock<IGroupRepository>();
            groupRepository.Setup(r => r.GroupNamed(group.TenantId, group.Name)).Returns(group);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            GroupMemberService groupMemberService = new GroupMemberService(userRepository.Object, groupRepository.Object);

            managerRole.AssignGroup(group, groupMemberService);
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

            Mock<IGroupRepository> groupRepository = new Mock<IGroupRepository>();
            groupRepository.Setup(r => r.GroupNamed(group.TenantId, group.Name)).Returns(group);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            GroupMemberService groupMemberService = new GroupMemberService(userRepository.Object, groupRepository.Object);

            managerRole.AssignGroup(group, groupMemberService);
            managerRole.AssignUser(user);

            managerRole.UnassignUser(user);
            managerRole.UnassignGroup(group);

            Assert.AreEqual(2, roleSomethingUnassignedCount);
            Assert.AreEqual(0, groupSomethingRemovedCount);
        }
    }
}