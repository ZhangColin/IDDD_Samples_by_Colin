using System;
using System.Collections.Generic;
using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;

namespace SaasOvation.IdentityAccess.Domain.Test.Identity.Repository {
    [TestFixture]
    public class UserRepositoryTest: RepositoryTest {
        [Test]
        public void TestAddUser() {
            User user = this.CreateUser(this.CreateTenant());

            UserRepository.Add(user);

            Assert.NotNull(UserRepository.UserWithUserName(user.TenantId, user.UserName));
        }

        [Test]
        public void TestFindUserByUserName() {
            User user = this.CreateUser(this.CreateTenant());

            UserRepository.Add(user);

            Assert.NotNull(UserRepository.UserWithUserName(user.TenantId, user.UserName));
        }

        [Test]
        public void TestRemoveUser() {
            User user = this.CreateUser(this.CreateTenant());

            UserRepository.Add(user);

            Assert.NotNull(UserRepository.UserWithUserName(user.TenantId, user.UserName));

            UserRepository.Remove(user);

            Assert.IsNull(UserRepository.UserWithUserName(user.TenantId, user.UserName));
        }

        [Test]
        public void TestFindSimilarlyNamedUsers() {
            User user = this.CreateUser(this.CreateTenant());;
            User user2 = this.CreateUser(this.CreateTenant());;

            UserRepository.Add(user);
            UserRepository.Add(user2);

            FullName name = user.Person.Name;

            ICollection<User> users = UserRepository.AllSimilarlyNamedUsers(user.TenantId, "",
                name.LastName.Substring(0, 2));

            Assert.AreEqual(2, users.Count);
        }
    }
}