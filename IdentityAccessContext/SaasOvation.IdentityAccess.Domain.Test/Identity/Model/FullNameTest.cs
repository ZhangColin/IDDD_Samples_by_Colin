using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Identity.Model;

namespace SaasOvation.IdentityAccess.Domain.Test.Identity.Model {
    [TestFixture]
    public class FullNameTest {
        private string _firstName = "colin";
        private const string _wrongFirstName = "wright";
        private const string _lastName = "zhang";
        private const string _wrongLastName = "stwyhm";

        [Test]
        public void TestChangedFirstName() {
            FullName name = new FullName(_wrongFirstName, _lastName);
            name = name.WithChangedFirstName(this._firstName);
            Assert.AreEqual(string.Format("{0} {1}", _firstName, _lastName), name.AsFormattedName());
        }
        
        [Test]
        public void TestChangedLastName() {
            FullName name = new FullName(_firstName, _wrongLastName);
            name = name.WithChangedLastName(_lastName);
            Assert.AreEqual(string.Format("{0} {1}", _firstName, _lastName), name.AsFormattedName());
        }

        [Test]
        public void TestFormattedName() {
            FullName name = new FullName(this._firstName, _lastName);
            Assert.AreEqual(string.Format("{0} {1}", _firstName, _lastName), name.AsFormattedName());
        }
    }
}