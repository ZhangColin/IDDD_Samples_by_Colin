using Moq;
using NUnit.Framework;
using SaasOvation.IdentityAccess.Application.Commands;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Application.Test {
    [TestFixture]
    public class AccessApplicationServiceTest : ApplicationServiceTest{
        private Mock<ITenantRepository> _tenantRepository;
        private Mock<IGroupRepository> _groupRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<IRoleRepository> _roleRepository;
        private GroupMemberService _groupMemberService;
        private AccessApplicationService _accessApplicationService;

        [SetUp]
        protected override void SetUp() {
            this._tenantRepository = new Mock<ITenantRepository>();
            this._groupRepository = new Mock<IGroupRepository>();
            this._userRepository = new Mock<IUserRepository>();
            this._roleRepository = new Mock<IRoleRepository>();

            this._groupMemberService = new GroupMemberService(this._userRepository.Object, this._groupRepository.Object);

            this._accessApplicationService = new AccessApplicationService(
                this._tenantRepository.Object, this._groupRepository.Object, this._userRepository.Object, this._roleRepository.Object);

            base.SetUp();
        }

        [Test]
        public void TestAssignUserToRole() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);
            Role role = this.CreateRole(tenant);

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);
            this._roleRepository.Setup(r => r.RoleNamed(role.TenantId, role.Name)).Returns(role);

            Assert.IsFalse(role.IsInRole(user, this._groupMemberService));

            this._accessApplicationService.AssignUserToRole(new AssignUserToRoleCommand(user.TenantId.Id, user.UserName,
                role.Name));

            Assert.IsTrue(role.IsInRole(user, this._groupMemberService));
        }

        [Test]
        public void TestIsUserInRole() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);
            Role role = this.CreateRole(tenant);

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);
            this._roleRepository.Setup(r => r.RoleNamed(role.TenantId, role.Name)).Returns(role);

            Assert.IsFalse(_accessApplicationService.IsUserInRole(user.TenantId.Id, user.UserName, role.Name));

            _accessApplicationService.AssignUserToRole(new AssignUserToRoleCommand(user.TenantId.Id, user.UserName,
                role.Name));

            Assert.IsTrue(_accessApplicationService.IsUserInRole(user.TenantId.Id, user.UserName, role.Name));
        }
        
        [Test]
        public void TestUserInRole() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);
            Role role = this.CreateRole(tenant);

            this._userRepository.Setup(r => r.UserWithUserName(user.TenantId, user.UserName)).Returns(user);
            this._roleRepository.Setup(r => r.RoleNamed(role.TenantId, role.Name)).Returns(role);

            User userNotInRole = _accessApplicationService.UserInRole(user.TenantId.Id, user.UserName, role.Name);
            Assert.IsNull(userNotInRole);

            _accessApplicationService.AssignUserToRole(new AssignUserToRoleCommand(user.TenantId.Id, user.UserName,
                role.Name));

            User userInRole = _accessApplicationService.UserInRole(user.TenantId.Id, user.UserName, role.Name);
            Assert.NotNull(userInRole);
        }
    }
}