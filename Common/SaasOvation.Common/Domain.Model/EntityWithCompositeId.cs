using System.Collections.Generic;
using System.Linq;

namespace SaasOvation.Common.Domain.Model {
    public abstract class EntityWithCompositeId: Entity {
        protected abstract IEnumerable<object> GetIdentityComponents();

        public override bool Equals(object obj) {
            if(object.ReferenceEquals(this, obj)) {
                return true;
            }
            if(object.ReferenceEquals(null, obj)) {
                return false;
            }
            if(this.GetType()!=obj.GetType()) {
                return false;
            }
            EntityWithCompositeId other = obj as EntityWithCompositeId;
            return this.GetIdentityComponents().SequenceEqual(other.GetIdentityComponents());
        }

        public override int GetHashCode() {
            return HashCodeHelper.CombineHashCodes(this.GetIdentityComponents());
        }
    }
}