using System;
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant {
    public class RegistrationInvitation: ConcurrencySafeEntity {
        public virtual TenantId TenantId { get; protected set; }
        public virtual string InvitationId { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual DateTime StartingOn { get; protected set; }
        public virtual DateTime Until { get; protected set; }

        protected RegistrationInvitation() { }

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

        public virtual bool IsAvailable() {
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

        public virtual bool IsIdentifiedBy(string invitationIdentifier) {
            bool isIdentified = this.InvitationId.Equals(invitationIdentifier);
            if(!isIdentified && this.Description!=null) {
                isIdentified = this.Description.Equals(invitationIdentifier);
            }

            return isIdentified;
        }

        public virtual RegistrationInvitation OpenEnded() {
            this.StartingOn = DateTime.MinValue;
            this.Until = DateTime.MinValue;
            return this;
        }

        public virtual RegistrationInvitation RedefineAs() {
            this.StartingOn = DateTime.MinValue;
            this.Until = DateTime.MinValue;
            return this;
        }

        public virtual InvitationDescriptor ToDescriptor() {
            return new InvitationDescriptor(this.TenantId, this.InvitationId, this.Description, this.StartingOn, this.Until);
        }

        public virtual RegistrationInvitation WillStartOn(DateTime date) {
            if(this.Until!=DateTime.MinValue) {
                throw new InvalidOperationException("Cannot set starting-on date after until date.");
            }

            this.StartingOn = date;
            this.Until = new DateTime(date.Ticks+86400000);

            return this;
        }

        public virtual RegistrationInvitation LastingUntil(DateTime date) {
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