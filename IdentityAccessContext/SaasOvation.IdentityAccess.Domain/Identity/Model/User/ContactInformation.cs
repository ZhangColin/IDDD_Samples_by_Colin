using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class ContactInformation: ValueObject {
        public EmailAddress EmailAddress { get; private set; }
        public PostalAddress PostalAddress { get; private set; }
        public Telephone PrimaryTelephone { get; private set; }
        public Telephone SecondaryTelephone { get; private set; }

        protected ContactInformation() {}

        public ContactInformation(EmailAddress emailAddress, PostalAddress postalAddress, Telephone primaryTelephone,
            Telephone secondaryTelephone) {
            this.EmailAddress = emailAddress;
            this.PostalAddress = postalAddress;
            this.PrimaryTelephone = primaryTelephone;
            this.SecondaryTelephone = secondaryTelephone;
        }

        public ContactInformation(ContactInformation contactInformation)
            : this(contactInformation.EmailAddress, contactInformation.PostalAddress,
                contactInformation.PrimaryTelephone, contactInformation.SecondaryTelephone) {}

        public ContactInformation ChangeEmailAddress(EmailAddress emailAddress) {
            return new ContactInformation(emailAddress, PostalAddress, PrimaryTelephone, SecondaryTelephone);
        }

        public ContactInformation ChangePostalAddress(PostalAddress postalAddress) {
            return new ContactInformation(EmailAddress, postalAddress, PrimaryTelephone, SecondaryTelephone);
        }

        public ContactInformation ChangePrimaryTelephone(Telephone telephone) {
            return new ContactInformation(EmailAddress, PostalAddress, telephone, SecondaryTelephone);
        }

        public ContactInformation ChangeSecondaryTelephone(Telephone telephone) {
            return new ContactInformation(EmailAddress, PostalAddress, PrimaryTelephone, telephone);
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.EmailAddress;
            yield return this.PostalAddress;
            yield return this.PrimaryTelephone;
            yield return this.SecondaryTelephone;
        }

        public override string ToString() {
            return "ContactInformation [emailAddress=" + EmailAddress
                    + ", postalAddress=" + PostalAddress
                    + ", primaryTelephone=" + PrimaryTelephone
                    + ", secondaryTelephone=" + SecondaryTelephone + "]";
        }
    }
}