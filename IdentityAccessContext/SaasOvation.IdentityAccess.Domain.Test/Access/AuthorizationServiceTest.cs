using Moq;
using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Access.Service;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Test.Access {
    [TestFixture]
    public class AuthorizationServiceTest: DomainTest {

        [Test]
        public void TestUserInRoleAuthorization() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);

            Role managerRole = tenant.ProvisionRole("Manager", "A manager role.", true);
            managerRole.AssignUser(user);

            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(s => s.ConfirmUser(managerRole.Group, user)).Returns(true);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            Mock<IRoleRepository> roleRepository = new Mock<IRoleRepository>();
            roleRepository.Setup(r => r.RoleNamed(tenant.TenantId, "Manager")).Returns(managerRole);

            AuthorizationService service = new AuthorizationService(groupMemberService.Object,
                roleRepository.Object, userRepository.Object);

            Assert.IsTrue(service.IsUserInRole(user, "Manager"));
            Assert.IsFalse(service.IsUserInRole(user, "Director"));
        }

        [Test]
        public void TestUserNameInRoleAuthorization() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);

            Role managerRole = tenant.ProvisionRole("Manager", "A manager role.", true);
            managerRole.AssignUser(user);

            Mock<IGroupMemberService> groupMemberService = new Mock<IGroupMemberService>();
            groupMemberService.Setup(s => s.ConfirmUser(managerRole.Group, user)).Returns(true);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.UserWithUserName(tenant.TenantId, user.UserName)).Returns(user);
            Mock<IRoleRepository> roleRepository = new Mock<IRoleRepository>();
            roleRepository.Setup(r => r.RoleNamed(tenant.TenantId, "Manager")).Returns(managerRole);

            AuthorizationService service = new AuthorizationService(groupMemberService.Object,
                roleRepository.Object, userRepository.Object);

            Assert.IsTrue(service.IsUserInRole(tenant.TenantId, user.UserName, "Manager"));
            Assert.IsFalse(service.IsUserInRole(tenant.TenantId, user.UserName, "Director"));
        }
    }
}