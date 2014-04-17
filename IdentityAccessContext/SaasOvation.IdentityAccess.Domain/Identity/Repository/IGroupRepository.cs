using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Group;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Repository {
    public interface IGroupRepository {
        Group GroupNamed(TenantId tenantId, string groupName);
        void Add(Group group);
        void Remove(Group group);
        ICollection<Group> AllGroups(TenantId tenantId);
    }
}