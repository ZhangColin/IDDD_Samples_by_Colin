using System.Collections.Generic;
using System.Linq;

namespace SaasOvation.Common.Domain.Model {
    public abstract class ValueObject {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj) {
            if (object.ReferenceEquals(this, obj)) {
                return true;
            }
            if (object.ReferenceEquals(null, obj)) {
                return false;
            }
            if (this.GetType() != obj.GetType()) {
                return false;
            }
            ValueObject valueObject = obj as ValueObject;
            return this.GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode() {
            return HashCodeHelper.CombineHashCodes(this.GetEqualityComponents());
        }
    }
}