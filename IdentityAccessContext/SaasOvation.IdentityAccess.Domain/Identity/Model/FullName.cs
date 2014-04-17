using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model {
    public class FullName: ValueObject {
        private readonly string _firstName;
        private readonly string _lastName;

        protected FullName() { }

        public FullName(string firstName, string lastName) {
            this._firstName = firstName;
            this._lastName = lastName;
        }

        public FullName(FullName fullName): this(fullName.FirstName, fullName.LastName) {}

        public string FirstName {
            get { return this._firstName; }
        }

        public string LastName {
            get { return this._lastName; }
        }

        public string AsFormattedName() {
            return this.FirstName + " " + this.LastName;
        }

        public FullName WithChangedFirstName(string firstName) {
            return new FullName(firstName, this.LastName);
        }

        public FullName WithChangedLastName(string lastName) {
            return new FullName(this.FirstName, lastName);
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.FirstName;
            yield return this.LastName;
        }

        public override string ToString() {
            return "FullName [firstName=" + this.FirstName + ", lastName=" + this.LastName + "]";
        }
    }
}