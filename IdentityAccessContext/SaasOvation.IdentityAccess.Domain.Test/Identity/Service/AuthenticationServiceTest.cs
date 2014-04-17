using System;
using Moq;
using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Test.Identity.Service {
    [TestFixture]
    public class AuthenticationServiceTest: DomainTest {
        private const string _password = "secretPassword!";
        [Test]
        public void TestAuthenticationSuccess() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);

            Mock<ITenantRepository> tenantRepository = new Mock<ITenantRepository>();
            tenantRepository.Setup(r => r.Get(tenant.TenantId)).Returns(tenant);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.UserFromAuthenticCredentials(tenant.TenantId, user.UserName, _password)).Returns(user);
            Mock<IEncryptionService> encryptionService = new Mock<IEncryptionService>();
            encryptionService.Setup(s => s.EncryptedValue(_password)).Returns(_password);

            AuthenticationService service = new AuthenticationService(tenantRepository.Object, userRepository.Object,
                encryptionService.Object);

            UserDescriptor userDescriptor = service.Authenticate(user.TenantId, user.UserName, _password);

            Assert.NotNull(userDescriptor);
            Assert.IsFalse(userDescriptor.IsNullDescriptor());
            Assert.AreEqual(userDescriptor.TenantId, user.TenantId);
            Assert.AreEqual(userDescriptor.UserName, user.UserName);
            Assert.AreEqual(userDescriptor.EmailAddress, user.Person.EmailAddress.Address);
        }

        [Test]
        public void TestAuthenticationTenantFailure() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);

            Mock<ITenantRepository> tenantRepository = new Mock<ITenantRepository>();
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            Mock<IEncryptionService> encryptionService = new Mock<IEncryptionService>();

            AuthenticationService service = new AuthenticationService(tenantRepository.Object, userRepository.Object,
                encryptionService.Object);

            UserDescriptor userDescriptor = service.Authenticate(new TenantId(Guid.NewGuid().ToString()), 
                user.UserName, _password);

            Assert.NotNull(userDescriptor);
            Assert.IsTrue(userDescriptor.IsNullDescriptor());
        }
        
        [Test]
        public void TestAuthenticationUsernameFailure() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);

            Mock<ITenantRepository> tenantRepository = new Mock<ITenantRepository>();
            tenantRepository.Setup(r => r.Get(tenant.TenantId)).Returns(tenant);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            Mock<IEncryptionService> encryptionService = new Mock<IEncryptionService>();
            encryptionService.Setup(s => s.EncryptedValue(_password)).Returns(_password);

            AuthenticationService service = new AuthenticationService(tenantRepository.Object, userRepository.Object,
                encryptionService.Object);
            UserDescriptor userDescriptor = service.Authenticate(user.TenantId, "jinhua", _password);

            Assert.NotNull(userDescriptor);
            Assert.IsTrue(userDescriptor.IsNullDescriptor());
        }
        
        [Test]
        public void TestAuthenticationPasswordFailure() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);

            Mock<ITenantRepository> tenantRepository = new Mock<ITenantRepository>();
            tenantRepository.Setup(r => r.Get(tenant.TenantId)).Returns(tenant);
            Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.UserFromAuthenticCredentials(tenant.TenantId, user.UserName, _password)).Returns(user);
            Mock<IEncryptionService> encryptionService = new Mock<IEncryptionService>();
            encryptionService.Setup(s => s.EncryptedValue(_password)).Returns(_password);

            AuthenticationService service = new AuthenticationService(tenantRepository.Object, userRepository.Object,
                encryptionService.Object);
            UserDescriptor userDescriptor = service.Authenticate(user.TenantId, user.UserName, _password+"-");

            Assert.NotNull(userDescriptor);
            Assert.IsTrue(userDescriptor.IsNullDescriptor());
        }
    }
}