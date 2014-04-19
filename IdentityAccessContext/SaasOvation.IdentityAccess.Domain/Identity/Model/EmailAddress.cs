using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model {
    public class EmailAddress: ValueObject {
        private string _address;

        protected EmailAddress() { }

        public EmailAddress(string address) {
            this.Address = address;
        }

        public EmailAddress(EmailAddress emailAddress): this(emailAddress.Address) {}

        public string Address {
            get { return this._address; }
            set {
                AssertionConcern.NotEmpty(value, "The email address is required.");
                AssertionConcern.Length(value, 1, 100, "Email address must be 100 characters or less.");
                AssertionConcern.Matches("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*",
                        value, "Email address format is invalid.");

                this._address = value;
            }
        }

        public override string ToString() {
            return "EmailAddress [address=" + this.Address + "]";
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.Address.ToUpper();
        }
    }
}