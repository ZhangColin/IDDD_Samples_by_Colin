using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;
using SaasOvation.IdentityAccess.Domain.Identity.Repository;

namespace SaasOvation.IdentityAccess.Infrastructure.Persistence {
    public class UserRepository: IUserRepository {
        private readonly ISession _session;

        public UserRepository(ISession session) {
            this._session = session;
        }

        public void Add(User user) {
            if (this.UserWithUserName(user.TenantId, user.UserName) != null) {
                throw new InvalidOperationException("User is not unique.");
            }
            _session.Save(user);
        }

        public User UserFromAuthenticCredentials(TenantId tenantId, string userName, string encryptedPassword) {
            ICriteria criteria = _session.CreateCriteria<User>();
            criteria.Add(Restrictions.Eq("TenantId", tenantId));
            criteria.Add(Restrictions.Eq("UserName", userName));
            criteria.Add(Restrictions.Eq("Password", encryptedPassword));

            return criteria.List<User>().SingleOrDefault();
        }

        public User UserWithUserName(TenantId tenantId, string userName) {
            ICriteria criteria = _session.CreateCriteria<User>();
            criteria.Add(Restrictions.Eq("TenantId", tenantId));
            criteria.Add(Restrictions.Eq("UserName", userName));

            return criteria.List<User>().SingleOrDefault();
        }

        public void Remove(User user) {
            _session.Delete(user);
        }

        public ICollection<User> AllSimilarlyNamedUsers(TenantId tenantId, string firstNamePrefix, string lastNamePrefix) {
            ICriteria criteria = _session.CreateCriteria<User>()
                .Add(Restrictions.Eq("TenantId", tenantId));
            criteria.CreateCriteria("Person")
                .Add(Restrictions.Like("Name.FirstName", firstNamePrefix, MatchMode.Start))
                .Add(Restrictions.Like("Name.LastName", lastNamePrefix, MatchMode.Start));

            return criteria.List<User>();
        }
    }
}