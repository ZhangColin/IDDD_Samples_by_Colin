using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Service;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.Group {
    public class Group: ConcurrencySafeEntity {
        public const string RoleGroupPrefix = "ROLE-INTERNAL-GROUP: ";

        public virtual TenantId TenantId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }

        public virtual IList<GroupMember> GroupMembers { get; protected set; } 

        protected Group() {
            GroupMembers = new List<GroupMember>();
        }

        public Group(TenantId tenantId, string name, string description): this() {
            this.TenantId = tenantId;
            this.Name = name;
            this.Description = description;
        }

        private bool IsInternalGroup {
            get { return this.Name.StartsWith(RoleGroupPrefix); }
        }

        public virtual void AddGroup(Group group, IGroupMemberService groupMemberService) {
            AssertionConcern.NotNull(group, "Group must not be null.");
            AssertionConcern.Equals(this.TenantId, group.TenantId, "Wrong tenant for this group.");
            AssertionConcern.False(groupMemberService.IsMemberGroup(group, this.ToGroupMember()), "Group recurrsion.");

            /*if (this.GroupMembers.Add(group.ToGroupMember()) && !this.IsInternalGroup) {
                DomainEventPublisher.Instance.Publish(new GroupGroupAdded(this.TenantId, this.Name, group.Name));
            }*/
            this.GroupMembers.Add(group.ToGroupMember());
            if(!this.IsInternalGroup) {
                DomainEventPublisher.Instance.Publish(new GroupGroupAdded(this.TenantId, this.Name, group.Name));
            }
        }

        public virtual void RemoveGroup(Group group) {
            AssertionConcern.NotNull(group, "Group must not be null.");
            AssertionConcern.Equals(this.TenantId, group.TenantId, "Wrong tenant for this group.");

            if(this.GroupMembers.Remove(group.ToGroupMember()) && !this.IsInternalGroup) {
                DomainEventPublisher.Instance.Publish(new GroupGroupRemoved(this.TenantId, this.Name, group.Name));
            }
        }

        public virtual void AddUser(User.User user) {
            AssertionConcern.NotNull(user, "User must not be null.");
            AssertionConcern.Equals(this.TenantId, user.TenantId, "Wrong tenant for this group.");
            AssertionConcern.True(user.IsEnabled, "User is not enabled.");

            /*if (this.GroupMembers.Add(user.ToGroupMember()) && !this.IsInternalGroup) {
                DomainEventPublisher.Instance.Publish(new GroupUserAdded(this.TenantId, this.Name, user.UserName));
            }*/
            this.GroupMembers.Add(user.ToGroupMember());
            if (!this.IsInternalGroup) {
                DomainEventPublisher.Instance.Publish(new GroupUserAdded(this.TenantId, this.Name, user.UserName));
            }
        }

        public virtual void RemoveUser(User.User user) {
            AssertionConcern.NotNull(user, "User must not be null.");
            AssertionConcern.Equals(this.TenantId, user.TenantId, "Wrong tenant for this group.");

            if (this.GroupMembers.Remove(user.ToGroupMember()) && !this.IsInternalGroup) {
                DomainEventPublisher.Instance.Publish(new GroupUserRemoved(this.TenantId, this.Name, user.UserName));
            }
        }

        public virtual bool IsMember(User.User user, IGroupMemberService groupMemberService) {
            AssertionConcern.NotNull(user, "User must not be null.");
            AssertionConcern.Equals(this.TenantId, user.TenantId, "Wrong tenant for this group.");
            AssertionConcern.True(user.IsEnabled, "User is not enabled.");

            bool isMember = this.GroupMembers.Contains(user.ToGroupMember());

            if(isMember) {
                isMember = groupMemberService.ConfirmUser(this, user);
            }
            else {
                isMember = groupMemberService.IsUserInNestedGroup(this, user);
            }

            return isMember;
        }

        protected internal virtual GroupMember ToGroupMember() {
            return new GroupMember(this.TenantId, this.Name, GroupMemberType.Group);
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.Name;
        }

        public override string ToString() {
            return "Group [description=" + Description + ", name=" + Name + ", tenantId=" + TenantId + "]";
        }
    }
}