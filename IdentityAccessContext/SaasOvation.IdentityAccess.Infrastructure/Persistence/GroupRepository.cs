using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence {
    public class GroupRepository: IGroupRepository {
        private readonly ISession _session;

        public GroupRepository(ISession session) {
            this._session = session;
        }

        public Group GroupNamed(TenantId tenantId, string groupName) {
            if(groupName.StartsWith(Group.RoleGroupPrefix)) {
                throw new InvalidOperationException("May not find internal groups.");
            }
            ICriteria criteria = _session.CreateCriteria<Group>();
            criteria.Add(Restrictions.Eq("TenantId", tenantId));
            criteria.Add(Restrictions.Eq("Name", groupName));

            return criteria.List<Group>().SingleOrDefault();
        }

        public void Add(Group group) {
            if (this.GroupNamed(group.TenantId, group.Name) != null) {
                throw new InvalidOperationException("Group is not unique.");
            }

            foreach(GroupMember member in group.GroupMembers) {
                member.Group = group;
            }

            _session.Save(group);
        }

        public void Remove(Group group) {
            _session.Delete(group);
        }

        public ICollection<Group> AllGroups(TenantId tenantId) {
            ICriteria criteria = _session.CreateCriteria<Group>();
            criteria.Add(Restrictions.Eq("TenantId", tenantId));
            criteria.Add(Restrictions.Not(Restrictions.Like("Name", Group.RoleGroupPrefix, MatchMode.Start)));

            return criteria.List<Group>();
        }
    }
}