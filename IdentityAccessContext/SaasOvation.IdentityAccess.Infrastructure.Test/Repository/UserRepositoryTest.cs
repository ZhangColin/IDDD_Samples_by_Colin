using System.Collections.Generic;
using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;

namespace SaasOvation.IdentityAccess.Infrastructure.Test.Repository {
    [TestFixture]
    public class UserRepositoryTest: RepositoryTest {
        [Test]
        public void TestAddUser() {
            User user = this.CreateUser(this.CreateTenant());

            this.UserRepository.Add(user);

            Assert.NotNull(this.UserRepository.UserWithUserName(user.TenantId, user.UserName));
        }

        [Test]
        public void TestFindUserByUserName() {
            User user = this.CreateUser(this.CreateTenant());

            this.UserRepository.Add(user);

            Assert.NotNull(this.UserRepository.UserWithUserName(user.TenantId, user.UserName));
        }

        [Test]
        public void TestRemoveUser() {
            User user = this.CreateUser(this.CreateTenant());

            this.UserRepository.Add(user);

            Assert.NotNull(this.UserRepository.UserWithUserName(user.TenantId, user.UserName));

            this.UserRepository.Remove(user);

            Assert.IsNull(this.UserRepository.UserWithUserName(user.TenantId, user.UserName));
        }

        [Test]
        public void TestFindSimilarlyNamedUsers() {
            Tenant tenant = this.CreateTenant();
            User user = this.CreateUser(tenant);
            User user2 = CreateUser2(tenant);

            this.UserRepository.Add(user);
            this.UserRepository.Add(user2);

            FullName name = user.Person.Name;

            ICollection<User> users = this.UserRepository.AllSimilarlyNamedUsers(user.TenantId,
                name.FirstName.Substring(0, 2), name.LastName.Substring(0, 2));

            Assert.AreEqual(2, users.Count);
        }

        private static User CreateUser2(Tenant tenant) {
            RegistrationInvitation registrationInvitation = tenant.OfferRegistrationInvitation("User2Registration");
            User user2 = tenant.RegisterUser(registrationInvitation.InvitationId,
                "colinZhang", "secretPassword!", Enablement.IndefiniteEnablement(), PersonEntity(tenant));
            return user2;
        }
    }
}