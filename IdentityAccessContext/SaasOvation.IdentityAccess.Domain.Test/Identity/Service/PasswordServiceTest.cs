using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Test.Identity.Service {
    [TestFixture]
    public class PasswordServiceTest {
        private readonly PasswordService _passwordService = new PasswordService();
        [Test]
        public void TestGenerateStrongPassword() {
            string password = this._passwordService.GenerateStrongPassword();

            Assert.IsTrue(_passwordService.IsStrong(password));
            Assert.IsFalse(_passwordService.IsWeak(password));
        }

        [Test]
        public void TestIsStrongPassword() {
            const string password = "Th1sShudBStrong.";

            Assert.IsTrue(_passwordService.IsStrong(password));
            Assert.IsFalse(_passwordService.IsVeryStrong(password));
            Assert.IsFalse(_passwordService.IsWeak(password));
        }
        
        [Test]
        public void TestIsVeryStrongPassword() {
            const string password = "Th1sSh0uldBV3ryStrong!";

            Assert.IsTrue(_passwordService.IsStrong(password));
            Assert.IsTrue(_passwordService.IsVeryStrong(password));
            Assert.IsFalse(_passwordService.IsWeak(password));
        }
        
        [Test]
        public void TestIsWeakPassword() {
            const string password = "Weakness";

            Assert.IsFalse(_passwordService.IsStrong(password));
            Assert.IsFalse(_passwordService.IsVeryStrong(password));
            Assert.IsTrue(_passwordService.IsWeak(password));
        }
    }
}