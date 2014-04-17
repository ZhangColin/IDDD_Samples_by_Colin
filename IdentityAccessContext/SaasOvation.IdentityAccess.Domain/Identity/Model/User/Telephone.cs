using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class Telephone: ValueObject {
        private string _number;

        public string Number {
            get { return this._number; }
            set {
                AssertionConcern.NotEmpty(value, "Telephone number is required.");
                AssertionConcern.Length(value, 5, 20, "Telephone number may not be more than 20 characters.");

                this._number = value;
            }
        }

        protected Telephone() {}

        public Telephone(string number) {
            this.Number = number;
        }

        public Telephone(Telephone telephone): this(telephone.Number) {}

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Number;
        }

        public override string ToString() {
            return "Telephone [number=" + Number + "]";
        }
    }
}