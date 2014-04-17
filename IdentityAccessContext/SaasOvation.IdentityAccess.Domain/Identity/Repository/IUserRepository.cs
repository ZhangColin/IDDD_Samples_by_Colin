using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;
using SaasOvation.IdentityAccess.Domain.Identity.Model.User;

namespace SaasOvation.IdentityAccess.Domain.Identity.Repository {
    public interface IUserRepository {
        void Add(User user);
        User UserFromAuthenticCredentials(TenantId tenantId, string userName, string encryptedPassword);
        User UserWithUserName(TenantId tenantId, string userName);
        void Remove(User user);
        ICollection<User> AllSimilarlyNamedUsers(TenantId tenantId, string firstNamePrefix, string lastNamePrefix);
    }
}