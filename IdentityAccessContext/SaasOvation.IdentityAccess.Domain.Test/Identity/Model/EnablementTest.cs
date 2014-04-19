using System;
using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;

namespace SaasOvation.IdentityAccess.Domain.Test.Identity.Model {
    [TestFixture]
    public class EnablementTest {
        [Test]
        public void TestEnablementEnabled() {
            Enablement enablement = new Enablement(true, null, null);

            Assert.IsTrue(enablement.IsEnablementEnabled());
        }

        [Test]
        public void TestEnablementDisabled() {
            Enablement enablement = new Enablement(false, null, null);

            Assert.IsFalse(enablement.IsEnablementEnabled());
        }

        [Test]
        public void TestEnablementOutsideStartEndDates() {
            Enablement enablement = new Enablement(true, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));

            Assert.IsFalse(enablement.IsEnablementEnabled());
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestEnablementUnsequenceDates() {
            Enablement enablement = new Enablement(true, DateTime.Now.AddDays(1), DateTime.Now);
        }

        [Test]
        public void TestEnablementEndsTimeExpired() {
            Enablement enablement = new Enablement(true, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));

            Assert.IsTrue(enablement.IsTimeExpired());
        }

        [Test]
        public void TestEnablementHasNotBegunTimeExpired() {
            Enablement enablement = new Enablement(true, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2));

            Assert.True(enablement.IsTimeExpired());
        }
    }
}