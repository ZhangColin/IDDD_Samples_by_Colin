using System;
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Collaborators {
    public abstract class Collaborator: ValueObject, IComparable<Collaborator> {
        public string Identity { get; private set; }
        public string Name { get; private set; }
        public string EmailAddress { get; private set; }
        protected Collaborator() {}

        protected Collaborator(string identity, string name, string emailAddress) {
            this.Identity = identity;
            this.Name = name;
            this.EmailAddress = emailAddress;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.EmailAddress;
            yield return this.Identity;
            yield return this.Name;
        }

        public int CompareTo(Collaborator other) {
            int diff = string.Compare(this.Identity, other.Identity, StringComparison.Ordinal);
            if(diff==0) {
                diff = string.Compare(this.EmailAddress, other.EmailAddress, StringComparison.Ordinal);
                if(diff==0) {
                    diff = string.Compare(this.Name, other.Name, StringComparison.Ordinal);
                }
            }

            return diff;
        }

        public override string ToString() {
            return this.GetType().Name +
                " [EmailAddress=" + this.EmailAddress + ", Identity=" + this.Identity + ", Name=" + this.Name + "]";
        }
    }
}