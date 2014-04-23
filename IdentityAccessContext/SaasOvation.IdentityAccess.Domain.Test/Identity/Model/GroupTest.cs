using System;
using Moq;
using NUnit.Framework;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Test.Identity.Model {
    [TestFixture]
    public class GroupTest: DomainTest {
        [Test]
        public void TestAddGroup() {
            int groupGroupAddedCount = 0;
            DomainEventPublisher.Instance.Subscribe<GroupGroupAdded>(e => groupGroupAddedCount++);

            Tenant tenant = this.CreateTenant();

            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA.");
            Group groupB = tenant.ProvisionGroup("GroupB", "A group named GroupB.");

            Mock<IGroupRepository> groupRepository =new Mock<IGroupRepository>();
            groupRepository.Setup(r => r.GroupNamed(groupA.TenantId, groupA.Name)).Returns(groupA);
            groupRepository.Setup(r => r.GroupNamed(groupB.TenantId, groupB.Name)).Returns(groupB);
            Mock<IUserRepository> userRepository =new Mock<IUserRepository>();

            groupA.AddGroup(groupB, new GroupMemberService(userRepository.Object, groupRepository.Object));

            Assert.AreEqual(1, groupA.GroupMembers.Count);
            Assert.AreEqual(0, groupB.GroupMembers.Count);
            Assert.AreEqual(1, groupGroupAddedCount);
        }

        [Test]
        public void TestRemoveGroup() {
            int groupGroupRemovedCount = 0;
            DomainEventPublisher.Instance.Subscribe<GroupGroupRemoved>(e=>groupGroupRemovedCount++);

            Tenant tenant = this.CreateTenant();

            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA.");
            Group groupB = tenant.ProvisionGroup("GroupB", "A group named GroupB.");

            Mock<IGroupRepository> groupRepository = new Mock<IGroupRepository>();
            groupRepository.Setup(r => r.GroupNamed(groupA.TenantId, groupA.Name)).Returns(groupA);
            groupRepository.Setup(r => r.GroupNamed(groupB.TenantId, groupB.Name)).Returns(groupB);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();

            groupA.AddGroup(groupB, new GroupMemberService(userRepository.Object, groupRepository.Object));

            Assert.AreEqual(1, groupA.GroupMembers.Count);

            groupA.RemoveGroup(groupB);
            Assert.AreEqual(0, groupA.GroupMembers.Count);
            Assert.AreEqual(1, groupGroupRemovedCount);
        }

        [Test]
        public void TestAddUser() {
            int groupUserAddedCount = 0;
            DomainEventPublisher.Instance.Subscribe<GroupUserAdded>(e=>groupUserAddedCount++);

            Tenant tenant = this.CreateTenant();

            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA.");
            User user = this.CreateUser(tenant);
            groupA.AddUser(user);

            Mock<IGroupRepository> groupRepository = new Mock<IGroupRepository>();
            groupRepository.Setup(r => r.GroupNamed(groupA.TenantId, groupA.Name)).Returns(groupA);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);
            
            Assert.AreEqual(1, groupA.GroupMembers.Count);
            Assert.IsTrue(groupA.IsMember(user, new GroupMemberService(userRepository.Object, groupRepository.Object)));
            Assert.AreEqual(1, groupUserAddedCount);
        }

        [Test]
        public void TestRemoveUser() {
            int groupUserRemovedCount = 0;
            DomainEventPublisher.Instance.Subscribe<GroupUserRemoved>(e=>groupUserRemovedCount++);

            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);

            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA.");
            groupA.AddUser(user);

            Assert.AreEqual(1, groupA.GroupMembers.Count);

            groupA.RemoveUser(user);
            Assert.AreEqual(0, groupA.GroupMembers.Count);
            Assert.AreEqual(1, groupUserRemovedCount);
        }

        [Test]
        public void TestUserIsMemberOfNestedGroup() {
            int groupGroupAddedCount = 0;
            DomainEventPublisher.Instance.Subscribe<GroupGroupAdded>(e => groupGroupAddedCount++);

            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);

            Group groupA = tenant.ProvisionGroup("GroupA", "A group named GroupA.");
            Group groupB = tenant.ProvisionGroup("GroupB", "A group named GroupB.");

            Mock<IGroupRepository> groupRepository = new Mock<IGroupRepository>();
            groupRepository.Setup(r => r.GroupNamed(groupA.TenantId, groupA.Name)).Returns(groupA);
            groupRepository.Setup(r => r.GroupNamed(groupB.TenantId, groupB.Name)).Returns(groupB);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            GroupMemberService groupMemberService = new GroupMemberService(userRepository.Object, groupRepository.Object);

            groupA.AddGroup(groupB, groupMemberService);

            groupB.AddUser(user);

            Assert.IsTrue(groupB.IsMember(user, groupMemberService));
            Assert.IsTrue(groupA.IsMember(user, groupMemberService));

            Assert.AreEqual(1, groupGroupAddedCount);
        }

        [Test]
        public void TestUserIsNotMember() {
            User user = this.CreateUser(this.CreateTenant());
            
            Group groupA = new Group(user.TenantId, "GroupA", "A group named GroupA.");
            Group groupB = new Group(user.TenantId, "GroupB", "A group named GroupB.");

            Mock<IGroupRepository> groupRepository = new Mock<IGroupRepository>();
            groupRepository.Setup(r => r.GroupNamed(groupA.TenantId, groupA.Name)).Returns(groupA);
            groupRepository.Setup(r => r.GroupNamed(groupB.TenantId, groupB.Name)).Returns(groupB);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            GroupMemberService groupMemberService = new GroupMemberService(userRepository.Object, groupRepository.Object);

            Assert.IsFalse(groupB.IsMember(user, groupMemberService));
            Assert.IsFalse(groupA.IsMember(user, groupMemberService));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNoRecursiveGroupings() {
            User user = this.CreateUser(this.CreateTenant());

            Group groupA = new Group(user.TenantId, "GroupA", "A group named GroupA.");
            Group groupB = new Group(user.TenantId, "GroupB", "A group named GroupB.");
            Group groupC = new Group(user.TenantId, "GroupC", "A group named GroupC.");

            Mock<IGroupRepository> groupRepository = new Mock<IGroupRepository>();
            groupRepository.Setup(r => r.GroupNamed(groupA.TenantId, groupA.Name)).Returns(groupA);
            groupRepository.Setup(r => r.GroupNamed(groupB.TenantId, groupB.Name)).Returns(groupB);
            groupRepository.Setup(r => r.GroupNamed(groupC.TenantId, groupC.Name)).Returns(groupC);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();

            GroupMemberService groupMemberService = new GroupMemberService(userRepository.Object, groupRepository.Object);
            groupA.AddGroup(groupB, groupMemberService);
            groupB.AddGroup(groupC, groupMemberService);

            groupC.AddGroup(groupA, groupMemberService);
        }

        
    }
}