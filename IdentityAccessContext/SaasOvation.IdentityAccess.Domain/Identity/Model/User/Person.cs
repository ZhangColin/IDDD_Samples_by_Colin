using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class Person: ConcurrencySafeEntity {
        private TenantId _tenantId;
        private FullName _name;
        private ContactInformation _contactInformation;

        public virtual TenantId TenantId {
            get { return this._tenantId; }
            set {
                AssertionConcern.NotNull(value, "The tenantId is required.");
                this._tenantId = value;
            }
        }

        public virtual FullName Name {
            get { return this._name; }
            protected set {
                AssertionConcern.NotNull(value, "The person name is required.");
                this._name = value;
            }
        }

        public virtual ContactInformation ContactInformation {
            get { return this._contactInformation; }
            protected set {
                AssertionConcern.NotNull(value, "The person contact information is required.");
                this._contactInformation = value;
            }
        }

        public virtual User User { get; set; }

        public virtual EmailAddress EmailAddress {
            get { return ContactInformation.EmailAddress; }
        }

        protected Person() {}

        public Person(TenantId tenantId, FullName name, ContactInformation contactInformation) {
            this.TenantId = tenantId;
            this.Name = name;
            this.ContactInformation = contactInformation;
        }

        public virtual void ChangeContactInformation(ContactInformation contactInformation) {
            this.ContactInformation = contactInformation;

            DomainEventPublisher.Instance.Publish(new PersonContactInformationChanged(this.TenantId, this.User.UserName,
                this.ContactInformation));
        }

        public virtual void ChangeName(FullName name) {
            this.Name = name;

            DomainEventPublisher.Instance.Publish(new PersonNameChanged(this.TenantId, this.User.UserName, this.Name));
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return TenantId;
            yield return this.User.UserName;
        }

        public override string ToString() {
            return "Person [tenantId=" + TenantId
                + ", name=" + Name
                + ", contactInformation=" + ContactInformation + "]";
        }
    }
}