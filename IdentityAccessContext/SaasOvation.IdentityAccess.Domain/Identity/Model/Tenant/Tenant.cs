using System;
using System.Collections.Generic;
using System.Linq;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant {
    public class Tenant: EntityWithCompositeId {
        private readonly ISet<RegistrationInvitation> _registrationInvitations;

        public TenantId TenantId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }

        protected Tenant() {}

        public Tenant(TenantId tenantId, string name, string description, bool active) {
            AssertionConcern.NotNull(tenantId, "TenantId is required.");

            AssertionConcern.NotEmpty(name, "The tenant name is required.");
            AssertionConcern.Length(name, 1, 100, "The name must be 100 characters or less.");
            
            AssertionConcern.NotEmpty(description, "The tenant description is required.");
            AssertionConcern.Length(description, 1, 100, "The description must be 100 characters or less.");

            this.TenantId = tenantId;
            this.Name = name;
            this.Description = description;
            this.Active = active;

            this._registrationInvitations = new HashSet<RegistrationInvitation>();
        }

        public void Activate() {
            if(!this.Active) {
                this.Active = true;
                DomainEventPublisher.Instance.Publish(new TenantActivated(this.TenantId));
            }
        }

        public void Deactivate() {
            if (this.Active) {
                this.Active = false;
                DomainEventPublisher.Instance.Publish(new TenantDeactivated(this.TenantId));
            }
        }

        public ICollection<InvitationDescriptor> AllAvailableRegistrationInvitations() {
            AssertionConcern.True(this.Active, "Tenant is not active.");
            return this.AllRegistrationInvitationsFor(true);
        } 
        
        public ICollection<InvitationDescriptor> AllUnavailableRegistrationInvitations() {
            AssertionConcern.True(this.Active, "Tenant is not active.");
            return this.AllRegistrationInvitationsFor(false);
        }

        public bool IsRegistrationAvailableThrough(string invitationIdentifier) {
            AssertionConcern.True(this.Active, "Tenant is not active.");
            RegistrationInvitation invitation = this.GetInvitation(invitationIdentifier);
            return invitation != null && invitation.IsAvailable();
        }

        public RegistrationInvitation OfferRegistrationInvitation(string description) {
            AssertionConcern.True(this.Active, "Tenant is not active.");
            AssertionConcern.False(this.IsRegistrationAvailableThrough(description), "Invitation already exists.");

            RegistrationInvitation invitation = new RegistrationInvitation(this.TenantId, Guid.NewGuid().ToString(),
                description);

            AssertionConcern.True(_registrationInvitations.Add(invitation), "The invitation should have been added.");

            return invitation;
        }

        public Group.Group ProvisionGroup(string name, string description) {
            AssertionConcern.True(this.Active, "Tenant is not active.");

            Group.Group group = new Group.Group(this.TenantId, name, description);

            DomainEventPublisher.Instance.Publish(new GroupProvisioned(this.TenantId, name));

            return group;
        }

        public Role ProvisionRole(string name, string description, bool supportsNesting = false) {
            AssertionConcern.True(this.Active, "Tenant is not active.");

            Role role = new Role(this.TenantId, name, description, supportsNesting);

            DomainEventPublisher.Instance.Publish(new RoleProvisioned(this.TenantId, name));

            return role;
        }

        public RegistrationInvitation RedefineRegistrationInvitationAs(string invitationIdentifier) {
            AssertionConcern.True(this.Active, "Tenant is not active.");
            RegistrationInvitation invitation = this.GetInvitation(invitationIdentifier);
            if(invitation!=null) {
                invitation.RedefineAs().OpenEnded();
            }
            return invitation;
        }

        public User.User RegisterUser(string invitationIdentifier, string userName, string password,
            Enablement enablement, Person person) {
            AssertionConcern.True(this.Active, "Tenant is not active.");
            User.User user = null;
            if(this.IsRegistrationAvailableThrough(invitationIdentifier)) {
                person.TenantId = this.TenantId;
                user = new User.User(this.TenantId, userName, password, enablement, person);
            }
            return user;
        }

        public void WithdrawInvitation(string invitationIdentifier) {
            RegistrationInvitation invitation = this.GetInvitation(invitationIdentifier);
            if (invitation != null) {
                _registrationInvitations.Remove(invitation);
            }
        }

        private List<InvitationDescriptor> AllRegistrationInvitationsFor(bool isAvailable) {
            return this._registrationInvitations.Where(ri => ri.IsAvailable() == isAvailable)
                .Select(ri => ri.ToDescriptor()).ToList();
        }

        private RegistrationInvitation GetInvitation(string invitationIdentifier) {
            return this._registrationInvitations.FirstOrDefault(ri => ri.IsIdentifiedBy(invitationIdentifier));
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return TenantId;
            yield return Name;
        }

        public override string ToString() {
            return "Tenant [tenantId=" + TenantId + ", name=" + Name + ", description=" + Description + ", active="
                + Active + "]";
        }
    }
}