using System;

namespace SaasOvation.Common.Domain.Model {
    public abstract class Identity: IEquatable<Identity>, IIdentity {
        protected Identity() {
            this.Id = Guid.NewGuid().ToString();
        }

        protected Identity(string id) {
            AssertionConcern.NotEmpty(id, "The basic identity is required.");
            AssertionConcern.Length(id, 36, "The basic identity must be 36 characters.");

            this.ValidateId(id);

            this.Id = id;
        }

        public string Id { get; protected set; }

        public bool Equals(Identity id) {
            if(object.ReferenceEquals(this, id)) {
                return true;
            }
            if(object.ReferenceEquals(null, id)) {
                return false;
            }
            return this.Id.Equals(id.Id);
        }

        public override bool Equals(object obj) {
            return Equals(obj as Identity);
        }

        public override int GetHashCode() {
            return (this.GetType().GetHashCode() * 907) + this.Id.GetHashCode();
        }

        public override string ToString() {
            return this.GetType().Name + " [Id=" + Id + "]";
        }

        protected virtual void ValidateId(string id) {
            
        }
    }
}