using System;
using Moq;
using NUnit.Framework;
using SaasOvation.Common;
using SaasOvation.IdentityAccess.Application.Commands;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Application.Test {
    [TestFixture]
    public class IdentityApplicationServiceTest: ApplicationServiceTest {
        private Mock<ITenantRepository> _tenantRepository;
        private Mock<IGroupRepository> _groupRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<IRoleRepository> _roleRepository;
        private GroupMemberService _groupMemberService;
        private AuthenticationService _authenticationService;
        private TenantProvisioningService _tenantProvisioningService;
        private IdentityApplicationService _identityApplicationService;

        [SetUp]
        protected override void SetUp() {
            base.SetUp();

            this._tenantRepository = new Mock<ITenantRepository>();
            this._groupRepository = new Mock<IGroupRepository>();
            this._userRepository = new Mock<IUserRepository>();
            this._roleRepository = new Mock<IRoleRepository>();

            this._authenticationService = new AuthenticationService(_tenantRepository.Object, _userRepository.Object,
                ServiceLocator.GetService<IEncryptionService>());
            this._groupMemberService = new GroupMemberService(this._userRepository.Object, this._groupRepository.Object);
            this._tenantProvisioningService = new TenantProvisioningService(_tenantRepository.Object,
                _userRepository.Object, _roleRepository.Object);

            this._identityApplicationService = new IdentityApplicationService(this._authenticationService,
                this._groupMemberService, this._tenantProvisioningService, this._tenantRepository.Object,
                this._groupRepository.Object, this._userRepository.Object);

        }

        [Test]
        public void TestActivateTenant() {
            Tenant tenant = this.CreateTenant();
            tenant.Deactivate();

            Assert.IsFalse(tenant.Active);

            _tenantRepository.Setup(r => r.Get(tenant.TenantId)).Returns(tenant);

            _identityApplicationService.ActivateTenant(new ActivateTenantCommand(tenant.TenantId.Id));

            Tenant changedTenant = _tenantRepository.Object.Get(tenant.TenantId);

            Assert.NotNull(changedTenant);
            Assert.AreEqual(tenant.Name, changedTenant.Name);
            Assert.IsTrue(changedTenant.Active);
        }
        
        [Test]
        public void TestDeactivateTenant() {
            Tenant tenant = this.CreateTenant();

            Assert.IsTrue(tenant.Active);

            _tenantRepository.Setup(r => r.Get(tenant.TenantId)).Returns(tenant);

            _identityApplicationService.DeactivateTenant(new DeactivateTenantCommand(tenant.TenantId.Id));

            Tenant changedTenant = _tenantRepository.Object.Get(tenant.TenantId);

            Assert.NotNull(changedTenant);
            Assert.AreEqual(tenant.Name, changedTenant.Name);
            Assert.IsFalse(changedTenant.Active);
        }

        [Test]
        public void TestQueryTenant() {
            Tenant tenant = this.CreateTenant();

            _tenantRepository.Setup(r => r.Get(tenant.TenantId)).Returns(tenant);

            Tenant queriedTenant = this._identityApplicationService.GetTenant(tenant.TenantId.Id);

            Assert.NotNull(queriedTenant);
            Assert.AreEqual(tenant, queriedTenant);
        }

        [Test]
        public void TestAddGroupToGroup() {
            Tenant tenant = this.CreateTenant();
            Group parentGroup = this.CreateGroup1(tenant);
            Group childGroup = this.CreateGroup2(tenant);

            this._groupRepository.Setup(r => r.GroupNamed(parentGroup.TenantId, parentGroup.Name)).Returns(parentGroup);
            this._groupRepository.Setup(r => r.GroupNamed(childGroup.TenantId, childGroup.Name)).Returns(childGroup);

            Assert.AreEqual(0, parentGroup.GroupMembers.Count);

            this._identityApplicationService.AddGroupToGroup(new AddGroupToGroupCommand(parentGroup.TenantId.Id,
                parentGroup.Name, childGroup.Name));

            Assert.AreEqual(1, parentGroup.GroupMembers.Count);
        }

        [Test]
        public void TestAddUserToGroup() {
            Tenant tenant = this.CreateTenant();
            Group parentGroup = this.CreateGroup1(tenant);
            Group childGroup = this.CreateGroup2(tenant);
            User user = this.CreateUser(tenant);

            this._groupRepository.Setup(r => r.GroupNamed(parentGroup.TenantId, parentGroup.Name)).Returns(parentGroup);
            this._groupRepository.Setup(r => r.GroupNamed(childGroup.TenantId, childGroup.Name)).Returns(childGroup);
            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            Assert.AreEqual(0, parentGroup.GroupMembers.Count);
            Assert.AreEqual(0, childGroup.GroupMembers.Count);

            parentGroup.AddGroup(childGroup, this._groupMemberService);

            this._identityApplicationService.AddUserToGroup(new AddUserToGroupCommand(childGroup.TenantId.Id,
                childGroup.Name, user.UserName));

            Assert.AreEqual(1, parentGroup.GroupMembers.Count);
            Assert.AreEqual(1, childGroup.GroupMembers.Count);

            Assert.IsTrue(parentGroup.IsMember(user, this._groupMemberService));
            Assert.IsTrue(childGroup.IsMember(user, this._groupMemberService));
        }

        [Test]
        public void TestIsGroupMember() {
            Tenant tenant = this.CreateTenant();
            Group parentGroup = this.CreateGroup1(tenant);
            Group childGroup = this.CreateGroup2(tenant);
            User user = this.CreateUser(tenant);

            this._groupRepository.Setup(r => r.GroupNamed(parentGroup.TenantId, parentGroup.Name)).Returns(parentGroup);
            this._groupRepository.Setup(r => r.GroupNamed(childGroup.TenantId, childGroup.Name)).Returns(childGroup);
            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            Assert.AreEqual(0, parentGroup.GroupMembers.Count);
            Assert.AreEqual(0, childGroup.GroupMembers.Count);

            parentGroup.AddGroup(childGroup, this._groupMemberService);
            childGroup.AddUser(user);

            Assert.IsTrue(this._identityApplicationService.IsGroupMember(parentGroup.TenantId.Id, parentGroup.Name, user.UserName));
            Assert.IsTrue(this._identityApplicationService.IsGroupMember(childGroup.TenantId.Id, childGroup.Name, user.UserName));
        }

        [Test]
        public void TestRemoveGroupFromGroup() {
            Tenant tenant = this.CreateTenant();
            Group parentGroup = this.CreateGroup1(tenant);
            Group childGroup = this.CreateGroup2(tenant);

            this._groupRepository.Setup(r => r.GroupNamed(parentGroup.TenantId, parentGroup.Name)).Returns(parentGroup);
            this._groupRepository.Setup(r => r.GroupNamed(childGroup.TenantId, childGroup.Name)).Returns(childGroup);

            parentGroup.AddGroup(childGroup, this._groupMemberService);
            Assert.AreEqual(1, parentGroup.GroupMembers.Count);

            this._identityApplicationService.RemoveGroupFromGroup(new RemoveGroupFromGroupCommand(parentGroup.TenantId.Id,
                parentGroup.Name, childGroup.Name));

            Assert.AreEqual(0, parentGroup.GroupMembers.Count);
        }

        [Test]
        public void TestRemoveUserFromGroup() {
            Tenant tenant = this.CreateTenant();
            Group parentGroup = this.CreateGroup1(tenant);
            Group childGroup = this.CreateGroup2(tenant);
            User user = this.CreateUser(tenant);

            this._groupRepository.Setup(r => r.GroupNamed(parentGroup.TenantId, parentGroup.Name)).Returns(parentGroup);
            this._groupRepository.Setup(r => r.GroupNamed(childGroup.TenantId, childGroup.Name)).Returns(childGroup);
            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            Assert.AreEqual(0, parentGroup.GroupMembers.Count);
            Assert.AreEqual(0, childGroup.GroupMembers.Count);

            parentGroup.AddGroup(childGroup, this._groupMemberService);
            childGroup.AddUser(user);

            Assert.AreEqual(1, parentGroup.GroupMembers.Count);
            Assert.AreEqual(1, childGroup.GroupMembers.Count);

            Assert.IsTrue(parentGroup.IsMember(user, this._groupMemberService));
            Assert.IsTrue(childGroup.IsMember(user, this._groupMemberService));

            this._identityApplicationService.RemoveUserFromGroup(new RemoveUserFromGroupCommand(childGroup.TenantId.Id,
                childGroup.Name, user.UserName));

            Assert.AreEqual(1, parentGroup.GroupMembers.Count);
            Assert.AreEqual(0, childGroup.GroupMembers.Count);

            Assert.IsFalse(parentGroup.IsMember(user, this._groupMemberService));
            Assert.IsFalse(childGroup.IsMember(user, this._groupMemberService));
        }

        [Test]
        public void TestAuthenticateUser() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);

            this._tenantRepository.Setup(r => r.Get(tenant.TenantId)).Returns(tenant);
            this._userRepository.Setup(r => r.UserFromAuthenticCredentials(user.TenantId, user.UserName, 
                ServiceLocator.GetService<IEncryptionService>().EncryptedValue("secretPassword!"))).Returns(user);

            UserDescriptor userDescriptor = this._identityApplicationService.AuthenticateUser(
                new AuthenticateUserCommand(user.TenantId.Id, user.UserName, "secretPassword!"));

            Assert.NotNull(userDescriptor);
            Assert.AreEqual(user.UserName, userDescriptor.UserName);
        }
        
        [Test]
        public void TestChangeUserContactInformation() {
            User user = this.CreateUser(this.CreateTenant());

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            this._identityApplicationService.ChangeUserContactInformation(
                new ChangeContactInfoCommand(user.TenantId.Id, user.UserName, "mynewemailaddress@saasovation.com",
                    "777-555-1211", "777-555-1212", "123 Pine Street", "Loveland", "CO", "80771", "US"));

            User changedUser = this._userRepository.Object.UserWithUserName(user.TenantId, user.UserName);
            Assert.NotNull(changedUser);
            Assert.AreEqual("mynewemailaddress@saasovation.com", changedUser.Person.EmailAddress.Address);
            Assert.AreEqual("777-555-1211", changedUser.Person.ContactInformation.PrimaryTelephone.Number);
            Assert.AreEqual("777-555-1212", changedUser.Person.ContactInformation.SecondaryTelephone.Number);
            Assert.AreEqual("123 Pine Street", changedUser.Person.ContactInformation.PostalAddress.StreetAddress);
            Assert.AreEqual("Loveland", changedUser.Person.ContactInformation.PostalAddress.City);
        }

        [Test]
        public void TestChangeUserEmailAddress() {
            User user = this.CreateUser(this.CreateTenant());

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            this._identityApplicationService.ChangeUserEmailAddress(new ChangeEmailAddressCommand(user.TenantId.Id,
                user.UserName, "mynewemailaddress@saasovation.com"));

            User changedUser = this._userRepository.Object.UserWithUserName(user.TenantId, user.UserName);
            Assert.NotNull(changedUser);
            Assert.AreEqual("mynewemailaddress@saasovation.com", changedUser.Person.EmailAddress.Address);
        }

        [Test]
        public void TestChangeUserPostalAddress() {
            User user = this.CreateUser(this.CreateTenant());

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            this._identityApplicationService.ChangeUserPostalAddress(new ChangePostalAddressCommand(user.TenantId.Id,
                user.UserName, "123 Pine Street", "Loveland", "CO", "80771", "US"));

            User changedUser = this._userRepository.Object.UserWithUserName(user.TenantId, user.UserName);
            Assert.NotNull(changedUser);
            Assert.AreEqual("123 Pine Street", changedUser.Person.ContactInformation.PostalAddress.StreetAddress);
            Assert.AreEqual("Loveland", changedUser.Person.ContactInformation.PostalAddress.City);
        }

        [Test]
        public void TestChangeUserPrimaryTelephone() {
            User user = this.CreateUser(this.CreateTenant());

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            this._identityApplicationService.ChangeUserPrimaryTelephone(new ChangePrimaryTelephoneCommand(
                user.TenantId.Id, user.UserName, "777-555-1211"));

            User changedUser = this._userRepository.Object.UserWithUserName(user.TenantId, user.UserName);
            Assert.NotNull(changedUser);
            Assert.AreEqual("777-555-1211", changedUser.Person.ContactInformation.PrimaryTelephone.Number);
        }
        
        [Test]
        public void TestChangeUserSecondaryTelephone() {
            User user = this.CreateUser(this.CreateTenant());

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            this._identityApplicationService.ChangeUserSecondaryTelephone(new ChangeSecondaryTelephoneCommand(
                user.TenantId.Id, user.UserName, "777-555-1212"));

            User changedUser = this._userRepository.Object.UserWithUserName(user.TenantId, user.UserName);
            Assert.NotNull(changedUser);
            Assert.AreEqual("777-555-1212", changedUser.Person.ContactInformation.SecondaryTelephone.Number);
        }

        [Test]
        public void TestChangeUserPassword() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);

            this._tenantRepository.Setup(r => r.Get(tenant.TenantId)).Returns(tenant);
            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);
            this._userRepository.Setup(r => r.UserFromAuthenticCredentials(user.TenantId, user.UserName,
                ServiceLocator.GetService<IEncryptionService>().EncryptedValue("THIS.IS.JOE'S.NEW.PASSWORD"))).Returns(user);

            this._identityApplicationService.ChangeUserPassword(new ChangeUserPasswordCommand(
                user.TenantId.Id, user.UserName, "secretPassword!", "THIS.IS.JOE'S.NEW.PASSWORD"));

            UserDescriptor userDescriptor = _identityApplicationService.AuthenticateUser(
                new AuthenticateUserCommand(user.TenantId.Id, user.UserName, "THIS.IS.JOE'S.NEW.PASSWORD"));

            Assert.NotNull(userDescriptor);
            Assert.AreEqual(user.UserName, userDescriptor.UserName);
        }
        
        [Test]
        public void TestChangeUserPersonalName() {
            User user = this.CreateUser(this.CreateTenant());

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            this._identityApplicationService.ChangeUserPersonalName(new ChangeUserPersonalNameCommand(
                user.TenantId.Id, user.UserName, "stwyhm", "zhang"));

            User changedUser = this._userRepository.Object.UserWithUserName(user.TenantId, user.UserName);
            Assert.NotNull(changedUser);
            Assert.AreEqual("stwyhm zhang", changedUser.Person.Name.AsFormattedName());
        }
        
        [Test]
        public void TestDefineUserEnablement() {
            User user = this.CreateUser(this.CreateTenant());

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            this._identityApplicationService.DefineUserEnablement(new DefineUserEnablementCommand(
                user.TenantId.Id, user.UserName, true, DateTime.Now, DateTime.Now.AddYears(1)));

            User changedUser = this._userRepository.Object.UserWithUserName(user.TenantId, user.UserName);
            Assert.NotNull(changedUser);
            Assert.IsTrue(changedUser.IsEnabled);
        }
        
        [Test]
        public void TestQueryUser() {
            User user = this.CreateUser(this.CreateTenant());

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            User queryUser = this._identityApplicationService.GetUser(user.TenantId.Id, user.UserName);

            Assert.NotNull(queryUser);
            Assert.AreEqual(user, queryUser);
        }
        
        [Test]
        public void TestQueryUserDescriptor() {
            User user = this.CreateUser(this.CreateTenant());

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);

            UserDescriptor queryUserDescriptor = this._identityApplicationService.GetUserDescriptor(user.TenantId.Id, user.UserName);

            Assert.NotNull(queryUserDescriptor);
            Assert.AreEqual(user.UserDescriptor, queryUserDescriptor);
        }
    }
}