using System;
using NUnit.Framework;
using SaasOvation.Common;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Test.Identity.Model {
    [TestFixture]
    public class UserTest: DomainTest {
        private const string _password = "secretPassword!";

        [Test]
        public void TestUserEnablementEnabled() {
            User user = this.CreateUser(this.CreateTenant());
            Assert.IsTrue(user.IsEnabled);
        }

        [Test]
        public void TestUserEnablementDisabled() {
            bool handled = false;
            User user = this.CreateUser(this.CreateTenant());

            DomainEventPublisher.Instance.Subscribe<UserEnablementChanged>(e => {
                Assert.AreEqual(e.UserName, user.UserName);
                Assert.AreEqual(e.TenantId, user.TenantId.Id);
                handled = true;
            });

            user.DefineEnablement(new Enablement(false, null, null));

            Assert.IsFalse(user.IsEnabled);
            Assert.IsTrue(handled);
        }
        
        [Test]
        public void TestUserEnablementWithinStartEndDates() {
            bool handled = false;
            User user = this.CreateUser(this.CreateTenant());

            DomainEventPublisher.Instance.Subscribe<UserEnablementChanged>(e => {
                Assert.AreEqual(e.UserName, user.UserName);
                Assert.AreEqual(e.TenantId, user.TenantId.Id);
                handled = true;
            });

            DateTime now = DateTime.Now;
            user.DefineEnablement(new Enablement(true, now, now.AddDays(1)));

            Assert.IsTrue(user.IsEnabled);
            Assert.IsTrue(handled);
        }
        
        [Test]
        public void TestUserEnablementOutsideStartEndDates() {
            bool handled = false;
            User user = CreateUser(this.CreateTenant());

            DomainEventPublisher.Instance.Subscribe<UserEnablementChanged>(e => {
                Assert.AreEqual(e.UserName, user.UserName);
                Assert.AreEqual(e.TenantId, user.TenantId.Id);
                handled = true;
            });

            DateTime now = DateTime.Now;
            user.DefineEnablement(new Enablement(true, now.AddDays(-2), now.AddDays(-1)));

            Assert.IsFalse(user.IsEnabled);
            Assert.IsTrue(handled);
        }
        
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestUserEnablementUnsequencedDates() {
            User user = CreateUser(this.CreateTenant());

            DateTime now = DateTime.Now;
            user.DefineEnablement(new Enablement(true, now.AddDays(1), now));
        }

        [Test]
        public void TestUserDescriptor() {
            User user = CreateUser(this.CreateTenant());
            UserDescriptor userDescriptor = user.UserDescriptor;

            Assert.NotNull(userDescriptor.EmailAddress);
            Assert.AreEqual(userDescriptor.EmailAddress, "colin@saasovation.com");

            Assert.NotNull(userDescriptor.TenantId);
            Assert.AreEqual(userDescriptor.TenantId, user.TenantId);

            Assert.NotNull(userDescriptor.UserName);
            Assert.AreEqual(userDescriptor.UserName, user.UserName);
        }

        [Test]
        public void TestUserChangePassword() {
            User user = CreateUser(this.CreateTenant());
            bool handled = false;

            DomainEventPublisher.Instance.Subscribe<UserPasswordChanged>(e => {
                Assert.AreEqual(e.UserName, user.UserName);
                Assert.AreEqual(e.TenantId, user.TenantId.Id);
                handled = true;
            });

            user.ChangePassword("secretPassword!", "This Is ANewPassword.");

            Assert.IsTrue(handled);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestUserChangePasswordFails() {
            User user = CreateUser(this.CreateTenant());

            user.ChangePassword("no clue", "This Is ANewPassword."); 
        }

        [Test]
        public void TestUserPasswordHashedOnConstruction() {
            User user = CreateUser(this.CreateTenant());

            Assert.IsFalse(_password.Equals(user.Password));
        }

        [Test]
        public void TestUserPasswordHashedOnChange() {
            User user = CreateUser(this.CreateTenant());

            string strongPassword = ServiceLocator.GetService<PasswordService>().GenerateStrongPassword();

            user.ChangePassword(_password, strongPassword);

            Assert.IsFalse(_password.Equals(user.Password));
            Assert.IsFalse(strongPassword.Equals(user.Password));
        }

        [Test]
        public void TestUserPersonalContactInformationChanged() {
            User user = CreateUser(this.CreateTenant());

            bool handled = false;
            DomainEventPublisher.Instance.Subscribe<PersonContactInformationChanged>(e => {
                Assert.AreEqual(e.UserName, user.UserName);
                Assert.AreEqual(e.TenantId, user.TenantId.Id);
                handled = true;
            });

            user.ChangePersonalContactInformation(new ContactInformation(new EmailAddress("colinzhang@saasovation.com"),
                new PostalAddress("123 Mockingbird Lane", "Boulder", "CO", "80301", "US"), new Telephone("303-555-1210"),
                new Telephone("303-555-1212")));

            Assert.AreEqual(new EmailAddress("colinzhang@saasovation.com"), user.Person.EmailAddress);
            Assert.AreEqual("123 Mockingbird Lane", user.Person.ContactInformation.PostalAddress.StreetAddress);
            Assert.IsTrue(handled);
        }

        [Test]
        public void TestUserPersonNameChanged() {
            User user = CreateUser(this.CreateTenant());

            bool handled = false;
            DomainEventPublisher.Instance.Subscribe<PersonNameChanged>(e => {
                Assert.AreEqual(e.UserName, user.UserName);
                Assert.AreEqual(e.TenantId, user.TenantId.Id);
                Assert.AreEqual(e.Name.FirstName, "jinhua");
                Assert.AreEqual(e.Name.LastName, "zhang");
                handled = true;
            });

            user.ChangePersonalName(new FullName("jinhua", "zhang"));

            Assert.IsTrue(handled);
        }
    }
}