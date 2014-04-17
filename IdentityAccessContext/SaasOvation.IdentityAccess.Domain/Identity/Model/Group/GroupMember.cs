using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Group {
    public class GroupMember: ValueObject {
        public TenantId TenantId { get; private set; }
        public string Name { get; private set; }
        public GroupMemberType Type { get; private set; }
        protected GroupMember() {}

        public GroupMember(TenantId tenantId, string name, GroupMemberType type) {
            AssertionConcern.NotNull(tenantId, "The tenantId must be provided.");
            AssertionConcern.NotEmpty(name, "Member name is required.");
            AssertionConcern.Length(name, 1, 100, "Member name must be 100 characters or less.");

            this.TenantId = tenantId;
            this.Name = name;
            this.Type = type;
        }

        public bool IsGroup {
            get { return this.Type == GroupMemberType.Group; }
        }
        
        public bool IsUser {
            get { return this.Type == GroupMemberType.User; }
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return TenantId;
            yield return Name;
            yield return Type;
        }

        public override string ToString() {
            return "GroupMember [name=" + Name + ", tenantId=" + TenantId + ", type=" + Type + "]";
        }
    }
}