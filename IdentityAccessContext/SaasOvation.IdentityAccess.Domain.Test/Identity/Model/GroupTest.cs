using System;
using Moq;
using NUnit.Framework;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
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

            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(
                s => s.IsMemberGroup(groupB, new GroupMember(groupA.TenantId, groupA.Name, GroupMemberType.Group)));
            groupA.AddGroup(groupB, groupMemberService.Object);

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
            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(
                s => s.IsMemberGroup(groupB, new GroupMember(groupA.TenantId, groupA.Name, GroupMemberType.Group)));
            groupA.AddGroup(groupB, groupMemberService.Object);

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

            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(s => s.ConfirmUser(groupA, user)).Returns(true);
            
            Assert.AreEqual(1, groupA.GroupMembers.Count);
            Assert.IsTrue(groupA.IsMember(user, groupMemberService.Object));
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

            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(
                s => s.IsMemberGroup(groupB, new GroupMember(groupA.TenantId, groupA.Name, GroupMemberType.Group)));
            groupA.AddGroup(groupB, groupMemberService.Object);
            groupMemberService.Setup(s => s.IsUserInNestedGroup(groupA, user)).Returns(true);
            groupMemberService.Setup(s => s.ConfirmUser(groupB, user)).Returns(true);

            groupB.AddUser(user);

            Assert.IsTrue(groupB.IsMember(user, groupMemberService.Object));
            Assert.IsTrue(groupA.IsMember(user, groupMemberService.Object));

            Assert.AreEqual(1, groupGroupAddedCount);
        }

        [Test]
        public void TestUserIsNotMember() {
            User user = this.CreateUser(this.CreateTenant());
            
            Group groupA = new Group(user.TenantId, "GroupA", "A group named GroupA.");
            Group groupB = new Group(user.TenantId, "GroupB", "A group named GroupB.");

            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(s => s.IsMemberGroup(null, null)).Returns(false);
            groupMemberService.Setup(s => s.IsUserInNestedGroup(groupA, user)).Returns(false);
            groupMemberService.Setup(s => s.ConfirmUser(groupB, user)).Returns(false);
            groupA.AddGroup(groupB, groupMemberService.Object);

            Assert.IsFalse(groupB.IsMember(user, groupMemberService.Object));
            Assert.IsFalse(groupA.IsMember(user, groupMemberService.Object));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNoRecursiveGroupings() {
            User user = this.CreateUser(this.CreateTenant());

            Group groupA = new Group(user.TenantId, "GroupA", "A group named GroupA.");
            Group groupB = new Group(user.TenantId, "GroupB", "A group named GroupB.");
            Group groupC = new Group(user.TenantId, "GroupC", "A group named GroupC.");

            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(
                s => s.IsMemberGroup(groupB, new GroupMember(groupA.TenantId, groupA.Name, GroupMemberType.Group)))
                .Returns(false);
            groupMemberService.Setup(
                s => s.IsMemberGroup(groupC, new GroupMember(groupB.TenantId, groupB.Name, GroupMemberType.Group)))
                .Returns(false);
            groupMemberService.Setup(
                s => s.IsMemberGroup(groupA, new GroupMember(groupC.TenantId, groupC.Name, GroupMemberType.Group)))
                .Returns(true);

            groupA.AddGroup(groupB, groupMemberService.Object);
            groupB.AddGroup(groupC, groupMemberService.Object);

            groupC.AddGroup(groupA, groupMemberService.Object);
        }

        
    }
}