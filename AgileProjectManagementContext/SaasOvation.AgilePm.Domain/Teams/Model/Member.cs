using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Teams.Model {
    public abstract class Member: Entity {
        public TenantId TenantId { get; private set; }
        public string UserName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public bool Enabled { get; private set; }
        public MemberChangeTracker ChangeTracker { get; private set; }

        protected Member(TenantId tenantId, string userName, string firstName, string lastName, string emailAddress,
            DateTime initializedOn) {
            AssertionConcern.NotEmpty(userName, "The username must be provided.");
            AssertionConcern.Length(userName, 250, "The username must be 250 characters or less.");
            if(!string.IsNullOrEmpty(emailAddress)) {
                AssertionConcern.Length(emailAddress, 250, "The email address must be 100 characters or less.");
            }
            if(!string.IsNullOrEmpty(firstName)) {
                AssertionConcern.Length(firstName, 50, "First name must be 50 characters or less.");
            }
            if(!string.IsNullOrEmpty(lastName)) {
                AssertionConcern.Length(lastName, 50, "Last name must be 50 characters or less.");
            }

            this.TenantId = tenantId;
            this.UserName = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAddress = emailAddress;
            this.Enabled = true;
            this.ChangeTracker = new MemberChangeTracker(initializedOn, initializedOn, initializedOn);
        }

        public void ChangeEmailAddress(string emailAddress, DateTime asOfDate) {
            if(this.ChangeTracker.CanChangeEmailAddress(asOfDate) && this.EmailAddress!=emailAddress) {
                this.EmailAddress = emailAddress;
                this.ChangeTracker = this.ChangeTracker.EmailAddressChangedOn(asOfDate);
            }
        }

        public void ChangeName(string firstName, string lastName, DateTime asOfDate) {
            if(this.ChangeTracker.CanChangeName(asOfDate)) {
                this.FirstName = firstName;
                this.LastName = lastName;
                this.ChangeTracker = this.ChangeTracker.NameChangedOn(asOfDate);
            }
        }

        public void Disable(DateTime asOfDate) {
            if(this.ChangeTracker.CanToggleEnabling(asOfDate)) {
                this.Enabled = false;
                this.ChangeTracker = this.ChangeTracker.EnablingOn(asOfDate);
            }
        }
        
        public void Enable(DateTime asOfDate) {
            if(this.ChangeTracker.CanToggleEnabling(asOfDate)) {
                this.Enabled = true;
                this.ChangeTracker = this.ChangeTracker.EnablingOn(asOfDate);
            }
        }
    }
}