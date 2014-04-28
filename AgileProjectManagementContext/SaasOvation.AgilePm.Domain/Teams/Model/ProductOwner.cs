using System;
using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Domain.Teams.Model {
    public class ProductOwner: Member {
        public ProductOwner(TenantId tenantId, string userName, string firstName, string lastName, string emailAddress,
            DateTime initializedOn): base(tenantId, userName, firstName, lastName, emailAddress, initializedOn) {
            
        }

        public ProductOwnerId ProductOwnerId { get; private set; }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.UserName;
        }

        public override string ToString() {
            return "ProductOwner [ProductOwnerId=" + ProductOwnerId +
                ", EmailAddress=" + EmailAddress +
                ", IsEnabled=" + Enabled +
                ", FirstName=" + FirstName +
                ", LastName=" + LastName +
                ", TenantId=" + TenantId +
                ", UserName=" + UserName + "]";
        }
    }
}