using System;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence {
    public class TenantRepository: ITenantRepository {
        private readonly ISession _session;

        public TenantRepository(SessionProvider sessionProvider) {
            this._session = sessionProvider.GetSession();
        }

        public TenantId GetNextIdentity() {
            return new TenantId(Guid.NewGuid().ToString());
        }

        public void Remove(Tenant tenant) {
            _session.Delete(tenant);
        }

        public void Add(Tenant tenant) {
            if(this.Get(tenant.TenantId)!=null) {
                throw new InvalidOperationException("Tenant is not unique.");
            }

            _session.Save(tenant);
        }

        public Tenant Get(TenantId tenantId) {
            ICriteria criteria = _session.CreateCriteria<Tenant>();
            criteria.Add(Restrictions.Eq("TenantId", tenantId));

            return criteria.List<Tenant>().SingleOrDefault();
        }

        public Tenant GetByName(string name) {
            ICriteria criteria = _session.CreateCriteria<Tenant>();
            criteria.Add(Restrictions.Eq("Name", name));

            return criteria.List<Tenant>().SingleOrDefault();
        }
    }
}