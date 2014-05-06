using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using SaasOvation.Common.Persistence;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Access.Repository;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence {
    public class RoleRepository: IRoleRepository {
        private readonly ISession _session;

        public RoleRepository(SessionProvider sessionProvider) {
            this._session = sessionProvider.GetSession();
        }
        
        public void Add(Role role) {
            if(this.RoleNamed(role.TenantId, role.Name)!=null) {
                throw new InvalidOperationException("Role is not unique.");
            }
            _session.Save(role.Group);
            _session.Save(role);
        }

        public ICollection<Role> AllRoles(TenantId tenantId) {
            ICriteria criteria = _session.CreateCriteria<Role>();
            criteria.Add(Restrictions.Eq("TenantId", tenantId));

            return criteria.List<Role>();
        }

        public Role RoleNamed(TenantId tenantId, string roleName) {
            ICriteria criteria = _session.CreateCriteria<Role>();
            criteria.Add(Restrictions.Eq("TenantId", tenantId));
            criteria.Add(Restrictions.Eq("Name", roleName));

            return criteria.List<Role>().SingleOrDefault();
        }

        public void Remove(Role role) {
            _session.Delete(role);
        }
    }
}