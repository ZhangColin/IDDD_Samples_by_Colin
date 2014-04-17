using System;
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Access.Model {
    public class Role: ConcurrencySafeEntity {
        private readonly Group _group;

        public int Id { get; private set; }
        public TenantId TenantId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool SupportsNesting { get; private set; }

        public Group Group {
            get { return this._group; }
        }

        protected Role() {}

        public Role(TenantId tenantId, string name, string description, bool supportsNesting) {
            this.TenantId = tenantId;
            this.Name = name;
            this.Description = description;
            this.SupportsNesting = supportsNesting;

            this._group = CreateInternalGroup();
        }

        private Group CreateInternalGroup() {
            string groupName = Group.RoleGroupPrefix + Guid.NewGuid().ToString();
            return new Group(this.TenantId, groupName, "Role backing group for: "+this.Name);
        }

        public void AssignGroup(Group group, IGroupMemberService groupMemberService) {
            AssertionConcern.True(this.SupportsNesting, "This role does not support group nestiong.");
            AssertionConcern.NotNull(group, "Group must not be null.");
            AssertionConcern.Equals(this.TenantId, group.TenantId, "Wrong tenant for this group.");

            this.Group.AddGroup(group, groupMemberService);

            DomainEventPublisher.Instance.Publish(new GroupAssignedToRole(this.TenantId, this.Name, group.Name));
        }

        public void UnassignGroup(Group group) {
            AssertionConcern.True(this.SupportsNesting, "This role does not support group nestiong.");
            AssertionConcern.NotNull(group, "Group must not be null.");
            AssertionConcern.Equals(this.TenantId, group.TenantId, "Wrong tenant for this group.");

            this.Group.RemoveGroup(group);

            DomainEventPublisher.Instance.Publish(new GroupUnassignedFromRole(this.TenantId, this.Name, group.Name));
        }

        public void AssignUser(User user) {
            AssertionConcern.NotNull(user, "User must not be null.");
            AssertionConcern.Equals(this.TenantId, user.TenantId, "Wrong tenant for this user.");

            this.Group.AddUser(user);

            DomainEventPublisher.Instance.Publish(new UserAssignedToRole(this.TenantId, this.Name, user.UserName,
                user.Person.Name, user.Person.EmailAddress));
        }

        public void UnassignUser(User user) {
            AssertionConcern.NotNull(user, "User must not be null.");
            AssertionConcern.Equals(this.TenantId, user.TenantId, "Wrong tenant for this user.");

            this.Group.RemoveUser(user);

            DomainEventPublisher.Instance.Publish(new UserUnassignedFromRole(this.TenantId, this.Name, user.UserName));
        }

        public bool IsInRole(User user, IGroupMemberService groupMemberService) {
            return this.Group.IsMember(user, groupMemberService);
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.Name;
        }

        public override string ToString() {
            return "Role [tenantId=" + TenantId + ", name=" + Name
                    + ", description=" + Description
                    + ", supportsNesting=" + SupportsNesting
                    + ", group=" + this.Group + "]";
        }
    }
}