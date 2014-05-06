using System;
using System.Collections.Generic;

namespace SaasOvation.Common.Domain.Model {
    public abstract class Identity: ValueObject {
        public string Id { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.Id;
        }

        protected Identity() { }

        protected Identity(string id) {
            AssertionConcern.NotEmpty(id, "The basic identity is required.");
//            AssertionConcern.Length(id, 36, "The basic identity must be 36 characters.");

            this.ValidateId(id);

            this.Id = id;
        }


        public override string ToString() {
            return this.GetType().Name + " [Id=" + Id + "]";
        }

        protected virtual void ValidateId(string id) {
            
        }
    }
}