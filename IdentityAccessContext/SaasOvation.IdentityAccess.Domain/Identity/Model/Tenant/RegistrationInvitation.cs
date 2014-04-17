using System;
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant {
    public class RegistrationInvitation: EntityWithCompositeId {
        public TenantId TenantId { get; private set; }
        public string InvitationId { get; private set; }
        public string Description { get; private set; }
        public DateTime StartingOn { get; private set; }
        public DateTime Until { get; private set; }

        public RegistrationInvitation(TenantId tenantId, string invitationId,
            string description, DateTime startingOn, DateTime until) {
            this.TenantId = tenantId;
            this.InvitationId = invitationId;
            this.Description = description;
            this.StartingOn = startingOn;
            this.Until = until;
        }

        public RegistrationInvitation(TenantId tenantId, string invitationId, string description):
            this(tenantId, invitationId, description, DateTime.MinValue, DateTime.MinValue) {}

        public bool IsAvailable() {
            bool isAvailable = false;

            if(this.StartingOn==DateTime.MinValue && this.Until==DateTime.MinValue) {
                isAvailable = true;
            }
            else {
                long time = DateTime.Now.Ticks;
                if(time >= this.StartingOn.Ticks && time <= this.Until.Ticks) {
                    isAvailable = true;
                }
            }
            return isAvailable;
        }

        public bool IsIdentifiedBy(string invitationIdentifier) {
            bool isIdentified = this.InvitationId.Equals(invitationIdentifier);
            if(!isIdentified && this.Description!=null) {
                isIdentified = this.Description.Equals(invitationIdentifier);
            }

            return isIdentified;
        }

        public RegistrationInvitation OpenEnded() {
            this.StartingOn = DateTime.MinValue;
            this.Until = DateTime.MinValue;
            return this;
        }

        public RegistrationInvitation RedefineAs() {
            this.StartingOn = DateTime.MinValue;
            this.Until = DateTime.MinValue;
            return this;
        }

        public InvitationDescriptor ToDescriptor() {
            return new InvitationDescriptor(this.TenantId, this.InvitationId, this.Description, this.StartingOn, this.Until);
        }

        public RegistrationInvitation WillStartOn(DateTime date) {
            if(this.Until!=DateTime.MinValue) {
                throw new InvalidOperationException("Cannot set starting-on date after until date.");
            }

            this.StartingOn = date;
            this.Until = new DateTime(date.Ticks+86400000);

            return this;
        }

        public RegistrationInvitation LastingUntil(DateTime date) {
            if(this.StartingOn==DateTime.MinValue) {
                throw new InvalidOperationException("Cannot set until date before setting starting-on date.");
            }

            this.Until = date;

            return this;
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.InvitationId;
        }

        public override string ToString() {
            return "RegistrationInvitation ["
                + "tenantId=" + this.TenantId
                + ", description=" + this.Description
                + ", invitationId=" + this.InvitationId
                + ", startingOn=" + this.StartingOn
                + ", until=" + this.Until + "]";
        }
    }
}