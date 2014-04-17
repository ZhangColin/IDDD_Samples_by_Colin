using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class UserDescriptor: ValueObject {
        public TenantId TenantId { get; private set; }
        public string UserName { get; private set; }
        public string EmailAddress { get; private set; }

        public static UserDescriptor NullDescriptorInstance() {
            return new UserDescriptor();
        }

        public bool IsNullDescriptor() {
            return this.EmailAddress == null || this.TenantId == null || this.UserName == null;
        }

        private UserDescriptor() { }

        public UserDescriptor(TenantId tenantId, string userName, string emailAddress) {
            this.TenantId = tenantId;
            this.UserName = userName;
            this.EmailAddress = emailAddress;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.EmailAddress;
            yield return this.TenantId;
            yield return this.UserName;
        }

        public override string ToString() {
            return "UserDescriptor [emailAddress=" + this.EmailAddress
                    + ", tenantId=" + this.TenantId + ", username=" + this.UserName + "]";
        }
    }
}