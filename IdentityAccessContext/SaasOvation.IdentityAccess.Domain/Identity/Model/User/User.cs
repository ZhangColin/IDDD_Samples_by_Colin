using System.Collections.Generic;
using SaasOvation.Common;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class User: EntityWithCompositeId {
        private TenantId _tenantId;
        private string _userName;
        private Enablement _enablement;
        private Person _person;

        public TenantId TenantId {
            get { return this._tenantId; }
            set {
                AssertionConcern.NotNull(value, "The tenantId is required.");
                this._tenantId = value;
            }
        }

        public string UserName {
            get { return this._userName; }
            set {
                AssertionConcern.NotEmpty(value, "The username is required.");
                AssertionConcern.Length(value, 3, 250, "The username must be 3 to 250 characters.");

                this._userName = value;
            }
        }

        public string Password { get; private set; }

        public Enablement Enablement {
            get { return this._enablement; }
            set {
                AssertionConcern.NotNull(value, "The enablement is required.");
                this._enablement = value;
            }
        }

        public Person Person {
            get { return this._person; }
            set {
                AssertionConcern.NotNull(value, "The person is required.");
                this._person = value;
            }
        }

        protected User() {}

        public User(TenantId tenantId, string userName, string password, Enablement enablement, Person person) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.Enablement = enablement;
            this.Person = person;

            ProtectPassword("", password);

            person.User = this;

            DomainEventPublisher.Instance.Publish(new UserRegistered(
                tenantId, userName, person.Name, person.ContactInformation.EmailAddress));
        }

        public bool IsEnabled {
            get { return this.Enablement.IsEnablementEnabled(); }
        }

        public UserDescriptor UserDescriptor {
            get {
                return new UserDescriptor(TenantId, UserName, Person.EmailAddress.Address);
            }
        }

        public void ChangePassword(string currentPassword, string changedPassword) {
            AssertionConcern.NotEmpty(currentPassword, "Current and new password must be provided.");
            AssertionConcern.Equals(this.Password, this.AsEncryptedValue(currentPassword),
                "Current password not confirmed");

            this.ProtectPassword(currentPassword, changedPassword);

            DomainEventPublisher.Instance.Publish(new UserPasswordChanged(this.TenantId, this.UserName));
        }

        public void ChangePersonalContactInformation(ContactInformation contactInformation) {
            Person.ChangeContactInformation(contactInformation);
        }

        public void ChangePersonalName(FullName personalName) {
            Person.ChangeName(personalName);
        }

        public void DefineEnablement(Enablement enablement) {
            this.Enablement = enablement;
            DomainEventPublisher.Instance.Publish(new UserEnablementChanged(this.TenantId, this.UserName,
                this.Enablement));
        }

        internal GroupMember ToGroupMember() {
            return new GroupMember(TenantId, UserName, GroupMemberType.User);
        }

        private void ProtectPassword(string currentPassword, string changedPassword) {
            AssertionConcern.NotEquals(currentPassword, changedPassword, "The password is unchanged.");
            AssertionConcern.False(ServiceLocator.GetService<PasswordService>().IsWeak(changedPassword),
                "The password must be stronger.");
            AssertionConcern.NotEquals(UserName, changedPassword, "The username and password must not be the same.");

            this.Password = this.AsEncryptedValue(changedPassword);
        }

        private string AsEncryptedValue(string plainTextPassword) {
            // TODO: 服务定位器的使用使得对外暴露了实现细节，使用者必须知道如何正确地提供IEncryptionService
            return ServiceLocator.GetService<IEncryptionService>().EncryptedValue(plainTextPassword);
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return TenantId;
            yield return UserName;
        }

        public override string ToString() {
            return "User [tenantId=" + TenantId + ", username=" + UserName
                    + ", person=" + Person + ", enablement=" + Enablement + "]";
        }
    }
}