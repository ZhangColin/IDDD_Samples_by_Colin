using NUnit.Framework;
using SaasOvation.IdentityAccess.Domain.Identity.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;

namespace SaasOvation.IdentityAccess.Domain.Test.Identity.Model {
    [TestFixture]
    public class ContactInformationTest {
        private const string _city = "Boulder";
        private const string _emailAddress = "colin@saasovation.com";
        private const string _stateProvince = "CO";
        private const string _streetAddress = "123 Pearl Street";
        private const string _postalCode = "80301";
        private const string _countryCode = "US";
        private const string _primaryTelephone = "303-555-1210";
        private const string _secondaryTelephone = "303-555-1212";

        protected ContactInformation ContactInformation() {
            return new ContactInformation(new EmailAddress(_emailAddress), 
                new PostalAddress(_streetAddress, _city, _stateProvince, _postalCode, _countryCode), 
                new Telephone(_primaryTelephone), new Telephone(_secondaryTelephone));
        }

        [Test]
        public void TestContactInformation() {
            ContactInformation contactInformation = this.ContactInformation();

            Assert.AreEqual(_emailAddress, contactInformation.EmailAddress.Address);
            Assert.AreEqual(_city, contactInformation.PostalAddress.City);
            Assert.AreEqual(_stateProvince, contactInformation.PostalAddress.StateProvince);
        }

        [Test]
        public void TestChangeEmailAddress() {
            ContactInformation contactInformation = this.ContactInformation();
            ContactInformation contactInformationCopy = new ContactInformation(contactInformation);

            const string changeEmailAddress = "colinzhang@saasovation.com";
            ContactInformation contactInformation2 = contactInformation.ChangeEmailAddress(new EmailAddress(changeEmailAddress));

            Assert.AreEqual(contactInformationCopy, contactInformation);
            Assert.IsFalse(contactInformation.Equals(contactInformation2));
            Assert.IsFalse(contactInformationCopy.Equals(contactInformation2));

            Assert.AreEqual(changeEmailAddress, contactInformation2.EmailAddress.Address);
        }
        
        [Test]
        public void TestChangePostalAddress() {
            ContactInformation contactInformation = this.ContactInformation();
            ContactInformation contactInformationCopy = new ContactInformation(contactInformation);

            ContactInformation contactInformation2 = contactInformation.ChangePostalAddress(
                new PostalAddress("321 Mockingbird Lane", "Denver", _stateProvince, "81121", _countryCode));

            Assert.AreEqual(contactInformationCopy, contactInformation);
            Assert.IsFalse(contactInformation.Equals(contactInformation2));
            Assert.IsFalse(contactInformationCopy.Equals(contactInformation2));

            Assert.AreEqual("321 Mockingbird Lane", contactInformation2.PostalAddress.StreetAddress);
            Assert.AreEqual("Denver", contactInformation2.PostalAddress.City);
            Assert.AreEqual(_stateProvince, contactInformation.PostalAddress.StateProvince);
        }
        
        [Test]
        public void TestChangePrimaryTelephone() {
            ContactInformation contactInformation = this.ContactInformation();
            ContactInformation contactInformationCopy = new ContactInformation(contactInformation);

            const string changeNumber = "720-555-1212";
            ContactInformation contactInformation2 =
                contactInformation.ChangePrimaryTelephone(new Telephone(changeNumber));

            Assert.AreEqual(contactInformationCopy, contactInformation);
            Assert.IsFalse(contactInformation.Equals(contactInformation2));
            Assert.IsFalse(contactInformationCopy.Equals(contactInformation2));

            Assert.AreEqual(changeNumber, contactInformation2.PrimaryTelephone.Number);
        }
        
        [Test]
        public void TestChangeSecondaryTelephone() {
            ContactInformation contactInformation = this.ContactInformation();
            ContactInformation contactInformationCopy = new ContactInformation(contactInformation);

            const string changeNumber = "720-555-1212";
            ContactInformation contactInformation2 =
                contactInformation.ChangeSecondaryTelephone(new Telephone(changeNumber));

            Assert.AreEqual(contactInformationCopy, contactInformation);
            Assert.IsFalse(contactInformation.Equals(contactInformation2));
            Assert.IsFalse(contactInformationCopy.Equals(contactInformation2));

            Assert.AreEqual(changeNumber, contactInformation2.SecondaryTelephone.Number);
        }
    }
}